using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EO;
using EO.Base;
using EO.WebBrowser;
using EO.WebEngine;

namespace Viewerbot
{
    class Core
    {
        int count = 0;
        List<Engine> egpool = new List<Engine>();
        List<ThreadRunner> Threadpool;

        public Core(int threadcount, string channel)
        {
            Random rnd = new Random();
            Threadpool = new List<ThreadRunner>();
            count = threadcount;
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\thoma\Desktop\Viewer\prox.txt");
            for (int i = 0; i < threadcount; i++)
            {
                egpool.Add(Engine.Create(i.ToString()));
                

                string[] ip = lines[rnd.Next(lines.Length - 1)].Split(':');
                ProxyInfo proxy = new ProxyInfo(ProxyType.HTTP, ip[0], int.Parse(ip[1]));
                egpool[i].Options.Proxy = proxy;
            }
            

        }
        public void opensite()
        {
            List<WebView> viewpool = new List<WebView>();
            for (int i = 0; i < count; i++)
            {
                Threadpool.Add(new EO.WebBrowser.ThreadRunner(i.ToString(), egpool[i]));
                viewpool.Add(Threadpool[i].CreateWebView());


                Threadpool[i].Send(() =>
                {
                    //Load Google's home page
                    viewpool[i].LoadUrlAndWait("http://www.twitch.tv/thomasglover");

                    //Capture screens-hot and save it to a file
                    viewpool[i].Capture().Save("screen_shot_file_name" + i.ToString()+".bmp");
                });
        }

    }
}  
}
