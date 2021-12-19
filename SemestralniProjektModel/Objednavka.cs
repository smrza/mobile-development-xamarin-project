using SemestralniProjekt.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SemestralniProjektModel
{
    public class Objednavka : ViewModelBase
    {
        public const string TableName = "OBJEDNAVKA";
        public const string IDString = "id";
        public const string IDUzivatelString = "idUzivatel";
        public const string StavString = "Stav";
        public const string DatumString = "Datum";

        public long Id
        {
            get;
            set;
        }

        private long idUzivatel;

        public long IdUzivatel
        {
            get
            {
                return idUzivatel;
            }
            set
            {
                if(idUzivatel != value)
                {
                    idUzivatel = value;
                    OnPropertyChanged();
                }
            }
        }
        private StavObjednavky stav;

        public StavObjednavky Stav
        {
            get
            {
                return stav;
            }
            set
            {
                if(stav != value)
                {
                    stav = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<Produkt> produkty;
        public List<Produkt> Produkty
        {
            get
            {
                return produkty;
            }
            set
            {
                if(produkty != value)
                {
                    produkty = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime datum;
        public DateTime Datum
        {
            get
            {
                return datum;
            }
            set
            {
                if(datum != value)
                {
                    datum = value;
                    OnPropertyChanged();
                }
            }
        }

        public Objednavka(long id, long idUzivatel, List<Produkt> produkty, StavObjednavky stav, DateTime datum)
        {
            Id = id;
            IdUzivatel = idUzivatel;
            Produkty = produkty;
            Stav = stav;
            Datum = datum;
        }
        public override string ToString()
        {
            return $"ID: {Id}, ID Zakaznika:{IdUzivatel}, Stav objednavky: {Stav.ToString()}, Datum objednavky: {Datum.Day}. {Datum.Month}. {Datum.Year} ";

        }
    }

    public enum StavObjednavky
    {
        ZALOZENA,
        POTVRZENA,
        ODESLANA,
        ZRUSENA
    }
}
