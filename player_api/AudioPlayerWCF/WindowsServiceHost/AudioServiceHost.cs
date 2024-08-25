using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceHost
{
    public partial class AudioServiceHost : ServiceBase
    {
        private ServiceHost _host;

        public AudioServiceHost()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _host = new ServiceHost(typeof(AudioPlayerWCF.AudioPlayerService));
            _host.Open();
        }

        protected override void OnStop()
        {
            if (_host != null) _host.Close();
        }
    }
}
