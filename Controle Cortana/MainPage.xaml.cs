using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage;
using System.Net.Http;
using System.Net;
using System.IO;
using Windows.UI.Core;
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
            Sensor();
            Setting();
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
    }
}
