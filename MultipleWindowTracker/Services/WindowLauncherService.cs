using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MultipleWindowTracker.Services
{
    internal class WindowLauncherService
    {
        public static WindowLauncherService Instance { get; } = new WindowLauncherService();

        private Dictionary<int, WindowType> WindowLogs { get; set; }

        internal async void HandleWindowSwitch(WindowType typeOfWindow)
        {
            if (WindowLogs?.ContainsValue(typeOfWindow) == true)
            {

                var existingViewID = WindowLogs?.FirstOrDefault(x => x.Value == typeOfWindow).Key;
                if (existingViewID != null)
                {
                    await ApplicationViewSwitcher.TryShowAsStandaloneAsync(Convert.ToInt32(existingViewID));
                }
                else
                {
                    //Handle Error Here!
                }
            }
            else
            {
                await OpenNewWindow(typeOfWindow);
            }
        }


        /// <summary>
        /// Logs the new window.
        /// </summary>
        /// <param name="WindowID">The window identifier.</param>
        /// <param name="typeOfWindow">The type of window.</param>
        private void LogNewWindow(int WindowID, WindowType typeOfWindow)
        {
            if (WindowLogs?.ContainsKey(WindowID) == true)
                return;

            if (WindowLogs == null)
                WindowLogs = new Dictionary<int, WindowType>();

            WindowLogs.Add(WindowID, typeOfWindow);
        }


        /// <summary>
        /// Opens the new window and if success logs it.
        /// </summary>
        /// <param name="typeOfWindow">The type of window.</param>
        /// <returns></returns>
        private async Task OpenNewWindow(WindowType typeOfWindow)
        {
            Type windowToLaunch = null;
            switch (typeOfWindow)
            {
                case WindowType.Main:
                    windowToLaunch = typeof(MainPage);
                    break;
                case WindowType.First:
                    windowToLaunch = typeof(Scenarios.FirstWindow);
                    break;
                case WindowType.Second:
                    windowToLaunch = typeof(Scenarios.SecondWindow);
                    break;
                case WindowType.Third:
                    windowToLaunch = typeof(Scenarios.ThridWindow);
                    break;
                default:
                    return;
            }



            CoreApplicationView newView = CoreApplication.CreateNewView();

            var currentFrame = Window.Current.Content as Frame;
            if (currentFrame.Content is MainPage)
            {
                var mainFrameID = ApplicationView.GetForCurrentView().Id;
                LogNewWindow(mainFrameID, WindowType.Main);
            }

            int newViewId = 0;
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Frame frame = new Frame();
                frame.Navigate(windowToLaunch, null);
                Window.Current.Content = frame;
                // You have to activate the window in order to show it later.
                Window.Current.Activate();

                newViewId = ApplicationView.GetForCurrentView().Id;
            });
            bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
            if (viewShown)
            {
                LogNewWindow(newViewId, typeOfWindow);
            }
            else
            {
                //handle error on launch here!
            }
        }

    }

    public enum WindowType
    {
        Main,
        First,
        Second,
        Third
    }
}
