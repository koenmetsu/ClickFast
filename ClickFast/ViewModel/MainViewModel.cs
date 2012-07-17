using System;
using System.Diagnostics;
using System.Windows.Media;
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
        private readonly Stopwatch clickedWatch;
        private SolidColorBrush _buttonColor;
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
            PressedCommand = new RelayCommand(OnPressedCommand, () => ButtonPressable);
            clickedWatch = new Stopwatch();
            ButtonText = "Wait for it...";
            Scheduler.Dispatcher.Schedule(StartTimer, TimeSpan.FromSeconds(new Random().Next(3, 10)));
            ButtonPressable = true;
        }

        public string ButtonText
        {
            get { return m_buttonText; }
            set
            {
                m_buttonText = value;
                RaisePropertyChanged(() => ButtonText);
            }
        }

        public SolidColorBrush ButtonColor
        {
            get { return _buttonColor; }
            set
            {
                _buttonColor = value;
                RaisePropertyChanged(() => ButtonColor);
            }
        }

        public RelayCommand PressedCommand { get; private set; }

        private bool ButtonPressable
        {
            get { return m_buttonPressable; }
            set
            {
                if (value != m_buttonPressable)
                {
                    m_buttonPressable = value;
                    RaisePropertyChanged(() => ButtonPressable);
                    PressedCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private void StartTimer()
        {
            if (m_buttonPressable)
            {
                clickedWatch.Start();
                ButtonText = "Click fast!";
            }
        }

        private void OnPressedCommand()
        {
            if (clickedWatch.IsRunning)
            {
                clickedWatch.Stop();
                ButtonColor = new SolidColorBrush(Colors.Blue);
                ButtonText = string.Format("You clicked in at {0:0.000} seconds", clickedWatch.Elapsed.TotalSeconds);
                m_buttonPressable = false;
            }
            else
            {
                ButtonColor = new SolidColorBrush(Colors.Red);
                ButtonText = "Aaaaah, you were too fast!";
                m_buttonPressable = false;
            }
        }
    }
}