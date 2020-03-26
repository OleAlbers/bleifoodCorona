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
using Microsoft.AspNetCore.Components;
using Blazored.LocalStorage;

namespace CoronaBL
{
    public class BrowserStorage: IBrowserStorage
    {
        private const string Prefix = "Bleifood";
        private ILocalStorageService _localStorageService;
    
        public BrowserStorage(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }



        public void StoreData<T>(T data)
        {
            if (data == null) return;
            Type dataType = typeof(T);
            Task.Run(() =>
           {
                _localStorageService.RemoveItemAsync(dataType.Name);
                _localStorageService.SetItemAsync(dataType.Name, data);
           });
        }

        public Task<T> ReadData<T>() where T : new()
        {
            // TODO: Async
            Type dataType = typeof(T);
            return _localStorageService.GetItemAsync<T>(dataType.Name);
        }
    }
}
