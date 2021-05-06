using System;
using System.Collections.Generic;
using System.Configuration;

namespace Charts {

    public class Config {

        private Dictionary<string, string> data = new Dictionary<string, string> ();

        public Config () {
            var appSettings = ConfigurationManager.AppSettings;
            var keys = appSettings.AllKeys;
            foreach (var key in keys) {
                if (key == "dirpath") {
                    var value = appSettings.Get (key);
                    var values = value.Split ("|");
                    if (System.Environment.OSVersion.Platform == PlatformID.Unix) {
                        data.Add (key, values[1]);
                    }
                    else {
                        data.Add (key, values[0]);
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