using System.IO; 
using App2Night.Droid.Service;
using App2Night.Service.Interface;
using SQLite.Net;
using Xamarin.Forms;

[assembly: Dependency(typeof(SqLiteAndroid))]
namespace App2Night.Droid.Service
{
    public class SqLiteAndroid : IDatabaseService
    {
        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "App2NightSQLite.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, sqliteFilename); 
            if (!File.Exists(path)) File.Create(path);
            var plat = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
            var conn = new SQLite.Net.SQLiteConnection(plat, path);
            // Return the database connection 
            return conn;
        }
    }
}