using SemestralniProjekt.UzivatelObjednavky;
using SemestralniProjekt.UzivatelProdukty;
using SemestralniProjekt.UzivatelKosik;
using SemestralniProjekt.ViewModel;
using SemestralniProjektModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SemestralniProjekt
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UzivatelLoggedInTabbedPage : TabbedPage
    {
        UzivatelLoggedInTabbedPageViewModel ViewModel;
        UzivatelKosikViewModel kosik;

        public UzivatelLoggedInTabbedPage(Databaze databaze, Uzivatel uzivatel)
        {
            this.ViewModel = new UzivatelLoggedInTabbedPageViewModel(uzivatel, databaze, this.Navigation);
            this.kosik = new UzivatelKosikViewModel(databaze);
            this.BindingContext = this.ViewModel;
            InitializeComponent();

            Children.Add(new UzivatelKosikPage(databaze, kosik));
            Children.Add(new UzivatelProduktyPage(databaze, kosik));
            Children.Add(new UzivatelObjednavkyPage(databaze));
        }
    }
}