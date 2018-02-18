using System;
using System.Collections.ObjectModel;

using ReactiveUI;
using Splat;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive;

namespace Intiface.ViewModels
{
    public class NavigationViewModel : ReactiveObject, ISupportsActivation
    {
        public ObservableCollection<NavigationEntry> NavigationItems = new ObservableCollection<NavigationEntry>();

        private NavigationEntry _selectedItem;
        public NavigationEntry SelectedItem {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }

        static NavigationViewModel()
        {
            Default = new NavigationViewModel()
            {
                NavigationItems =
                {
                    new NavigationEntry
                    {
                        Title = Properties.Resource.ViewSettingsTitle,
                        Command = ReactiveCommand.Create(() => Locator.Current.GetService<IScreen>().Router.Navigate.Execute(new ServerSettingsViewModel()).Subscribe())
                    },
                    new NavigationEntry
                    {
                        Title = Properties.Resource.ViewAboutTitle,
                        Command = ReactiveCommand.Create(() => Locator.Current.GetService<IScreen>().Router.Navigate.Execute(new AboutViewModel()).Subscribe())
                    },
                    new NavigationEntry
                    {
                        Title = Properties.Resource.ViewLicenseTitle,
                        Command = ReactiveCommand.Create(() => Locator.Current.GetService<IScreen>().Router.Navigate.Execute(new LicenseViewModel()).Subscribe())
                    }
                }
            };
        }

        public static NavigationViewModel Default { get; internal set; }

        public ViewModelActivator Activator { get; } = new ViewModelActivator();

        public NavigationViewModel()
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
