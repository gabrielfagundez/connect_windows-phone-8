using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Windows;
using System.IO.IsolatedStorage;

namespace Connect.Classes
{
    class Session
    {

        public string GetStringObject(string key)
        {
            //Retrieve email Data
            var settings = IsolatedStorageSettings.ApplicationSettings;
            if (settings.Contains(key))
            {
                if (settings[key] == null){
                    return "";
                } else {
                return settings[key].ToString();
                }
            }
            else
            {
                return "";
            }
        }

        public void SaveStringObject(string key, string data)
        {
            var settings = IsolatedStorageSettings.ApplicationSettings;
            if (settings.Contains(key))
            {
                settings[key] = data;
            }
            else
            {
                settings.Add(key, data);
            }
        }

        public bool Contains(string key)
        {
            var settings = IsolatedStorageSettings.ApplicationSettings;
            return settings.Contains(key);
        }

        public void RemoveStringObject(string key)
        {
            var settings = IsolatedStorageSettings.ApplicationSettings;

            bool existe = settings.Contains(key);

            if (existe) 
            {
                settings.Remove(key);
            }

        }
    }
}
