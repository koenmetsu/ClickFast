using System;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Threading;
using ClickFast.Framework;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Reactive;
using NavigationService = System.Windows.Navigation.NavigationService;
using ClickFast.Model;

namespace ClickFast.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService m_navigationService;

        private SolidColorBrush _buttonColor;
        private bool m_buttonPressable;
        private string m_buttonText;
        private readonly Game game;

        private readonly INavigationService navigationService;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}

            game = new Game();
            game.CountDownStarted += GameOnCountDownStarted;
            game.CountdownTick += GameOnCountdownTick;
            game.WaitForItStarted += GameOnWaitForItStarted;
            game.ClickFastStarted += GameOnClickFastStarted;
            game.UserCanPressChanged += GameOnUserCanPressChanged;
            game.Ended += GameOnEnded;

            ClickFastCommand = new RelayCommand(game.UserClick, () => game.UserCanPress);
            RetryCommand = new RelayCommand(game.Retry, () => true);
            ShowHighScoresCommand = new RelayCommand(() => this.navigationService.NavigateTo(ViewModelLocator.SettingsPageUri), () => true);

            game.StartCountDown();
        }

        public RelayCommand ClickFastCommand { get; private set; }
        public RelayCommand RetryCommand { get; private set; }
        public RelayCommand ShowHighScoresCommand { get; private set; }


        public string ClickFastButtonText
        {
            get { return m_buttonText; }
            set
            {
                m_buttonText = value;
                RaisePropertyChanged(() => ClickFastButtonText);
            }
        }

        public SolidColorBrush ClickFastButtonColor
        {
            get { return _buttonColor; }
            set
            {
                _buttonColor = value;
                RaisePropertyChanged(() => ClickFastButtonColor);
            }
        }

        private void GameOnCountdownTick(Game sender, int eventArgs)
        {
            ClickFastButtonText = eventArgs.ToString();
        }

        private void GameOnWaitForItStarted(object sender, EventArgs eventArgs)
        {
            ClickFastButtonText = "Wait for it...";
        }


        private void GameOnClickFastStarted(object sender, EventArgs eventArgs)
        {
            ClickFastButtonText = "Click fast!";
        }

        private void GameOnCountDownStarted(object sender, EventArgs eventArgs)
        {
            ClickFastButtonColor = new SolidColorBrush(Colors.Black);
            ClickFastButtonText = string.Empty;
        }


        private void GameOnUserCanPressChanged(Game sender, bool eventArgs)
        {
            ClickFastCommand.RaiseCanExecuteChanged();
        }

        private void GameOnEnded(Game sender, bool eventArgs)
        {
            if (eventArgs)
            {
                ClickFastButtonColor = new SolidColorBrush(Colors.Blue);
                ClickFastButtonText = string.Format("You clicked in at {0:0.000} seconds",
                                                    sender.SecondsPassed);
            }
            else
            {
                ClickFastButtonColor = new SolidColorBrush(Colors.Red);
                ClickFastButtonText = "Aaaaah, you were too fast!";
            }
        }
    }
}