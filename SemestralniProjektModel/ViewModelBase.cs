using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SemestralniProjekt.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        //událost, implementovaná z interface INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        //pokud zde definujeme atribut CallerMemberName,
        //tak při zavolání této metody ze settu nějaké vlastnosti (property) již nemusíme nazev vlastnosti udavat,
        //misto toho se prenese automaticky
        protected void OnPropertyChanged([CallerMemberName]string name = null)
        {
            //pokud má událost nastavena pozorovatele (pokud je na ni napojena obslužná metoda, nebo byla "nabindována"), tak se daná událost vyvolá
            //první parametr určuje, která třída vyvolala událost, druhý určuje argumenty události (konkrétně jméno property/vlastnosti, která se má měnit)
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
