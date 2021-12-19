using SemestralniProjekt.Uzivatele;
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
    public partial class UzivatelMainTabbedPage : TabbedPage
    {
        public UzivatelMainTabbedPage(Databaze databaze)
        {
            InitializeComponent();
            Children.Add(new SeznamUzivateluPage(databaze));
        }
    }
}