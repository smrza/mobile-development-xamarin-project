using SemestralniProjektModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SemestralniProjekt.ViewModel
{
    public class AdminObjednavkyViewModel : ViewModelBase
    {
        public Databaze databaze { get; set; }

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
        public AdminObjednavkyViewModel(Databaze databaze)
        {
            SeznamObjednavkaObservable = new ObservableCollection<Objednavka>();
            this.databaze = databaze;
            this.NactiObjednavky();
        }

        public void NactiObjednavky()
        {
            SeznamObjednavkaObservable.Clear();
            List<Objednavka> objednavky = databaze.VratObjednavkyAdmin();
            SeznamObjednavkaObservable = new ObservableCollection<Objednavka>(objednavky);
        }
    }
}
