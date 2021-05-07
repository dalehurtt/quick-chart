using System;
using System.Collections.Generic;
using System.Configuration;

namespace Charts {

    public class Config {

        private Dictionary<string, string> data = new ();

        public Config () {
            var appSettings = ConfigurationManager.AppSettings;
            var keys = appSettings.AllKeys;
            foreach (var key in keys) {
                if (key == "dirpath") {
                    if (Environment.OSVersion.Platform == PlatformID.Unix) {
                        data.Add (key, appSettings.Get ("dirpathmac"));
                    }
                    else {
                        data.Add (key, appSettings.Get (key));
                    }
                }
                else {
                    data.Add (key, appSettings.Get (key));
                }
            }
        }

        public string? Get (string key) {
            string value = null;
            try {
                value = data [key];
            }
            catch { }
            return value;
        }
    }
}