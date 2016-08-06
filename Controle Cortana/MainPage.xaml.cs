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
using Windows.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;
using Windows.UI;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Controle_Cortana
{
    public sealed partial class MainPage : Page
    {
        public static MainPage Current;

        ApplicationDataContainer localSettings = null;

        public LightSensor _lightsensor;

        Uri liga_quarto = new Uri("http://192.168.1.2/?pin=LIGA1");
        Uri desliga_quarto = new Uri("http://192.168.1.2/?pin=DESLIGA1");
        Uri liga_sala = new Uri("http://192.168.1.2/?pin=LIGA2");
        Uri desliga_sala = new Uri("http://192.168.1.2/?pin=DESLIGA2");
        Uri LIGATODOSURI = new Uri("http://192.168.1.2/?pin=LIGA3");
        Uri DESLIGATODOSURI = new Uri("http://192.168.1.2/?pin=DESLIGA3");

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
        const string diasSemanaAtivosTextBlockSetting = "diasSemanaAtivosTextBlockSetting";
        const string ligarQuarto = "ligarQuarto";
        const string desligarQuarto = "desligarQuarto";
        const string ligarSala = "ligarSala";
        const string desligarSala = "desligarSala";
        const string LIGARTODOS = "LIGARTODOS";
        const string DESLIGARTODOS = "DESLIGARTODOS";
        const string TODOSSETTINGS = "LIGARTODOSSETTINGS";
        const string COMBOBOXCOMODOSSETTINGS = "COMBOBOXCOMODOSSETTINGS";
        const string SENSORTOGGLESWITCHSETTINGS = "SENSORTOGGLESWITCHSETTINGS";

        bool boolTimerToggle = false;
        bool boolQuartoCheckBox = false;
        bool boolSalaCheckBox = false;
        bool travaInicial = false;

        int horaProgramada;
        int minutoProgramado;

        public MainPage()
        {
            this.InitializeComponent();

            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = Colors.DarkBlue;
            Current = this;
            localSettings = ApplicationData.Current.LocalSettings;
            mostrarTimer();
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
                timerToggleSwitch.IsOn = (bool)valuetimerToggle;
                boolTimerToggle = (bool)valuetimerToggle;
            }
            object valueSensorToggleSwitch = localSettings.Values[SENSORTOGGLESWITCHSETTINGS];
            if (valueSensorToggleSwitch != null)
            {
                sensorToggleSwitch.IsOn = (bool)valueSensorToggleSwitch;
            }
            object diasSemanaAtivosTextBlockValue = localSettings.Values[diasSemanaAtivosTextBlockSetting];
            if (diasSemanaAtivosTextBlockValue != null)
            {
                diasSemanaAtivosTextBlock.Text = (string)localSettings.Values[diasSemanaAtivosTextBlockSetting];
            }
            object todosToggleValue = localSettings.Values[TODOSSETTINGS];
            if (todosToggleValue != null)
            {
                todosToggleSwitch.IsOn = (bool)localSettings.Values[TODOSSETTINGS];
            }
            await Task.Delay(delay);
            travaInicial = true;
        }

        public async void ligarDesligar(bool enviarSinalServidor, string comodo, bool flyout)
        {
            HttpClient client = new HttpClient();

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
                                notificacaoTextBlock.Text = "Não foi possível \rconectar com o servidor.";
                                FlyoutBase.ShowAttachedFlyout(pagePage);
                            }
                        }
                    }
                    client.Dispose();
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
                                notificacaoTextBlock.Text = "Não foi possível \rconectar com o servidor.";
                                FlyoutBase.ShowAttachedFlyout(pagePage);
                            }
                        }
                    }
                    client.Dispose();
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
                                notificacaoTextBlock.Text = "Não foi possível \rconectar com o servidor.";
                                FlyoutBase.ShowAttachedFlyout(pagePage);
                            }
                        }
                    }
                    client.Dispose();
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
                                notificacaoTextBlock.Text = "Não foi possível \rconectar com o servidor.";
                                FlyoutBase.ShowAttachedFlyout(pagePage);
                            }
                        }
                    }
                    client.Dispose();
                    break;

                case LIGARTODOS:
                    localSettings.Values[TODOSSETTINGS] = false;
                    if (enviarSinalServidor)
                    {
                        try
                        {
                            var result = await client.GetStringAsync(LIGATODOSURI);
                        }
                        catch
                        {
                            if (flyout)
                            {
                                notificacaoTextBlock.Text = "Não foi possível \rconectar com o servidor.";
                                FlyoutBase.ShowAttachedFlyout(pagePage);
                            }
                        }
                    }
                    client.Dispose();
                    break;

                case DESLIGARTODOS:
                    localSettings.Values[TODOSSETTINGS] = false;
                    if (enviarSinalServidor)
                    {
                        try
                        {
                            var result = await client.GetStringAsync(DESLIGATODOSURI);
                        }
                        catch
                        {
                            if (flyout)
                            {
                                notificacaoTextBlock.Text = "Não foi possível \rconectar com o servidor.";
                                FlyoutBase.ShowAttachedFlyout(pagePage);
                            }
                        }
                    }
                    client.Dispose();
                    break;
            }
        }

        public void toggleSwitchQuarto_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitchQuarto = sender as ToggleSwitch;
            if (toggleSwitchQuarto != null)
            {
                if (toggleSwitchQuarto.IsOn)
                {
                    ligarDesligar(travaInicial, ligarQuarto, true);
                }
                else
                {
                    ligarDesligar(travaInicial, desligarQuarto, true);
                }
            }
        }

        public void toggleSwitchSala_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitchSala = sender as ToggleSwitch;
            if (toggleSwitchSala != null)
            {
                if (toggleSwitchSala.IsOn)
                {
                    ligarDesligar(travaInicial, ligarSala, true);
                }
                else
                {
                    ligarDesligar(travaInicial, desligarSala, true);
                }
            }
        }

        public void toggleAutomaticoQuarto_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleAutomaticoQuarto = sender as ToggleSwitch;
            if (toggleAutomaticoQuarto != null)
            {
                if (toggleAutomaticoQuarto.IsOn)
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
            ToggleSwitch toggleAutomaticoSala = sender as ToggleSwitch;
            if (toggleAutomaticoSala != null)
            {
                if (toggleAutomaticoSala.IsOn)
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
                if (reading.IlluminanceInLux < 2)
                {
                    ligarAutomaticamenteSensor(500);
                }
            });
        }

        bool travaRootPivot = true;
        bool travaInicialPivot = true;

        private void ligarAutomaticamenteSensor(int delay)
        {
            if (rootPivot.SelectedIndex == 0)
            {
                travaRootPivot = true;
            }
            if ((rootPivot.SelectedIndex == 1 && travaRootPivot) || travaInicialPivot)
            {
                if (sensorToggleSwitch.IsOn)
                {
                    travaInicialPivot = false;
                    travaRootPivot = false;
                    if (sensorComboBox.SelectedItem == quartoAutoTextBlock)
                    {
                        ligarDesligar(true, ligarQuarto, true);
                    }
                    else if (sensorComboBox.SelectedItem == salaAutoTextBlock)
                    {
                        ligarDesligar(true, ligarSala, true);
                    }

                    else if (sensorComboBox.SelectedItem == todosAutoTextBlock)
                    {
                        ligarDesligar(true, LIGARTODOS, true);
                    }
                }
            }
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
                pivotItemSensor.Visibility = Visibility.Collapsed;
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
                    //alarmeSalvoTextBlock.Text = (int)valueHoraProgramada + ":0" + (int)valueMinutoProgramada;
                }
                else
                {
                    //alarmeSalvoTextBlock.Text = (int)valueHoraProgramada + ":" + (int)valueMinutoProgramada;
                }
            }
        }

        public void alarmeSalvoTextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(pagePage);
        }

        ThreadPoolTimer _periodicTimer = null;

        public void timerToggle_Toggled(object sender, RoutedEventArgs e)
        {
            _periodicTimer = ThreadPoolTimer.CreatePeriodicTimer(new TimerElapsedHandler(verificarDiasSemana), TimeSpan.FromSeconds(1));
            localSettings.Values[timerToggleSetting] = timerToggleSwitch.IsOn;
            boolTimerToggle = timerToggleSwitch.IsOn;
        }

        public async void verificarDiasSemana(ThreadPoolTimer timer)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.High,
            () =>
            {
                DateTime dataTempo = DateTime.Now;
                int horaNow = dataTempo.Hour;
                int minutoNow = dataTempo.Minute;
                int segundoNow = dataTempo.Second;

                string itemSelecionadoText = string.Empty;
                CheckBox[] diasCheckBox = {segundaCheckBox,tercaCheckBox,quartaCheckBox,
                                           quintaCheckBox,sextaCheckBox,sabadoCheckBox,domingoCheckBox};

                foreach (CheckBox c in diasCheckBox)
                {
                    if (c.IsChecked == true)
                    {
                        if (itemSelecionadoText.Length > 1)
                        {
                            itemSelecionadoText += ", ";
                        }
                        itemSelecionadoText += c.Content;
                    }
                }
                if (itemSelecionadoText == "")
                {
                    diasSemanaAtivosTextBlock.Text = "Nenhum dia selecionado.";
                }
                else
                {
                    diasSemanaAtivosTextBlock.Text = itemSelecionadoText;
                }

                string[] diasSemana = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
                bool[] diasCheckBoxIsChecked = {(bool)segundaCheckBox.IsChecked,(bool)tercaCheckBox.IsChecked,(bool)quartaCheckBox.IsChecked,
                                           (bool)quintaCheckBox.IsChecked,(bool)sextaCheckBox.IsChecked,(bool)sabadoCheckBox.IsChecked,(bool)domingoCheckBox.IsChecked
                };
                for (int i = 0; i < diasCheckBox.Length; i++)
                {
                    if (boolTimerToggle)
                    {
                        if (diasCheckBoxIsChecked[i])
                        {
                            if (diasSemana[i] == dataTempo.DayOfWeek.ToString())
                            {
                                if ((horaNow == horaProgramada) && (minutoNow == minutoProgramado) && (segundoNow == 0))
                                {
                                    if (comboBoxComodos.SelectedItem == quartoTextBlock)
                                    {
                                        notificacaoTextBlock.Text = "Luz do quarto ligada \rno horário programado.";
                                        Flyout.ShowAttachedFlyout(pagePage);
                                        toggleSwitchQuarto.IsOn = true;
                                    }
                                    else if (comboBoxComodos.SelectedItem == salaTextBlock)
                                    {
                                        notificacaoTextBlock.Text = "Luz da sala ligada \rno horário programado.";
                                        Flyout.ShowAttachedFlyout(pagePage);
                                        toggleSwitchSala.IsOn = true;
                                    }
                                    else if (comboBoxComodos.SelectedItem == todosTextBlock)
                                    {
                                        notificacaoTextBlock.Text = "Luz da sala ligada \rno horário programado.";
                                        Flyout.ShowAttachedFlyout(pagePage);
                                        todosToggleSwitch.IsOn = true;
                                    }
                                }
                            }
                        }
                    }
                }
            });
        }

        bool theme = false;
        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            theme = !theme;
            if (theme)
            {
                rootPivot.RequestedTheme = ElementTheme.Dark;
                commandBar.RequestedTheme = ElementTheme.Dark;
            }
            else
            {
                rootPivot.RequestedTheme = ElementTheme.Light;
                commandBar.RequestedTheme = ElementTheme.Light;
            }
        }

        private void todosToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch todosToggleSwitch = sender as ToggleSwitch;
            if (todosToggleSwitch != null)
            {
                if (todosToggleSwitch.IsOn)
                {
                    ligarDesligar(travaInicial, LIGARTODOS, true);
                }
                else
                {
                    ligarDesligar(travaInicial, DESLIGARTODOS, true);
                }
            }
        }

        private void timerTimePicker_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {
            horaProgramada = timerTimePicker.Time.Hours;
            minutoProgramado = timerTimePicker.Time.Minutes;

            localSettings.Values[horaTimerSetting] = horaProgramada;
            localSettings.Values[minutoTimerSetting] = minutoProgramado;
            if (minutoProgramado > 9)
            {
                horaProgramadaTextBlock.Text = "" + horaProgramada + ":" + minutoProgramado;
            }
            else
            {
                horaProgramadaTextBlock.Text = "" + horaProgramada + ":0" + minutoProgramado;
            }
            timerToggleSwitch.IsOn = true;
        }

        private void todosCheckBox_Click(object sender, RoutedEventArgs e)
        {
            string itemSelecionadoText = string.Empty;
            CheckBox[] diasCheckBox = {segundaCheckBox,tercaCheckBox,quartaCheckBox,
                                       quintaCheckBox,sextaCheckBox,sabadoCheckBox,domingoCheckBox};
            if (todosCheckBox.IsChecked == true)
            {
                foreach (CheckBox c in diasCheckBox)
                {
                    c.IsChecked = true;
                    if (itemSelecionadoText.Length > 1)
                    {
                        itemSelecionadoText += ", ";
                    }
                    itemSelecionadoText += c.Content;
                }
                diasSemanaAtivosTextBlock.Text = itemSelecionadoText;
            }
            else
            {
                foreach (CheckBox c in diasCheckBox)
                {
                    c.IsChecked = false;
                }
            }
        }

        private void diasSemanaAtivosCabecarioTextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(timerTextBlock);
        }

        private void sensorToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            localSettings.Values[SENSORTOGGLESWITCHSETTINGS] = sensorToggleSwitch.IsOn;
        }

        private void sensorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sensorToggleSwitch.IsOn = true;
        }

        private void comboBoxComodos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            timerToggleSwitch.IsOn = true;
        }
    }
}





