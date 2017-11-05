using System;
using System.Collections.ObjectModel;

using ReactiveUI;
using Splat;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive;

namespace ButtplugApp.ViewModels
{
    public class MainPageViewModel : ReactiveObject, ISupportsActivation
    {
        public ObservableCollection<NavigationEntry> NavigationItems = new ObservableCollection<NavigationEntry>();

        private NavigationEntry _selectedItem;
        public NavigationEntry SelectedItem {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }

        static MainPageViewModel()
        {
            Default = new MainPageViewModel()
            {
                NavigationItems =
                {
                    new NavigationEntry
                    {
                        Title = "Settings",
                        Command = ReactiveCommand.Create(() => Locator.Current.GetService<IScreen>().Router.Navigate.Execute(new ServerSettingsViewModel()).Subscribe())
                    }
                }
            };
        }

        public static MainPageViewModel Default { get; internal set; }

        public ViewModelActivator Activator { get; } = new ViewModelActivator();

        public MainPageViewModel()
        {
            this.WhenActivated((CompositeDisposable disposables) =>
            {
                this.WhenAnyValue(v => v.SelectedItem)
                    .Where(x => x != null)
                    .Subscribe(entry => ExecuteNavigationCommand(entry))
                    .DisposeWith(disposables);
            });
        }


        private void ExecuteNavigationCommand(NavigationEntry entry)
        {
            var command = entry.Command as System.Windows.Input.ICommand;

            if (command.CanExecute(Unit.Default))
                command.Execute(Unit.Default);
        }
    }
}
