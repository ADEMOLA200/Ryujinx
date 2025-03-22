using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using Ryujinx.Ava.Common.Locale;
using Ryujinx.Ava.UI.ViewModels;
using System;
using System.Linq;

namespace Ryujinx.Ava.UI.Windows
{
    public partial class GameSpecificSettingsWindow : StyleableAppWindow
    {
        internal readonly SettingsViewModel ViewModel;

        // Parameterless constructor required by Avalonia's runtime loader.
        // This constructor is intended for design-time/XAML instantiation only.
        public GameSpecificSettingsWindow()
        {
            InitializeComponent();
        }

        public GameSpecificSettingsWindow(MainWindowViewModel viewModel, bool findUserConfigDir = true)
        {
            Title = string.Format(LocaleManager.Instance[LocaleKeys.SettingsWithInfo], viewModel.SelectedApplication.Name, viewModel.SelectedApplication.IdString);

            DataContext = ViewModel = new SettingsViewModel(
                viewModel.VirtualFileSystem, 
                viewModel.ContentManager,
                viewModel.IsGameRunning,
                viewModel.SelectedApplication.Path,
                viewModel.SelectedApplication.Name,
                viewModel.SelectedApplication.IdString,
                viewModel.SelectedApplication.Icon,
                findUserConfigDir);

            ViewModel.CloseWindow += Close;
            ViewModel.SaveSettingsEvent += SaveSettings;

            InitializeComponent();
            Load();
        }

        public void SaveSettings()
        {
            InputPage.InputView?.SaveCurrentProfile();
        }

        private void Load()
        {
            Pages.Children.Clear();
            NavPanel.SelectionChanged += NavPanelOnSelectionChanged;
            NavPanel.SelectedItem = NavPanel.MenuItems.ElementAt(0);
        }

        private void NavPanelOnSelectionChanged(object sender, NavigationViewSelectionChangedEventArgs e)
        {
            if (e.SelectedItem is NavigationViewItem navItem && navItem.Tag is not null)
            {
                switch (navItem.Tag.ToString())
                {
                    case nameof(UiPage):
                        UiPage.ViewModel = ViewModel;
                        NavPanel.Content = UiPage;
                        break;
                    case nameof(InputPage):
                        NavPanel.Content = InputPage;
                        break;
                    case nameof(SystemPage):
                        SystemPage.ViewModel = ViewModel;
                        NavPanel.Content = SystemPage;
                        break;
                    case nameof(CpuPage):
                        NavPanel.Content = CpuPage;
                        break;
                    case nameof(GraphicsPage):
                        NavPanel.Content = GraphicsPage;
                        break;
                    case nameof(AudioPage):
                        NavPanel.Content = AudioPage;
                        break;
                    case nameof(NetworkPage):
                        NetworkPage.ViewModel = ViewModel;
                        NavPanel.Content = NetworkPage;
                        break;
                    case nameof(LoggingPage):
                        NavPanel.Content = LoggingPage;
                        break;
                    case nameof(HacksPage):
                        HacksPage.DataContext = ViewModel;
                        NavPanel.Content = HacksPage;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }        
        }

        protected override void OnClosing(WindowClosingEventArgs e)
        {
            InputPage.Dispose();
            base.OnClosing(e);
        }
    }
}
