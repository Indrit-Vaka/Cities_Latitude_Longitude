using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cities_Latitude_Longitude.Model;

public class City
{
    [Name("name")]
    public string Name { get; set; }
    [Name("country_code")]
    public string CountryCode { get; set; }
    [Name("country_name")]
    public string CountryName { get; set; }
    [Name("latitude")]
    public double Latitude { get; set; }
    [Name("longitude")]
    public double Longitude { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is City city &&
               Name == city.Name &&
               CountryCode == city.CountryCode &&
               CountryName == city.CountryName;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, CountryCode, CountryName);
    }

    public override string? ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
    }

}
