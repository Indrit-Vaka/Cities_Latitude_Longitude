using System.Globalization;
using Cities_Latitude_Longitude.Model;

namespace CitiesTest.Cities;

[TestClass]
public class RemoveDistinctTests
{
    [TestMethod()]
    public void RemoveDistinctCities()
    {
        Console.WriteLine("Removing destinct");
        //TODO please replace the root path with your project location
        string projectRoot = Path.GetDirectoryName(typeof(RemoveDistinctTests).Assembly.Location);
        string root = @"C:\Users\indri\source\repos\KN_Console\KN_Console\";
        string path = @$"{root}Cities\cities.csv";
        var data = File.ReadAllLines(path);
        Console.WriteLine("Proccessing data");
        var distinct = data.Distinct();
        File.WriteAllLines(path, distinct);
        Console.WriteLine("removing dublicates at citiel-atitude");
        string[] cities = File.ReadAllLines(root + "source/cities-latitude.csv");
        File.WriteAllLines(root + "source/distinct-cities-latitude.csv", cities.Distinct());

        var cities1
            = DataFromCSV.LoadCities().Distinct();
        using (var writer = new StreamWriter(@"C:\Users\indri\source\repos\KN_Console\KN_Console\Cities\cities-latitude.csv"))
        using (CsvWriter csvWriter = new(writer, CultureInfo.InvariantCulture))
        {
            csvWriter.WriteHeader(typeof(City));
            csvWriter.WriteRecords(cities1);
        }
        
    }
}
