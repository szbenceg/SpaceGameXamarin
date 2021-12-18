using beadXamarin.Model;
using beadXamarin.Persistence;
using SpaceGame.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace beadXamarin.ViewModel
{

    public class GameViewModel : ViewModelBase
    {

        //----------------------------------------------

        private String gameTime;
        private String lifeNumber;

        public SpaceModel spaceModel;
        public PlayerField player;

        public bool pausePressed = true;
        public bool simpleGameStart = true;

        public ObservableCollection<GameObjectField> fields;

        private static Timer targetMoveTimer;
        private static Timer targetCreateTimer;
        public static Timer sppedUpTimer;
        public static Timer ellapsedTimer;


        public Dictionary<Target, TargetField> targets;

        private int speedUpTimerTick = 1000;


        public DelegateCommand LeftKeyDown { get; set; }
        public DelegateCommand RightKeyDown { get; set; }
        public DelegateCommand PKeyDown { get; set; }
        public DelegateCommand StartButtonPressed { get; set; }
        public DelegateCommand PauseButtonPressed { get; set; }
        public DelegateCommand OpenFilePressed { get; set; }
        public DelegateCommand NewGameButtonPressed { get; set; }
        public DelegateCommand SaveGamePressed { get; set; }

        public ObservableCollection<GameObjectField> Fields
        {
            get { return fields; }
            set { fields = value; }
        }

        public string GameTime
        {
            get { return gameTime; }
            set { gameTime = value; }
        }

        public string LifeNumber
        {
            get { return lifeNumber; }
            set { lifeNumber = value; }
        }

        //----------------------------------------------

        private IGameDataAccess iffielmanager;

        
        public DelegateCommand LoadGameButton { get; private set; }
        public DelegateCommand SizeCommand { get; private set; }


        public event EventHandler LevelOne;
        public event EventHandler SizeEvent;

        public GameViewModel()
        {

            iffielmanager = DependencyService.Get<IGameDataAccess>();

            LeftKeyDown = new DelegateCommand(LeftKeyDownHandler);
            RightKeyDown = new DelegateCommand(RightKeyDownHandler);
            PKeyDown = new DelegateCommand(PKeyDownHandler);
            StartButtonPressed = new DelegateCommand(StartButtonPressedHandler);
            NewGameButtonPressed = new DelegateCommand(StartButtonPressedHandler);
            PauseButtonPressed = new DelegateCommand(PauseButtonPressedHandler);
            Fields = new ObservableCollection<GameObjectField>();

            //iffielmanager = DependencyService.Get<IFFileManager>();

            GameTime = "0";
            LifeNumber = "0";

            LoadGameButton = new DelegateCommand(param => OnLoadgame());
            SaveGamePressed = new DelegateCommand(param => SaveGameButtonClicked());

            spaceModel = new SpaceModel(iffielmanager);
            targets = new Dictionary<Target, TargetField>();


            StartGame();

        }

        private void SaveGameButtonClicked()
        {
            Pause();
            iffielmanager.SaveAsync(spaceModel.spaceWord);
        }

        //------------------------------------------------------------------------------------------

        public void StartGame()
        {

            pausePressed = false;
            speedUpTimerTick = 1000;

            Fields.Clear();

            if (simpleGameStart)
            {
                spaceModel = new SpaceModel(iffielmanager);

                spaceModel.PlayerChanged += new EventHandler<Player>(PlayerChangedEventhandler);
                spaceModel.TargetChanged += new EventHandler<Target>(TargetChangedEventHandler);
                spaceModel.LifeChanged += new EventHandler<int>(LifeChangedHandler);
                spaceModel.GameOver += new EventHandler(GameOverEventHandler);

                spaceModel.StartGame(600, 750, 50, 50, 20, 20);
            }
            else
            {

                spaceModel.PlayerChanged += new EventHandler<Player>(PlayerChangedEventhandler);
                spaceModel.TargetChanged += new EventHandler<Target>(TargetChangedEventHandler);
                spaceModel.LifeChanged += new EventHandler<int>(LifeChangedHandler);
                spaceModel.GameOver += new EventHandler(GameOverEventHandler);

                //targetCreateTimer.Interval = new TimeSpan(0, 0, 0, 0, model.TargetCreateTimer);
                speedUpTimerTick = (int)spaceModel.TargetCreateTimer;

                spaceModel.initializeGame();
                simpleGameStart = true;
            }

            targetMoveTimer = new Timer(_ => TargetMoveTimerTick(), null, 500, Timeout.Infinite);
            targetCreateTimer = new Timer(_ => TargetCreateTimerTick(), null, 1000 * 3, Timeout.Infinite);
            sppedUpTimer = new Timer(_ => SpeedUpTimerTick(), null, 3000, Timeout.Infinite);
            ellapsedTimer = new Timer(_ => EllapsedTimeTick(), null, 1000, Timeout.Infinite);



        }
        public void GameOverEventHandler(object sender, EventArgs eventArgs)
        {
            Pause();
        }


        public void LifeChangedHandler(object sender, int lifeNumber)
        {
            if (lifeNumber < 0) return;

            LifeNumber = lifeNumber.ToString();
            OnPropertyChanged("LifeNumber");
        }

        public void PlayerChangedEventhandler(object sender, Player component)
        {
            if (!fields.Contains(player))
            {
                player = new PlayerField(component.Width, component.Height, component.PositionX, component.PositionY);
                Fields.Add(player);
            }
            else
            {
                player.PositionX = component.PositionX;
            }
            
        }

        public async void TargetChangedEventHandler(object sender, Target target)
        {
            if (targets.ContainsKey(target))
            {
                targets[target].PositionX = target.PositionX;
                targets[target].PositionY = target.PositionY;
                if (target.status == "DELETE")
                {
                    await Task.Run(async () =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Fields.Remove(targets[target]);
                            targets.Remove(target);
                        });
                    });
                }
            }
            else
            {
                TargetField targetField = new TargetField(target.Width, target.Height, target.PositionX, target.PositionY);
                await Task.Run(async () =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Fields.Add(targetField);
                        targets.Add(target, targetField);
                    });
                });
            }
        }


        public void Pause()
        {
            if (!pausePressed)
            {
                pausePressed = true;

                sppedUpTimer.Dispose();
                targetMoveTimer.Dispose();
                targetCreateTimer.Dispose();
                ellapsedTimer.Dispose();

                //targetMoveTimer.Stop();
                //targetCreateTimer.Stop();
                //ellapsedTimer.Stop();
                //sppedUpTimer.Stop();
                spaceModel.PlayerChanged -= new EventHandler<Player>(PlayerChangedEventhandler);
                spaceModel.TargetChanged -= new EventHandler<Target>(TargetChangedEventHandler);
            }
            else
            {
                pausePressed = false;
                //targetMoveTimer.Start();
                //targetCreateTimer.Start();
                //ellapsedTimer.Start();
                //sppedUpTimer.Stop();
                ellapsedTimer = new Timer(_ => EllapsedTimeTick(), null, 1000, Timeout.Infinite);
                targetMoveTimer = new Timer(_ => TargetMoveTimerTick(), null, 50, Timeout.Infinite);
                targetCreateTimer = new Timer(_ => TargetCreateTimerTick(), null, speedUpTimerTick, Timeout.Infinite);
                sppedUpTimer = new Timer(_ => TargetCreateTimerTick(), null, speedUpTimerTick, Timeout.Infinite);

                spaceModel.PlayerChanged += new EventHandler<Player>(PlayerChangedEventhandler);
                spaceModel.TargetChanged += new EventHandler<Target>(TargetChangedEventHandler);
            }
        }

        public void TargetMoveTimerTick()
        {
            spaceModel.moveTargets();
            targetMoveTimer.Dispose();
            targetMoveTimer = new Timer(_ => TargetMoveTimerTick(), null, 50, Timeout.Infinite);
        }

        public void EllapsedTimeTick()
        {
            spaceModel.GameTimeSeconds += 1;
            GameTime = spaceModel.GameTimeSeconds.ToString();
            OnPropertyChanged("GameTime");
            ellapsedTimer.Dispose();
            ellapsedTimer = new Timer(_ => EllapsedTimeTick(), null, 1000, Timeout.Infinite);
        }


        public void TargetCreateTimerTick()
        {
            spaceModel.createTarget();
            targetCreateTimer.Dispose();
            targetCreateTimer = new Timer(_ => TargetCreateTimerTick(), null, speedUpTimerTick, Timeout.Infinite);
        }

        public void SpeedUpTimerTick()
        {
            //targetCreateTimer.Stop();
            if (speedUpTimerTick > 200)
            {
                speedUpTimerTick = speedUpTimerTick - 100;
                spaceModel.TargetCreateTimer = speedUpTimerTick;
            }
            targetCreateTimer.Dispose();
            targetCreateTimer = new Timer(_ => TargetCreateTimerTick(), null, speedUpTimerTick, Timeout.Infinite);

            sppedUpTimer.Dispose();
            sppedUpTimer = new Timer(_ => SpeedUpTimerTick(), null, 3000, Timeout.Infinite);
        }

        public void LeftKeyDownHandler(object p)
        {
            if (!pausePressed)
            {
                spaceModel.moveLeft();
            }
        }

        public void RightKeyDownHandler(object p)
        {
            if (!pausePressed)
            {
                spaceModel.moveRight();
            }
        }

        public void PKeyDownHandler(object p)
        {
            Pause();
        }

        public void StartButtonPressedHandler(object p)
        {
            StartGame();
        }

        public void PauseButtonPressedHandler(object p)
        {
            Pause();
        }

        //------------------------------------------------------------------------------------------


        private async void OnLoadgame()
        {
            Pause();
            spaceModel.spaceWord = (await iffielmanager.LoadAsync("asd"));
            simpleGameStart = false;
            StartGame();

        }
        

    }
}
