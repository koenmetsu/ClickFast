using System;
using System.Windows.Navigation;

namespace ClickFast.Framework
{
    public interface INavigationService
    {
        event NavigatingCancelEventHandler Navigating;
        void NavigateTo(Uri pageUri);
        void GoBack();
    }
}