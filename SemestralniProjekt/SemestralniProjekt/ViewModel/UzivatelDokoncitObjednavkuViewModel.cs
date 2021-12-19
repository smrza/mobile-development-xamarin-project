using SemestralniProjektModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace SemestralniProjekt.ViewModel
{
    public class UzivatelDokoncitObjednavkuViewModel : ViewModelBase
    {
        public INavigation navigace { get; set; }
        public Databaze databaze;
        public long idUzivatel;
        public UzivatelKosikViewModel Kosik;

        public void DokoncitObjednavku()
        {
            databaze.VlozObjednavku(idUzivatel, Kosik.SeznamProduktuObservable.ToList());
            Kosik.SeznamProduktuObservable.Clear();
            navigace.PopAsync();

        }
        public UzivatelDokoncitObjednavkuViewModel(Databaze databaze, UzivatelKosikViewModel kosikViewModel, long idUzivatel, INavigation navigation)
        {
            this.databaze = databaze;
            this.Kosik = kosikViewModel;
            this.idUzivatel = idUzivatel;
            this.navigace = navigation;
            Cena = Kosik.Cena;
            Pocet = Kosik.SeznamProduktuObservable.Count.ToString();
        }


        public string Cena { get; set; }
        public string Pocet { get; set; }
    }
}
