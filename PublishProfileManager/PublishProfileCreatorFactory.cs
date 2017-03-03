using System;
using System.IO;
using System.Xml;
using PublishProfileContracts;
using PublishProfileManager.Models;

namespace PublishProfileManager
{
    public class PublishProfileCreatorFactory
    {
        public static PublishProfile CreatePublishProfile(string publishMethod)
        {
            PublishProfile publishProfile = null;
            switch (publishMethod)
            {
                case MSDeployPublishProfile.PublishMethod:
                    publishProfile = new MSDeployPublishProfile();
                    break;

                case MSDeployPackagePublishProfile.PublishMethod:
                    publishProfile = new MSDeployPackagePublishProfile();
                    break;

                case FileSystemPublishProfile.PublishMethod:
                    publishProfile = new FileSystemPublishProfile();
                    break;
            }

            return publishProfile;
        }


        public static string CreateMSDeployPublishProfileFromPublishSettings(string publishSettingsContents)
        {
            using (XmlTextReader reader = new XmlTextReader(new StringReader(publishSettingsContents)))
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(reader);

                XmlNode root = xmldoc.DocumentElement;
                XmlNode profileNode = root.FirstChild;
                for (; profileNode != null; profileNode = profileNode.NextSibling)
                {
                    XmlAttribute publishMethodAttr = null;
                    if (profileNode.Attributes != null)
                    {
                        publishMethodAttr = profileNode.Attributes["publishMethod"];
                    }

                    if (publishMethodAttr != null && !string.IsNullOrEmpty(publishMethodAttr.Value) && string.Equals(publishMethodAttr.Value, MSDeployPublishProfile.PublishMethod, StringComparison.Ordinal))
                    {
                        IPublishProfile msDeployPublishProfile = new MSDeployPublishProfile()
                        {
                            
                            MSDeployServiceURL = profileNode.Attributes["publishUrl"]?.Value,
                            DeployIisAppPath = profileNode.Attributes["msdeploySite"]?.Value,
                            SiteUrlToLaunchAfterPublish = profileNode.Attributes["destinationAppUrl"]?.Value,
                            LaunchSiteAfterPublish = true,
                            UserName = profileNode.Attributes["userName"]?.Value
                        };

                        return msDeployPublishProfile.ToString();
                    }
                }
            }

            return null;
        }

        public static string CreateUserPublishProfileFromPublishSettings(string publishSettingsContents)
        {
            using (XmlTextReader reader = new XmlTextReader(new StringReader(publishSettingsContents)))
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(reader);

                XmlNode root = xmldoc.DocumentElement;
                XmlNode profileNode = root.FirstChild;
                for (; profileNode != null; profileNode = profileNode.NextSibling)
                {
                    XmlAttribute plainTextPassword = null;
                    if (profileNode.Attributes != null)
                    {
                        plainTextPassword = profileNode.Attributes["userPWD"];
                    }

                    if (plainTextPassword != null && !string.IsNullOrEmpty(plainTextPassword.Value))
                    {
                        var protectedPassword = DataProtection.GetProtectedPassword(plainTextPassword.Value);
                        UserPublishProfile userPublishProfile = new UserPublishProfile()
                        {
                            EncryptedPassword = Convert.ToBase64String(protectedPassword)
                        };

                        return userPublishProfile.ToString();
                    }
                }
            }

            return null;
        }
    }
}
