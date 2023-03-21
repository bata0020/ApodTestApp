namespace ApodTestApp;

public partial class Random : ContentPage
{
    int value = 1;

	public Random()
	{
		InitializeComponent();

        value = App.NumberOfRandomImages;
        randomSlider.Value = value;
	}

    private async void goRandomButton_Clicked(object sender, EventArgs e)
    {
        App.NumberOfRandomImages = value;

        await Shell.Current.GoToAsync("//MainPage");
    }

    private void randomSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        value = Convert.ToInt32(e.NewValue);
        sliderValue.Text = value.ToString();
    }
}