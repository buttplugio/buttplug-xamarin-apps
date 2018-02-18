using ReactiveUI.XamForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Intiface.ViewModels;


namespace Intiface.Views
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