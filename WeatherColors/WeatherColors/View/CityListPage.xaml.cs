using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.MobileCore.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Weather.MobileCore
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CityListPage : ContentPage
    {
        public CityListPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _ =(BindingContext as MultiWeatherViewModel).GetGroupedWeatherAsync();
        }
    }
}