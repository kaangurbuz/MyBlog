﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.CommonLayer.Helper
{
    public class ConfigHelper
    {
        //public static string Get(string key)
        //{
        //    return ConfigurationManager.AppSettings[key];
        //}

        public static T Get<T>(string key)
        {
            return (T)Convert.ChangeType(ConfigurationManager.AppSettings[key], typeof(T));
        }
    }
}