﻿using System;
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

        // This event handler writes the current light-sensor reading to 
        // the textbox named "txtLUX" on the app' s main page.

        async private void ReadingChanged(object sender, LightSensorReadingChangedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                LightSensorReading reading = e.Reading;
                sensorDeLuz.Text = "Lux: " + string.Format("{0,5:0.00}", reading.IlluminanceInLux);

                if (contadorSensor < 2)
                {
                    contadorSensor++;
                }
                if (reading.IlluminanceInLux == 0 && contadorSensor == 1)
                {
                    ligarQuarto();
                }
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
                textoSensorLuz.Text = ":(";
                sensorDeLuz.Text = "Aparelho não possui sensor de luminosidade.";
            }
        }
    }
}

