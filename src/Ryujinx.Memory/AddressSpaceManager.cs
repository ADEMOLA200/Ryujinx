using Ryujinx.Memory.Range;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Ryujinx.Memory
{
    /// <summary>
    /// Represents an address space manager.
    /// Supports virtual memory region mapping, address translation, and read/write access to mapped regions.
    /// </summary>
    public sealed class AddressSpaceManager : VirtualMemoryManagerBase, IVirtualMemoryManager
    {
        /// <inheritdoc/>
        public bool UsesPrivateAllocations => false;

        /// <summary>
        /// Gets the address space width in bits.
        /// </summary>
        public int AddressSpaceBits { get; }

        private readonly MemoryBlock _backingMemory;
        private readonly PageTable<nuint> _pageTable;

        protected override ulong AddressSpaceSize { get; }

        /// <summary>
        /// Creates a new instance of the memory manager.
        /// </summary>
        /// <param name="backingMemory">The physical backing memory to which virtual memory will be mapped.</param>
        /// <param name="addressSpaceSize">The desired size of the virtual address space.</param>
        public AddressSpaceManager(MemoryBlock backingMemory, ulong addressSpaceSize)
        {
            // Calculate the minimum address space size as a power-of-two not less than the given size.
            ulong asSize = PageSize;
            int asBits = PageBits;

            while (asSize < addressSpaceSize)
            {
                asSize <<= 1;
                asBits++;
            }

            AddressSpaceBits = asBits;
            AddressSpaceSize = asSize;
            _backingMemory = backingMemory;
            _pageTable = new PageTable<nuint>();
        }

        /// <inheritdoc/>
        public void Map(ulong va, ulong pa, ulong size, MemoryMapFlags flags)
        {
            AssertValidAddressAndSize(va, size);

            // Map each page individually.
            while (size != 0)
            {
                // Get the host pointer for the given physical address.
                nuint hostPtr = (nuint)(ulong)_backingMemory.GetPointer(pa, PageSize);
                _pageTable.Map(va, hostPtr);

                va += PageSize;
                pa += PageSize;
                size -= PageSize;
            }
        }

        /// <inheritdoc/>
        public override void MapForeign(ulong va, nuint hostPointer, ulong size)
        {
            AssertValidAddressAndSize(va, size);

            // Map foreign memory pages.
            while (size != 0)
            {
                _pageTable.Map(va, hostPointer);

                va += PageSize;
                hostPointer += PageSize;
                size -= PageSize;
            }
        }

        /// <inheritdoc/>
        public void Unmap(ulong va, ulong size)
        {
            AssertValidAddressAndSize(va, size);

            // Unmap each page individually.
            while (size != 0)
            {
                _pageTable.Unmap(va);
                va += PageSize;
                size -= PageSize;
            }
        }

        /// <inheritdoc/>
        public unsafe ref T GetRef<T>(ulong va) where T : unmanaged
        {
            // Ensure that the memory region is contiguous for the requested type.
            if (!IsContiguous(va, Unsafe.SizeOf<T>()))
            {
                ThrowMemoryNotContiguous();
            }

            return ref *(T*)GetHostAddress(va);
        }

        /// <inheritdoc/>
        public IEnumerable<HostMemoryRange> GetHostRegions(ulong va, ulong size)
        {
            if (size == 0)
            {
                yield break;
            }

            foreach (HostMemoryRange hostRegion in GetHostRegionsImpl(va, size))
            {
                yield return hostRegion;
            }
        }

        /// <inheritdoc/>
        public IEnumerable<MemoryRange> GetPhysicalRegions(ulong va, ulong size)
        {
            if (size == 0)
            {
                yield break;
            }

            // Use host regions to calculate corresponding physical regions.
            IEnumerable<HostMemoryRange> hostRegions = GetHostRegionsImpl(va, size);
            if (hostRegions == null)
            {
                yield break;
            }

            ulong backingStart = (ulong)_backingMemory.Pointer;
            ulong backingEnd = backingStart + _backingMemory.Size;

            foreach (HostMemoryRange hostRegion in hostRegions)
            {
                if (hostRegion.Address >= backingStart && hostRegion.Address < backingEnd)
                {
                    yield return new MemoryRange(hostRegion.Address - backingStart, hostRegion.Size);
                }
            }
        }

        /// <summary>
        /// Helper method to aggregate contiguous host memory regions.
        /// </summary>
        /// <param name="va">The starting virtual address.</param>
        /// <param name="size">The size of the region to evaluate.</param>
        /// <returns>An enumerable of contiguous host memory ranges.</returns>
        private IEnumerable<HostMemoryRange> GetHostRegionsImpl(ulong va, ulong size)
        {
            if (!ValidateAddress(va) || !ValidateAddressAndSize(va, size))
            {
                yield break;
            }

            int pages = GetPagesCount(va, size, out va);
            // Initialize region aggregation with the host address corresponding to the starting virtual address.
            nuint regionStart = GetHostAddress(va);
            ulong regionSize = PageSize;
            nuint previousHostAddress = regionStart;

            // Process subsequent pages to aggregate contiguous regions.
            for (int page = 1; page < pages; page++)
            {
                ulong currentVA = va + (ulong)(page * PageSize);
                if (!ValidateAddress(currentVA))
                {
                    yield break;
                }

                nuint currentHostAddress = GetHostAddress(currentVA);

                // Check if the current page is contiguous with the previous page.
                if (previousHostAddress + PageSize != currentHostAddress)
                {
                    // Yield the aggregated contiguous region.
                    yield return new HostMemoryRange(regionStart, regionSize);
                    regionStart = currentHostAddress;
                    regionSize = PageSize;
                }
                else
                {
                    regionSize += PageSize;
                }

                previousHostAddress = currentHostAddress;
            }

            yield return new HostMemoryRange(regionStart, regionSize);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool IsMapped(ulong va)
        {
            if (!ValidateAddress(va))
            {
                return false;
            }

            return _pageTable.Read(va) != 0;
        }

        /// <inheritdoc/>
        public bool IsRangeMapped(ulong va, ulong size)
        {
            if (size == 0)
            {
                return true;
            }

            if (!ValidateAddressAndSize(va, size))
            {
                return false;
            }

            int pages = GetPagesCount(va, (uint)size, out va);
            for (int page = 0; page < pages; page++)
            {
                if (!IsMapped(va))
                {
                    return false;
                }

                va += PageSize;
            }

            return true;
        }

        /// <summary>
        /// Calculates the host address corresponding to a given virtual address.
        /// </summary>
        /// <param name="va">The virtual address.</param>
        /// <returns>The computed host address.</returns>
        private nuint GetHostAddress(ulong va)
        {
            // Combines the mapped base pointer from the page table with the offset within the page.
            return _pageTable.Read(va) + (nuint)(va & PageMask);
        }

        /// <inheritdoc/>
        public void Reprotect(ulong va, ulong size, MemoryPermission protection)
        {
            // If needed, implement protection changes on the mapped pages.
        }

        /// <inheritdoc/>
        public void TrackingReprotect(ulong va, ulong size, MemoryPermission protection, bool guest = false)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a Memory&lt;byte&gt; instance wrapping the physical memory starting at the specified address.
        /// </summary>
        /// <param name="pa">The physical address.</param>
        /// <param name="size">The size of the memory region.</param>
        /// <returns>A Memory&lt;byte&gt; instance representing the physical memory.</returns>
        protected unsafe override Memory<byte> GetPhysicalAddressMemory(nuint pa, int size)
            => new NativeMemoryManager<byte>((byte*)pa, size).Memory;

        /// <summary>
        /// Returns a Span&lt;byte&gt; instance wrapping the physical memory starting at the specified address.
        /// </summary>
        /// <param name="pa">The physical address.</param>
        /// <param name="size">The size of the memory region.</param>
        /// <returns>A Span&lt;byte&gt; instance representing the physical memory.</returns>
        protected override unsafe Span<byte> GetPhysicalAddressSpan(nuint pa, int size) => new((void*)pa, size);

        /// <summary>
        /// Translates a virtual address to a host address (checked).
        /// </summary>
        /// <param name="va">The virtual address.</param>
        /// <returns>The corresponding host address.</returns>
        protected override nuint TranslateVirtualAddressChecked(ulong va)
            => GetHostAddress(va);

        /// <summary>
        /// Translates a virtual address to a host address (unchecked).
        /// </summary>
        /// <param name="va">The virtual address.</param>
        /// <returns>The corresponding host address.</returns>
        protected override nuint TranslateVirtualAddressUnchecked(ulong va)
            => GetHostAddress(va);
    }
}
