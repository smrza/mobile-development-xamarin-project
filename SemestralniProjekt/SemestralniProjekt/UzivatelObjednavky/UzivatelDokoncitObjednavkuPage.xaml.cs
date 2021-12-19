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
    public partial class UzivatelDokoncitObjednavkuPage : ContentPage
    {
        private UzivatelDokoncitObjednavkuViewModel ViewModel;
        public UzivatelDokoncitObjednavkuPage(Databaze databaze, UzivatelKosikViewModel kosik, long idUzivatel)
        {
            InitializeComponent();
            ViewModel = new UzivatelDokoncitObjednavkuViewModel(databaze, kosik, idUzivatel, Navigation);
            this.BindingContext = ViewModel;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            ViewModel.DokoncitObjednavku();
        }
    }
}