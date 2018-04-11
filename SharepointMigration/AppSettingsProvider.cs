using System.Collections.Generic;
using System.Configuration;
using System.Net;

namespace SharepointMigration
{
    public sealed class AppSettingsProvider
    {
        #region Private Fields

        #region Static

        private static AppSettingsProvider _instance;

        #endregion Static

        #region Instance

        private NetworkCredential _sourceServerCredentials = null;
        private NetworkCredential _targetServerCredentials = null;

        #endregion Instance

        #endregion Private Fields

        #region Constructor

        private AppSettingsProvider()
        {
        }

        #endregion Constructor

        #region Properties

        public static AppSettingsProvider Instance
        {
            get
            {
                _instance = _instance ?? new AppSettingsProvider();
                return _instance;
            }
        }

        public string SourceServerUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["source.sharepoint.url"];
            }
        }

        public NetworkCredential SourceServerCredentials
        {
            get
            {
                _sourceServerCredentials = _sourceServerCredentials ?? GetCredential("source.sharepoint");
                return _sourceServerCredentials;
            }
        }

        public string TargetServerUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["target.sharepoint.url"];
            }
        }

        public NetworkCredential TargetServerCredentials
        {
            get
            {
                _targetServerCredentials = _targetServerCredentials ?? GetCredential("target.sharepoint");
                return _targetServerCredentials;
            }
        }

        #endregion Properties

        #region Public Methods

        #region Static

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        #endregion Static

        public List<string> GetListNamesToImport()
        {
            return new List<string>()
            {
                "Preg/lac",
                //"ADR team calendar"
            };
        }

        #endregion Public Methods

        #region Private Methods

        private string GetDecodedData(string encodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(encodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        private NetworkCredential GetCredential(string configKeyPrefix)
        {
            string username = GetDecodedData(ConfigurationManager.AppSettings[$"{configKeyPrefix}.username"]);
            string password = GetDecodedData(ConfigurationManager.AppSettings[$"{configKeyPrefix}.password"]);
            string domain = ConfigurationManager.AppSettings[$"{configKeyPrefix}.domain"];
            return new NetworkCredential(username, password, domain);
        }

        #endregion Private Methods
    }
}