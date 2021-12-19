using SemestralniProjekt.AdminObjednavky;
using SemestralniProjekt.AdminProdukty;
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
    public partial class AdministratorMainTabbedPage : TabbedPage
    {
        public AdministratorMainTabbedPage(Databaze databaze)
        {
            InitializeComponent();

            Children.Add(new AdminProduktyPage(databaze));
            Children.Add(new AdminObjednavkyPage(databaze));
        }
    }
}