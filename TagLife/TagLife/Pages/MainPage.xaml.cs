using System;
using Plugin.Geolocator;
using TagLife.Controls;
using TagLife.Extensions;
using TagLife.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace TagLife.Pages
{
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel _mainPageViewModel;

        public MainPage()
        {
            // sorry no binding pushpins to map control in forms - we'll do it later with workaround
            _mainPageViewModel = new MainPageViewModel();
            BindingContext = _mainPageViewModel;
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await _mainPageViewModel.StartTracking();
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();

            await _mainPageViewModel.StopTracking();
        }
    }
}
