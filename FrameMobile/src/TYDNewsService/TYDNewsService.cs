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

namespace TYDNewsService
{
    public partial class TYDNewsService : ServiceBase
    {
        private readonly IQuartzServer server;
        IDataProvider provider = ProviderFactory.GetProvider(ConnectionStrings.NEWS_MYSQL_CONNECTSTRING);

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
            //Initialize DB and Create Tables and Index
            NewsDBInitialize();
            NLogHelper.WriteInfo("Initialize DB is done!");
            server.Start();
        }

        private void NewsDBInitialize()
        {
            #region Create tables
            BatchQuery query = new BatchQuery(provider);
            Assembly assembly = Assembly.Load("FrameMobile.Model");
            string spacename = "FrameMobile.Model.News";

            var migrator = new SubSonic.Schema.Migrator(assembly);
            string[] commands = migrator.MigrateFromModel(spacename, provider);

            foreach (var s in commands)
            {
                query.QueueForTransaction(new QueryCommand(s.Trim(), provider));
            }
            query.ExecuteTransaction();
            #endregion

        }
    }
}
