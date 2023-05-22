namespace KinopoiskBot;

using System.Net;
using System.Configuration;
using Newtonsoft.Json;

public class KinopoiskApi
{
    private string GetApiResponse(string film)
    {
        var headers = new WebHeaderCollection
        {
            { "accept", "application/json" },
            { "X-API-KEY", ConfigurationManager.AppSettings["KINOPOISK_TOKEN"] }
        };

        var url = $"https://api.kinopoisk.dev/v1.2/movie/search?page=1&limit=10&query={film}";
        var client = new WebClient { Headers = headers };
        var response = client.DownloadString(url);
        return response;
    }

    public (dynamic? name, dynamic? description) Search(string film)
    {
        var response = GetApiResponse(film);
        var file = JsonConvert.DeserializeObject<dynamic>(response);

        using (var client = new WebClient())
        {
            var imgUrl = file?["docs"][0]["poster"].ToString();
            var imgData = client.DownloadData(imgUrl);
            File.WriteAllBytes("img.jpg", imgData);
        }

        var name = file?["docs"][0]["name"].ToString();
        var description = file?["docs"][0]["description"].ToString();
        return (name, description);
    }

    public float GetFilmRating(string film)
    {
        var response = GetApiResponse(film);
        var file = JsonConvert.DeserializeObject<dynamic>(response);
        return float.Parse(file?["docs"][0]["rating"].ToString());
    }

    public int GetFilmYear(string film)
    {
        var response = GetApiResponse(film);
        var file = JsonConvert.DeserializeObject<dynamic>(response);
        return int.Parse(file?["docs"][0]["year"].ToString());
    }

    public string GetFilmCountries(string film)
    {
        var response = GetApiResponse(film);
        var file = JsonConvert.DeserializeObject<dynamic>(response);
        var countries = string.Join(", ", file?["docs"][0]["countries"]);
        return countries;
    }

    public string GetFilmGenres(string film)
    {
        var response = GetApiResponse(film);
        var file = JsonConvert.DeserializeObject<dynamic>(response);
        var genres = string.Join(", ", file?["docs"][0]["genres"]);
        return genres;
    }
}
