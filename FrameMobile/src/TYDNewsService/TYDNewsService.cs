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
using NCore;

namespace TYDNewsService
{
    public partial class TYDNewsService : ServiceBase
    {
        IScheduler sched = null;
        ISchedulerFactory sfactory = new StdSchedulerFactory();
        IDataProvider provider = ProviderFactory.GetProvider(ConnectionStrings.NEWS_MYSQL_CONNECTSTRING);

        public TYDNewsService()
        {
            InitializeComponent();

            sched = sfactory.GetScheduler();
        }

        public void JobStart()
        {
            //Initialize DB and Create Tables and Index
            NewsDBInitialize();

            //Fetch and Save

            //var trigger = (ISimpleTrigger)TriggerBuilder.Create()
            //                               .WithIdentity("toutiao")
            //                               .StartAt(DateTime.Now)
            //                               .WithSimpleSchedule(x => x.WithIntervalInSeconds("TimeIntervalToLoadData".ConfigValue().ToInt32()).RepeatForever())
            //                               .Build();

            var toutiaoTrigger = (ICronTrigger)TriggerBuilder.Create()
                                           .WithIdentity("toutiaoTrigger")
                                           .WithCronSchedule("0 0/10 * * * ?")
                                           .Build();

            var toutiaoJob = JobBuilder.Create<TouTiaoJob>()
               .WithIdentity("toutiaoJob")
               .Build();

            sched.ScheduleJob(toutiaoJob, toutiaoTrigger);
            sched.Start();
        }

        protected override void OnStart(string[] args)
        {
            JobStart();
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
