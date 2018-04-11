using log4net;
using Microsoft.SharePoint.Client;
using System.Net;

namespace SharepointMigration
{
    public partial class Context : ClientContext
    {
        public ILog Log => LogManager.GetLogger(typeof(Context));

        public Context(string url, ICredentials credentials) : base(url)
        {
            Credentials = credentials;
        }
    }
}