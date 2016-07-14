using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Controle_Cortana
{
    public sealed partial class MainPage : Page
    {
        private LightSensor _lightsensor; // Our app' s lightsensor object
        int i;
        async private void ReadingChanged(object sender, LightSensorReadingChangedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                bool[] togglesIsOn = { toggleAutomaticoQuarto.IsOn, toggleAutomaticoSala.IsOn };
                string[] comodos = { "quarto", "sala" };
                LightSensorReading reading = e.Reading;
                sensorDeLuz.Text = "Lux: " + string.Format("{0,5:0.00}", reading.IlluminanceInLux);

                for (i = 0; i < 2; i++)
                {
                    if (togglesIsOn[i] != null)
                    {
                        if (togglesIsOn[i] == true && reading.IlluminanceInLux < 2)
                        {
                            if (comodos[i] == "quarto")
                            {
                                ligarQuarto();
                            }
                            if (comodos[i] == "sala")
                            {
                                ligarSala();
                            }
                        }
                    }
                }
                i = 0;
            });
        }
        public void Sensor()
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
            else
            {
                toggleAutomaticoQuarto.Visibility = Visibility.Collapsed;
                retangudoAutomaticoQuarto.Visibility = Visibility.Collapsed;
                toggleAutomaticoSala.Visibility = Visibility.Collapsed;
                retangudoAutomaticoSala.Visibility = Visibility.Collapsed;
                textoSensorLuz.Text = ":(";
                sensorDeLuz.Text = "Dispositivo não possui sensor.";
            }
        }
    }
}

