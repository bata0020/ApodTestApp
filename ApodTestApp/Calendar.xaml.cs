namespace ApodTestApp;

public partial class Calendar : ContentPage
{
	public Calendar()
	{
		InitializeComponent();

        pickDate.Date = App.SelectDate;

        pickDate.MaximumDate = DateTime.Now;
        pickDate.MinimumDate = new DateTime(1995, 06, 16);
	}

    private async void pickDateButton_Clicked(object sender, EventArgs e)
    {
        App.SelectDate = pickDate.Date;

        await Shell.Current.GoToAsync("//MainPage");
    }

}