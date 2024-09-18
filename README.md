# Cities with country_name,country_code,latitude,longitude

Here is a list of all world-wide cities including:
- Country name
- Country Code (ALPHA-2 CODE)
- Latitude
- Longitude
### [wold-wide cities csv](Cities_Latitude_Longitude/Source/Processed/cities.csv)
We have used [this API](https://nominatim.openstreetmap.org/) to get 
latitude and longitude.

## What this application does?
Sends Request to an API and gets the latitude and longitude for that city. 

[GetCities class](Cities_Latitude_Longitude/GetCities.cs)

The application is configured each time you run it, it will start sending request to the server
on parallel and save them into [CitiesLp1](Cities_Latitude_Longitude/Source/Processed/citiesLp1.csv), 
and all unsupported cities over [unsupportedCitiesLp1](Cities_Latitude_Longitude/Source/Processed/UpsupportedCitiesLp1.csv)

# To get all unprocessed cities please run Unprocessed cities assembly
### [UpdateUnprocessedCitiesTests.cs](CitiesTest/Cities/UpdateUnprocessedCitiesTests.cs)
1. MargeProcessedCities()
2. GettingAllProcessedCities()


### MargeProcessedCities()
This will marge all processed cities and unprocessed cities, basically will prepare the data for next phase
### GettingAllProcessedCities()
This will save all unprocessed cities into [UnProcessedCities.csv](Cities_Latitude_Longitude/Source/Processed/UnProcessedCities.csv)

## Starting the application
The application starts at [Cities_Latitude_longitude](Cities_Latitude_Longitude/GetCities.cs) assembly


### Note
> This app wil send a lot of request to the server, please make sure that you send request only for cities you need.
Modify [unProcessedCities](Cities_Latitude_Longitude/Source/Processed/UnProcessedCities.csv)
