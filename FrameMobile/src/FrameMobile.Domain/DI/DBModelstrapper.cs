﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FrameMobile.Common;
using SubSonic.DataProviders;
using SubSonic.Query;

namespace FrameMobile.Domain
{
    public class DBModelstrapper
    {
        public static string COMMONCONNECTSTRING = ConnectionStrings.COMMON_MYSQL_CONNECTSTRING;

        public static string NEWSCONNECTSTRING = ConnectionStrings.NEWS_MYSQL_CONNECTSTRING;

        public static string THEMECONNECTSTRING = ConnectionStrings.THEME_MYSQL_CONNECTSTRING;

        public static string SECURITYCONNECTSTRING = ConnectionStrings.SECURITY_MYSQL_CONNECTSTRING;

        public static void Initialize()
        {
            UserInitialize();
            NewsInitialize();
            RadarInitialize();
            MobileInitialize();
            ThemeInitialize();
            SerurityInitialize();
        }

        public static void UserInitialize()
        {
            var accountModelSpace = string.Format("{0}.{1}", Const.FRAME_MODEL_MASTER, "Account");
            Initialize(COMMONCONNECTSTRING, accountModelSpace);
        }

        public static void NewsInitialize()
        {
            var newsModelSpace = string.Format("{0}.{1}", Const.FRAME_MODEL_MASTER, "News");
            Initialize(NEWSCONNECTSTRING, newsModelSpace);
        }

        public static void RadarInitialize()
        {
            var radarModelSpace = string.Format("{0}.{1}", Const.FRAME_MODEL_MASTER, "Radar");
            Initialize(COMMONCONNECTSTRING, radarModelSpace);
        }

        public static void MobileInitialize()
        {
            var mobileModelSpace = string.Format("{0}.{1}", Const.FRAME_MODEL_MASTER, "Mobile");
            Initialize(COMMONCONNECTSTRING, mobileModelSpace);
        }

        public static void ThemeInitialize()
        {
            var themeModelSpace = string.Format("{0}.{1}", Const.FRAME_MODEL_MASTER, "Theme");
            Initialize(THEMECONNECTSTRING, themeModelSpace);
        }

        public static void SerurityInitialize()
        {
            var securityModelSpace = string.Format("{0}.{1}", Const.FRAME_MODEL_MASTER, "Security");
            Initialize(SECURITYCONNECTSTRING, securityModelSpace);
        }

        #region Helper

        private static void Initialize(string connectString, string modelSpaceName)
        {
            IDataProvider provider = ProviderFactory.GetProvider(connectString);
            BatchQuery query = new BatchQuery(provider);

            Assembly assembly = Assembly.Load(Const.FRAME_MODEL_MASTER);

            var migrator = new SubSonic.Schema.Migrator(assembly);
            string[] commands = migrator.MigrateFromModel(modelSpaceName, provider);

            foreach (var s in commands)
            {
                query.QueueForTransaction(new QueryCommand(s.Trim(), provider));
            }
            query.ExecuteTransaction();
        }

        #endregion
    }
}
