﻿using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Resources;
using Intiface.Models;

namespace Intiface.ViewModels
{
    public class StatusViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => Properties.Resource.ViewStatusTitle;

        public IScreen HostScreen { get; }

        public List<string> Addresses { get; set; } = new List<string>();

        public ReactiveCommand StartStopCommand { get; }

        public string StatusText => $"{Properties.Resource.ViewStatusServerStatusLabel}: {Properties.Resource.ServerStatusStopped}";

        public string StartStopText => Properties.Resource.ActionServerStart;

        public StatusViewModel(IScreen hostScreen = null)
        {
            HostScreen = hostScreen ?? Locator.Current.GetService<IScreen>();

            // TODO: Remove this example address
            Addresses.Add("ws://192.168.1.10:12345/buttplug");

            StartStopCommand = ReactiveCommand.Create(() => MessagingCenter.Send(new ServerCommandMessage { Command = ServerCommand.Start }, nameof(ServerCommandMessage)));
        }
    }
}
