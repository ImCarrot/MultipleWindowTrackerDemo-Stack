using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MultipleWindowTracker.Scenarios
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SecondWindow : Page
    {
        public SecondWindow()
        {
            this.InitializeComponent();
        }

        private void HandleWindowChange(object sender, RoutedEventArgs e)
        {
            var s = sender as Button;
            var conversionSuccessful = Enum.TryParse((string)s.Tag, true, out Services.WindowType TypeOfWindow);
            if (conversionSuccessful)
            {
                Services.WindowLauncherService.Instance.HandleWindowSwitch(TypeOfWindow);
            }
        }
    }
}
