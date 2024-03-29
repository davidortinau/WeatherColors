﻿//using MvvmHelpers;
using AsyncAwaitBestPractices;
using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherApp.Models;
using WeatherApp.Services;
using WeatherColors;

namespace Weather.MobileCore.ViewModel
{
    public class MultiWeatherViewModel : MvvmHelpers.BaseViewModel
    {
        private WeatherForecastRoot _forecast;
        private ICommand _reloadCommand;
        private ICommand _openFlyoutCommand;

        public MultiWeatherViewModel() : base()
        {
            GetGroupedWeatherAsync().SafeFireAndForget();
        }

        public WeatherForecastRoot Forecast
        {
            get => _forecast;
            set
            {
                _forecast = value;
                OnPropertyChanged();
            }
        }

        public ICommand ReloadCommand =>
            _reloadCommand ??
            (_reloadCommand = new AsyncCommand(GetGroupedWeatherAsync));

        public ICommand OpenFlyoutCommand =>
            _openFlyoutCommand ??
            (_openFlyoutCommand = new Command(() => OpenFlyout()));

        public ICommand ContinentSelectedCommand =>
            _continentSelectedCommand ??
            (_continentSelectedCommand = new Command<Continent>((e) => SetCities(e)));

        public ICommand GoToContinentCommand =>
            _gotoContinentCommand ??
            (_gotoContinentCommand = new Command<Continent>(async (e) => await GoToContinentAsync(e)));

        private void OpenFlyout()
        {
            Shell.Current.FlyoutIsPresented = true;
        }

        public ObservableRangeCollection<City> Cities { get; } = new ObservableRangeCollection<City>();

        private ICommand _continentSelectedCommand;
        private ICommand _gotoContinentCommand;

        public ObservableRangeCollection<Continent> Continents { get; } = new ObservableRangeCollection<Continent>();

        private async Task<List<City>> GetWeatherForCitiesAsync(IEnumerable<string> cities)
        {
            var units = Units.Imperial;
            var payload = await WeatherService.Instance.GetWeatherAsync(cities, units).ConfigureAwait(false);

            return payload.CityList;
        }

        public async Task GetGroupedWeatherAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var euCitiesTask = GetWeatherForCitiesAsync(WeatherService.EUROPE_CITIES.Take(20));
                var naCitiesTask = GetWeatherForCitiesAsync(WeatherService.NORTH_AMERICA_CITIES.Take(20));
                var saCitiesTask = GetWeatherForCitiesAsync(WeatherService.SOUTH_AMERICA_CITIES.Take(20));
                var afCitiesTask = GetWeatherForCitiesAsync(WeatherService.AFRICA_CITIES.Take(20));
                var asCitiesTask = GetWeatherForCitiesAsync(WeatherService.ASIA_CITIES.Take(20));
                var auCitiesTask = GetWeatherForCitiesAsync(WeatherService.AUSTRALIA_CITIES.Take(20));


                var getWeatherResults = await Task.WhenAll(euCitiesTask,
                                                            naCitiesTask,
                                                            saCitiesTask,
                                                            afCitiesTask,
                                                            asCitiesTask,
                                                            auCitiesTask).ConfigureAwait(false);

                var europeWeather = new Continent(
                                        name: "Europe",
                                        cities: await euCitiesTask.ConfigureAwait(false));
                var northAmericaWeather = new Continent(
                                        name: "North America",
                                        cities: await naCitiesTask.ConfigureAwait(false));

                var southAmericaWeather = new Continent(
                                        name: "South America",
                                        cities: await saCitiesTask.ConfigureAwait(false));

                var africaWeather = new Continent(
                                        name: "Africa",
                                        cities: await afCitiesTask.ConfigureAwait(false));

                var australiaWeather = new Continent(
                                        name: "Australia",
                                        cities: await auCitiesTask.ConfigureAwait(false));

                var asiaWeather = new Continent(
                                        name: "Asia",
                                        cities: await asCitiesTask.ConfigureAwait(false));


                var weatherList = new[]
                {
                    europeWeather,
                    northAmericaWeather,
                    southAmericaWeather,
                    africaWeather,
                    australiaWeather,
                    asiaWeather
                };

                App.Current.Dispatcher.Dispatch(() =>
                {
                    Continents.Clear();
                    Continents.AddRange(weatherList);
                    SetCities(weatherList.First());
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

        private IEnumerable<City> GetCitiesIn(IEnumerable<City> cities, List<string> cityIds)
        {
            return cities.Where(x => cityIds.Contains(x.Id.ToString()));
        }

        private void SetCities(Continent e)
        {
            Cities.Clear();
            Cities.AddRange(e.ToArray());
            //foreach(var c in e.Cities)
            //{
            //    Cities.Add(c);
            //}
        }

        private async Task GoToContinentAsync(Continent c)
        {
            await Shell.Current.GoToAsync($"continent?continent={c.Name}");
        }
    }
}