using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;

namespace ClickFast.View
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            OrientationChanged += (sender, args) => MainControl.OnChangePageOrientation(args);
        }
    }
}