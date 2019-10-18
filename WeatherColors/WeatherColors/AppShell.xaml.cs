using System;
using System.Collections.Generic;
using WeatherColors.View;
using Xamarin.Forms;

namespace WeatherColors
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("continent", typeof(ContinentCitiesPage));
        }
    }
}
