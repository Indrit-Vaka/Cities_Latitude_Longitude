using System.Diagnostics;
using System.Globalization;
using Cities_Latitude_Longitude;
using Cities_Latitude_Longitude.Model;
using CsvHelper;
using DAO;

namespace UnprocessedCities;

public class UpdateUnprocessedCities
{
    private string _root;
    private string _processedCitiesPath;
    private string unsupportedCitiesCsv;
    private string processedCitiesLp1;
    private string allProcessedCities;
    string unsupportedCitiesLp1;

    public UpdateUnprocessedCities()
    {
        _root = Application.GetSolutionRootPath() + "Cities_Latitude_Longitude/";
        _processedCitiesPath = @$"{_root}Source/Processed/cities.csv";
        unsupportedCitiesCsv = $@"{_root}Source/Processed/notSupported_cities.csv";
        processedCitiesLp1 = _root + "Source/Processed/citiesLp1.csv";
        unsupportedCitiesLp1 = _root + "Source/Processed/UpsupportedCitiesLp1.csv";
        allProcessedCities = _root + "Source/Processed/allProcessedCities.csv";
    }


    private const string Headers = "name,country_name,country_code,latitude,longitude";


    public void MargeProcessedCities()
    {
        //Removing headers and last el which may be uncompleted
        var supportedCities = File.ReadAllLines(processedCitiesLp1).Skip(1); //.Take(processedCitiesLp1.Length - 2);
        //Adding all lines to processed Cities
        File.AppendAllLines(_processedCitiesPath, supportedCities);

        var unsupportedCities =
            File.ReadAllLines(unsupportedCitiesLp1).Skip(1); //.Take(unsupportedCitiesLp1.Length - 2);

        File.AppendAllLines(unsupportedCitiesCsv, unsupportedCities);

        //------------Getting processed cities
        GettingAllProcessedCities();
        CleaningUpFiles(processedCitiesLp1, unsupportedCitiesLp1);
    }

    private void GettingAllProcessedCities()
    {
        Console.WriteLine("Extracting all processed cities");
        string[] supportedCities = File.ReadAllLines(_processedCitiesPath);
        string[] unSupportedProcessed = File.ReadAllLines( unsupportedCitiesCsv);
        List<string> processedCitiesAsText = new()
        {
            "name,country_name,country_code"
        };
        //skipping headers, st1 row
        processedCitiesAsText.AddRange(supportedCities.Skip(1));
        processedCitiesAsText.AddRange(unSupportedProcessed.Skip(1));
        //----------Getting processed cities

        //var top10 = citiesCsv.Take(10);
        File.WriteAllLines(allProcessedCities, processedCitiesAsText);
    }


    private void CleaningUpFiles(string processedCitiesLp1, string unsupportedCitiessLp1)
    {
        File.WriteAllText(unsupportedCitiessLp1, "");
        File.WriteAllText(processedCitiesLp1, "");
    }


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
        File.WriteAllText(_processedCitiesPath + "logUnprocessed.txt",
            "gettingUnprocessedCities took: " + gettingUnprocessedCities.Elapsed.TotalMilliseconds + "ms");


        Console.WriteLine("Saving unfinished cities");

        using (var writer = new StreamWriter(unsupportedCitiesCsv))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(unFinished);
        }

        Console.WriteLine("all unprocessed cities where saved successfully");
    }

    private City[] GetProcessedCitiesAsCity()
    {
        City[] processedCities;
        using (var reader = new StreamReader(allProcessedCities))
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
        return processedCities;
    }
}