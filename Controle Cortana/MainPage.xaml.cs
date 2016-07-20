using System;
using Windows.Foundation;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Sensors;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.ApplicationModel.Background;
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
        HttpClient client = new HttpClient();
        private LightSensor _lightsensor;
        const string settingQuarto = "quartoSetting";
        const string settingSala = "salaSetting";
        const string settingAutoQuarto = "AutoQuartoSetting";
        const string settingAutoSala = "AutoSalaSetting";
        const string horaTimerSetting = "horaTimer";
        const string minutoTimerSetting = "minutoTimer";
        const string timerToggleSetting = "timerToggleSetting";
        const string quartoCheckBoxSetting = "quartoCheckBoxSetting";
        const string salaCheckBoxSetting = "salaCheckBoxSetting";
        const string cabeçarioAlarmeTextBlockSetting = "cabeçarioAlarmeTextBlock";
        bool travaInicial = false;
        int horaProgramada;
        int minutoProgramado;
        int x;
        public MainPage()
        {
            this.InitializeComponent();
            localSettings = ApplicationData.Current.LocalSettings;
            mostrarTimer();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Sensor();
            Setting(500);
        }
        public async void Setting(int delay)
        {
            await Task.Delay(delay);
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
            object valuetimerToggle = localSettings.Values[timerToggleSetting];
            if (valuetimerToggle != null)
            {
                timerToggle.IsOn = (bool)valuetimerToggle;
            }
            object valueQuartoCheckBox = localSettings.Values[quartoCheckBoxSetting];
            if (valueQuartoCheckBox != null)
            {
                quartoCheckBox.IsChecked = (bool)valueQuartoCheckBox;
            }
            object valueSalaCheckBox = localSettings.Values[salaCheckBoxSetting];
            if (valueSalaCheckBox != null)
            {
                salaCheckBox.IsChecked = (bool)valueSalaCheckBox;
            }
            object valueCabeçarioTextBlock = localSettings.Values[cabeçarioAlarmeTextBlockSetting];
            if (valueCabeçarioTextBlock != null)
            {
                cabeçarioAlarmeTextBlock.Text = (string)valueCabeçarioTextBlock;
            }
            await Task.Delay(delay);
            travaInicial = true;
        }
        public async void ligarQuarto(bool enviarSinalServidor)
        {
            localSettings.Values[settingQuarto] = true;
            if (enviarSinalServidor)
            {
                try
                {
                    var result = await client.GetStringAsync(liga_quarto);
                }
                catch
                {
                    FlyoutBase.ShowAttachedFlyout(rootPivot);
                }
            }

        }
        public async void desligarQuarto(bool enviarSinalServidor)
        {
            localSettings.Values[settingQuarto] = false;
            if (enviarSinalServidor)
            {
                try
                {
                    var result = await client.GetStringAsync(desliga_quarto);
                }
                catch
                {
                    FlyoutBase.ShowAttachedFlyout(rootPivot);
                }
            }
        }
        public async void ligarSala(bool enviarSinalServidor)
        {
            localSettings.Values[settingSala] = true;
            if (enviarSinalServidor)
            {
                try
                {
                    var result = await client.GetStringAsync(liga_sala);
                }
                catch
                {
                    FlyoutBase.ShowAttachedFlyout(rootPivot);
                }
            }
        }
        public async void desligarSala(bool enviarSinalServidor)
        {
            localSettings.Values[settingSala] = false;
            if (enviarSinalServidor)
            {
                try
                {
                    var result = await client.GetStringAsync(desliga_sala);
                }
                catch
                {
                    FlyoutBase.ShowAttachedFlyout(rootPivot);
                }
            }
        }
        public void toggleSwitchQuarto_Toggled(object sender, RoutedEventArgs e)
        {
            if (toggleSwitchQuarto != null)
            {

                if (toggleSwitchQuarto.IsOn == false)
                {
                    desligarQuarto(travaInicial);
                }
                else
                {
                    ligarQuarto(travaInicial);
                }
            }
        }
        public void toggleSwitchSala_Toggled(object sender, RoutedEventArgs e)
        {
            if (toggleSwitchSala != null)
            {
                if (toggleSwitchSala.IsOn == false)
                {
                    desligarSala(travaInicial);
                }
                else
                {
                    ligarSala(travaInicial);
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
        async private void ReadingChanged(object sender, LightSensorReadingChangedEventArgs e)
        {

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                LightSensorReading reading = e.Reading;
                sensorDeLuz.Text = "Lux: " + string.Format("{0,5:0.00}", reading.IlluminanceInLux);
                comparaTempo(horaProgramada, minutoProgramado, 0, 250);
                bool i = true;
                if (travaInicial)
                {
                    if (rootPivot.SelectedIndex == 1)
                    {
                        if (x < 1)
                        {
                            x++;
                            i = false;
                        }
                        else
                        {

                            i = true;
                        }
                    }
                    else
                    {
                        x = 0;
                    }
                }
                else
                {
                    i = false;
                }
                if (i == false && toggleAutomaticoQuarto.IsOn && reading.IlluminanceInLux < 2)
                {
                    toggleSwitchQuarto.IsOn = true;
                }
                if (i == false && toggleAutomaticoSala.IsOn && reading.IlluminanceInLux < 2)
                {
                    toggleSwitchSala.IsOn = true;
                }
            });
        }
        //public async void aviso()
        //{
        //    ContentDialog noWifiDialog = new ContentDialog()
        //    {
        //        Title = "Não é permitido.",
        //        Content = "Por motivos lógicos não é permitido ligar os dois botões ao mesmo tempo.",
        //        PrimaryButtonText = ":)"
        //    };

        //    ContentDialogResult result = await noWifiDialog.ShowAsync();
        //}
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
        public async void comparaTempo(int hora, int minuto, int segundo, int delay)
        {
            DateTime dataTempo = DateTime.Now;
            int horaNow = dataTempo.Hour;
            int minutoNow = dataTempo.Minute;
            int segundoNow = dataTempo.Second;

            if (timerToggle.IsOn == true)
            {
                if ((horaNow == hora) && (minutoNow == minuto) && (segundoNow == segundo))
                {
                    if (quartoCheckBox.IsChecked == true)
                    {
                        toggleSwitchQuarto.IsOn = true;
                    }
                    await Task.Delay(delay);
                    if (salaCheckBox.IsChecked == true)
                    {
                        toggleSwitchSala.IsOn = true;
                    }
                }
            }
        }
        private void timerToggle_Toggled(object sender, RoutedEventArgs e)
        {
            localSettings.Values[timerToggleSetting] = timerToggle.IsOn;
        }
        private void quartoCheckBox_Click(object sender, RoutedEventArgs e)
        {
            localSettings.Values[quartoCheckBoxSetting] = quartoCheckBox.IsChecked;
        }
        private void salaCheckBox_Click(object sender, RoutedEventArgs e)
        {
            localSettings.Values[salaCheckBoxSetting] = salaCheckBox.IsChecked;
        }
        private void cabeçarioAlarmeTextBlock_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(cabeçarioAlarmeTextBlock);
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            cabeçarioAlarmeTextBlock.Text = cabeçarioAlarmeTextBox.Text;
            localSettings.Values[cabeçarioAlarmeTextBlockSetting] = cabeçarioAlarmeTextBox.Text;
        }
        private void frenteAppBarButton(object sender, RoutedEventArgs e)
        {
            if (rootPivot.SelectedIndex < rootPivot.Items.Count - 1)
            {
                // If not at the first item, go back to the previous one.
                rootPivot.SelectedIndex += 1;
            }
            else
            {
                // The first PivotItem is selected, so loop around to the last item.
                rootPivot.SelectedIndex = 0;
            }
        }
        private void voltaAppBarButton(object sender, RoutedEventArgs e)
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
    }
}


