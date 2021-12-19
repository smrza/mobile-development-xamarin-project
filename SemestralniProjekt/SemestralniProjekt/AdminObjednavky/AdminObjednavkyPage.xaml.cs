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
    public partial class AdminObjednavkyPage : ContentPage
    {
        private AdminObjednavkyViewModel ViewModel;

        public AdminObjednavkyPage(Databaze databaze)
        {
            ViewModel = new AdminObjednavkyViewModel(databaze);
            this.BindingContext = ViewModel;

            InitializeComponent();
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (sender is ListView)
            {
                AdminObjednavkyStavPage page = new AdminObjednavkyStavPage(ViewModel.databaze, ((sender as ListView).SelectedItem as Objednavka).Id);
                Navigation.PushAsync(page);
            }

        }

        protected override void OnAppearing()
        {
            ViewModel.NactiObjednavky();
        }
    }
}