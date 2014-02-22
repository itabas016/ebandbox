using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using FrameMobile.Common;
using Quartz;
using Quartz.Impl;
using SubSonic.DataProviders;
using SubSonic.Query;
using SubSonic.Repository;
using Quartz.Server.Core;
using FrameMobile.Core;
using FrameMobile.Domain;

namespace TYDNewsService
{
    public partial class TYDNewsService : ServiceBase
    {
        private readonly IQuartzServer server;

        public TYDNewsService()
        {
            InitializeComponent();

            server = QuartzServerFactory.CreateServer();

            server.Initialize();
        }

        protected override void OnStart(string[] args)
        {
            JobStart();
        }

        protected override void OnStop()
        {
            server.Stop();
        }

        public void JobStart()
        {
            Bootstrapper.Start();
            server.Start();
        }
    }
}
