using System;
using System.IO;
using System.Xml;
using Microsoft.Build.Evaluation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PublishProfileManager;
using PublishProfileManagerTests.Properties;

namespace PublishProfileManagerTests
{
    [TestClass]
    public class PublishProfileCreatorFactoryTests
    {
        [TestMethod]
        public void PublishProfileFactory_ReturnsMsDeployProfileWithCorrectSetttings_FromPublishSettings()
        {
            string msDeployProfile = PublishProfileCreatorFactory.CreateMSDeployPublishProfileFromPublishSettings(TestResources.PublishSettings);
            Assert.AreEqual(TestResources.MsDeployFromPublishSettings, msDeployProfile);
        }

        [TestMethod]
        public void PublishProfileFactory_ReturnsEncryptedUserProfile()
        {
            string userProfile = PublishProfileCreatorFactory.CreateUserPublishProfileFromPublishSettings(TestResources.PublishSettings);
            using (XmlTextReader reader = new XmlTextReader(new StringReader(userProfile)))
            {
                Project userProject = new Project(reader);
                Assert.IsNotNull(userProject.GetProperty("EncryptedPassword").EvaluatedValue);
            }
        }

        [TestMethod]
        public void PublishProfileFactory_ReturnsCorrectMsDeployPublishProfile()
        {
            string msDeployProfile = PublishProfileCreatorFactory.CreatePublishProfile("MSDeploy").ToString();
            Assert.AreEqual(TestResources.MSDeployPublishProfile, msDeployProfile);
        }

        [TestMethod]
        public void PublishProfileFactory_ReturnsCorrectMsDeployPackagePublishProfile()
        {
            string packageProfile = PublishProfileCreatorFactory.CreatePublishProfile("Package").ToString();
            Assert.AreEqual(TestResources.MSDeployPackagePublishProfile, packageProfile);
        }

        [TestMethod]
        public void PublishProfileFactory_ReturnsCorrectFileSystemProfile()
        {
            string fileSystemProfile = PublishProfileCreatorFactory.CreatePublishProfile("FileSystem").ToString();
            Assert.AreEqual(TestResources.FileSystemPublishProfile, fileSystemProfile);
        }

    }
}
