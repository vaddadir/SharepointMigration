using log4net;
using System;

namespace SharepointMigration
{
    internal partial class Program
    {
        public static void Main(string[] args)
        {
            ILog log = LogManager.GetLogger(typeof(Program));
            SharepointMigrationProcess migrationProcess = new SharepointMigrationProcess();
            migrationProcess.Run();
            Console.WriteLine("Yaay! Migration complete !!! Really?");
            Console.Read();
        }
    }
}