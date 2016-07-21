using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace SegundoPlano
{
    public sealed class segundoPlano : IBackgroundTask
    {
        Uri liga_quarto = new Uri("http://192.168.1.2/?pin=LIGA1");
        HttpClient client = new HttpClient();
 
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
             await client.GetStringAsync(liga_quarto);
        }
    }
  
}
