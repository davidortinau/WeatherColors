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
        private WeatherForecastRoot _forecast;
        private ICommand _reloadCommand;
        private ICommand _openFlyoutCommand;
        
        public MultiWeatherViewModel() : base()
        {
            Task.Run(async () =>
            {
                await GetGroupedWeatherAsync().ConfigureAwait(false);
            });
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

        public ICommand ReloadCommand =>
            _reloadCommand ??
            (_reloadCommand = new Command(async () => await GetGroupedWeatherAsync().ConfigureAwait(false)));

        public ICommand OpenFlyoutCommand =>
            _openFlyoutCommand ??
            (_openFlyoutCommand = new Command(() => OpenFlyout()));
        
        public ICommand ContinentSelectedCommand =>
            _continentSelectedCommand ??
            (_continentSelectedCommand = new Command<Continent>((e) => SetCities(e)));

        private void OpenFlyout()
        {
            Shell.Current.FlyoutIsPresented = true;
        }

        ObservableCollection<City> _cities = new ObservableCollection<City>(); 
        public ObservableCollection<City> Cities
        {
            get
            {
                return _cities; }
            set
            {
                _cities = value;
                OnPropertyChanged();
            }
        }

        ObservableCollection<Continent> _continents = new ObservableCollection<Continent>();
        private ICommand _continentSelectedCommand;

        public ObservableCollection<Continent> Continents
        {
            get { return _continents; }
            set
            {
                _continents = value;
                OnPropertyChanged();
            }
        }

        public async Task<ObservableCollection<City>> GetWeatherForCitiesAsync(List<string> cities)
        {
            //if (IsBusy)
            //    return null;

            ObservableCollection<City> c = null;

            IsBusy = true;
            try
            {
                CitiesWeatherRoot payload = null;
                var units = Units.Imperial;
                payload = await WeatherService.Instance.GetWeatherAsync(cities, units).ConfigureAwait(false); 

                c = new ObservableCollection<City>( payload.CityList );
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;    
            }

            return c;
        }

        public async Task GetGroupedWeatherAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                var euCities = GetWeatherForCitiesAsync(WeatherService.EUROPE_CITIES);
                var naCities = GetWeatherForCitiesAsync(WeatherService.NORTH_AMERICA_CITIES);
                var saCities = GetWeatherForCitiesAsync(WeatherService.SOUTH_AMERICA_CITIES);
                var afCities = GetWeatherForCitiesAsync(WeatherService.AFRICA_CITIES);
                var asCities = GetWeatherForCitiesAsync(WeatherService.ASIA_CITIES);
                var auCities = GetWeatherForCitiesAsync(WeatherService.AUSTRALIA_CITIES);

                List<Task> tasks = new List<Task>
                {
                    euCities,
                    naCities,
                    saCities,
                    afCities,
                    asCities,
                    auCities
                };

                Task.WaitAll(tasks.ToArray());

                Device.BeginInvokeOnMainThread(() =>
                {
                    _continents.Add(
                        new Continent(
                            name: "Europe",
                            cities: euCities.Result
                        )
                    );

                    _continents.Add(
                        new Continent(
                            name: "North America",
                            cities: naCities.Result
                        )
                    );

                    _continents.Add(
                        new Continent(
                            name: "South America",
                            cities: saCities.Result
                        )
                    );

                    _continents.Add(
                        new Continent(
                            name: "Africa",
                            cities: afCities.Result
                        )
                    );

                    _continents.Add(
                        new Continent(
                            name: "Asia",
                            cities: asCities.Result
                        )
                    );

                    _continents.Add(
                        new Continent(
                            name: "Australia",
                            cities: auCities.Result
                        )
                    );

                    //_continents.Add(
                    //    new Continent(
                    //        name: "Antarctica",
                    //        cities: null
                    //    )
                    //);
                    OnPropertyChanged(nameof(Continents));

                    SetCities(_continents[0]);
                });

                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void SetCities(Continent continent)
        {
            _cities.Clear();
            foreach(var c in continent)
            {
                _cities.Add(c);
            }

            OnPropertyChanged(nameof(Cities));
        }

        private List<City> GetCitiesIn(ObservableCollection<City> cities, List<string> cityIds)
        {
            return cities.Where(x => cityIds.Contains(x.Id.ToString())).ToList();
        }
    }
}