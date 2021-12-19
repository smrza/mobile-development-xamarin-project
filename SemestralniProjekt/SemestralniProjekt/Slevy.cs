using SemestralniProjektModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SemestralniProjekt
{
    public abstract class Slevy
    {
        public abstract double Sleva(List<Produkt> produkty);
    }

    public class Jaro : Slevy
    {
        public override double Sleva(List<Produkt> produkty)
        {
            int Count = produkty.Count();
            double sleva = 1;
            double cena = 0;

            if (Count >= 3 && Count < 8)
            {
                sleva = 0.8;
            }
            else if (Count >= 8 && Count < 12)
            {
                sleva = 0.7;
            }
            else if (Count >= 12)
            {
                sleva = 0.6;
            }

            foreach (Produkt produkt in produkty)
            {
                cena += produkt.Cena;
            }

            return cena * sleva;
        }
    }


    public class Leto : Slevy
    {
        private Databaze databaze;
        public Leto(Databaze databaze)
        {
            this.databaze = databaze;
        }

        public override double Sleva(List<Produkt> produkty)
        {
            int pocet = databaze.VratObjednavkyUzivatel(databaze.LoggedIdUzivatele).Count;
            double sleva = 1;
            double cena = 0;

            if (pocet >= 10)
            {
                sleva = 0.8;
            }
            else if (pocet >= 5)
            {
                sleva = 0.9;
            }

            foreach (Produkt produkt in produkty)
            {
                cena += produkt.Cena;
            }

            return cena * sleva;
        }
    }


    public class Podzim : Slevy
    {
        public override double Sleva(List<Produkt> produkty)
        {
            double cena = 0;

            foreach (Produkt produkt in produkty)
            {
                if(produkt.Id == 1)
                {
                    cena += produkt.Cena * 0.6;
                }
                else if (produkt.Id == 2)
                {
                    cena += produkt.Cena * 0.7;
                }
                else if (produkt.Id == 3)
                {
                    cena += produkt.Cena * 0.8;
                }
                else if (produkt.Id == 4)
                {
                    cena += produkt.Cena * 0.9;
                }
                else if (produkt.Id == 5)
                {
                    cena += produkt.Cena * 0.1;
                }
                else cena += produkt.Cena;
            }

            return cena;
        }
    }

    public class Zima : Slevy
    {
        public override double Sleva(List<Produkt> produkty)
        {
            //bohužel nestuduji marketing a
            //všechny 3 předchozí slevy pokrývají veškerené obchodní strategie pro zlevňování, které mě napadají
            //tudíž mě nenapadá žádná jiná strategie než zdražit vše o 20% 
            //a specielně o 100% jeden den před vánocema
            double sleva = 1.2;

            DateTime datum = DateTime.Now;
            if(datum.Month == 12 && datum.Day == 23)
            {
                sleva = 2.0;
            }

            double cena = 0;

            foreach (Produkt produkt in produkty)
            {
                cena += produkt.Cena;
            }

            return cena * sleva;
        }
    }

    public static class TypSlevy
    {
        public static Slevy VratTypSlevy(Databaze databaze)
        {
            DateTime datum = DateTime.Now;

            ////////////////////////////////////
            //test slev
            //short datum = 3;
            //if (datum == 3 || datum == 4 || datum == 5)
            //{
            //    return new Jaro();
            //}
            //else if (datum == 6 || datum == 7 || datum == 8)
            //{
            //    return new Leto(databaze);
            //}
            //else if (datum == 9 || datum == 10 || datum == 11)
            //{
            //    return new Podzim();
            //}
            //else
            //{
            //    return new Zima();
            //}
            ////////////////////////////////////

            if (datum.Month == 3 || datum.Month == 4 || datum.Month == 5)
            {
                return new Jaro();
            }
            else if (datum.Month == 6 || datum.Month == 7 || datum.Month == 8)
            {
                return new Leto(databaze);
            }
            else if (datum.Month == 9 || datum.Month == 10 || datum.Month == 11)
            {
                return new Podzim();
            }
            else
            {
                return new Zima();
            }
        }
    }
}
