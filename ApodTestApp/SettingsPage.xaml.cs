namespace ApodTestApp;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		InitializeComponent();

        Title = "Settings";

        HiDefSwitch.IsToggled = App.UseHiDef;
        LoopSwitch.IsToggled = App.LoopAtEnd;

        DisplayAppInfo();

        DisplayDeviceInfo();
	}

    private void DisplayDeviceInfo()
    {
        var infoString = $"Device Type: {DeviceInfo.DeviceType}{Environment.NewLine}" +
            $"Idiom: {DeviceInfo.Idiom}{Environment.NewLine}" +
            $"Manufacturer: {DeviceInfo.Manufacturer}{Environment.NewLine}" +
            $"Model: {DeviceInfo.Model}{Environment.NewLine}" +
            $"Name: {DeviceInfo.Name}{Environment.NewLine}" +
            $"Platform: {DeviceInfo.Platform}{Environment.NewLine}" +
            $"Version: {DeviceInfo.Version}{Environment.NewLine}";

        DeviceInformation.Text = infoString;
    }

    private void DisplayAppInfo()
    {
        string infoString = $"{AppInfo.Name}{Environment.NewLine}" +
            $"Version: {AppInfo.Version}{Environment.NewLine}" +
            $"Build: {AppInfo.BuildString}{Environment.NewLine}";

        AppInformation.Text= infoString ;
    }

    private void AppSettingsButton_Clicked(object sender, EventArgs e)
    {
        AppInfo.ShowSettingsUI();
    }

    private void HiDefSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        App.UseHiDef = HiDefSwitch.IsToggled;
    }

    private void LoopSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        App.LoopAtEnd = LoopSwitch.IsToggled;
    }
}