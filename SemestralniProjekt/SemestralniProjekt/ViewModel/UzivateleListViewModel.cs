using SemestralniProjektModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SemestralniProjekt.ViewModel
{
    public class UzivateleListViewModel : ViewModelBase
    {
        /// <summary>
        /// kolekce, ktera je v podstate stejna jako List, jen je v ni implementovan interface INotifyPropertyChanged
        /// V teto aplikci se dostaneme do situace, kdy budeme menit celou referenci ObservableCollection. Abychom tuto zmenu zachytili, tak definujeme vlastni get a set.
        /// </summary>
        private ObservableCollection<Uzivatel> seznamUzivateluObservable;
        public ObservableCollection<Uzivatel> SeznamUzivateluObservable
        {
            get
            {
                return seznamUzivateluObservable;
            }
            set
            {
                if (seznamUzivateluObservable != value)
                {
                    seznamUzivateluObservable = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// command pro refresh seznamu produktu (nyni je to implementovano jako smazani soucasnych produktu a vytvoreni vychoziho stavu)
        /// </summary>
        public ICommand CommandRefreshUzivatelu
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
        private void ObnoveniSeznamuUzivatelu()
        {
            this.SeznamUzivateluObservable.Clear();

            this.NactiUzivateleDoObservableCollection();

            //vyvolame udalost, ze byl refresh dokoncen
            this.ObnoveniDokonceno?.Invoke();
        }

        private Databaze databaze;

        public UzivateleListViewModel(Databaze databaze)
        {
            this.databaze = databaze;

            this.NactiUzivateleDoObservableCollection();

            this.CommandRefreshUzivatelu = new Command(ObnoveniSeznamuUzivatelu);
        }

        private void NactiUzivateleDoObservableCollection()
        {
            List<Uzivatel> uzivatele = databaze.VratVsechnyUzivatele();

            //konverze Listu pres konstruktor
            this.SeznamUzivateluObservable = new ObservableCollection<Uzivatel>(uzivatele);

        }

    }
}
