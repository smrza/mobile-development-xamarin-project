using SemestralniProjektModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SemestralniProjekt.ViewModel
{
    class UzivatelLoggedInTabbedPageViewModel : ViewModelBase
    {
        protected long idUzivatele;

        /// <summary>
        /// kopie instance editovaneho produktu, ktery se meni prostrednictvim UI
        /// </summary>
        public Uzivatel UzivatelPrihlaseny
        {
            get;
            set;
        }

        /// <summary>
        /// ulozena reference na puvodniho produktu, ktereho zmenime jen tehdy, pokud byly potvrzeny zmeny
        /// </summary>
        private Uzivatel uzivatel;

        /// <summary>
        /// interface pro command (nezavisly na Xamarin.Forms)
        /// </summary>
        public ICommand CommandLogIn
        {
            get;
            set;
        }

        protected Databaze databaze;
        private INavigation navigace;

        /// <summary>
        /// konstruktor s inicializací
        /// </summary>
        /// <param name="produkt"></param>
        public UzivatelLoggedInTabbedPageViewModel(Uzivatel uzivatel, Databaze databaze, INavigation navigace)
        {

            this.databaze = databaze;
            this.navigace = navigace;

            //referenci puvodniho produktu ulozime
            this.uzivatel = uzivatel;
            //vytvorime instanci produktu se stejnymi daty
            //(pokud bychom toto neudělali, tak se při každé změně změní i původní data produktu v seznamu. My je ale chceme zmenit az po potvrzeni zmen.)
            this.UzivatelPrihlaseny = new Uzivatel(uzivatel.Id, uzivatel.Jmeno);

            this.CommandLogIn = new Command(LogIn);

        }

        public UzivatelLoggedInTabbedPageViewModel(Databaze databaze)
        {

            this.databaze = databaze;

        }


        protected void LogIn()
        {

            idUzivatele = this.databaze.ReturnUzivatele(this.uzivatel);

            this.uzivatel.Id = idUzivatele;
        }

        
    }
}
