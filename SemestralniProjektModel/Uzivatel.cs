using SemestralniProjekt.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SemestralniProjektModel
{
    public class Uzivatel : ViewModelBase
    {
        public const string TableName = "UZIVATEL";
        public const string IDString = "id";
        public const string JmenoString = "jmeno";

        public long Id
        {
            get;
            set;
        }
        private string jmeno;
        public string Jmeno
        {
            get
            {
                return jmeno;
            }
            set
            {
                if (jmeno != value)
                {
                    jmeno = value;
                    OnPropertyChanged();
                }
            }
        }

        public Uzivatel()
        {
            Jmeno = String.Empty;
        }

        public Uzivatel(long id, string jmeno)
        {
            Id = id;
            Jmeno = jmeno;
        }

        public override string ToString()
        {
            return $"Id: {Id}, Jmeno: {Jmeno}";
        }
    }
}
