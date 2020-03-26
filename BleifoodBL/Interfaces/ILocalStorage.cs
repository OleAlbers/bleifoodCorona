using System;
using System.Threading.Tasks;

namespace CoronaBL.Interfaces
{
    public interface IBrowserStorage
    {
        void StoreData<T>(T data);
         Task<T> ReadData<T>() where T : new();
    }
}