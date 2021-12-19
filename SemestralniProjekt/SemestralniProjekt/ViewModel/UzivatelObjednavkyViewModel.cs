using SemestralniProjektModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SemestralniProjekt.ViewModel
{
    public class UzivatelObjednavkyViewModel : ViewModelBase
    {
        public Databaze databaze;

        private ObservableCollection<Objednavka> seznamObjednavekObservable;
        public ObservableCollection<Objednavka> SeznamObjednavkaObservable
        {
            get
            {
                return seznamObjednavekObservable;
            }
            set
            {
                if (seznamObjednavekObservable != value)
                {
                    seznamObjednavekObservable = value;
                    OnPropertyChanged();
                }
            }
        }
        public UzivatelObjednavkyViewModel(Databaze databaze, long idUzivatel)
        {
            //SeznamObjednavkaObservable.Clear();
            SeznamObjednavkaObservable = new ObservableCollection<Objednavka>();
            this.databaze = databaze;
            NactiProduktyDoObservableCollection(idUzivatel);

        }
        public void NactiProduktyDoObservableCollection(long idUzivatel)
        {

            List<Objednavka> objednavky = databaze.VratObjednavkyUzivatel(idUzivatel);
            SeznamObjednavkaObservable = new ObservableCollection<Objednavka>(objednavky);

        }
    }
}
