using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ApodTestApp
{

    // describes what the return value would look like
    public class ApodData
    {
        public string copyright { get; set; }
        public string date { get; set; }
        public string explanation { get; set; }
        public string hdurl { get; set; }
        public string media_type { get; set; }
        public string service_version { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string thumbnail_url { get; set; }
    }

    class Apod
    {

        #region apiKey

        private const string apiKey = "Xvccuoh4AONYEaEPgYhEebrzjijg3a9ErJbYP0WP";

        #endregion

        #region Constants

        private const string baseUrl = "https://api.nasa.gov/planetary/apod?api_key=";
        private const string errorUrl = "https://placehold.jp/3d4070/ffffff/1024x768.png?text=Error";
        private const string thumbsParameter = "&thumbs=true";
        private const string noImageMessage = "No Apod image for this day";
        private const string noCopyright = "None";
        private const string dateParameter = "&date=";
        private const string startDateParameter = "&start_date=";
        private const string endDateParameter = "&end_date=";
        private const string countParamenter = "&count=";


        #endregion

        #region Properties

        public string Description { get; private set; } = string.Empty;
        public string Information { get; private set; } = string.Empty;
        public string Title { get; private set; } = string.Empty;
        public string LastError { get; private set; } = string.Empty;
        public bool HighDef { get; set; } = false;
        public string Date { get; set; } = string.Empty;
        public Uri LastUri { get; set; } = null;

        #endregion

        #region Members

        private HttpClient httpClient = new();

        private DateTime today;
        private readonly DateTime epoc;
        private DateTime lastDate;

        private ApodData currentApodData;

        private ApodData[] dateRangeApodData;

        public Uri Url { get; private set; } = null;
        public List<string> ImageUrlList { get; private set; } = new();
        public List<string> ImageDescriptionList { get; private set; } = new();

        #endregion

        #region Constructor

        public Apod(bool highDef = false)
        {
            HighDef = highDef;

            //date of first Apod image
            epoc = new DateTime(1995, 06, 16);

            today = DateTime.Now;
        }

        #endregion 

        #region Methods

        //Gets the most recent Uri
        public async Task<Uri> GetApodUri()
        {
            today = DateTime.Now;

            lastDate = today;

            var request = new Uri($"{baseUrl}{apiKey}{thumbsParameter}");

            bool success = await GetApod(request);

            if (success)
            {
                return GetValidUri();
            }
            else
            {
                return new Uri($"{errorUrl}{LastError}");
            }
        }

        private Uri GetValidUri()
        {
            if(currentApodData.media_type == "image")
            {
                if(HighDef && currentApodData.hdurl != null)
                {   
                    LastUri = new Uri(currentApodData.hdurl);
                }
                else
                {
                    LastUri = new Uri(currentApodData.url);
                }
            }
            else if(currentApodData.media_type == "video" && !string.IsNullOrEmpty(currentApodData.thumbnail_url))
            {
                LastUri = new Uri(currentApodData.thumbnail_url);
            }
            else
            {
                LastError = $"No valid URL for {currentApodData.date}";
                LastUri = new Uri($"{errorUrl}{LastError}");
            }

            return LastUri;
        }

        private async Task<bool> GetApod(Uri request)
        {
            try
            {
                currentApodData = await httpClient.GetFromJsonAsync<ApodData>(request);

                SetInformationAndDescription();

                return true;
            }
            catch(Exception e)
            {
                LastError = e.Message;
                return false;
            }
        }

        private void SetInformationAndDescription()
        {
            Title = currentApodData.title;
            Date = currentApodData.date;

            if(string.IsNullOrEmpty(currentApodData.copyright))
            {
                currentApodData.copyright = noCopyright;
            }

            Description = $"{currentApodData.title} (\u00A9) {currentApodData.copyright}";

            Information = $"{currentApodData.explanation}{Environment.NewLine}{Environment.NewLine} (©) {currentApodData.copyright}" +
                $"{Environment.NewLine}{Environment.NewLine} Image Date: {currentApodData.date}";
        }

        public async Task<Uri> GetPreviousUri()
        {
            lastDate = lastDate.AddDays(-1);
            return await GetApodUriByDate(lastDate);
        }

        public async Task<Uri> GetNextUri()
        {
            lastDate = lastDate.AddDays(1);
            return await GetApodUriByDate(lastDate);
        }

        public async Task<Uri> GetApodUriByDate(DateTime newDate)
        {
            today = DateTime.Now;

            if(newDate > today) // can't get an image from the future
            {
                newDate = today;
            }
            else if (newDate < epoc) // can't get an image before APOD start date
            {
                newDate = epoc;
            }

            lastDate = newDate;

            var date = newDate.ToString("yyyy-MM-dd");

            System.Diagnostics.Debug.WriteLine(date);

            var request = new Uri($"{baseUrl}{apiKey}{dateParameter}{date}{thumbsParameter}");

            var success = await GetApod(request);

            if (success)
            {
                return GetValidUri();
            }
            else
            {
                return new Uri($"{errorUrl}{LastError}");
            }
        }

        //Get Apod Uri by range of dates

        public async Task<Uri> GetApodUriByDateRange(DateTime startDate, DateTime endDate)
        {
            today = DateTime.Now;

            //if(startDate > endDate)
            //{
            //    startDate = today;
            //}
            //else if (startDate > today)
            //{
            //    startDate = today;
            //}
            //else if (endDate > startDate.AddDays(-100))
            //{
            //    endDate = endDate.AddDays(-100);
            //}
            //else if (startDate < epoc)
            //{
            //    startDate = epoc;
            //}

            lastDate = startDate;

            var start = startDate.ToString("yyyy-MM-dd");
            var end = endDate.ToString("yyyy-MM-dd");

            System.Diagnostics.Debug.WriteLine(start + " to " + end);

            var request = new Uri($"{baseUrl}{apiKey}{startDateParameter}{start}{endDateParameter}{end}{thumbsParameter}");
            
            var success = await GetApodDataByDateRange(request);


            if (success)
            {
                return GetValidUris();
            }
            else
            {
                return new Uri($"{errorUrl}{LastError}");
            }
        }

        private async Task<bool> GetApodDataByDateRange(Uri request)
        {
            try
            {
                dateRangeApodData = await httpClient.GetFromJsonAsync<ApodData[]>(request);

                //SetInformationAndDescription();

                SetRangedDatesInfo();

                return true;
            }
            catch (Exception e)
            {
                LastError = e.Message;
                return false;
            }
        }

        private void SetRangedDatesInfo()
        {
            Title = dateRangeApodData[0].title;
            Date = dateRangeApodData[0].date;

            if (string.IsNullOrEmpty(dateRangeApodData[0].copyright))
            {
                dateRangeApodData[0].copyright = noCopyright;
            }

            Description = $"{dateRangeApodData[0].title} (\u00A9) {dateRangeApodData[0].copyright}";

            Information = $"{dateRangeApodData[0].explanation}{Environment.NewLine}{Environment.NewLine} (©) {dateRangeApodData[0].copyright}" +
                $"{Environment.NewLine}{Environment.NewLine} Image Date: {dateRangeApodData[0].date}";
        }

        private Uri GetValidUris()
        {
            if (dateRangeApodData[0].media_type == "image")
            {
                if (HighDef && dateRangeApodData[0].hdurl != null)
                {
                    LastUri = new Uri(dateRangeApodData[0].hdurl);
                }
                else
                {
                    LastUri = new Uri(dateRangeApodData[0].url);
                }
            }
            else if (dateRangeApodData[0].media_type == "video" && !string.IsNullOrEmpty(dateRangeApodData[0].thumbnail_url))
            {
                LastUri = new Uri(dateRangeApodData[0].thumbnail_url);
            }
            else
            {
                LastError = $"No valid URL for {dateRangeApodData[0].date}";
                LastUri = new Uri($"{errorUrl}{LastError}");
            }

            return LastUri;
        }

        #endregion
    }
}


//create task to get next day uri
//add fonts
//add svg images for icons
//figure out navigation
//figure out design of app