using SemestralniProjekt.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SemestralniProjektModel
{

    //v teto verzi samozrejme neni mozne pouzit atributy
    public class Produkt : ViewModelBase
    {
        public const string TableName = "PRODUKT";
        public const string IDString = "id";
        public const string NazevString = "nazev";
        public const string KategorieString = "kategorie";
        public const string CenaString = "cena";
        public const string PopisString = "popis";
        public const string BarvaString = "barva";

        public long Id
        {
            get;
            set;
        }
        private string nazev;
        public string Nazev
        {
            get
            {
                return nazev;
            }
            set
            {
                if (nazev != value)
                {
                    nazev = value;
                    OnPropertyChanged();
                }
            }
        }
        private string kategorie;
        public string Kategorie
        {
            get
            {
                return kategorie;
            }
            set
            {
                if (kategorie != value)
                {
                    kategorie = value;
                    OnPropertyChanged();
                }
            }
        }
        private int cena;
        public int Cena
        {
            get
            {
                return cena;
            }
            set
            {
                if (cena != value)
                {
                    cena = value;
                    OnPropertyChanged();
                }
            }
        }
        private string popis;
        public string Popis
        {
            get
            {
                return popis;
            }
            set
            {
                if (popis != value)
                {
                    popis = value;
                    OnPropertyChanged();
                }
            }
        }

        private string barva;
        public string Barva
        {
            get
            {
                return barva;
            }
            set
            {
                if (barva != value)
                {
                    barva = value;
                    OnPropertyChanged();
                }
            }
        }

        public Produkt()
        {
            Nazev = String.Empty;
            Kategorie = String.Empty;
            Cena = 0;
            Popis = String.Empty;
            Barva = String.Empty;
        }

        public Produkt(long id, string nazev, string kategorie, int cena, string popis, string barva)
        {
            Id = id;
            Nazev = nazev;
            Kategorie = kategorie;
            Cena = cena;
            Popis = popis;
            Barva = barva;
        }

        public override string ToString()
        {
            return $"Id: {Id}, Nazev: {Nazev}, Kategorie: {Kategorie}, Cena: {Cena}, Popis: {Popis}, Barva: {Barva}";
        }
    }
}
