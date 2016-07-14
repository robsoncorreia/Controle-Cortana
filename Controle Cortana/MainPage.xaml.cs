using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage;
using System.Net.Http;
using System.Net;
using System.IO;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Controle_Cortana
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

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
            Sensor();
            Setting();
        }
        void Setting()
        {
            Object valueQuarto =  localSettings.Values[settingQuarto];
            if( valueQuarto != null){
               //toggleSwitchQuarto.IsOn = ((bool)valueQuarto);
            }
            Object valueSala = localSettings.Values[settingSala];
            if (valueSala != null)
            {
                //toggleSwitchSala.IsOn = ((bool)valueSala);
            }
            Object valueAutoQuarto = localSettings.Values[settingAutoQuarto];
            if (valueAutoQuarto != null)
            {
                toggleAutomaticoQuarto.IsOn = ((bool)valueAutoQuarto);
            }
            Object valueAutoSala = localSettings.Values[settingAutoSala];
            if (valueAutoSala != null)
            {
                toggleAutomaticoSala.IsOn = ((bool)valueAutoSala);
            }
        }
        public void ligarQuarto()
        {
            web.Navigate(liga_quarto);
            localSettings.Values[settingQuarto] = true;

        }
        public void desligarQuarto()
        {
            web.Navigate(desliga_quarto);
            localSettings.Values[settingQuarto] = false;
        }
        public void ligarSala()
        {
            web.Navigate(liga_sala);
            localSettings.Values[settingSala] = true;
        }
        public void desligarSala()
        {
            web.Navigate(desliga_sala);
            localSettings.Values[settingSala] = false;
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
        private void Rectangle_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            toggleSwitchQuarto.IsOn = !toggleSwitchQuarto.IsOn;
        }

        private void Rectangle_Tapped_1(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            toggleSwitchSala.IsOn = !toggleSwitchSala.IsOn;
        }

        private void retangudoAutomaticoQuarto_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            toggleAutomaticoQuarto.IsOn = !toggleAutomaticoQuarto.IsOn;
        }

        private void retangudoAutomaticoSala_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            toggleAutomaticoSala.IsOn = !toggleAutomaticoSala.IsOn;
        }

        async void toggleAutomaticoQuarto_Toggled(object sender, RoutedEventArgs e)
        {
            if(toggleAutomaticoQuarto != null)
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
        async void toggleAutomaticoSala_Toggled(object sender, RoutedEventArgs e)
        {
            if(toggleAutomaticoSala != null)
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
    }
}
