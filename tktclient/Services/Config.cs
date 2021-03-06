using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tktclient.Services
{
    class Config
    {
        private static IniData data = null;
        public Config()
        {
            if (data == null)
            {
                var parser = new FileIniDataParser();
                data = parser.ReadFile("config.ini");
            }
        }
        public string GetTktServiceAddress()
        {
            return data["TktService"]["URL"];
        }

        public string GetTktServiceValue(String name)
        {
            return data["TktService"][name];
        }
        public static string GetTktClientNo()
        {
            return data["TktService"]["TktClientNo"];
        }

        public string GetPrinterTemplateFileName()
        {
            return data["Print"]["TemplateFileName"];
        }

        public string GetPrintName()
        {
            return data["Print"]["PrintName"];
        }

        public string GetPrinterNo()
        {
            return data["Print"]["No"];
        }

        public int GetInitPrinterTickets()
        {
            return int.Parse(data["Print"]["Tickets"]);
        }

        public int GetInitPrinterRibbons()
        {
            return int.Parse(data["Print"]["Ribbons"]);
        }


        public List<string> GetLoginUserNames()
        {
            var names = data["LoginUsers"]["UserNames"];
            if (names == "")
            {
                return new List<string>();
            }

            return names.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public string GetDbProvider()
        {
            return data["Db"]["Provider"];
        }

        public string DbConnectionString()
        {
            return data["Db"]["Connection"];
        }

       
    }
}
