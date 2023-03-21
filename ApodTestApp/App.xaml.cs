
namespace ApodTestApp;

public partial class App : Application
{
	//add global properties

	//for settings switch
	public static bool UseHiDef { get; set; } = false;
	public static bool LoopAtEnd { get; set; } = true;

    //for number of random of images
	public static int NumberOfRandomImages { get; set; } = 1;

    //for select a date
    public static DateTime SelectDate { get; set; } = DateTime.Now;

    //for range of dates
    public static DateTime DateRangeStart { get; set; } = DateTime.Now;
    public static DateTime DateRangeEnd { get; set; } = DateTime.Now;


	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}

    protected override void OnStart()
    {
        base.OnStart();

		UseHiDef = Preferences.Default.Get("UseHiDef", false);

        //TODO: add for loop at end

        NumberOfRandomImages = Preferences.Default.Get("NumberOfRandomImages", 1);

        SelectDate = Preferences.Default.Get("SelectDate", DateTime.Now);

        DateRangeStart = Preferences.Default.Get("DateRangeStart", DateTime.Now.AddDays(-1));
        DateRangeEnd = Preferences.Default.Get("DateRangeEnd", DateTime.Now);

    }

    protected override void OnSleep()
    {
        base.OnSleep();

        Preferences.Default.Set("UseHiDef", UseHiDef);

        //TODO: add for loop at end

        Preferences.Default.Set("NumberOfRandomImages", NumberOfRandomImages);

        Preferences.Default.Set("SelectDate", SelectDate);

        Preferences.Default.Set("DateRangeStart", DateRangeStart);
        Preferences.Default.Set("DateRangeEnd", DateRangeEnd);
    }
}

public enum ImageMode
{
    Default,
    Date,
    DateRange,
    Random
}