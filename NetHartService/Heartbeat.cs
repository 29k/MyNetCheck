using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Configuration;
using System.Collections.Specialized;

namespace NetHartService
{
    class Heartbeat
    {

        public string storePath = ConfigurationManager.AppSettings.Get("storePath");

        private readonly Timer _timer;
        public Heartbeat()
        {
            _timer = new Timer(60000) { AutoReset = true };
            _timer.Elapsed += TimerElapsed;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                Ping myPing = new Ping();
                String host = "baidu.com";
                byte[] buffer = new byte[32];
                int timeout = 5000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                if(reply.Status == IPStatus.Success)
                {
                    string[] lines = new string[] { DateTime.Now.ToString() + "Net connection Baidu OK + " };
                    File.AppendAllLines(storePath, lines);
                }
                else
                {
                    string[] lines = new string[] { DateTime.Now.ToString() + "Net connection NG, restart PC, Net connection NG, restart PC" };
                    File.AppendAllLines(storePath, lines);
                    System.Diagnostics.Process.Start("shutdown.exe", "-r -t 20");
                }
            }
            catch (Exception)
            {
                string[] lines = new string[] { DateTime.Now.ToString() + "Net connection NG, restart PC, Net connection NG, restart PC" };
                File.AppendAllLines(storePath, lines);
                System.Diagnostics.Process.Start("shutdown.exe", "-r -t 10");
            }
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }
    }
}
