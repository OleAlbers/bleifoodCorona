using Bleifood.BL.Interfaces;
using Bleifood.Entities.Attributes;
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

namespace Bleifood.BL
{
    public class BrowserStorage : IBrowserStorage
    {
        private const string Prefix = "Bleifood";
        private ILocalStorageService _localStorageService;

        public BrowserStorage(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async void StoreData<T>(T data)
        {
            if (data == null) return;
            Type dataType = typeof(T);

            await _localStorageService.RemoveItemAsync(dataType.Name);
            await _localStorageService.SetItemAsync(dataType.Name, data);
        }

        public async Task<T> ReadData<T>() where T : new()
        {
            Type dataType = typeof(T);
            return await _localStorageService.GetItemAsync<T>(dataType.Name);
        }
    }
}
