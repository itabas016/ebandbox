using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;

namespace TYDNewsService
{
    [RunInstaller(true)]
    public partial class TYDNewsServiceInstaller : Installer
    {
        public TYDNewsServiceInstaller()
        {
            InitializeComponent();
        }
    }
}
