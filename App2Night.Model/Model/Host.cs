using System;
using SQLite.Net.Attributes;

namespace App2Night.Model.Model
{
    public class Host
    {

        //Table id
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public Guid HostId { get; set; }
        public string UserName { get; set; }
    }
}