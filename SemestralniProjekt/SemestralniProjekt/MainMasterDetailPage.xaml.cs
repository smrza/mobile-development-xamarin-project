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
    public partial class MainMasterDetailPage : MasterDetailPage
    {
        Databaze databaze;
        public MainMasterDetailPage(Databaze databaze)
        {
            this.databaze = databaze;
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MainMasterDetailPageMasterMenuItem;
            if (item == null)
                return;

            //zde se vytvari instance jednotlivych stranek a byla zde provedena zmena: druhym parametrem je "databaze".
            //To znamena, ze se aplikace pokusi nalezt konstruktor s nejblizsi shodou se zadanymi parametry (v tomto pripade konstruktor v Page, ktery ma variantu s jednim vstupem s predanim "Databaze databaze")
            var page = (Page)Activator.CreateInstance(item.TargetType, databaze);
            page.Title = item.Title;

            Detail = new NavigationPage(page);
            IsPresented = false;

            MasterPage.ListView.SelectedItem = null;
        }
    }
}