using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Windows.UI.Core; // Required to access the core dispatcher object
using Windows.Devices.Sensors; // Required to access the sensor platform and the ALS

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/p/?linkid=234238

namespace Controle_Cortana
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage : Page
    {
        private LightSensor _lightsensor; // Our app' s lightsensor object

        // This event handler writes the current light-sensor reading to 
        // the textbox named "txtLUX" on the app' s main page.

        async private void ReadingChanged(object sender, LightSensorReadingChangedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                LightSensorReading reading = e.Reading;
                txtLuxValue.Text = String.Format("{0,5:0.00}", reading.IlluminanceInLux);
            });
        }

        public BlankPage()
        {
            InitializeComponent();
            _lightsensor = LightSensor.GetDefault(); // Get the default light sensor object

            // Assign an event handler for the ALS reading-changed event
            if (_lightsensor != null)
            {
                // Establish the report interval for all scenarios
                uint minReportInterval = _lightsensor.MinimumReportInterval;
                uint reportInterval = minReportInterval > 16 ? minReportInterval : 16;
                _lightsensor.ReportInterval = reportInterval;

                // Establish the even thandler
                _lightsensor.ReadingChanged += new TypedEventHandler<LightSensor, LightSensorReadingChangedEventArgs>(ReadingChanged);
            }

        }
        
        private void txtLuxValue_Tapped(object sender, TappedRoutedEventArgs e)
        {
            
            Frame rootFlame = Window.Current.Content as Frame;
            rootFlame.Navigate(typeof(MainPage));
        }
    }
}