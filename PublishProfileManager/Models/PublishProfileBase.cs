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
			using (StringWriter stringWriter = new StringWriter())
			{
				using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter) { Formatting = Formatting.Indented})
				{
					Type t = GetType();
					var properties = t.GetProperties()
					   .Where(prop => prop.PropertyType == typeof(string) || prop.PropertyType == typeof(bool))
					   .OrderBy(p => p.GetCustomAttributes(typeof(DisplayAttribute), true)
					   .Cast<DisplayAttribute>()
					   .Select(a => a.Order)
					   .FirstOrDefault());

					xmlWriter.WriteStartDocument();
					xmlWriter.WriteStartElement("Project");
					xmlWriter.WriteAttributeString("ToolsVersion", "4.0");
					xmlWriter.WriteAttributeString("xmlns", "http://schemas.microsoft.com/developer/msbuild/2003");

					foreach (PropertyInfo pi in properties)
					{
						xmlWriter.WriteElementString(pi.Name, pi.GetValue(this, null)?.ToString() ?? string.Empty);
					}

					xmlWriter.WriteEndElement();
					xmlWriter.WriteEndDocument();
					// TODO: Logic to handle itemgroups
					msDeployPublishXmlContents = stringWriter.ToString();
				}
			}

			return msDeployPublishXmlContents;
		}
    }
}
