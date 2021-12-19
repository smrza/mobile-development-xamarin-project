using SemestralniProjekt.ViewModel;
using SemestralniProjektModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SemestralniProjekt.AdminObjednavky
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminObjednavkyStavPage : ContentPage
    {
        public AdminObjednavkyStavViewModel ViewModel { get; set; }
        public AdminObjednavkyStavPage(Databaze databaze, long id)
        {
            InitializeComponent();
            ViewModel = new AdminObjednavkyStavViewModel(databaze, id, Navigation);
            this.BindingContext = ViewModel;
        }
    }
}