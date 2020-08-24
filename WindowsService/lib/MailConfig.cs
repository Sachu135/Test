using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService
{
    public class MailPackageElement : ConfigurationElement
    {
        [ConfigurationProperty("email", IsKey = true, IsRequired = true)]
        public string Email
        {
            get { return (string)this["email"]; }
        }

        [ConfigurationProperty("type",  IsRequired = true)]
        public string Type
        {
            get { return (string)this["type"]; }
        }
       
        [ConfigurationProperty("password", IsRequired = false)]
        public string Password
        {
            get { return (string)this["password"]; }
        }
        [ConfigurationProperty("port", IsRequired = false)]
        public int Port
        {
            get { return (int)this["port"]; }
        }
        [ConfigurationProperty("host", IsRequired = false)]
        public string Host
        {
            get { return (string)this["host"]; }
        }
        [ConfigurationProperty("enableSsl", IsRequired = false)]
        public bool EnableSsl 
        {
            get { return (bool)this["enableSsl"]; }
        }
    }

    public class MailPackageElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new MailPackageElement();
        }


        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MailPackageElement)element).Email;
        }
    }

    public class MailPackageValuesSection : ConfigurationSection
    {
        [ConfigurationProperty("Values")]
        public MailPackageElementCollection Values
        {
            get { return (MailPackageElementCollection)this["Values"]; }
        }
    }
}
