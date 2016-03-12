using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TesseractBoxCreator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string ExecutableFilePath { get { return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location); } }
    }
}
