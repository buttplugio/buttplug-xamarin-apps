using ReactiveUI.XamForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ButtplugApp.ViewModels;


namespace ButtplugApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DeviceListView : ReactiveContentPage<DeviceListViewModel>
	{
		public DeviceListView ()
		{
			InitializeComponent ();
		}
	}
}