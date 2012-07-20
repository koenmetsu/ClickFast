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

namespace ClickFast.Model
{
    public class Score
    {
        public Score()
        {
            
        }
        public Score(double seconds, DateTime date)
        {
            Seconds = seconds;
            Date = date;
        }
        public double Seconds { get; set; }
        public DateTime Date { get; set; }
    }
}
