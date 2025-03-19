using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Ryujinx.Common.Memory
{
    /// <summary>
    /// Represents an array of 128 bytes.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = Size, Pack = 1)]
    public struct ByteArray128 : IArray<byte>, IEnumerable<byte>
    {
        private const int Size = 128;
        private byte _element;

        /// <inheritdoc/>
        public int Length => Size;

        /// <summary>
        /// Gets a reference to the byte at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index.</param>
        /// <returns>A reference to the byte at the given index.</returns>
        public ref byte this[int index] => ref AsSpan()[index];

        /// <summary>
        /// Creates a span over the underlying byte array.
        /// </summary>
        public Span<byte> AsSpan() => MemoryMarshal.CreateSpan(ref _element, Size);

        /// <summary>
        /// Returns a managed copy of the array.
        /// </summary>
        public byte[] ToArray() => AsSpan().ToArray();

        /// <inheritdoc/>
        public IEnumerator<byte> GetEnumerator() => ((IEnumerable<byte>)AsSpan().ToArray()).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// Represents an array of 256 bytes.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = Size, Pack = 1)]
    public struct ByteArray256 : IArray<byte>, IEnumerable<byte>
    {
        private const int Size = 256;
        private byte _element;

        /// <inheritdoc/>
        public int Length => Size;

        /// <summary>
        /// Gets a reference to the byte at the specified index.
        /// </summary>
        public ref byte this[int index] => ref AsSpan()[index];

        /// <summary>
        /// Creates a span over the underlying byte array.
        /// </summary>
        public Span<byte> AsSpan() => MemoryMarshal.CreateSpan(ref _element, Size);

        /// <summary>
        /// Returns a managed copy of the array.
        /// </summary>
        public byte[] ToArray() => AsSpan().ToArray();

        /// <inheritdoc/>
        public IEnumerator<byte> GetEnumerator() => ((IEnumerable<byte>)AsSpan().ToArray()).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// Represents an array of 512 bytes.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = Size, Pack = 1)]
    public struct ByteArray512 : IArray<byte>, IEnumerable<byte>
    {
        private const int Size = 512;
        private byte _element;

        /// <inheritdoc/>
        public int Length => Size;

        /// <summary>
        /// Gets a reference to the byte at the specified index.
        /// </summary>
        public ref byte this[int index] => ref AsSpan()[index];

        /// <summary>
        /// Creates a span over the underlying byte array.
        /// </summary>
        public Span<byte> AsSpan() => MemoryMarshal.CreateSpan(ref _element, Size);

        /// <summary>
        /// Returns a managed copy of the array.
        /// </summary>
        public byte[] ToArray() => AsSpan().ToArray();

        /// <inheritdoc/>
        public IEnumerator<byte> GetEnumerator() => ((IEnumerable<byte>)AsSpan().ToArray()).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// Represents an array of 1024 bytes.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = Size, Pack = 1)]
    public struct ByteArray1024 : IArray<byte>, IEnumerable<byte>
    {
        private const int Size = 1024;
        private byte _element;

        /// <inheritdoc/>
        public int Length => Size;

        /// <summary>
        /// Gets a reference to the byte at the specified index.
        /// </summary>
        public ref byte this[int index] => ref AsSpan()[index];

        /// <summary>
        /// Creates a span over the underlying byte array.
        /// </summary>
        public Span<byte> AsSpan() => MemoryMarshal.CreateSpan(ref _element, Size);

        /// <summary>
        /// Returns a managed copy of the array.
        /// </summary>
        public byte[] ToArray() => AsSpan().ToArray();

        /// <inheritdoc/>
        public IEnumerator<byte> GetEnumerator() => ((IEnumerable<byte>)AsSpan().ToArray()).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// Represents an array of 2048 bytes.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = Size, Pack = 1)]
    public struct ByteArray2048 : IArray<byte>, IEnumerable<byte>
    {
        private const int Size = 2048;
        private byte _element;

        /// <inheritdoc/>
        public int Length => Size;

        /// <summary>
        /// Gets a reference to the byte at the specified index.
        /// </summary>
        public ref byte this[int index] => ref AsSpan()[index];

        /// <summary>
        /// Creates a span over the underlying byte array.
        /// </summary>
        public Span<byte> AsSpan() => MemoryMarshal.CreateSpan(ref _element, Size);

        /// <summary>
        /// Returns a managed copy of the array.
        /// </summary>
        public byte[] ToArray() => AsSpan().ToArray();

        /// <inheritdoc/>
        public IEnumerator<byte> GetEnumerator() => ((IEnumerable<byte>)AsSpan().ToArray()).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// Represents an array of 3000 bytes.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = Size, Pack = 1)]
    public struct ByteArray3000 : IArray<byte>, IEnumerable<byte>
    {
        private const int Size = 3000;
        private byte _element;

        /// <inheritdoc/>
        public int Length => Size;

        /// <summary>
        /// Gets a reference to the byte at the specified index.
        /// </summary>
        public ref byte this[int index] => ref AsSpan()[index];

        /// <summary>
        /// Creates a span over the underlying byte array.
        /// </summary>
        public Span<byte> AsSpan() => MemoryMarshal.CreateSpan(ref _element, Size);

        /// <summary>
        /// Returns a managed copy of the array.
        /// </summary>
        public byte[] ToArray() => AsSpan().ToArray();

        /// <inheritdoc/>
        public IEnumerator<byte> GetEnumerator() => ((IEnumerable<byte>)AsSpan().ToArray()).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// Represents an array of 4096 bytes.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = Size, Pack = 1)]
    public struct ByteArray4096 : IArray<byte>, IEnumerable<byte>
    {
        private const int Size = 4096;
        private byte _element;

        /// <inheritdoc/>
        public int Length => Size;

        /// <summary>
        /// Gets a reference to the byte at the specified index.
        /// </summary>
        public ref byte this[int index] => ref AsSpan()[index];

        /// <summary>
        /// Creates a span over the underlying byte array.
        /// </summary>
        public Span<byte> AsSpan() => MemoryMarshal.CreateSpan(ref _element, Size);

        /// <summary>
        /// Returns a managed copy of the array.
        /// </summary>
        public byte[] ToArray() => AsSpan().ToArray();

        /// <inheritdoc/>
        public IEnumerator<byte> GetEnumerator() => ((IEnumerable<byte>)AsSpan().ToArray()).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
