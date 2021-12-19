using SemestralniProjekt.ViewModel;
using SemestralniProjektModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SemestralniProjekt.Uzivatele
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UzivatelPridatPage : ContentPage
    {
        UzivatelPridatViewModel ViewModel;

        public event Action<Uzivatel> ProvedenoVlozeniUzivateleDoDatabaze;

        public UzivatelPridatPage(Databaze databaze, Uzivatel uzivatel)
        {
            this.ViewModel = new UzivatelPridatViewModel(uzivatel, databaze, this.Navigation);
            this.BindingContext = this.ViewModel;
            this.ViewModel.ProvedenoVlozeniUzivateleDoDatabaze += ViewModel_ProvedenoVlozeniUzivateleDoDatabaze;
            InitializeComponent();
        }

        private void ViewModel_ProvedenoVlozeniUzivateleDoDatabaze(Uzivatel uzivatel)
        {
            this.ProvedenoVlozeniUzivateleDoDatabaze?.Invoke(uzivatel);
        }
    }
}