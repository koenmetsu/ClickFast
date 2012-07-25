using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace ClickFast.View
{
    public partial class MainControl
    {
        public MainControl()
        {
            InitializeComponent(); 
        }

        public void OnChangePageOrientation(OrientationChangedEventArgs e)
        {
            if ((e.Orientation & PageOrientation.Portrait) == (PageOrientation.Portrait))
            {
                
                Grid.SetRow(RetryButton, 1);
                Grid.SetColumn(RetryButton, 0);
            }

            // If not in the portrait mode, move buttonList content to a visible row and column.

            else
            {
                Grid.SetRow(RetryButton, 0);
                Grid.SetColumn(RetryButton, 1);
            }
        }
    }
}
