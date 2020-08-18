using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockSample
{
    public class PackageElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
        }
        [ConfigurationProperty("package", IsRequired = true)]
        public string Package
        {
            get { return (string)this["package"]; }
        }
        [ConfigurationProperty("isEnvPathRequired", IsRequired = true)]
        public bool IsEnvPathRequired
        {
            get { return (bool)this["isEnvPathRequired"]; }
        }
        [ConfigurationProperty("version", IsRequired = false)]
        public string Version
        {
            get { return (string)this["version"]; }
        }
        [ConfigurationProperty("envVariable", IsRequired = false)]
        public string EnvVariable
        {
            get { return (string)this["envVariable"]; }
        }
    }

    public class PackageElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new PackageElement();
        }


        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PackageElement)element).Name;
        }
    }

    public class PackageValuesSection : ConfigurationSection
    {
        [ConfigurationProperty("Values")]
        public PackageElementCollection Values
        {
            get { return (PackageElementCollection)this["Values"]; }
        }
    }
}

