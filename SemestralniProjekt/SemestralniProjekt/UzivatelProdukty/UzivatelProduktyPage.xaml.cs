using SemestralniProjekt.ViewModel;
using SemestralniProjektModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SemestralniProjekt.UzivatelProdukty
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UzivatelProduktyPage : ContentPage
    {
        private UzivatelProduktyViewModel ViewModel;
        //private UzivatelLoggedInTabbedPageViewModel ViewModelUzivatel;

        private Databaze databaze;

        public UzivatelProduktyPage(Databaze databaze, UzivatelKosikViewModel kosik)
        {
            ViewModel = new UzivatelProduktyViewModel(databaze, kosik);
            //ViewModelUzivatel = new UzivatelLoggedInTabbedPageViewModel(databaze);
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
                    ViewModel.kosik.SeznamProduktuObservable.Add(produkt);
                    ViewModel.kosik.SumaObjednavky();
                    //dojde k presmerovani na stranku s editaci produktu

                    //zrusime oznaceni polozky
                    listView.SelectedItem = null;
                }
            }
        }

        protected void KonecObnoveni()
        {
            //nastavi stav obnovovani ListView na false - bud primo, nebo pomoci metody EndRefresh (oboji dela to same)
            //listViewSeznamProduktu.IsRefreshing = false;
            listViewSeznamProduktu.EndRefresh();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            ViewModel.GetMostSoldItem();
        }
    }
}