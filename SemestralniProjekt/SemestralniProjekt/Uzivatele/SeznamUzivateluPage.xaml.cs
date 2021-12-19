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
    public partial class SeznamUzivateluPage : ContentPage
    {
        private UzivateleListViewModel ViewModel;

        private Databaze databaze;

        public SeznamUzivateluPage(Databaze databaze)
        {
            ViewModel = new UzivateleListViewModel(databaze);
            this.BindingContext = ViewModel;

            ViewModel.ObnoveniDokonceno += this.KonecObnoveni;

            this.databaze = databaze;

            InitializeComponent();
        }

        protected void ListView_ItemSelected(object sender, EventArgs args)
        {
            if (sender is ListView listView)
            {
                //zjistime, zda ma ListView ten spravny prvek
                if (listView.SelectedItem is Uzivatel uzivatel)
                {
                    //dojde k presmerovani na stranku s editaci produktu

                    databaze.LoggedIdUzivatele = uzivatel.Id;
                    this.Navigation.PushAsync(new UzivatelLoggedInTabbedPage(databaze, uzivatel));

                    //zrusime oznaceni polozky
                    listView.SelectedItem = null;
                }
            }
        }

        //protected void MenuItemUprav_Clicked(object sender, EventArgs args)
        //{
        //    //pokud je prvek MenuItem
        //    if (sender is MenuItem menuItemEdit)
        //    {
        //        //prevedeme CommandParameter na Produkt - bude to ten prvek, nad kterym jsme vyvolali kontextove menu
        //        if (menuItemEdit.CommandParameter is Produkt produkt)
        //        {
        //            //dojde k presmerovani na stranku s editaci produktu
        //            this.Navigation.PushAsync(new AdminProduktyPridatEditovatPage(databaze, produkt));
        //        }
        //    }
        //}

        protected void PridaniUzivatele_Clicked(object sender, EventArgs args)
        {
            UzivatelPridatPage uzivatelPridatPage = new UzivatelPridatPage(databaze, new Uzivatel());
            uzivatelPridatPage.ProvedenoVlozeniUzivateleDoDatabaze += UzivatelPridatPage_ProvedenoVlozeniUzivateleDoDatabaze;
            this.Navigation.PushAsync(uzivatelPridatPage);
        }

        private void UzivatelPridatPage_ProvedenoVlozeniUzivateleDoDatabaze(Uzivatel uzivatel)
        {
            //pridame zaslany prvek do seznamu produktu
            this.ViewModel.SeznamUzivateluObservable.Add(uzivatel);

            ////takto je mozne zavolat command s refreshem, ale zbytecne by se obnovovalo vsechno a vykonostne by to trvalo dele
            //if (this.ViewModel.CommandRefreshProduktu.CanExecute(this))
            //    this.ViewModel.CommandRefreshProduktu.Execute(this);
        }

        protected void KonecObnoveni()
        {
            //nastavi stav obnovovani ListView na false - bud primo, nebo pomoci metody EndRefresh (oboji dela to same)
            //listViewSeznamProduktu.IsRefreshing = false;
            listViewSeznamUzivatelu.EndRefresh();
        }
    }
}