using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using GalaSoft.MvvmLight;
using ClickFast.Model;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Messaging;

namespace ClickFast.ViewModel
{
    public class HighScoresViewModel: ViewModelBase
    {
        private readonly IMessenger messenger;
        private readonly ScoreStorage scoreStorage;
        public HighScoresViewModel(IMessenger messenger)
        {
            this.messenger = messenger;
            messenger.Register<HighScoresUpdateMessage>(this, (msg) => UpdateHighScores());
            scoreStorage = new ScoreStorage();
            m_highScores = new ObservableCollection<Score>(scoreStorage.GetScores());
        }

        private ObservableCollection<Score> m_highScores;
        public ObservableCollection<Score> HighScores
        {
            get { return m_highScores; }
        }

        private void UpdateHighScores()
        {
            m_highScores.Clear();
            foreach (var highScore in scoreStorage.GetScores().OrderBy(x => x.Seconds).Take(10))
            {
                m_highScores.Add(highScore);
            }
        }
    }
}
