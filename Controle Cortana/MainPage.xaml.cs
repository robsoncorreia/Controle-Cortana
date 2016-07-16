using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage;
using System.Net.Http;
using System.Net;
using System.IO;
using Windows.UI.Core;
using Windows.Devices.Sensors;
using Windows.Foundation;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Controle_Cortana
{
    public sealed partial class MainPage : Page
    {
        Uri liga_quarto = new Uri("http://192.168.1.2/?pin=LIGA1");
        Uri desliga_quarto = new Uri("http://192.168.1.2/?pin=DESLIGA1");
        Uri liga_sala = new Uri("http://192.168.1.2/?pin=LIGA2");
        Uri desliga_sala = new Uri("http://192.168.1.2/?pin=DESLIGA2");
        ApplicationDataContainer localSettings = null;

        const string settingQuarto = "quartoSetting";
        const string settingSala = "salaSetting";
        const string settingAutoQuarto = "AutoQuartoSetting";
        const string settingAutoSala = "AutoSalaSetting";
        public MainPage()
        {
            this.InitializeComponent();
            localSettings = ApplicationData.Current.LocalSettings;
        }
        void Setting()
        {
            object valueQuarto = localSettings.Values[settingQuarto];
            if (valueQuarto != null)
            {
                toggleSwitchQuarto.IsOn = ((bool)valueQuarto);
            }
            object valueSala = localSettings.Values[settingSala];
            if (valueSala != null)
            {
                toggleSwitchSala.IsOn = ((bool)valueSala);
            }
            object valueAutoQuarto = localSettings.Values[settingAutoQuarto];
            if (valueAutoQuarto != null)
            {
                toggleAutomaticoQuarto.IsOn = ((bool)valueAutoQuarto);
            }
            object valueAutoSala = localSettings.Values[settingAutoSala];
            if (valueAutoSala != null)
            {
                toggleAutomaticoSala.IsOn = ((bool)valueAutoSala);
            }
        }
        public void ligarQuartoAuto()
        {
            web.Navigate(liga_quarto);
            toggleSwitchQuarto.IsOn = true;
        }
        public void ligarSalaAuto()
        {
            web.Navigate(liga_sala);
            toggleSwitchSala.IsOn = true;
        }
        public void ligarQuarto()
        {
            localSettings.Values[settingQuarto] = true;
            web.Navigate(liga_quarto);
        }
        public void desligarQuarto()
        {
            localSettings.Values[settingQuarto] = false;
            web.Navigate(desliga_quarto);
        }
        public void ligarSala()
        {
            localSettings.Values[settingSala] = true;
            web.Navigate(liga_sala);
        }
        public void desligarSala()
        {
            localSettings.Values[settingSala] = false;
            web.Navigate(desliga_sala);
        }

        public void toggleSwitchQuarto_Toggled(object sender, RoutedEventArgs e)
        {
            if (toggleSwitchQuarto != null)
            {
                if (toggleSwitchQuarto.IsOn == false)
                {
                    desligarQuarto();
                }
                else
                {
                    ligarQuarto();
                }
            }
        }
        public void toggleSwitchSala_Toggled(object sender, RoutedEventArgs e)
        {
            if (toggleSwitchSala != null)
            {
                if (toggleSwitchSala.IsOn == false)
                {
                    desligarSala();
                }
                else
                {
                    ligarSala();
                }
            }
        }
        public void toggleAutomaticoQuarto_Toggled(object sender, RoutedEventArgs e)
        {
            if (toggleAutomaticoQuarto != null)
            {
                if (toggleAutomaticoQuarto.IsOn == true)
                {
                    localSettings.Values[settingAutoQuarto] = true;
                }
                else
                {
                    localSettings.Values[settingAutoQuarto] = false;
                }
            }
        }
        public void toggleAutomaticoSala_Toggled(object sender, RoutedEventArgs e)
        {
            if (toggleAutomaticoSala != null)
            {
                if (toggleAutomaticoSala.IsOn == true)
                {
                    localSettings.Values[settingAutoSala] = true;
                }
                else
                {
                    localSettings.Values[settingAutoSala] = false;
                }
            }
        }
        private LightSensor _lightsensor;
        int i;
        bool x;
        int j;
        async private void ReadingChanged(object sender, LightSensorReadingChangedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                LightSensorReading reading = e.Reading;
                sensorDeLuz.Text = "Lux: " + string.Format("{0,5:0.00}", reading.IlluminanceInLux);
                if (pivotItemSensor.IsHitTestVisible == true && x == true)
                {
                    x = false;
                    i = 0;
                }
                if (toggleAutomaticoQuarto.IsOn == true && toggleAutomaticoSala.IsOn == true)
                {
                    toggleAutomaticoQuarto.IsOn = false;
                    toggleAutomaticoSala.IsOn = false;
                }
                if (pivotItemSensor.IsHitTestVisible == false)
                {
                    x = true;
                }
                if (i < 2 || j < 2)
                {
                    i++;
                    j++;

                    if ((i == 1 || j == 1) && toggleAutomaticoQuarto.IsOn == true && reading.IlluminanceInLux < 2)
                    {
                        ligarQuartoAuto();
                    }
                    else if ((i == 1 || j == 1) && toggleAutomaticoSala.IsOn == true && reading.IlluminanceInLux < 2)
                    {
                        ligarSalaAuto();
                    }
                }
            });
        }
        void Sensor()
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
            Sensor();
            Setting();
        }
    }
}
