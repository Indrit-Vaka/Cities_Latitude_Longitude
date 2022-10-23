using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text.Json;
using Cities_Latitude_Longitude.Model;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

namespace Cities_Latitude_Longitude;

public class GetCities
{
    public static async Task Main(string[] args)
    {

        Object key = new Object();


        string root = Application.GetSolutionRootPath() + "Source/Processed";

        const string headers = "name,country_name,country_code,latitude,longitude";

        City[] unProcessedCities;
        using (var writer = new StreamReader(root + "unProcessedCities.csv"))
        using (var csv = new CsvReader(writer, CultureInfo.InvariantCulture))
        {
            unProcessedCities = csv.GetRecords<City>().ToArray();
        }

        var sw = new StreamWriter(root + "citiesLp1.csv");
        var notSupportedWriter = new StreamWriter(root + "UpsupportedCitiesLp1.csv");
        notSupportedWriter.Write("name,country_name,country_code,reasn");
        await sw.WriteLineAsync(headers);
        Stopwatch wringingInParable = new Stopwatch();
        wringingInParable.Start();

        unProcessedCities.AsParallel().ForAll(city =>
        {
            try
            {
                var obj = new dao();
                var cities = obj.GetCityData(city.Name, city.CountryCode);
                var cityData = cities[0];

                string row =
                $"{city.Name},{city.CountryName},{city.CountryCode},{cityData.lat},{cityData.lon}";
                //Console.WriteLine(headers);
                //Console.WriteLine(row);
                lock (key)
                {
                    sw.WriteLine(row);
                }
            }
            catch (Exception e)
            {
                string row = $"{city.Name},{city.CountryName},{city.CountryCode}";
                string reasn;
                reasn = e.Message.Equals("Index was outside the bounds of the array.") ? "no data" : e.Message;
                lock (key)
                {
                    notSupportedWriter.WriteLine(row + "," + reasn);
                }
                Console.WriteLine(e);
                //throw;
            }
        });
        wringingInParable.Stop();
        //File.WriteAllText(@"C:\Users\indri\source\repos\KN_ConsoleTest\KN_ConsoleTest\Cities\log.txt", "To write in parallel took " + wringingInParable.Elapsed.TotalMilliseconds + "ms");
        sw.Close();
        notSupportedWriter.Close();
    }



    /// <summary>
    /// 
    /// DOC https://nominatim.org/release-docs/latest/api/Search/
    /// </summary>
    /// <param name="city">the city name</param>
    /// <param name="countryCodes">Limit search results to one or more countries.
    ///     countrycode must be the ISO 3166-1alpha2 code, e.g. gb for the United Kingdom, de for Germany.</param>
    /// <returns></returns>
    public static async Task<CityApiModel?[]> GetCityDataAsync(string city, string countryCodes)
    {
        var client = new HttpClient();

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
        client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
        var streamTask =
            client.GetStreamAsync(
                $"https://nominatim.openstreetmap.org/search.php?city={city}&limit=1&countrycodes={countryCodes}&format=jsonv2");
        var cities = await JsonSerializer.DeserializeAsync<CityApiModel[]>(await streamTask);
        return cities;
    }

    class dao
    {

        /// <summary>
        /// 
        /// DOC https://nominatim.org/release-docs/latest/api/Search/
        /// </summary>
        /// <param name="city">the city name</param>
        /// <param name="countryCodes">Limit search results to one or more countries.
        ///     countrycode must be the ISO 3166-1alpha2 code, e.g. gb for the United Kingdom, de for Germany.</param>
        /// <returns></returns>
        public CityApiModel[]? GetCityData(string city, string countryCodes)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
            var streamTask =
                client.GetStreamAsync(
                    $"https://nominatim.openstreetmap.org/search.php?city={city}&limit=1&countrycodes={countryCodes}&format=jsonv2");
            streamTask.Wait();
            var cities = JsonSerializer.Deserialize<CityApiModel[]>(streamTask.Result);

            return cities;
        }
    }
    public class city
    {
        [Name("name")]
        public string Name { get; set; }
        [Name("country_code")]
        public string CountryCode { get; set; }
        [Name("country_name")]
        public string CountryName { get; set; }
    };
}