using IttezanPos.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace IttezanPos.Services
{
  public  class OfflineDataBase
    {
        public OfflineDataBase()
        {
            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");

            var db = new SQLiteConnection(dbpath);

            db.CreateTable<Client>();
        }
        
    }
}
