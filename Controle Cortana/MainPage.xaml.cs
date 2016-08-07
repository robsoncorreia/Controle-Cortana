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
using Windows.Media.SpeechSynthesis;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Controle_Cortana
{
    public sealed partial class MainPage : Page
    {


        ApplicationDataContainer localSettings = null;

        public LightSensor _lightsensor;

        Uri liga_quarto = new Uri("http://192.168.1.2/?pin=LIGA1");
        Uri desliga_quarto = new Uri("http://192.168.1.2/?pin=DESLIGA1");
        Uri liga_sala = new Uri("http://192.168.1.2/?pin=LIGA2");
        Uri desliga_sala = new Uri("http://192.168.1.2/?pin=DESLIGA2");
        Uri LIGATODOSURI = new Uri("http://192.168.1.2/?pin=LIGA3");
        Uri DESLIGATODOSURI = new Uri("http://192.168.1.2/?pin=DESLIGA3");

        const string SETTINGAUTOQUARTO = "AUTOQUARTOSETTING";
        const string SETTINGAUTOSALA = "AUTOSALASETTING";
        const string HORATIMERSETTING = "HORATIMER";
        const string MINUTOTIMERSETTING = "MINUTOTIMER";
        const string LIGARQUARTO = "LIGARQUARTO";
        const string DESLIGARQUARTO = "DESLIGARQUARTO";
        const string LIGARSALA = "LIGARSALA";
        const string DESLIGARSALA = "DESLIGARSALA";
        const string LIGARTODOS = "LIGARTODOS";
        const string DESLIGARTODOS = "DESLIGARTODOS";
        public string semConeccao = "🖥 Não foi possível conectar com o servidor.";
        bool boolTimerToggle = false;
        bool travaInicial = false;
        bool travaRootPivot = true;
        bool travaInicialPivot = true;
        int horaProgramada;
        int minutoProgramado;

        public MainPage()
        {
            this.InitializeComponent();

            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = Colors.DarkBlue;

            localSettings = ApplicationData.Current.LocalSettings;
            mostrarTimer();
            Sensor();
            gatilhoTimerElapsedHandler();
            Setting(400);
        }

        public async void Setting(int delay)
        {
            await Task.Delay(delay);
            object valueQuarto = localSettings.Values[toggleSwitchQuarto.Name.ToString()];
            if (valueQuarto != null)
            {
                toggleSwitchQuarto.IsOn = ((bool)valueQuarto);
            }
            object valueSala = localSettings.Values[toggleSwitchSala.Name.ToString()];
            if (valueSala != null)
            {
                toggleSwitchSala.IsOn = ((bool)valueSala);
            }
            object valueHoraProgramada = localSettings.Values[HORATIMERSETTING];
            if (valueHoraProgramada != null)
            {
                horaProgramada = (int)valueHoraProgramada;
            }
            object valueMinutoProgramada = localSettings.Values[MINUTOTIMERSETTING];
            if (valueMinutoProgramada != null)
            {
                minutoProgramado = (int)valueMinutoProgramada;
            }
            object valuetimerToggle = localSettings.Values[timerToggleSwitch.Name.ToString()];
            if (valuetimerToggle != null)
            {
                timerToggleSwitch.IsOn = (bool)localSettings.Values[timerToggleSwitch.Name.ToString()];
                boolTimerToggle = timerToggleSwitch.IsOn;
            }
            object valueSensorToggleSwitch = localSettings.Values[sensorToggleSwitch.Name.ToString()];
            if (valueSensorToggleSwitch != null)
            {
                sensorToggleSwitch.IsOn = (bool)valueSensorToggleSwitch;
            }
            object todosToggleValue = localSettings.Values[todosToggleSwitch.Name.ToString()];
            if (todosToggleValue != null)
            {
                todosToggleSwitch.IsOn = (bool)todosToggleValue;
            }
            CheckBox[] diasCheckBox = { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday };

            foreach (CheckBox c in diasCheckBox)
            {
                if (localSettings.Values[c.Content.ToString()] != null)
                {
                    c.IsChecked = (bool)localSettings.Values[c.Content.ToString()];
                }
            }
            if (localSettings.Values[comboBoxComodos.Name.ToString()] != null)
            {
                comboBoxComodos.SelectedIndex = (int)localSettings.Values[comboBoxComodos.Name.ToString()];
            }
            if (localSettings.Values[sensorComboBox.Name.ToString()] != null)
            {
                sensorComboBox.SelectedIndex = (int)localSettings.Values[sensorComboBox.Name.ToString()];
            }
            if (localSettings.Values[semanaDiasRun.Name.ToString()] != null)
            {
                semanaDiasRun.Text = (string)localSettings.Values[semanaDiasRun.Name.ToString()];
            }
            await Task.Delay(delay);
            travaInicial = true;
        }

        public async void falarString(string oQueSeraDito)
        {
            // The string to speak with SSML customizations.
            string Ssml =
                @"<speak version='1.0' " + "xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='pt-BR'>"
                + oQueSeraDito +
                "<prosody rate='slow' contour='(0%,+20Hz) (10%,+30%) (40%,+10Hz)'/>" +
                "</speak>";

            // The media object for controlling and playing audio.
            MediaElement mediaElement = new MediaElement();
            var synth = new SpeechSynthesizer();

            // Generate the audio stream from plain text.
            SpeechSynthesisStream stream = await synth.SynthesizeSsmlToStreamAsync(Ssml);

            // Send the stream to the media object.
            mediaElement.SetSource(stream, stream.ContentType);
            mediaElement.Play();
        }

        public async void ligarDesligar(bool enviarSinalServidor, string comodo, bool flyout)
        {

            HttpClient client = new HttpClient();

            switch (comodo)
            {
                case LIGARQUARTO:
                    progresso.IsActive = true;
                    localSettings.Values[toggleSwitchQuarto.Name.ToString()] = true;
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
                                notificacaoTextBlock.Text = semConeccao;
                                FlyoutBase.ShowAttachedFlyout(pagePage);
                                if (fala)
                                {
                                    falarString(semConeccao);
                                }
                            }
                        }
                    }
                    client.Dispose();
                    progresso.IsActive = false;
                    break;
                case DESLIGARQUARTO:
                    progresso.IsActive = true;
                    localSettings.Values[toggleSwitchQuarto.Name.ToString()] = false;
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
                                notificacaoTextBlock.Text = semConeccao;
                                FlyoutBase.ShowAttachedFlyout(pagePage);
                                if (fala)
                                {
                                    falarString(semConeccao);
                                }
                            }
                        }
                    }
                    client.Dispose();
                    progresso.IsActive = false;
                    break;
                case LIGARSALA:
                    progresso.IsActive = true;
                    localSettings.Values[toggleSwitchSala.Name.ToString()] = true;
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
                                notificacaoTextBlock.Text = semConeccao;
                                FlyoutBase.ShowAttachedFlyout(pagePage);
                                if (fala)
                                {
                                    falarString(semConeccao);
                                }
                            }
                        }
                    }
                    client.Dispose();
                    progresso.IsActive = false;
                    break;
                case DESLIGARSALA:
                    progresso.IsActive = true;
                    localSettings.Values[toggleSwitchSala.Name.ToString()] = false;
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
                                notificacaoTextBlock.Text = semConeccao;
                                FlyoutBase.ShowAttachedFlyout(pagePage);
                                if (fala)
                                {
                                    falarString(semConeccao);
                                }
                            }
                        }
                    }
                    client.Dispose();
                    progresso.IsActive = false;
                    break;

                case LIGARTODOS:
                    progresso.IsActive = true;
                    localSettings.Values[todosToggleSwitch.Name.ToString()] = true;
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
                                notificacaoTextBlock.Text = semConeccao;
                                FlyoutBase.ShowAttachedFlyout(pagePage);
                                if (fala)
                                {
                                    falarString(semConeccao);
                                }
                            }
                        }
                    }
                    client.Dispose();
                    progresso.IsActive = false;
                    break;

                case DESLIGARTODOS:
                    progresso.IsActive = true;
                    localSettings.Values[todosToggleSwitch.Name.ToString()] = false;
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
                                notificacaoTextBlock.Text = semConeccao;
                                FlyoutBase.ShowAttachedFlyout(pagePage);
                                if (fala)
                                {
                                    falarString(semConeccao);
                                }
                            }
                        }
                    }
                    client.Dispose();
                    progresso.IsActive = false;
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
                    ligarDesligar(travaInicial, LIGARQUARTO, true);
                }
                else
                {
                    ligarDesligar(travaInicial, DESLIGARQUARTO, true);
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
                    ligarDesligar(travaInicial, LIGARSALA, true);
                }
                else
                {
                    ligarDesligar(travaInicial, DESLIGARSALA, true);
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
                    localSettings.Values[SETTINGAUTOQUARTO] = true;
                }
                else
                {
                    localSettings.Values[SETTINGAUTOQUARTO] = false;
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
                        ligarDesligar(true, LIGARQUARTO, true);
                    }
                    else if (sensorComboBox.SelectedItem == salaAutoTextBlock)
                    {
                        ligarDesligar(true, LIGARSALA, true);
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
            textoSensorLuz.Visibility = Visibility.Collapsed;
            sensorDeLuz.Text = "O dispositivo não possui\nsensor de luminosidade.💡";
        }

        public void mostrarTimer()
        {
            object valueHoraProgramada = localSettings.Values[HORATIMERSETTING];
            object valueMinutoProgramada = localSettings.Values[MINUTOTIMERSETTING];
            if ((valueHoraProgramada != null) && (valueMinutoProgramada != null))
            {
                if ((int)valueMinutoProgramada < 10)
                {
                    horaProgramadaTextBlock.Text = (int)valueHoraProgramada + ":0" + (int)valueMinutoProgramada;
                }
                else
                {
                    horaProgramadaTextBlock.Text = (int)valueHoraProgramada + ":" + (int)valueMinutoProgramada;
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
            localSettings.Values[timerToggleSwitch.Name.ToString()] = timerToggleSwitch.IsOn;
            boolTimerToggle = timerToggleSwitch.IsOn;
        }

        public void gatilhoTimerElapsedHandler()
        {
            _periodicTimer = ThreadPoolTimer.CreatePeriodicTimer(new TimerElapsedHandler(verificarDiasSemana), TimeSpan.FromSeconds(1));
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
                string hoje = dataTempo.DayOfWeek.ToString();

                //if (rootPivot.SelectedIndex == 1)
                //{
                //    abreTimer.Begin();
                //}
                //else
                //{
                //    fechaTimer.Begin();
                //}

                string itemSelecionadoText = string.Empty;
                CheckBox[] diasCheckBox = { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday };

                foreach (CheckBox c in diasCheckBox)
                {
                    localSettings.Values[c.Content.ToString()] = c.IsChecked;
                    if (c.IsChecked == true)
                    {
                        if (itemSelecionadoText.Length > 1)
                        {
                            itemSelecionadoText += ", ";
                        }
                        itemSelecionadoText += c.Content;

                        if ((c.Name.Equals(hoje)) && (c.IsChecked == true) && (boolTimerToggle))
                        {
                            if ((horaNow == horaProgramada) && (minutoNow == minutoProgramado) && (segundoNow == 0))
                            {
                                if (comboBoxComodos.SelectedIndex == 0)
                                {
                                    notificacaoTextBlock.Text = "⌚ Luz do quarto ligada \rno horário programado.";
                                    Flyout.ShowAttachedFlyout(pagePage);
                                    toggleSwitchQuarto.IsOn = true;
                                }
                                else if (comboBoxComodos.SelectedIndex == 1)
                                {
                                    notificacaoTextBlock.Text = "⌚ Luz da sala ligada \rno horário programado.";
                                    Flyout.ShowAttachedFlyout(pagePage);
                                    toggleSwitchSala.IsOn = true;
                                }
                                else if (comboBoxComodos.SelectedIndex == 2)
                                {
                                    notificacaoTextBlock.Text = "⌚ Todas as luzes ligadas \rno horário programado.";
                                    Flyout.ShowAttachedFlyout(pagePage);
                                    todosToggleSwitch.IsOn = true;
                                }
                            }
                        }
                    }
                }
                if (itemSelecionadoText == "")
                {

                    semanaDiasRun.Text = "Nenhum dia selecionado.";
                }
                else
                {
                    semanaDiasRun.Text = itemSelecionadoText;
                }
                localSettings.Values[semanaDiasRun.Name.ToString()] = semanaDiasRun.Text;
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
            localSettings.Values[todosToggleSwitch.Name.ToString()] = todosToggleSwitch.IsOn;

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

            localSettings.Values[HORATIMERSETTING] = horaProgramada;
            localSettings.Values[MINUTOTIMERSETTING] = minutoProgramado;

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
            CheckBox[] diasCheckBox = { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday };
            if (todosCheckBox.IsChecked == true)
            {

                foreach (CheckBox c in diasCheckBox)
                {
                    c.IsChecked = true;
                    localSettings.Values[c.Name.ToString()] = c.IsChecked;

                    if (itemSelecionadoText.Length > 1)
                    {
                        itemSelecionadoText += ", ";
                    }
                    itemSelecionadoText += c.Content;
                }
                semanaDiasRun.Text = itemSelecionadoText;
            }
            else
            {
                foreach (CheckBox c in diasCheckBox)
                {
                    c.IsChecked = false;
                    localSettings.Values[c.Name.ToString()] = c.IsChecked;

                    semanaDiasRun.Text = "Nenhum dia selecionado.";
                }
            }
            localSettings.Values[semanaDiasRun.Name.ToString()] = semanaDiasRun.Text;
        }

        private void diasSemanaAtivosCabecarioTextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(timerTextBlock);
        }

        private void sensorToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            localSettings.Values[sensorToggleSwitch.Name.ToString()] = sensorToggleSwitch.IsOn;
        }

        private void sensorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            localSettings.Values[sensorComboBox.Name.ToString()] = sensorComboBox.SelectedIndex;
        }

        private void comboBoxComodos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            localSettings.Values[comboBoxComodos.Name.ToString()] = comboBoxComodos.SelectedIndex;
        }

        private void diasSemanaAtivosCabecarioTextBlock_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            Flyout.ShowAttachedFlyout(timerTextBlock);
        }

        bool fala = false;
        private void btFala_Click(object sender, RoutedEventArgs e)
        {
            var vermelho = new SolidColorBrush(Color.FromArgb(255, 200, 50, 50));
            var verde = new SolidColorBrush(Color.FromArgb(255, 50, 200, 50));
            //AppBarButton bt = new AppBarButton();
            //bt.Icon.Style = FontIcon.

            fala = !fala;
            if (fala)
            {
                btFala.Foreground = verde;
            }
            else
            {
                btFala.Foreground = vermelho;
            }
        }
    }
}





