using SemestralniProjektModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SemestralniProjekt.ViewModel
{
    public class AdminObjednavkyStavViewModel : ViewModelBase
    {
        public INavigation navigace { get; set; }
        public ICommand CommandPotvrzeni { get; set; }
        public ICommand CommandOdeslani { get; set; }
        public ICommand CommandZruseni { get; set; }

        public Databaze databaze;

        public AdminObjednavkyStavViewModel(Databaze databaze, long id, INavigation navigace)
        {
            this.databaze = databaze;
            this.navigace = navigace;
            CommandPotvrzeni = new Command(() => ZmenaStavu(StavObjednavky.POTVRZENA, id));
            CommandOdeslani = new Command(() => ZmenaStavu(StavObjednavky.ODESLANA, id));
            CommandZruseni = new Command(() => ZmenaStavu(StavObjednavky.ZRUSENA, id));
        }

        void ZmenaStavu(StavObjednavky stav, long id)
        {
            switch (stav)
            {
                case StavObjednavky.POTVRZENA:
                    databaze.UpravStavObjednavky(id, stav);
                    break;
                case StavObjednavky.ODESLANA:
                    databaze.UpravStavObjednavky(id, stav);
                    break;
                case StavObjednavky.ZRUSENA:
                    databaze.UpravStavObjednavky(id, stav);
                    break;
            }
            navigace.PopAsync();
        }
    }
}
