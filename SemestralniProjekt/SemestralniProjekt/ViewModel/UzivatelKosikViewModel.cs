using SemestralniProjektModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SemestralniProjekt.ViewModel
{
    public class UzivatelKosikViewModel : ViewModelBase
    {

        public ICommand CommandSmazaniProduktu
        {
            get;
            set;
        }
        private ObservableCollection<Produkt> seznamProduktuObservable;
        public ObservableCollection<Produkt> SeznamProduktuObservable
        {
            get
            {
                return seznamProduktuObservable;
            }
            set
            {
                if (seznamProduktuObservable != value)
                {
                    seznamProduktuObservable = value;
                    OnPropertyChanged();
                }
            }
        }
        public Databaze databaze;
        private Slevy slevy;
        
        public UzivatelKosikViewModel(Databaze databaze)
        {
            this.databaze = databaze;
            DateTime datum = DateTime.Now;

            slevy = TypSlevy.VratTypSlevy(databaze);

            SeznamProduktuObservable = new ObservableCollection<Produkt>();
        }

        public string Cena
        {
            get; set;
        }

        public void SumaObjednavky()
        {
            Cena = Convert.ToInt32(Math.Round(slevy.Sleva(seznamProduktuObservable.ToList()))).ToString();
        }
    }
}

