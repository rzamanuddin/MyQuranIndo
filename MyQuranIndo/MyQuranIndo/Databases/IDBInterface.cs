using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyQuranIndo.Databases
{
    public interface IDBInterface
    {
        SQLiteAsyncConnection CreateConnection();
    }
}
