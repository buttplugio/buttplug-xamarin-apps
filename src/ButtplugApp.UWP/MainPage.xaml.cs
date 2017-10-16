namespace ButtplugApp.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new ButtplugApp.App());
        }
    }
}
