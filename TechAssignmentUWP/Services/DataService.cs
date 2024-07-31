using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TechAssignmentUWP.Core.Helpers;
using TechAssignmentUWP.Core.Models;
using Windows.ApplicationModel;
using Windows.Storage;

namespace TechAssignmentUWP.Services
{
    internal static class DataService
    {
        const string productsFileName = "products.json";
        const string cartFileName = "localCart.json";

        internal static async Task<IEnumerable<Product>> GetProductsDataAsync()
        {

            if (!await HasLocalProductsFileCreated())
                await SeedInitialProductsFile();

            var file = await ApplicationData.Current.LocalFolder.GetFileAsync(productsFileName);
            var productsText = await FileIO.ReadTextAsync(file);
            return await Json.ToObjectAsync<IEnumerable<Product>>(productsText);
        }
        internal static async Task SeedInitialProductsFile()
        {
            var initialProductsFilePath = Path.Combine(Package.Current.InstalledLocation.Path, $"Assets\\{productsFileName}");
            var uri = new Uri($"ms-appx:///Assets/{productsFileName}");
            var initialProductsFile = await StorageFile.GetFileFromApplicationUriAsync(uri);
            var localProducts = await initialProductsFile.CopyAsync(ApplicationData.Current.LocalFolder, productsFileName, NameCollisionOption.ReplaceExisting);
        }
        internal static async Task<bool> HasLocalProductsFileCreated()
        {
            return await ApplicationData.Current.LocalFolder.TryGetItemAsync(productsFileName) != null;
        }
        internal static async Task<IEnumerable<Product>> GetCartProductsAsync()
        {
            var cartFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(cartFileName, CreationCollisionOption.OpenIfExists);
            var cartFileText = await FileIO.ReadTextAsync(cartFile);
            var cartList = await Json.ToObjectAsync<Dictionary<long, int>>(cartFileText);

            var productsList = await GetProductsDataAsync();
            var result = new ObservableCollection<Product>();

            foreach (var item in cartList)
            {
                var product = productsList.FirstOrDefault((p) => p.Id == item.Key && item.Value >= 1);
                if (product == null) continue;
                result.Add(product);
            }
            return result;
        }
        internal static async Task AddProductToCartAsync(long productID)
        {
            var cartFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(cartFileName, CreationCollisionOption.OpenIfExists);
            var cartFileText = await FileIO.ReadTextAsync(cartFile);
            var cartList = String.IsNullOrEmpty(cartFileText) ? new Dictionary<long, int>() : await Json.ToObjectAsync<Dictionary<long, int>>(cartFileText);
            cartList[productID] = cartList.ContainsKey(productID) ? cartList[productID] + 1 : 1;
            await FileIO.WriteTextAsync(cartFile, await Json.StringifyAsync(cartList));
            await SyncAsync();
        }
        internal static async Task RemoveProductFromCartAsync(long productID)
        {
            var cartFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(cartFileName, CreationCollisionOption.OpenIfExists);
            var cartFileText = await FileIO.ReadTextAsync(cartFile);
            var cartList = String.IsNullOrEmpty(cartFileText) ? new Dictionary<long, int>() : await Json.ToObjectAsync<Dictionary<long, int>>(cartFileText);
            cartList[productID] = 0;
            await FileIO.WriteTextAsync(cartFile, await Json.StringifyAsync(cartList));
            await SyncAsync();
        }
        internal static async Task SyncAsync()
        {
            var productFile = await ApplicationData.Current.LocalFolder.GetFileAsync(productsFileName);
            var productsText = await FileIO.ReadTextAsync(productFile);
            var productsList = await Json.ToObjectAsync<IEnumerable<Product>>(productsText);

            var cartFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(cartFileName, CreationCollisionOption.OpenIfExists);
            var cartFileText = await FileIO.ReadTextAsync(cartFile);
            var cartList = String.IsNullOrEmpty(cartFileText) ? new Dictionary<long, int>() : await Json.ToObjectAsync<Dictionary<long, int>>(cartFileText);

            foreach (var product in productsList)
                if (cartList.ContainsKey(product.Id)) product.IsInCart = cartList[product.Id] > 0;
            await FileIO.WriteTextAsync(productFile, await Json.StringifyAsync(productsList));
        }
        internal static async Task<Dictionary<long, int>> GetProductsCountDictionaryInCart() {
            var cartFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(cartFileName, CreationCollisionOption.OpenIfExists);
            var cartFileText = await FileIO.ReadTextAsync(cartFile);
            return String.IsNullOrEmpty(cartFileText) ? new Dictionary<long, int>() : await Json.ToObjectAsync<Dictionary<long, int>>(cartFileText);
        }

        internal static async Task DecreaseAsync(long productID)
        {
            var cartFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(cartFileName, CreationCollisionOption.OpenIfExists);
            var cartFileText = await FileIO.ReadTextAsync(cartFile);
            var cartList = String.IsNullOrEmpty(cartFileText) ? new Dictionary<long, int>() : await Json.ToObjectAsync<Dictionary<long, int>>(cartFileText);
            cartList[productID] = cartList.ContainsKey(productID) && cartList[productID] >=1 ? cartList[productID] - 1 : 0;
            await FileIO.WriteTextAsync(cartFile, await Json.StringifyAsync(cartList));
            await SyncAsync();
        }
    }
}
