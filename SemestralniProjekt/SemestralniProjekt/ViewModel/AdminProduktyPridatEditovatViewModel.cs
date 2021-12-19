using SemestralniProjektModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SemestralniProjekt.ViewModel
{
    public class AdminProduktyPridatEditovatViewModel : ViewModelBase
    {

        public string HlavniLabel
        {
            get;
            set;
        }

        /// <summary>
        /// kopie instance editovaneho produktu, ktery se meni prostrednictvim UI
        /// </summary>
        public Produkt ProduktEditovany
        {
            get;
            set;
        }

        /// <summary>
        /// ulozena reference na puvodniho produktu, ktereho zmenime jen tehdy, pokud byly potvrzeny zmeny
        /// </summary>
        private Produkt produktPuvodni;

        /// <summary>
        /// interface pro command (nezavisly na Xamarin.Forms)
        /// </summary>
        public ICommand CommandPotvrzeni
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
        public AdminProduktyPridatEditovatViewModel(Produkt produkt, Databaze databaze, INavigation navigace)
        {

            this.databaze = databaze;
            this.navigace = navigace;

            //referenci puvodniho produktu ulozime
            this.produktPuvodni = produkt;
            //vytvorime instanci produktu se stejnymi daty
            //(pokud bychom toto neudělali, tak se při každé změně změní i původní data produktu v seznamu. My je ale chceme zmenit az po potvrzeni zmen.)
            this.ProduktEditovany = new Produkt(produkt.Id, produkt.Nazev, produkt.Kategorie, produkt.Cena, produkt.Popis, produkt.Barva);

            this.CommandPotvrzeni = new Command(PotvrzeniZmen);

            if (produkt.Id != 0)
            {
                HlavniLabel = "Editace produktu";
            }
            else
            {
                HlavniLabel = "Přidání produktu";
            }
        }

        /// <summary>
        /// obsahuje potvrzeni zmen a navrat na predchozi stranku.
        /// používáme asynchronní metodu PopAsync a chceme ji tak i spustit, takže i tato "rodičovská" metoda musí být označena jako "async".
        /// </summary>
        protected async void PotvrzeniZmen()
        {

            //pri potvrzeni zmen zmenime i puvodni data v ulozene referenci
            this.produktPuvodni.Nazev = this.ProduktEditovany.Nazev;
            this.produktPuvodni.Kategorie = this.ProduktEditovany.Kategorie;
            this.produktPuvodni.Cena = this.ProduktEditovany.Cena;
            this.produktPuvodni.Popis = this.ProduktEditovany.Popis;
            this.produktPuvodni.Barva = this.ProduktEditovany.Barva;
            this.ProduktEditovany = new Produkt();

            bool pridavanyPrvek = false;
            if (this.produktPuvodni.Id == 0)
                pridavanyPrvek = true;

            long idVlozenehoProduktu = this.databaze.VlozNeboUpdatujProdukt(this.produktPuvodni);

            //pokud byl vlozen produkt (pri ID == 0), tak vyvolej udalost
            if (pridavanyPrvek == true && idVlozenehoProduktu > 0)
            {
                this.produktPuvodni.Id = idVlozenehoProduktu;
                this.ProvedenoVlozeniProduktuDoDatabaze?.Invoke(this.produktPuvodni);
            }

            //asynchronne se vratime o jednu stranku zpet
            //(toto je skareda zavislost ViewModelu na Xamarinu :-( ale raději to nebudeme komplikovat ... prozatim)
            await navigace?.PopAsync();
        }

        public event Action<Produkt> ProvedenoVlozeniProduktuDoDatabaze;

    }

}
