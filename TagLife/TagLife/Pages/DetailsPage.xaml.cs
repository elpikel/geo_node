using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLife.Controls;
using TagLife.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TagLife.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailsPage : ContentPage
    {
        private DetailsViewModel _detailsViewModel;

        public DetailsPage(CustomPin pin)
        {
            InitializeComponent();

            _detailsViewModel = new DetailsViewModel(pin);

            BindingContext = _detailsViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await _detailsViewModel.LoadData();
        }
    }
}