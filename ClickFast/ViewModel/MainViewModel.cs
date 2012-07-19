using System;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Reactive;

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
        private DispatcherTimer countdownTimer;
        private SolidColorBrush _buttonColor;
        private Stopwatch clickedWatch;
        private int countdown;
        private bool m_buttonPressable;
        private string m_buttonText;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
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

            clickedWatch = new Stopwatch();
            countdownTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.7) };
            StartCountdown();
        }

        public RelayCommand ClickFastCommand { get; private set; }
        public RelayCommand RetryCommand { get; private set; }
        public RelayCommand ShowHighScoresCommand { get; private set; }

        private void OnClickFastCommand()
        {
            if (clickedWatch.IsRunning)
            {
                clickedWatch.Stop();
                ClickFastButtonColor = new SolidColorBrush(Colors.Blue);
                ClickFastButtonText = string.Format("You clicked in at {0:0.000} seconds", clickedWatch.Elapsed.TotalSeconds);
            }
            else
            {
                ClickFastButtonColor = new SolidColorBrush(Colors.Red);
                ClickFastButtonText = "Aaaaah, you were too fast!";
            }
        }

        private void OnRetryCommand()
        {
            StartCountdown();
        }

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
            countdownTimer.Tick -= OnCountdownTimerTick;
            countdown = 3;
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