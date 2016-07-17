using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage;
using Windows.UI.Core;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.UI.Xaml.Controls.Primitives;
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
        const string horaTimerSetting = "horaTimer";
        const string minutoTimerSetting = "minutoTimer";
        int horaProgramada;
        int minutoProgramado;
        public MainPage()
        {
            this.InitializeComponent();
            localSettings = ApplicationData.Current.LocalSettings;
            mostrarTimer();
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
            object valueHoraProgramada = localSettings.Values[horaTimerSetting];
            if (valueHoraProgramada != null)
            {
                horaProgramada = (int)valueHoraProgramada;
            }
            object valueMinutoProgramada = localSettings.Values[minutoTimerSetting];
            if (valueMinutoProgramada != null)
            {
                minutoProgramado = (int)valueMinutoProgramada;
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
                    FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
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
                    FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
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
                    aviso();
                }
                if (pivotItemSensor.IsHitTestVisible == false)
                {
                    x = true;
                }
                if (i < 2 || j < 2)
                {
                    i++;
                    j++;

                    if ((i == 1 || j == 1) && toggleAutomaticoQuarto.IsOn == true && reading.IlluminanceInLux < 1)
                    {
                        ligarQuartoAuto();
                    }
                    else if ((i == 1 || j == 1) && toggleAutomaticoSala.IsOn == true && reading.IlluminanceInLux < 1)
                    {
                        ligarSalaAuto();
                    }
                }
            });
        }
        public async void aviso()
        {
            ContentDialog noWifiDialog = new ContentDialog()
            {
                Title = "Não é permitido.",
                Content = "Por motivos lógicos não é permitido ligar os dois botões ao mesmo tempo.",
                PrimaryButtonText = ":)"
            };

            ContentDialogResult result = await noWifiDialog.ShowAsync();
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
        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (rootPivot.SelectedIndex > 0)
            {
                // If not at the first item, go back to the previous one.
                rootPivot.SelectedIndex -= 1;
            }
            else
            {
                // The first PivotItem is selected, so loop around to the last item.
                rootPivot.SelectedIndex = rootPivot.Items.Count - 1;
            }
        }
        private void relogioTimerPicker_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {
            horaProgramada = relogioTimerPicker.Time.Hours;
            minutoProgramado = relogioTimerPicker.Time.Minutes;

            localSettings.Values[horaTimerSetting] = horaProgramada;
            localSettings.Values[minutoTimerSetting] = minutoProgramado;

            
            if (minutoProgramado < 10)
            {
                alarmeSalvoTextBlock.Text = horaProgramada + ":0" + minutoProgramado;
            }
            else
            {
                alarmeSalvoTextBlock.Text = horaProgramada + ":" + minutoProgramado;
            }
        }
        public void mostrarTimer()
        {
            object valueHoraProgramada = localSettings.Values[horaTimerSetting];
            object valueMinutoProgramada = localSettings.Values[minutoTimerSetting];
            if ((valueHoraProgramada != null) && (valueMinutoProgramada != null))
            {
                if ((int)valueMinutoProgramada < 10)
                {
                    alarmeSalvoTextBlock.Text = (int)valueHoraProgramada + ":0" + (int)valueMinutoProgramada;
                }
                else
                {
                    alarmeSalvoTextBlock.Text = (int)valueHoraProgramada + ":" + (int)valueMinutoProgramada;
                }
            }

        }
    }
}

