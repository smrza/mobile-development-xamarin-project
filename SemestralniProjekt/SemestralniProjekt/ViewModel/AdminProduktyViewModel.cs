using SemestralniProjektModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SemestralniProjekt.ViewModel
{
    public class AdminProduktyViewModel : ViewModelBase
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

        ///// <summary>
        ///// command pro presmerovani na pridani produktu
        ///// </summary>
        //public ICommand CommandPridaniProduktu
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// command pro smazani produktu
        /// </summary>
        public ICommand CommandSmazaniProduktu
        {
            get;
            set;
        }

        /// <summary>
        /// command pro refresh seznamu produktu (nyni je to implementovano jako smazani soucasnych produktu a vytvoreni vychoziho stavu)
        /// </summary>
        public ICommand CommandRefreshProduktu
        {
            get;
            set;
        }

        /// <summary>
        /// Vlastnost urcujici, zda je tlacitko pro smazani viditelne
        /// </summary>
        private bool jeTlacitkoMazaniViditelne;
        public bool JeTlacitkoMazaniViditelne
        {
            get
            {
                return jeTlacitkoMazaniViditelne;
            }
            set
            {
                //pokud chceme mít sofistikovanější set, tak je mozne pouzit i nasledujici zápis, ktery provede změnu jen, pokud k nějaké došlo
                if (jeTlacitkoMazaniViditelne != value)
                {
                    jeTlacitkoMazaniViditelne = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// obsluzna metoda pro command smazani
        /// </summary>
        /// <param name="item"></param>
        private void SmazaniProduktu(object item)
        {
            if (item is Produkt produkt)
            {
                databaze.SmazProdukt(produkt.Id);
                this.SeznamProduktuObservable.Remove(produkt);
            }
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

        public AdminProduktyViewModel(Databaze databaze)
        {

            this.databaze = databaze;

            this.NactiProduktyDoObservableCollection();

            this.CommandSmazaniProduktu = new Command(SmazaniProduktu);
            this.CommandRefreshProduktu = new Command(ObnoveniSeznamuProduktu);

            JeTlacitkoMazaniViditelne = true;
        }

        private void NactiProduktyDoObservableCollection()
        {
            List<Produkt> produkty = databaze.VratVsechnyProdukty();

            //konverze Listu pres konstruktor
            this.SeznamProduktuObservable = new ObservableCollection<Produkt>(produkty);

            ////konverze pres foreach
            //foreach (Produkt produkt in produkty)
            //{
            //    this.SeznamProduktuObservable.Add(produkt);
            //}

        }

    }
}
