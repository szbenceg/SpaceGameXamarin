using beadXamarin.Model;
using beadXamarin.Persistence;
using beadXamarin.View;
using beadXamarin.ViewModel;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace beadXamarin
{
    public partial class App : Application
    {
        #region Fields

        private IGameDataAccess _GameDataAccess;
        private GameViewModel _GameViewModel;
        private GamePage _gamePage;


        private NavigationPage _mainPage;

        #endregion

        #region Application methods

        public App()
        {
            _GameDataAccess = DependencyService.Get<IGameDataAccess>(); 

            _GameViewModel = new GameViewModel();


            _gamePage = new GamePage();
            _gamePage.BindingContext = _GameViewModel;

            _mainPage = new NavigationPage(_gamePage);

            MainPage = _mainPage;

        }

        #endregion


    }
}
