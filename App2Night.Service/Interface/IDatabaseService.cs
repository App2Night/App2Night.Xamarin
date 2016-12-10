using SQLite;

namespace App2Night.Service.Interface
{
    public interface IDatabaseService
    {
        /// <summary>
        /// Connect to a local sqlight database on the device.
        /// </summary>
        /// <returns></returns>
        SQLiteConnection GetConnection();
    }
}