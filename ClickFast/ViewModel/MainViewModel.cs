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
        private readonly Stopwatch clickedWatch;
        private readonly DispatcherTimer countdownTimer;
        private SolidColorBrush _buttonColor;
        private int countdown;
        private bool gameIsActive;
        private bool m_buttonPressable;
        private string m_buttonText;

        private readonly INavigationService navigationService;
        private readonly ScoreStorage scoreStorage;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            this.scoreStorage = new ScoreStorage();
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}

            ClickFastCommand = new RelayCommand(OnClickFastCommand, () => ClickFastButtonPressable);
            RetryCommand = new RelayCommand(OnRetryCommand, () => true);
            ShowHighScoresCommand = new RelayCommand(() => this.navigationService.NavigateTo(ViewModelLocator.SettingsPageUri), () => true);

            clickedWatch = new Stopwatch();
            countdownTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.7) };
            StartCountdown();
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

        private bool ClickFastButtonPressable
        {
            get { return m_buttonPressable; }
            set
            {
                if (m_buttonPressable != value)
                {
                    m_buttonPressable = value;
                    RaisePropertyChanged(() => ClickFastButtonPressable);
                    ClickFastCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private void OnClickFastCommand()
        {
            if (gameIsActive)
            {
                if (clickedWatch.IsRunning)
                {
                    clickedWatch.Stop();
                    double secondsPassed = clickedWatch.Elapsed.TotalSeconds;
                    ClickFastButtonColor = new SolidColorBrush(Colors.Blue);
                    ClickFastButtonText = string.Format("You clicked in at {0:0.000} seconds",
                                                        secondsPassed);
                    scoreStorage.AddScore(new Score(secondsPassed, DateTime.Now));
                }
                else
                {
                    ClickFastButtonColor = new SolidColorBrush(Colors.Red);
                    ClickFastButtonText = "Aaaaah, you were too fast!";
                }
                gameIsActive = false;
            }
        }

        private void OnRetryCommand()
        {
            StartCountdown();
        }

        private void ClearClickFastButton()
        {
            ClickFastButtonColor = new SolidColorBrush(Colors.Black);
            ClickFastButtonPressable = false;
            ClickFastButtonText = string.Empty;
        }

        private void StartCountdown()
        {
            ClearClickFastButton();
            if (countdownTimer.IsEnabled)
            {
                countdownTimer.Stop();
            }
            if (clickedWatch.IsRunning)
            {
                clickedWatch.Stop();
            }
            countdown = 3;
            countdownTimer.Tick -= OnCountdownTimerTick;
            countdownTimer.Tick += OnCountdownTimerTick;
            countdownTimer.Start();
        }

        private void OnCountdownTimerTick(object sender, EventArgs args)
        {
            if (countdown == 0)
            {
                countdownTimer.Stop();
                ClickFastButtonText = "Wait for it...";
                ClickFastButtonPressable = true;
                Scheduler.Dispatcher.Schedule(StartTimer, TimeSpan.FromSeconds(new Random().Next(3, 10)));
                gameIsActive = true;
            }
            else
            {
                ClickFastButtonText = countdown.ToString();
                countdown--;
            }
        }

        private void StartTimer()
        {
            if (m_buttonPressable && countdown == 0)
            {
                clickedWatch.Reset();
                clickedWatch.Start();
                ClickFastButtonText = "Click fast!";
            }
        }
    }
}