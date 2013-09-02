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

namespace TYDNewsService
{
    public partial class TYDNewsService : ServiceBase
    {
        ISchedulerFactory sf = new StdSchedulerFactory();
        IScheduler sched = null;
        IDataProvider provider = ProviderFactory.GetProvider(ConnectionStrings.NEWS_MYSQL_CONNECTSTRING);
        SimpleRepository repo = new SimpleRepository(ConnectionStrings.NEWS_MYSQL_CONNECTSTRING);

        public TYDNewsService()
        {
            InitializeComponent();
        }

        public void JobStart()
        {
            NewsDBInitialize();

        }

        protected override void OnStart(string[] args)
        {
            JobStart();
        }

        private void NewsDBInitialize()
        {
            //Console.WriteLine("Database Initialize...");

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
