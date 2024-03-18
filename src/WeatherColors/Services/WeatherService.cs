using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using WeatherApp.Models;
using static Newtonsoft.Json.JsonConvert;

namespace WeatherApp.Services
{
    public enum Units
    {
        Imperial,
        Metric
    }

    public class WeatherService
    {
        const string WeatherCoordinatesUri  = "http://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&units={2}&appid=bb870ad9260d463680ecf5ec1f5a05ea";
        const string WeatherCityUri         = "http://api.openweathermap.org/data/2.5/weather?q={0}&units={1}&appid=bb870ad9260d463680ecf5ec1f5a05ea";
        const string ForecaseUri            = "http://api.openweathermap.org/data/2.5/forecast?id={0}&units={1}&appid=bb870ad9260d463680ecf5ec1f5a05ea";
        const string WeatherCitiesUri       = "http://api.openweathermap.org/data/2.5/group?id={0}&units={1}&appid=bb870ad9260d463680ecf5ec1f5a05ea";


        
        public static List<string> NORTH_AMERICA_CITIES = new List<string>() {
            "3530597", // Mexico City
            "5128581", // New York
            "6167865", // Toronto
            "4887398", // Chicago
            "3553478", // Havana
            "6077243", // Montreal
            "5368361", // Los Angeles
            "4699066", // Houston
            "5308655", // Phoenix
            "4684888", // Dallas
            "5391959", // San Francisco
            "5419384", // Denver
            "4990729", // Detroit
            "4560349", // Philadelphia
            "5809844", // Seattle
            "4726206", // San Antonio
            "4164138", // Miami
            "4180439", // Atlanta
            "4509177", // Columbus
            "5392171", // San Diego
            "4259418", // Indianapolis
            "5125771", // Manhattan
            "4335045", // New Orleans
            "5391811", // Sacramento
            "5780993", // Salt Lake City
            "5037649", // Minneapolis
        };
        
        public static List<string> SOUTH_AMERICA_CITIES = new List<string>()
        {
            "3448439", // "Sao Paulo",
            "3936456", // "Lima",
            "3688689", // "Bogota",
            "3451190", // "Rio de Janeiro",
            "1687801", // "Santiago",
            "3646738", // "Caracas",
            "3435910", // "Buenos Aires",
            "3689147", // "Barranquilla",
            "3871336", // "Santiago",
            "3936456", // "Lima",
            "3688689", // "Bogota",
            "3663517", // "Manaus",
            "3382160", // "Cayenne",
            "3380290", // "Sinnamary",
            "3381041", // "Mana",
            "3381538", // "Grand-Santi",
            "3380290", // "Sinnamary",
            "3381428", // "Iracoubo",
            "3382226", // "Camopi",
            "3381303", // "Kourou",
            "3382527", // "Roura",
            "3381041", // "Mana",
            "3381538", // "Grand-Santi",
            "3380290", // "Sinnamary",
            "3381428", // "Iracoubo",
            "3382226", // "Camopi",
            "3381303", // "Kourou",
            "3382527", // "Roura"

        };

        public static List<string> AFRICA_CITIES = new List<string>()
        {
            "360630", // "Cairo",
            "2332459", // "Lagos",
            "2314302", // "Kinshasa-Brazzaville",
            "993800", // "Johannesburg",
            "2240449", // "Luanda",
            "1105776", // "Pretoria",
            "184745", // "Nairobi",
            "2335204", // "Kano",
            "361058", // "Alexandria",
            "3923012", // "Tunis",
            "3383652", // "Tripoli",
            "2220957", // "Yaounde",
            "232422", // "Kampala",
            "2338810", // "Onitsha",
            "2294915", // "Accra",
            "2306104", // "Abidjan",
            "2394819", // "Cotonou",
            "2286304", // "Conakry",
        };

        public static List<string> EUROPE_CITIES = new List<string>()
        {
            "2756429", // "Ede"
            "745044", // "Istanbul",
            "524901", // "Moscow",
            "2643743", // "London",
            "498817", // "Saint Petersburg",
            "2950159", // "Berlin",
            "3169070", // "Rome",
            "3128760", // "Barcelona",
            "3117735", // "Madrid",
            "2988507", // "Paris",
            "3067696", // "Prague",
            "2761369", // "Vienna",
            "2747891", // "Amsterdam",
            "3165524", // "Venice",
            "2673730", // "Stockholm",
            "2643741", // "Liverpool",
            "2633352", // "York",
            "2643123", // "Manchester",
            "2657896", // "Edinburgh",
            "2643741", // "Glasgow",
            "264371",  // "Athens",
            "293397",  // "Tel Aviv",
            "3143244", // "Oslo",
            "2618425", // "Copenhagen",
            "2800866", // "Brussels",
            "3337389", // "Dublin",
        };

        public static List<string> ASIA_CITIES = new List<string>()
        {
            "1273294", // "Delhi"
            "1850147", // "Tokyo",
            "25200", // "Jakarta",
            "1275339", // "Mumbai",
            "1835848", // "Seoul",
            "1609350", // "Bangkok",
            "1796236", // "Shanghai",
            "1816670", // "Beijing",
            "1259229", // "Pune",
            "1275004", // "Kolkata",
            "1668341", // "Taipei",
            "1880252", // "Singapore",
            "1277333", // "Bengaluru",
            "1253405", // "Rajkot",
            "1258847", // "Ranchi",
            "1258980", // "Raipur",
            "1260086", // "Patna",
            "1262180", // "Nagpur",
            "1269743", // "Hyderabad",
            "1270642", // "Gurgaon",
        };

        public static List<string> AUSTRALIA_CITIES = new List<string>()
        {
            "2147714", // "Sydney",
            "2158177", // "Melbourne",
            "2174003", // "Brisbane",
            "2063523", // "Perth",
            "2078025", // "Adelaide",
            "2165087", // "Gold Coast",
            "2073124", // "Darwin",
            "2157698", // "Newcastle",
            "2154219", // "Orange",
            "2152286", // "Port Macquarie",
            "2151437", // "Rockhampton",
            "2150660", // "Sunshine Coast",
            "2146142", // "Toowoomba",
            "2147718", // "Townsville",
            "2143821", // "Wollongong",
            "2179670", // "Auckland, NZ",
            "2193733", // "Wellington, NZ",
            "2181133", // "Christchurch, NZ",
            "2208032", // "Hamilton, NZ",
            "2184155", // "Tauranga, NZ",
            "2190324", // "Dunedin, NZ",
            "2181742", // "Palmerston North, NZ",
            "2188164", // "Napier, NZ",
            "2180548", // "Rotorua, NZ",
            "2192362", // "Whangarei, NZ"
        };

        public static List<string> ANTARCTICA_CITIES = new List<string>()
        {
        };

        private static WeatherService _instance;

        public static WeatherService Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new WeatherService();

                return _instance;
            }
        }

        public async Task<WeatherRoot> GetWeatherAsync(double latitude, double longitude, Units units = Units.Imperial)
        {
            using (var client = new HttpClient())
            {
                var url = string.Format(WeatherCoordinatesUri, latitude, longitude, units.ToString().ToLower());
                var json = await client.GetStringAsync(url).ConfigureAwait(false);

                if (string.IsNullOrWhiteSpace(json))
                    return null;

                return DeserializeObject<WeatherRoot>(json);
            }

        }

        public async Task<WeatherRoot> GetWeatherAsync(string city, Units units = Units.Imperial)
        {
            using (var client = new HttpClient())
            {
                var url = string.Format(WeatherCityUri, HttpUtility.UrlEncode(city), units.ToString().ToLower());
                Debug.WriteLine(url);
                var json = await client.GetStringAsync(url).ConfigureAwait(false);

                if (string.IsNullOrWhiteSpace(json))
                    return null;

                return DeserializeObject<WeatherRoot>(json);
            }

        }

        public async Task<WeatherForecastRoot> GetForecast(int id, Units units = Units.Imperial)
        {
            using (var client = new HttpClient())
            {
                var url = string.Format(ForecaseUri, id, units.ToString().ToLower());
                var json = await client.GetStringAsync(url).ConfigureAwait(false);

                if (string.IsNullOrWhiteSpace(json))
                    return null;

                return DeserializeObject<WeatherForecastRoot>(json);
            }

        }

        public async Task<CitiesWeatherRoot> GetWeatherAsync(IEnumerable<string> cities, Units units = Units.Imperial)
        {
            string json = string.Empty;

            using (var client = new HttpClient())
            {
                var url = string.Format(WeatherCitiesUri, string.Join(",",cities), units.ToString().ToLower());
                try
                {
                    json = await client.GetStringAsync(url);
                }catch(Exception ex)
                {
                    Debug.WriteLine($"{ex.Message}");
                }

                if (string.IsNullOrWhiteSpace(json))
                    return null;

                return DeserializeObject<CitiesWeatherRoot>(json);
            }

        }
    }
}