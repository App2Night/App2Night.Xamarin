 


using System;
using System.IO;
using Windows.Storage;
using App2Night.Service.Interface;
using App2Night.UWP.Service;
using SQLite.Net;
using Xamarin.Forms;

[assembly: Dependency(typeof(UWPSQLite))]
namespace App2Night.UWP.Service
{
    public class UWPSQLite : IDatabaseService
    {
        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "App2NightSQLite.db3";
            var folder = ApplicationData.Current.LocalCacheFolder; // Documents folder 
            var path = Path.Combine(folder.Path, sqliteFilename);  

            // This is where we copy in the prepopulated database 
            if (!File.Exists(path))
            {
                File.Create(path);
            }

            var plat = new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT();
            var conn = new SQLite.Net.SQLiteConnection(plat, path);

            // Return the database connection 
            return conn;
        }
    }
}