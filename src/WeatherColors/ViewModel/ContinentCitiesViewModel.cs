using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models;
using WeatherApp.Services;

namespace WeatherColors.ViewModel
{
    [QueryProperty("Continent", "continent")]
    [QueryProperty("CityIDs", "cities")]
    public class ContinentCitiesViewModel : MvvmHelpers.BaseViewModel
    {
        public string Continent { get; set; }

        public string CityIDs { get; set; }
        public ObservableRangeCollection<City> Cities { get; } = new ObservableRangeCollection<City>();
        private async Task<List<City>> GetWeatherForCitiesAsync(IEnumerable<string> cities)
        {
            var units = Units.Imperial;
            var payload = await WeatherService.Instance.GetWeatherAsync(cities, units).ConfigureAwait(false);

            return payload.CityList;
        }
    }
}
