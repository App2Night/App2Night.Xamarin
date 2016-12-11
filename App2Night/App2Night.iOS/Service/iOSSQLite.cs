using System;
using System.IO;
using App2Night.iOS.Service;
using App2Night.Service.Interface;
using Xamarin.Forms;

[assembly: Dependency(typeof(iOSSQLite))]
namespace App2Night.iOS.Service
{
    public class iOSSQLite : IDatabaseService
    { 
        #region ISQLite implementation
        public SQLite.Net.SQLiteConnection GetConnection()
        {
            var sqliteFilename = "App2NightSQLite.db3";
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
            var path = Path.Combine(libraryPath, sqliteFilename);

            // This is where we copy in the prepopulated database 
            if (!File.Exists(path))
            {
                File.Create(path);
            }

            var plat = new SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS();
            var conn = new SQLite.Net.SQLiteConnection(plat, path);

            // Return the database connection 
            return conn;
        }
        #endregion
    }
}