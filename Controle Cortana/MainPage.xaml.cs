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
            object segundaCheckBoxValue = localSettings.Values[Monday];
            if (segundaCheckBoxValue != null)
            {
                segundaCheckBox.IsChecked = (bool)segundaCheckBoxValue;
            }
            object tercaCheckBoxValue = localSettings.Values[Tuesday];
            if (tercaCheckBoxValue != null)
            {
                tercaCheckBox.IsChecked = (bool)tercaCheckBoxValue;
            }
            object quartaCheckBoxValue = localSettings.Values[Wednesday];
            if (quartaCheckBoxValue != null)
            {
                quartaCheckBox.IsChecked = (bool)localSettings.Values[Wednesday];
            }
            object quintaCheckBoxValue = localSettings.Values[Thursday];
            if (quintaCheckBoxValue != null)
            {
                quintaCheckBox.IsChecked = (bool)localSettings.Values[Thursday];
            }
            object sextaCheckBoxValue = localSettings.Values[Friday];
            if (sextaCheckBoxValue != null)
            {
                sextaCheckBox.IsChecked = (bool)localSettings.Values[Friday];
            }
            object sabadoCheckBoxValue = localSettings.Values[Saturday];
            if (sabadoCheckBoxValue != null)
            {
                sabadoCheckBox.IsChecked = (bool)localSettings.Values[Saturday];
            }
            object domindoCheckBoxValue = localSettings.Values[Sunday];
            if (domindoCheckBoxValue != null)
            {
                domingoCheckBox.IsChecked = (bool)localSettings.Values[Sunday];
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
                                toggleSwitchQuartoTextBlockFlyout.Text = "Não foi possível \rconectar com o servidor.";
                                FlyoutBase.ShowAttachedFlyout(toggleSwitchQuarto);
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
                                toggleSwitchQuartoTextBlockFlyout.Text = "Não foi possível \rconectar com o servidor.";
                                FlyoutBase.ShowAttachedFlyout(toggleSwitchQuarto);
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
                                toggleSwitchSalaTextBlockFlyout.Text = "Não foi possível \rconectar com o servidor.";
                                FlyoutBase.ShowAttachedFlyout(toggleSwitchSala);
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
                                toggleSwitchSalaTextBlockFlyout.Text = "Não foi possível \rconectar com o servidor.";
                                FlyoutBase.ShowAttachedFlyout(toggleSwitchSala);
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
                                toggleSwitchSalaTextBlockFlyout.Text = "Não foi possível \rconectar com o servidor.";
                                FlyoutBase.ShowAttachedFlyout(toggleSwitchSala);
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
                                toggleSwitchSalaTextBlockFlyout.Text = "Não foi possível \rconectar com o servidor.";
                                FlyoutBase.ShowAttachedFlyout(toggleSwitchSala);
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

        private async void ligarAutomaticamenteSensor(int delay)
        {
            if (rootPivot.SelectedIndex == 0)
            {
                travaRootPivot = true;
            }
            if ((rootPivot.SelectedIndex == 1 && travaRootPivot) || travaInicialPivot)
            {
                if (travaInicialPivot)
                {
                    await Task.Delay(delay);
                }
                travaInicialPivot = false;
                travaRootPivot = false;
                if (toggleAutomaticoQuarto.IsOn)
                {
                    if (toggleSwitchQuarto.IsOn)
                    {
                        ligarDesligar(true, ligarQuarto, true);
                    }
                    else
                    {
                        toggleSwitchQuarto.IsOn = true;
                    }
                }
                if (toggleAutomaticoSala.IsOn)
                {
                    if (toggleSwitchSala.IsOn)
                    {
                        ligarDesligar(true, ligarSala, true);
                    }
                    else
                    {
                        toggleSwitchSala.IsOn = true;
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
                //pivotItemSensor.Visibility = Visibility.Collapsed;
                toggleAutomaticoQuarto.Visibility = Visibility.Collapsed;
                toggleAutomaticoSala.Visibility = Visibility.Collapsed;
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

        const string Monday = "Monday";
        const string Tuesday = "Tuesday";
        const string Wednesday = "Wednesday";
        const string Thursday = "Thursday";
        const string Friday = "Friday";
        const string Saturday = "Saturday";
        const string Sunday = "Sunday";

        string semanasTextBlock;

        public void diasSemanaTextBlock(string segunda, string terca, string quarta, string quinta, string sexta, string sabado, string domingo)
        {
            bool[] diasCheckBox = {(bool)segundaCheckBox.IsChecked,(bool)tercaCheckBox.IsChecked,(bool)quartaCheckBox.IsChecked,
                                   (bool)quintaCheckBox.IsChecked,(bool)sextaCheckBox.IsChecked,(bool)sabadoCheckBox.IsChecked,(bool)domingoCheckBox.IsChecked
            };
            string[] diasSemanaTextBox = { segunda, terca, quarta, quinta, sexta, sabado, domingo };
            semanasTextBlock = "";

            for (int i = 0; i < diasCheckBox.Length; i++)
            {
                int x = diasCheckBox.Rank;
                if (diasCheckBox[i])
                {
                    semanasTextBlock += " " + diasSemanaTextBox[i];
                }
                else
                {
                    semanasTextBlock += "";
                }
                diasSemanaAtivosTextBlock.Text = semanasTextBlock;
                localSettings.Values[diasSemanaAtivosTextBlockSetting] = diasSemanaAtivosTextBlock.Text;
            }
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

        private void segundaCheckBox_Click(object sender, RoutedEventArgs e)
        {
            localSettings.Values[Monday] = segundaCheckBox.IsChecked;
            diasSemanaTextBlock("seg", "ter", "qua", "qui", "sex", "sab", "dom");
        }

        private void tercaCheckBox_Click(object sender, RoutedEventArgs e)
        {
            localSettings.Values[Tuesday] = tercaCheckBox.IsChecked;
            diasSemanaTextBlock("seg", "ter", "qua", "qui", "sex", "sab", "dom");
        }

        private void quartaCheckBox_Click(object sender, RoutedEventArgs e)
        {
            localSettings.Values[Wednesday] = quartaCheckBox.IsChecked;
            diasSemanaTextBlock("seg", "ter", "qua", "qui", "sex", "sab", "dom");
        }

        private void quintaCheckBox_Click(object sender, RoutedEventArgs e)
        {
            localSettings.Values[Thursday] = quintaCheckBox.IsChecked;
            diasSemanaTextBlock("seg", "ter", "qua", "qui", "sex", "sab", "dom");
        }

        private void sextaCheckBox_Click(object sender, RoutedEventArgs e)
        {
            localSettings.Values[Friday] = sextaCheckBox.IsChecked;
            diasSemanaTextBlock("seg", "ter", "qua", "qui", "sex", "sab", "dom");
        }

        private void sabadoCheckBox_Click(object sender, RoutedEventArgs e)
        {
            localSettings.Values[Saturday] = sabadoCheckBox.IsChecked;
            diasSemanaTextBlock("seg", "ter", "qua", "qui", "sex", "sab", "dom");
        }

        private void domingoCheckBox_Click(object sender, RoutedEventArgs e)
        {
            localSettings.Values[Sunday] = domingoCheckBox.IsChecked;
            diasSemanaTextBlock("seg", "ter", "qua", "qui", "sex", "sab", "dom");
        }

        private void TimePickerFlyout_TimePicked(TimePickerFlyout sender, TimePickedEventArgs args)
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
            timerToggle.IsOn = true;
        }

        private void escolhaDiasSemana_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(diasSemanaAtivosTextBlock);
        }

        private void diasSemanaAtivosTextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(diasSemanaAtivosTextBlock);
        }

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(timerTextBlock);
        }

        public void alarmeSalvoTextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(timerTextBlock);
        }

        ThreadPoolTimer _periodicTimer = null;

        public void timerToggle_Toggled(object sender, RoutedEventArgs e)
        {
            _periodicTimer = ThreadPoolTimer.CreatePeriodicTimer(new TimerElapsedHandler(verificarDiasSemana), TimeSpan.FromSeconds(1));
            localSettings.Values[timerToggleSetting] = timerToggle.IsOn;
            boolTimerToggle = timerToggle.IsOn;
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

                string[] diasSemana = { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday };
                bool[] diasCheckBox = {(bool)segundaCheckBox.IsChecked,(bool)tercaCheckBox.IsChecked,(bool)quartaCheckBox.IsChecked,
                                       (bool)quintaCheckBox.IsChecked,(bool)sextaCheckBox.IsChecked,(bool)sabadoCheckBox.IsChecked,(bool)domingoCheckBox.IsChecked
            };
                for (int i = 0; i < diasCheckBox.Length; i++)
                {
                    if (boolTimerToggle)
                    {
                        if (diasCheckBox[i])
                        {
                            if (diasSemana[i] == dataTempo.DayOfWeek.ToString())
                            {
                                if ((horaNow == horaProgramada) && (minutoNow == minutoProgramado) && (segundoNow == 0))
                                {
                                    if (boolQuartoCheckBox)
                                    {
                                        alarmeTextBlockFlyout.Text = "Luz do quarto ligada \rno horário programado.";
                                        Flyout.ShowAttachedFlyout(escolhaHorarioTextBlock);
                                        toggleSwitchQuarto.IsOn = true;
                                    }
                                    if (boolSalaCheckBox)
                                    {
                                        alarmeTextBlockFlyout.Text = "Luz da sala ligada \rno horário programado.";
                                        Flyout.ShowAttachedFlyout(escolhaHorarioTextBlock);
                                        toggleSwitchSala.IsOn = true;
                                    }
                                }
                            }
                        }
                    }
                }
            });
        }

        public void criarNovosBotoes()
        {


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
                    //ligarDesligar(travaInicial, ligarQuarto, false);
                    //ligarDesligar(travaInicial, ligarSala, false);
                    //toggleSwitchQuarto.IsOn = true;
                    //toggleSwitchSala.IsOn = true;
                }
                else
                {
                    ligarDesligar(travaInicial, DESLIGARTODOS, true);
                    //ligarDesligar(travaInicial, desligarQuarto, false);
                    //ligarDesligar(travaInicial, ligarQuarto, false);
                    //toggleSwitchQuarto.IsOn = false;
                    //toggleSwitchSala.IsOn = false;
                }
            }
        }


        //private void RemoveButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (rectangleItems.Items.Count > 0)
        //        rectangleItems.Items.RemoveAt(0);
        //}

        //private void AddButton_Click(object sender, RoutedEventArgs e)
        //{
        //    string[] diasDaSemanaString = { "domingo", "segunda", "terça", "quarta", "quinta", "sexta", "sabado"};
        //    TextBlock texto = new TextBlock();
        //    texto.Text = "Escolha os dias da semana:";
        //    rectangleItems.Items.Add(texto);
        //    for (int i = 0; i < 7; i++)
        //    {
        //        CheckBox diasDaSemana = new CheckBox();
        //        diasDaSemana.Content = diasDaSemanaString[i];
        //        //Color rectColor = new Color();
        //        //rectColor.R = 200;
        //        //rectColor.A = 250;
        //        //Rectangle myRectangle = new Rectangle();
        //        //myRectangle.Fill = new SolidColorBrush(rectColor);
        //        //myRectangle.Width = 100;
        //        //myRectangle.Height = 100;
        //        //myRectangle.Margin = new Thickness(10);
        //        rectangleItems.Items.Add(diasDaSemana);
        //    }
        //}
    }
}





