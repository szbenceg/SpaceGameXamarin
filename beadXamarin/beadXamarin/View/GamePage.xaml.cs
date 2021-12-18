using DLToolkit.Forms.Controls;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace beadXamarin.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamePage : ContentPage
    {
        public GamePage()
        {
            InitializeComponent();
            FlowListView.Init();
        }
    }
}