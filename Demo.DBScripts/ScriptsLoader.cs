using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Demo.DBScripts
{
    public sealed class ScriptsLoader
    {
        private static readonly string scriptFileName;

        static Dictionary<string, string> scriptDictionary = new Dictionary<string, string>();
        static ScriptsLoader()
        {
            scriptFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Scripts\Scripts.xml");

            Load();
        }

        /*
         <?xml version="1.0" encoding="utf-8" ?>
<DBScripts>
  <DBScript name="Formulary_Insert">
    <Content>
    <![CDATA[
        SELECT * FROM dbo.Formualry
        WHERE FormularyID=@FormularyID
    ]]>
    </Content>
  </DBScript>
  <DBScript name="Patient_Insert">
    <Content>
      <![CDATA[
        SELECT * FROM dbo.Formualry WHERE FormularyID=@FormularyID
    ]]>
    </Content>
  </DBScript>
</DBScripts>*/
        static void Load()
        {
            XElement doc = XElement.Load(scriptFileName);
            var scripts = doc.Elements("LinkFile").ToList();

            foreach (var item in scripts)
            {
                var xmlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"Scripts\{item.Attribute("src").Value}");

                if (!File.Exists(xmlFile))
                {
                    //logger
                    continue;
                }

                LoadScriptXml(xmlFile);
            }
        }

        private static void LoadScriptXml(string xmlFileFullName)
        {
            XElement doc = XElement.Load(xmlFileFullName);

            var scripts = doc.Elements("DBScript").ToList();
            foreach (var item in scripts)
            {
                scriptDictionary[item.Attribute("name").Value.ToUpper()] = item.Value.Trim();
            }
        }

        public static string Get(string scriptName)
        {
            if (string.IsNullOrWhiteSpace(scriptName))
            {
                throw new ArgumentNullException("scriptName");
            }

            var upperName = scriptName.ToUpper();
            if (!scriptDictionary.Keys.Contains(upperName))
            {
                throw new KeyNotFoundException($"{scriptName}");
            }

            return scriptDictionary[upperName];
        }
    }
}

