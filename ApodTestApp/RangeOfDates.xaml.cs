

namespace ApodTestApp;

public partial class RangeOfDates : ContentPage
{
	public RangeOfDates()
	{
		InitializeComponent();

        dateRangeStart.Date = App.DateRangeStart;
        dateRangeEnd.Date = App.DateRangeEnd;

        //set the maximum and minimum start and end date
        dateRangeStart.MaximumDate = DateTime.Now;
        dateRangeEnd.MaximumDate = DateTime.Now;

        dateRangeStart.MinimumDate = new DateTime(1995, 06, 16);
        dateRangeEnd.MinimumDate = new DateTime(1995, 06, 16);

        //add end date
    }

    private async void dateRangeButton_Clicked(object sender, EventArgs e)
    {
        App.DateRangeStart = dateRangeStart.Date;
        App.DateRangeEnd = dateRangeEnd.Date;

        await Shell.Current.GoToAsync("//MainPage");
    }

 
}