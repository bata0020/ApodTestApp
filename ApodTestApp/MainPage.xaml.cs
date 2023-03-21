using System.Diagnostics;

namespace ApodTestApp;

public partial class MainPage : ContentPage
{

	private Apod apod = new(App.UseHiDef);
    
    public string ImageDescription { get; set; } = string.Empty;

	public MainPage()
	{

		InitializeComponent();

        //change apod here
        //apod.HighDef= true;

	}

    protected async override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        Uri uri = null;

        if (apod.LastUri == null)
            uri = await apod.GetApodUri();
        else
            uri = apod.LastUri;

        if(uri != null)
        {
            TheImage.Source = uri;
            ImageDescription = apod.Information;
            TheTitle.Text = apod.Date;
        }
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        Description.IsVisible = !Description.IsVisible;
        Description.Text = ImageDescription;
    }

    private async void OnSwiped(object sender, SwipedEventArgs e)
    {
        switch (e.Direction)
        {
            case SwipeDirection.Left:

                var uri = await apod.GetPreviousUri();

                TheImage.Source = uri;
                ImageDescription = apod.Information;
                TheTitle.Text = apod.Date;

                Description.Text = ImageDescription;

                SemanticProperties.SetDescription(TheImage, ImageDescription);

                //semantic properties to be added

                break;

            case SwipeDirection.Right:              

                uri = await apod.GetNextUri();

                TheImage.Source = uri;
                ImageDescription = apod.Information;
                TheTitle.Text = apod.Date;

                Description.Text = ImageDescription;

                SemanticProperties.SetDescription(TheImage, ImageDescription);

                break;
        }

    }

    private async void SettingsButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SettingsPage());
    }
}

