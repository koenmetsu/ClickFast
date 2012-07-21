using System;

namespace ClickFast.Model
{
    public interface IGame
    {
        bool UserCanPress { get; }
        double SecondsPassed { get; }
        event EventHandler CountDownStarted;
        event GenericEventHandler<int> CountdownTick;
        event EventHandler WaitForItStarted;
        event EventHandler ClickFastStarted;
        event GenericEventHandler<bool> UserCanPressChanged;
        event GenericEventHandler<bool> Ended;
        void StartCountDown();
        void Retry();
        void UserClick();
    }
}