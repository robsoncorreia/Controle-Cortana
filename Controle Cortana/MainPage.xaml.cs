﻿using System;
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
        public MainPage()
        {
            this.InitializeComponent();
            Sensor();
            settings();
        }

        Uri liga_quarto = new Uri("http://192.168.1.2/?pin=LIGA1");
        Uri desliga_quarto = new Uri("http://192.168.1.2/?pin=DESLIGA1");
        Uri liga_sala = new Uri("http://192.168.1.2/?pin=LIGA2");
        Uri desliga_sala = new Uri("http://192.168.1.2/?pin=DESLIGA2");

        int contadorSensor;

        public void settings()
        {           
            toggleAutomatico.IsOn = true;          
        }
        public void ligarQuarto()
        {
            web.Navigate(liga_quarto);
        }
        public void desligarQuarto()
        {
            web.Navigate(desliga_quarto);
        }
        public void ligarSala()
        {
            web.Navigate(liga_sala);
        }
        public void desligarSala()
        {
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

        private void toggleAutomatico_Toggled(object sender, RoutedEventArgs e)
        {

        }
    }
}
