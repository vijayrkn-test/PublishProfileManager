using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PublishProfileManager.Models;
using PublishProfileManagerTests.Properties;

namespace PublishProfileManagerTests
{
    public class UserPublishProfileTests
    {
        [TestMethod]
        public void UserPublishProfile_ReturnsValidPublishProfile()
        {
            string userPublishProfile = new UserPublishProfile().ToString();
            Assert.AreEqual(TestResources.UserPublishProfile, userPublishProfile);
        }
    }
}
