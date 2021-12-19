using SemestralniProjekt.UzivatelObjednavky;
using SemestralniProjekt.ViewModel;
using SemestralniProjektModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SemestralniProjekt.UzivatelKosik
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UzivatelKosikPage : ContentPage
    {
        private UzivatelKosikViewModel ViewModel;
        //private UzivatelLoggedInTabbedPageViewModel ViewModelUzivatel;

        private Databaze databaze;

        public UzivatelKosikPage(Databaze databaze, UzivatelKosikViewModel kosik)
        {
            ViewModel = kosik;
            this.BindingContext = kosik;
            this.databaze = databaze;
            //ViewModel.ObnoveniDokonceno += this.KonecObnoveni;

            //this.databaze = databaze;

            InitializeComponent();
        }

        protected void PridaniProduktuDoKosiku_Clicked(object sender, EventArgs args)
        {
            //AdminProduktyPridatEditovatPage adminProduktyPridatEditovatPage = new AdminProduktyPridatEditovatPage(databaze, new Produkt());
            //adminProduktyPridatEditovatPage.ProvedenoVlozeniProduktuDoDatabaze += AdminProduktyPridatEditovatPage_ProvedenoVlozeniProduktuDoDatabaze;
            //this.Navigation.PushAsync(adminProduktyPridatEditovatPage);
        }

        private void AdminProduktyPridatEditovatPage_ProvedenoVlozeniProduktuDoDatabaze(Produkt produkt)
        {
            //pridame zaslany prvek do seznamu produktu
            //this.ViewModel.SeznamProduktuObservable.Add(produkt);

            ////takto je mozne zavolat command s refreshem, ale zbytecne by se obnovovalo vsechno a vykonostne by to trvalo dele
            //if (this.ViewModel.CommandRefreshProduktu.CanExecute(this))
            //    this.ViewModel.CommandRefreshProduktu.Execute(this);
        }

        protected void KonecObnoveni()
        {
            //nastavi stav obnovovani ListView na false - bud primo, nebo pomoci metody EndRefresh (oboji dela to same)
            //listViewSeznamProduktu.IsRefreshing = false;
            listViewSeznamProduktu.EndRefresh();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

            if (ViewModel.SeznamProduktuObservable.Count > 0)
            {
                UzivatelDokoncitObjednavkuPage finish = new UzivatelDokoncitObjednavkuPage(ViewModel.databaze, ViewModel, databaze.LoggedIdUzivatele);
                Navigation.PushAsync(finish);

            }
        }

        protected override void OnAppearing()
        {

        }
        
    }
}