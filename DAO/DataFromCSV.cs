using System.Globalization;
using Cities_Latitude_Longitude;
using Cities_Latitude_Longitude.Model;
using CsvHelper;
namespace DAO
{
    public class DataFromCSV
    {
        public static City[] LoadCities()
        {
            City[] cities;
            string rootPath = Application.GetSolutionRootPath();
            using (var reader = new StreamReader($@"{rootPath}Cities_Latitude_Longitude/Source/cities.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                cities = csv.GetRecords<City>().ToArray();
            }
            return cities;
        }

    }
}
