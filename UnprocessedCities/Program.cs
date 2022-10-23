using UnprocessedCities;


var updateUnprocessedCities = new UpdateUnprocessedCities();
Console.WriteLine("Margin all cities | Preparing Cities for next phase");

updateUnprocessedCities.MargeProcessedCities();
Console.WriteLine("Getting unprocessed cities, this will take some time");

updateUnprocessedCities.GetUnprocessedCities();