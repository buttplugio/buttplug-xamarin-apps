using ButtplugApp.ViewModels;
using ReactiveUI.XamForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ButtplugApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class About : ReactiveContentPage<AboutViewModel>
	{
		public About ()
		{
			InitializeComponent ();
		}

        private void OpenLicense(object sender, EventArgs e)
        {
            
        }

        private void OpenSourceCode(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://github.com/metafetish"));
        }

        private void OpenDocumentation(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://buttplug.io/docs"));
        }
	}
}