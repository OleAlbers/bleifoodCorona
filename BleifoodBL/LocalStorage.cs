using CoronaBL.Interfaces;
using CoronaEntities.Attributes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace CoronaBL
{
    public class LocalStorage: ILocalStorage
    {
        private const string Prefix = "Bleifood";
        private ILocalStorageService _localStorageService;
    
        public LocalStorage(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }



        public async void StoreData<T>(T data)
        {
            if (data == null) return;
            Type dataType = typeof(T);
            await _localStorageService.SetItemAsync(dataType.Name, data);
        }

        public T ReadData<T>() where T : new()
        {
            // TODO: Async
            Type dataType = typeof(T);
            return _localStorageService.GetItemAsync<T>(dataType.Name).Result;
          
        }
    }
}
