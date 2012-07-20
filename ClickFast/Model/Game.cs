using System;
using System.Diagnostics;
using System.Windows.Threading;
using Microsoft.Phone.Reactive;

namespace ClickFast.Model
{
    public delegate void GenericEventHandler<T>(Game sender, T eventArgs);

    public class Game
    {
        private readonly Stopwatch clickedWatch;
        private readonly DispatcherTimer countdownTimer;
        private readonly ScoreStorage scoreStorage;
        private int countdown;
        private bool gameIsActive;
        private bool userCanPress;

        public Game()
        {
            scoreStorage = new ScoreStorage();
            clickedWatch = new Stopwatch();
            countdownTimer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(0.7)};
        }

        public bool UserCanPress
        {
            get { return userCanPress; }
        }

        public double SecondsPassed
        {
            get { return clickedWatch.Elapsed.TotalSeconds; }
        }

        public event EventHandler CountDownStarted;
        public event GenericEventHandler<int> CountdownTick;
        public event EventHandler WaitForItStarted;
        public event EventHandler ClickFastStarted;
        public event GenericEventHandler<bool> UserCanPressChanged;
        public event GenericEventHandler<bool> Ended;

        public void StartCountDown()
        {
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
            CountDownStarted(this, new EventArgs());
            
            userCanPress = false;
            UserCanPressChanged(this, false);
        }

        public void Retry()
        {
            StartCountDown();
        }


        public void UserClick()
        {
            if (gameIsActive)
            {
                if (clickedWatch.IsRunning)
                {
                    clickedWatch.Stop();
                    double secondsPassed = clickedWatch.Elapsed.TotalSeconds;
                    scoreStorage.AddScore(new Score(secondsPassed, DateTime.Now));
                    Ended(this, true);
                }
                else
                {
                    Ended(this, false);
                }
                gameIsActive = false;
            }
        }

        private void OnCountdownTimerTick(object sender, EventArgs args)
        {
            if (countdown == 0)
            {
                countdownTimer.Stop();
                Scheduler.Dispatcher.Schedule(StartTimer, TimeSpan.FromSeconds(new Random().Next(3, 10)));
                gameIsActive = true;
                WaitForItStarted(this, new EventArgs());
                
                userCanPress = true;
                UserCanPressChanged(this, true);
            }
            else
            {
                CountdownTick(this, countdown);
                countdown--;
            }
        }

        private void StartTimer()
        {
            if (userCanPress && countdown == 0)
            {
                clickedWatch.Reset();
                clickedWatch.Start();
                ClickFastStarted(this, new EventArgs());
            }
        }
    }
}