using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Microsoft.Build.Evaluation;
using PublishProfileManager.Properties;

namespace PublishProfileManager.Models
{
    public abstract class PublishProfileBase
    {
        public override string ToString()
        {
            string msDeployPublishXmlContents = null;
            using (XmlTextReader reader = new XmlTextReader(new StringReader(Resources.PublishProfileTemplate)))
            {
                Project project = new Project(reader);

                Type t = GetType();
                var properties = t.GetProperties()
                   .Where(prop => prop.PropertyType == typeof(string) || prop.PropertyType == typeof(bool))
                   .OrderBy(p => p.GetCustomAttributes(typeof(DisplayAttribute), true)
                   .Cast<DisplayAttribute>()
                   .Select(a => a.Order)
                   .FirstOrDefault());

                foreach (PropertyInfo pi in properties)
                {
                    project.SetProperty(pi.Name, pi.GetValue(this, null)?.ToString() ?? string.Empty);
                }

                // TODO: Logic to handle itemgroups
                msDeployPublishXmlContents = project.Xml.RawXml;
            }

            return msDeployPublishXmlContents;
        }
    }
}
