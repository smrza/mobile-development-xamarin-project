using SemestralniProjekt.ViewModel;
using SemestralniProjektModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SemestralniProjekt.UzivatelObjednavky
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UzivatelObjednavkyPage : ContentPage
    {
        Databaze databaze;

        public UzivatelObjednavkyViewModel ViewModel { get; set; }
        public UzivatelObjednavkyPage(Databaze databaze)
        {
            this.databaze = databaze;
            InitializeComponent();
            ViewModel = new UzivatelObjednavkyViewModel(databaze, databaze.LoggedIdUzivatele);
            this.BindingContext = ViewModel;
        }

        protected override void OnAppearing()
        {
            ViewModel.NactiProduktyDoObservableCollection(databaze.LoggedIdUzivatele);
        }
    }
}