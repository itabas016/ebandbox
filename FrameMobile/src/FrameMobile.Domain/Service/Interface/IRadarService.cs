using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Model;
using FrameMobile.Model.Radar;

namespace FrameMobile.Domain.Service
{
    public interface IRadarService
    {
        IList<RadarCategory> GetRadarCategoryList();
        void UpdateServerVersion<T>() where T : MySQLModelBase;
    }
}
