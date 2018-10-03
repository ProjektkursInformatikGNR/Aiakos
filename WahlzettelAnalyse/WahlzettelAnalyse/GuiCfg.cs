using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WahlzettelAnalyse
{
    public class GuiCfg : Config
    {
        private Server _defaultSrv;

        public GuiCfg(string filename)
        {
            _filename = filename;
        }

        public Server DefaultServer
        {
            get { return _defaultSrv; }
            set { _defaultSrv = value; }
        }

        public void writeToConfigFile()
        {
            Dictionary<string, string> elements = new Dictionary<string, string>();

            elements.Add("default", _defaultSrv.Name + ";" + _defaultSrv.Host + ";" + _defaultSrv.Port + ";" +
                _defaultSrv.UserId + ";" + _defaultSrv.Password + ";" + _defaultSrv.Database);

            writeConfig(elements);
        }

        public void readConfigFile()
        {
            Dictionary<string, string> config = readConfig();

            foreach (KeyValuePair<string,string> kvp in config)
            {
                if (kvp.Key.ToString() == "default")
                {
                    string[] tmp = kvp.Value.ToString().Split(';');
                    _defaultSrv = new Server(tmp[0], tmp[1], tmp[2], tmp[3], tmp[4], tmp[5]);
                }
            }

            if (_defaultSrv == null)
                _defaultSrv = new Server("", "", "", "", "", "");
        }
    }
}
