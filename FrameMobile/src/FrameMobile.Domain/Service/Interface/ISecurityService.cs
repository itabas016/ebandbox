using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Model;
using FrameMobile.Model.Security;

namespace FrameMobile.Domain.Service
{
    public interface ISecurityService
    {
        SecurityConfig GetConfig(string target);
        FloatingView GetFloatingConfig(SecurityConfig config, string channel, int version);
    }
}
