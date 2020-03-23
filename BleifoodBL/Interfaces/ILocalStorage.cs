using System;

namespace CoronaBL.Interfaces
{
    public interface ILocalStorage
    {
        void StoreData<T>(T data);
        T ReadData<T>() where T : new();
    }
}