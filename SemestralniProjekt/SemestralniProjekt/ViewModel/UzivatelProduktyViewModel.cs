using SemestralniProjektModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SemestralniProjekt.ViewModel
{
    class UzivatelProduktyViewModel : ViewModelBase
    {
        /// <summary>
        /// kolekce, ktera je v podstate stejna jako List, jen je v ni implementovan interface INotifyPropertyChanged
        /// V teto aplikci se dostaneme do situace, kdy budeme menit celou referenci ObservableCollection. Abychom tuto zmenu zachytili, tak definujeme vlastni get a set.
        /// </summary>
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

        private ObservableCollection<Objednavka> seznamObjednavekObservable { get; set; }

        /// <summary>
        /// command pro refresh seznamu produktu (nyni je to implementovano jako smazani soucasnych produktu a vytvoreni vychoziho stavu)
        /// </summary>
        public ICommand CommandRefreshProduktu
        {
            get;
            set;
        }

        /// <summary>
        /// udalost, indikujici, ze bylo obnoveni ListView dokonceno
        /// </summary>
        public event Action ObnoveniDokonceno;

        /// <summary>
        /// obsluzna metoda pro command obnoveni seznamu produktu
        /// </summary>
        private void ObnoveniSeznamuProduktu()
        {
            this.SeznamProduktuObservable.Clear();

            this.NactiProduktyDoObservableCollection();

            //vyvolame udalost, ze byl refresh dokoncen
            this.ObnoveniDokonceno?.Invoke();
        }

        private Databaze databaze;

        public UzivatelKosikViewModel kosik;

        public UzivatelProduktyViewModel(Databaze databaze, UzivatelKosikViewModel kosik)
        {
            this.databaze = databaze;
            this.kosik = kosik;

            this.NactiProduktyDoObservableCollection();
            this.CommandRefreshProduktu = new Command(ObnoveniSeznamuProduktu);
            this.CommandGetMostSoldItem = new Command(GetMostSoldItem);
        }

        
        private void NactiProduktyDoObservableCollection()
        {
            List<Produkt> produkty = databaze.VratVsechnyProdukty();

            //konverze Listu pres konstruktor
            this.SeznamProduktuObservable = new ObservableCollection<Produkt>(produkty);

        }

        public void NactiProduktyDoObservableCollection2()
        {
            List<Objednavka> objednavky = databaze.VratObjednavkyAdmin();
            seznamObjednavekObservable = new ObservableCollection<Objednavka>(objednavky);
        }

        public ICommand CommandPotvrzeni
        {
            get;
            set;
        }

        public double mostSoldItem;

        public double MostSoldItem
        {
            get
            {
                return mostSoldItem;
            }
            set
            {
                if (mostSoldItem != value)
                {
                    mostSoldItem = value;
                    OnPropertyChanged();
                }
            }
        }

        public void GetMostSoldItem()
        {
            NactiProduktyDoObservableCollection2();

            List<Produkt> vsechnyProdukty = new List<Produkt> { };

            foreach (var item in seznamObjednavekObservable)
            {
                foreach (var item2 in item.Produkty)
                {
                    vsechnyProdukty.Add(item2);
                }
            }

            int tmp = 0;
            int tmp2 = 0;
            long mostSoldItemIdTmp = 0;
            long mostSoldItemIdTmpFinal = 0;

            for (int i = 1; i <= vsechnyProdukty.Count; i++)
            {
                foreach (var item in vsechnyProdukty)
                {

                    if (item.Id == i)
                    {
                        tmp++;
                        mostSoldItemIdTmp = item.Id;
                    }


                }

                if (tmp > tmp2)
                {
                    tmp2 = tmp;
                    mostSoldItemIdTmpFinal = mostSoldItemIdTmp;
                }

                tmp = 0;

            }

            MostSoldItem = mostSoldItemIdTmpFinal;
        }


        public ICommand CommandGetMostSoldItem
        {
            get;
            set;
        }
    }
}
