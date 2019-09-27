using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherApp.Models;
using WeatherApp.Services;
using Xamarin.Forms;

namespace Weather.MobileCore.ViewModel
{
    public class MultiWeatherViewModel : BaseViewModel
    {
        private int _temp = 73;
        private string _condition;
        private WeatherForecastRoot _forecast;
        private ICommand _reloadCommand;
        private ICommand _openFlyoutCommand;
        private ICommand _navToOtherPage;


        public int Temp
        {
            get { return _temp; }
            set
            {
                _temp = value;
                OnPropertyChanged();
            }
        }

        public string Condition
        {
            get { return _condition; }
            set
            {
                _condition = value;
                OnPropertyChanged();
            }
        }

        public WeatherForecastRoot Forecast
        {
            get { return _forecast; }
            set
            {
                _forecast = value;
                OnPropertyChanged();
            }
        }

        bool useCelsius;
        public bool UseCelsius
        {
            get => useCelsius;
            set
            {
                if (SetProperty(ref useCelsius, value))
                {
                    //BackgroundColorConverter.UseCelcius = UseCelsius;
                    OnPropertyChanged(nameof(Temp));
                }
            }
        }

        string location = "St. Louis";
        public string Location
        {
            get => location;
            set
            {
                if (SetProperty(ref location, value))
                {
                    _ = GetWeatherAsync();
                }
            }
        }


        public ICommand ReloadCommand =>
            _reloadCommand ??
            (_reloadCommand = new Command(async () => await GetWeatherAsync()));

        public ICommand OpenFlyoutCommand =>
            _openFlyoutCommand ??
            (_openFlyoutCommand = new Command(() => OpenFlyout()));

        public ICommand NavigateToOtherPage =>
            _navToOtherPage ??
            (_navToOtherPage = new Command(() => GoToOther()));

        private void GoToOther()
        {
            Shell.Current.GoToAsync("//anything");
        }

        private void OpenFlyout()
        {
            Shell.Current.FlyoutIsPresented = true;
        }

        ObservableCollection<City> _cities; 
        public ObservableCollection<City> Cities
        {
            get { return _cities; }
            set
            {
                _cities = value;
                OnPropertyChanged();
            }
        }

        List<Continent> _continents;
        
        public List<Continent> Continents
        {
            get { return _continents; }
            set
            {
                _continents = value;
                OnPropertyChanged();
            }
        }

        public string CurrentConditionsIcon { get; set; }

        public async Task GetWeatherAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                WeatherRoot weatherRoot = null;
                var units = useCelsius ? Units.Metric : Units.Imperial;
                weatherRoot = await WeatherService.Instance.GetWeatherAsync(location.Trim(), units);
                //Forecast = await WeatherService.Instance.GetForecast(weatherRoot.CityId, units);
                var unit = useCelsius ? "C" : "F";
                //Temp = $"{weatherRoot?.MainWeather?.Temperature ?? 0}°{unit}";
                Temp = Convert.ToInt32(weatherRoot?.MainWeather?.Temperature);
                Condition = $"{weatherRoot.Name}: {weatherRoot?.Weather?[0]?.Description ?? string.Empty}";
                CurrentConditionsIcon = weatherRoot?.Weather?[0].Icon;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //Temp = "Unable to get Weather";
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task GetFlatWeatherAsync(List<string> cities)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                CitiesWeatherRoot payload = null;
                var units = useCelsius ? Units.Metric : Units.Imperial;
                payload = await WeatherService.Instance.GetWeatherAsync(cities, units); 

                Cities = new ObservableCollection<City>( payload.CityList );
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //Temp = "Unable to get Weather";
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task GetGroupedWeatherAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                List<string> allCities = WeatherService.NORTH_AMERICA_CITIES.Concat(WeatherService.SOUTH_AMERICA_CITIES).ToList<string>();
                
                Debug.WriteLine(allCities.Count());
                
                //await GetFlatWeatherAsync(allCities);
                CitiesWeatherRoot payload = null;
                var units = useCelsius ? Units.Metric : Units.Imperial;
                payload = await WeatherService.Instance.GetWeatherAsync(allCities, units);

                _cities = new ObservableCollection<City>( payload.CityList );

                //_continents = new List<Continent>();
                //_continents.Add(
                //    new Continent(name:"North America", cities: GetCitiesIn(_cities, WeatherService.NORTH_AMERICA_CITIES))                    
                //);

                //_continents.Add(
                //    new Continent(name:"South America", cities: GetCitiesIn(_cities, WeatherService.SOUTH_AMERICA_CITIES))
                //);

                //OnPropertyChanged(nameof(Continents));
                OnPropertyChanged(nameof(Cities));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //Temp = "Unable to get Weather";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private List<City> GetCitiesIn(List<City> cities, List<string> cityIds)
        {
            return cities.Where(x => cityIds.Contains(x.Id.ToString())).ToList();
        }
    }
}