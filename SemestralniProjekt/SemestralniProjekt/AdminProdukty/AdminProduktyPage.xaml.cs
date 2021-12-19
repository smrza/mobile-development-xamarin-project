using SemestralniProjekt.ViewModel;
using SemestralniProjektModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SemestralniProjekt.AdminProdukty
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminProduktyPage : ContentPage
    {
        private AdminProduktyViewModel ViewModel;

        private Databaze databaze;

        public AdminProduktyPage(Databaze databaze)
        {
            ViewModel = new AdminProduktyViewModel(databaze);
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
                if (listView.SelectedItem is Produkt produkt)
                {
                    //dojde k presmerovani na stranku s editaci produktu
                    this.Navigation.PushAsync(new AdminProduktyPridatEditovatPage(databaze, produkt));

                    //zrusime oznaceni polozky
                    listView.SelectedItem = null;
                }
            }
        }
        protected void MenuItemUprav_Clicked(object sender, EventArgs args)
        {
            //pokud je prvek MenuItem
            if (sender is MenuItem menuItemEdit)
            {
                //prevedeme CommandParameter na Produkt - bude to ten prvek, nad kterym jsme vyvolali kontextove menu
                if (menuItemEdit.CommandParameter is Produkt produkt)
                {
                    //dojde k presmerovani na stranku s editaci produktu
                    this.Navigation.PushAsync(new AdminProduktyPridatEditovatPage(databaze, produkt));
                }
            }
        }

        protected void PridaniProduktu_Clicked(object sender, EventArgs args)
        {
            AdminProduktyPridatEditovatPage adminProduktyPridatEditovatPage = new AdminProduktyPridatEditovatPage(databaze, new Produkt());
            adminProduktyPridatEditovatPage.ProvedenoVlozeniProduktuDoDatabaze += AdminProduktyPridatEditovatPage_ProvedenoVlozeniProduktuDoDatabaze;
            this.Navigation.PushAsync(adminProduktyPridatEditovatPage);
        }

        private void AdminProduktyPridatEditovatPage_ProvedenoVlozeniProduktuDoDatabaze(Produkt produkt)
        {
            //pridame zaslany prvek do seznamu produktu
            this.ViewModel.SeznamProduktuObservable.Add(produkt);

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
    }
}