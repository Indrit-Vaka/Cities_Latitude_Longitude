using System.Diagnostics;
using System.Globalization;
using Cities_Latitude_Longitude;
using Cities_Latitude_Longitude.Model;


namespace CitiesTest.Cities;

[TestClass]
public class UpdateUnprocessedCitiesTests
{
    private const string ProcessedCitiesPath = @"C:\Users\indri\source\repos\KN_Console\KN_Console\Cities\cities.csv";
    private const string Root = @"C:\Users\indri\source\repos\KN_Console\KN_Console\Cities\";

    private const string UnsupportedCitiesPath =
        @"C:\Users\indri\source\repos\KN_Console\KN_Console\Cities\notSupported_cities.csv";

    private const string Headers = "name,country_name,country_code,latitude,longitude";


    [TestMethod]
    public void MargeProcessedCities()
    {
        const string processedCitiesLp1 = Root + "citiesLp1.csv";
        //Removing headers and last el which may be uncompleted
        var supportedCities = File.ReadAllLines(processedCitiesLp1).Skip(1);//.Take(processedCitiesLp1.Length - 2);
        //Adding all lines to processed Cities
        File.AppendAllLines(ProcessedCitiesPath, supportedCities);

        const string unsupportedCitiesLp1 = Root + "UpsupportedCitiesLp1.csv";
        var unsupportedCities = File.ReadAllLines(unsupportedCitiesLp1).Skip(1);//.Take(unsupportedCitiesLp1.Length - 2);

        File.AppendAllLines(UnsupportedCitiesPath, unsupportedCities);

        //------------Getting processed cities
        GettingAllProcessedCities();
        CleaningUpFiles(processedCitiesLp1, unsupportedCitiesLp1);
    }

    private static void GettingAllProcessedCities()
    {
        Console.WriteLine("Extracting all processed cities");
        string[] supportedCities = File.ReadAllLines(Root + "cities.csv");
        string[] unSupportedProcessed = File.ReadAllLines(Root + "notSupported_cities.csv");
        List<string> processedCitiesAsText = new()
        {
            "name,country_name,country_code"
        };
        //skipping headers, st1 row
        processedCitiesAsText.AddRange(supportedCities.Skip(1));
        processedCitiesAsText.AddRange(unSupportedProcessed.Skip(1));
        //----------Getting processed cities

        //var top10 = citiesCsv.Take(10);
        File.WriteAllLines(Root + "allProcessedCities.csv", processedCitiesAsText);
    }


    private static void CleaningUpFiles(string processedCitiesLp1, string unsupportedCitiessLp1)
    {
        File.WriteAllText(unsupportedCitiessLp1, "");
        File.WriteAllText(processedCitiesLp1, "");
    }

    [TestMethod]
    public void GetUnprocessedCities()
    {
        Console.WriteLine("Loading cities");
        City[] citiesCsv = DataFromCSV.LoadCities();

        Console.WriteLine("getting object out of allProcessedCities.csv");
        var processedCities = GetProcessedCitiesAsCity();

        Stopwatch gettingUnprocessedCities = new Stopwatch();
        gettingUnprocessedCities.Start();
        Console.WriteLine("Loading...");
        var unFinished = citiesCsv.AsParallel().Where(city => !processedCities.AsParallel().Contains(city)).ToList();
        gettingUnprocessedCities.Stop();
        File.WriteAllText(ProcessedCitiesPath + "logUnprocessed.txt",
            "gettingUnprocessedCities took: " + gettingUnprocessedCities.Elapsed.TotalMilliseconds + "ms");


        Console.WriteLine("Saving unfinished cities");

        using (var writer = new StreamWriter(Root + "UnProcessedCities.csv"))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(unFinished);
        }

        Console.WriteLine("all unprocessed cities where saved successfully");
    }

    private static City[] GetProcessedCitiesAsCity()
    {
        City[] processedCities;
        using (var reader = new StreamReader(Root + "allProcessedCities.csv"))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            processedCities =
                csv.GetRecords<GetCities.city>()
                    .Select(city => new City
                    {
                        Name = city.Name,
                        CountryCode = city.CountryCode,
                        CountryName = city.CountryName
                    }).ToArray();
        }

        ;
        return processedCities;
    }
}