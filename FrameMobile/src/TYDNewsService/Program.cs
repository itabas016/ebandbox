using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using FrameMobile.Domain;

namespace TYDNewsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            Bootstrapper.Start();
            if (Environment.UserInteractive)
            {
                ProcessCommandLine(args);
            }
            else
            {
                RunService();
            }
        }

        private static void RunService()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            { 
                new TYDNewsService() 
            };
            ServiceBase.Run(ServicesToRun);
        }

        private static void ProcessCommandLine(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("================================");
                Console.WriteLine("For help, please run : TYDNewsService -help");
                Console.WriteLine("Press Ctrl + C to exit!");
                Console.WriteLine("================================");
                RunInCommandLine();
                return;
            }

            foreach (var x in args)
            {
                switch (x)
                {
                    case "-i":
                    case "-install":
                        InstallService();
                        return;
                    case "-u":
                    case "-uninstall":
                        UninstallService();
                        return;
                    case "-h":
                    case "-help":
                        ShowCommandHelp();
                        return;
                    default:
                        Console.WriteLine("Unknown argument: {0}", x);
                        return;
                }
            }
        }

        private static void ShowCommandHelp()
        {
            Console.WriteLine(@"
================================
Usage: TYDNewsService [-i | -install | -u | -uninstall | -h | -help]

-i
-install
    install windows service
-u
-uninstall
    uninstall windows service
-h
-help
    show usage information
================================
");
        }

        private static void UninstallService()
        {
            var installer = new AssemblyInstaller(typeof(TYDNewsServiceInstaller).Assembly, null);
            installer.UseNewContext = true;
            installer.Uninstall(null);
        }

        private static void InstallService()
        {
            var installer = new AssemblyInstaller(typeof(TYDNewsServiceInstaller).Assembly, null);
            installer.UseNewContext = true;
            installer.Install(null);
        }

        private static void RunInCommandLine()
        {
            TYDNewsService server = new TYDNewsService();
            server.JobStart();
            Console.ReadLine();
        }
    }
}
