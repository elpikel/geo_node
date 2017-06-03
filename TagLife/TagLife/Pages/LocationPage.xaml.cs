using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TagLife.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LocationPage : ContentPage
    {
        public LocationPage(string label)
        {
            InitializeComponent();
            Debug.WriteLine(label);
        }
    }
}