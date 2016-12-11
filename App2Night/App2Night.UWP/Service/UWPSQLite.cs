 


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
            var plat = new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT();
            SQLiteConnection conn = null;
             
            var path = GetFilePath(ApplicationData.Current.LocalCacheFolder.Path); 
            try
            {
                conn = new SQLite.Net.SQLiteConnection(plat, path); 
            } 
            catch(Exception)
            {
                try
                {
                    //Try to open the documents folder if the local cache folder is not available.
                    path = GetFilePath(KnownFolders.DocumentsLibrary.Path);
                    conn = new SQLiteConnection(plat, path);
                }
                catch (Exception)
                { 
                    //It is highly possible that the access to the documents library is denied, catch it.
                }
            }
            
            // Return the database connection 
            return conn;
        }

        string GetFilePath(string folderPath)
        {
            var sqliteFilename = "App2NightSQLite.db3"; 
            var path = Path.Combine(folderPath, sqliteFilename);

            // This is where we copy in the prepopulated database 
            if (!File.Exists(path))
            {
                File.Create(path);
            }

            return path;
        } 
    }
}