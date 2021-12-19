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
    public partial class AdminProduktyPridatEditovatPage : ContentPage
    {
        AdminProduktyPridatEditovatViewModel ViewModel;

        public event Action<Produkt> ProvedenoVlozeniProduktuDoDatabaze;

        public AdminProduktyPridatEditovatPage(Databaze databaze, Produkt produkt)
        {
            this.ViewModel = new AdminProduktyPridatEditovatViewModel(produkt, databaze, this.Navigation);
            this.BindingContext = this.ViewModel;
            this.ViewModel.ProvedenoVlozeniProduktuDoDatabaze += ViewModel_ProvedenoVlozeniProduktuDoDatabaze;
            InitializeComponent();
        }

        private void ViewModel_ProvedenoVlozeniProduktuDoDatabaze(Produkt produkt)
        {
            //událost pošleme dále do nadrazeneho Page
            this.ProvedenoVlozeniProduktuDoDatabaze?.Invoke(produkt);
        }
    }
}