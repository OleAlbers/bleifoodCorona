using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaDL
{
    public class DataConnection
    {
        private const string _connectionString = "test.liteDB";  // TODO: From COnfig; Encrypted
        private LiteDB.LiteDatabase GetDatabase()
        {
            return new LiteDB.LiteDatabase(_connectionString);
        }

        private LiteDB.ILiteCollection<T> GetCollection<T>()
        {
            return GetDatabase().GetCollection<T>();
        }

        public IEnumerable<T> SelectAll<T>()
        {
            return GetCollection<T>().FindAll();
        }

        public void Insert<T>(T entity)
        {
            GetCollection<T>().Insert(entity);
        }

        public void Update<T>(T entity)
        {
            GetCollection<T>().Update(entity);
        }

    }
}
