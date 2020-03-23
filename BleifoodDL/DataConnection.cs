using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaDL
{
    public class DataConnection
    {
        private LiteDB.LiteDatabase _database;
        private const string _connectionString = "test.liteDB";  // TODO: From COnfig; Encrypted

        public static LiteDB.LiteDatabase GetDatabase()
        {
            return new LiteDB.LiteDatabase(_connectionString);
        }

        public DataConnection(LiteDB.LiteDatabase database)
        {
            _database = database;
        }

   

        private LiteDB.ILiteCollection<T> GetCollection<T>()
        {
            return _database.GetCollection<T>();
        }

        public IEnumerable<T> SelectAll<T>()
        {
            return GetCollection<T>().FindAll();
        }

        public T SelectById<T>(Guid id)
        {
            return GetCollection<T>().FindById(id);
        }

        public void Insert<T>(T entity)
        {
            GetCollection<T>().Insert(entity);
        }

        public void Update<T>(T entity)
        {
            GetCollection<T>().Update(entity);
        }

        public void Delete<T>(Guid id) 
        {
            GetCollection<T>().Delete(id);
        }

    }
}
