using Weather.MobileCore.ViewModel;
using WeatherApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherColors.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CityListPage : ContentPage
    {
        public CityListPage()
        {
            InitializeComponent();
        }
        
        public void OnPositionChanged(object sender, PositionChangedEventArgs args)
        {
            
            IndicatorView.SelectionChanged -= IndicatorSelectionChanged;
            IndicatorView.SelectedItem = (BindingContext as MultiWeatherViewModel).Cities[args.CurrentPosition];
            IndicatorView.SelectionChanged += IndicatorSelectionChanged;
            
        }

        public void IndicatorSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            CitiesCarousel.PositionChanged -= OnPositionChanged;
            var pos = (BindingContext as MultiWeatherViewModel).Cities.IndexOf(args.CurrentSelection[0] as City);
            CitiesCarousel.ScrollTo(pos, animate: false);
            CitiesCarousel.PositionChanged += OnPositionChanged;
        }

        private bool skipPositioning = false;
    }
}