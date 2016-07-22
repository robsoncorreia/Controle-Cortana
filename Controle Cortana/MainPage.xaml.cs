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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Input;
using Windows.System.Threading;
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
        public LightSensor _lightsensor;
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
        const string ligarQuarto = "ligarQuarto";
        const string desligarQuarto = "desligarQuarto";
        const string ligarSala = "ligarSala";
        const string desligarSala = "desligarSala";
        bool boolTimerToggle = false;
        bool boolQuartoCheckBox = false;
        bool boolSalaCheckBox = false;
        bool travaInicial = false;
        int horaProgramada;
        int minutoProgramado;
        int x;
        public MainPage()
        {
            this.InitializeComponent();
            localSettings = ApplicationData.Current.LocalSettings;
            mostrarTimer();
            Sensor();
            Setting(500);
            gatilhoTimer(1);
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
                boolTimerToggle = (bool)valuetimerToggle;
            }
            object valueQuartoCheckBox = localSettings.Values[quartoCheckBoxSetting];
            if (valueQuartoCheckBox != null)
            {
                quartoCheckBox.IsChecked = (bool)valueQuartoCheckBox;
                boolQuartoCheckBox = (bool)valueQuartoCheckBox;
            }
            object valueSalaCheckBox = localSettings.Values[salaCheckBoxSetting];
            if (valueSalaCheckBox != null)
            {
                salaCheckBox.IsChecked = (bool)valueSalaCheckBox;
                boolSalaCheckBox = (bool)valueSalaCheckBox;
            }
            await Task.Delay(delay);
            travaInicial = true;
        }

        public async void ligarDesligar(bool enviarSinalServidor, string comodo, bool flyout)
        {
            switch (comodo)
            {
                case ligarQuarto:
                    localSettings.Values[settingQuarto] = true;
                    if (enviarSinalServidor)
                    {
                        try
                        {
                            var result = await client.GetStringAsync(liga_quarto);
                        }
                        catch
                        {
                            if (flyout)
                            {
                                //await Task.Delay(1000);
                                FlyoutBase.ShowAttachedFlyout(commandBar);
                            }
                        }
                    }
                    break;

                case desligarQuarto:
                    localSettings.Values[settingQuarto] = false;
                    if (enviarSinalServidor)
                    {
                        try
                        {
                            var result = await client.GetStringAsync(desliga_quarto);
                        }
                        catch
                        {

                            if (flyout)
                            {
                                FlyoutBase.ShowAttachedFlyout(commandBar);
                            }
                        }
                    }
                    break;
                case ligarSala:
                    localSettings.Values[settingSala] = true;
                    if (enviarSinalServidor)
                    {
                        try
                        {
                            var result = await client.GetStringAsync(liga_sala);
                        }
                        catch
                        {
                            if (flyout)
                            {
                                FlyoutBase.ShowAttachedFlyout(commandBar);
                            }
                        }
                    }
                    break;
                case desligarSala:
                    localSettings.Values[settingSala] = false;
                    if (enviarSinalServidor)
                    {
                        try
                        {
                            var result = await client.GetStringAsync(desliga_sala);
                        }
                        catch
                        {
                            if (flyout)
                            {
                                FlyoutBase.ShowAttachedFlyout(commandBar);
                            }
                        }
                    }
                    break;
            }
        }
        public void toggleSwitchQuarto_Toggled(object sender, RoutedEventArgs e)
        {
            if (toggleSwitchQuarto != null)
            {
                if (toggleSwitchQuarto.IsOn == false)
                {
                    ligarDesligar(travaInicial, desligarQuarto, true);
                }
                else
                {
                    ligarDesligar(travaInicial, ligarQuarto, true);
                }
            }
        }
        public void toggleSwitchSala_Toggled(object sender, RoutedEventArgs e)
        {
            if (toggleSwitchSala != null)
            {
                if (toggleSwitchSala.IsOn == false)
                {
                    ligarDesligar(travaInicial, desligarSala, true);
                }
                else
                {
                    ligarDesligar(travaInicial, ligarSala, true);
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
        async public void ReadingChanged(object sender, LightSensorReadingChangedEventArgs e)
        {

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                LightSensorReading reading = e.Reading;
                sensorDeLuz.Text = "Lux: " + string.Format("{0,5:0.00}", reading.IlluminanceInLux);
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
                //pivotItemSensor.Visibility = Visibility.Collapsed;
                toggleAutomaticoQuarto.Visibility = Visibility.Collapsed;
                retangudoAutomaticoQuarto.Visibility = Visibility.Collapsed;
                toggleAutomaticoSala.Visibility = Visibility.Collapsed;
                retangudoAutomaticoSala.Visibility = Visibility.Collapsed;
                textoSensorLuz.Text = "😁";
                sensorDeLuz.Text = "Dispositivo não possui sensor.";
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
        public void comparaTempo(int hora, int minuto, int segundo, int delay)
        {
            DateTime dataTempo = DateTime.Now;
            int horaNow = dataTempo.Hour;
            int minutoNow = dataTempo.Minute;
            int segundoNow = dataTempo.Second;

            if (boolTimerToggle)
            {
                if ((horaNow == hora) && (minutoNow == minuto) && (segundoNow == segundo))
                {
                    if (boolQuartoCheckBox)
                    {
                        ligarDesligar(true, ligarQuarto, false);
                    }
                    if (boolSalaCheckBox)
                    {
                        ligarDesligar(true, ligarSala, false);
                    }
                }
            }
        }
        public void timerToggle_Toggled(object sender, RoutedEventArgs e)
        {
            localSettings.Values[timerToggleSetting] = timerToggle.IsOn;
            boolTimerToggle = timerToggle.IsOn;
        }
        public void quartoCheckBox_Click(object sender, RoutedEventArgs e)
        {
            localSettings.Values[quartoCheckBoxSetting] = quartoCheckBox.IsChecked;
            boolQuartoCheckBox = (bool)quartoCheckBox.IsChecked;
        }
        public void salaCheckBox_Click(object sender, RoutedEventArgs e)
        {
            localSettings.Values[salaCheckBoxSetting] = salaCheckBox.IsChecked;
            boolSalaCheckBox = (bool)salaCheckBox.IsChecked;
        }
        public void alarmeSalvoTextBlock_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(timePickerRectangle);
        }
        public void TextBlock_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(timePickerRectangle);
        }
        public void voltaAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (rootPivot.SelectedIndex > 0)
            {
                rootPivot.SelectedIndex -= 1;
            }
            else
            {
                rootPivot.SelectedIndex = rootPivot.Items.Count - 1;
            }
        }
        public void frenteAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (rootPivot.SelectedIndex < rootPivot.Items.Count - 1)
            {
                rootPivot.SelectedIndex += 1;
            }
            else
            {
                rootPivot.SelectedIndex = 0;
            }
        }
        public void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(timePickerRectangle);
        }
        ThreadPoolTimer _periodicTimer = null;
        public void gatilhoTimer(int intervaloEmsegundos)
        {
            _periodicTimer = ThreadPoolTimer.CreatePeriodicTimer(new TimerElapsedHandler(PeriodicTimerCallback), TimeSpan.FromSeconds(intervaloEmsegundos));
        }
        public void PeriodicTimerCallback(ThreadPoolTimer timer)
        {
            comparaTempo(horaProgramada, minutoProgramado, 1, 0);
        }
        public void relogioTimerPicker_TimePicked(TimePickerFlyout sender, TimePickedEventArgs args)
        {

        }

        private void relogioTimerPicker_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {
            horaProgramada = timerPicker.Time.Hours;
            minutoProgramado = timerPicker.Time.Minutes;

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
    }
}



