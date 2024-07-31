using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TechAssignmentUWP.Core.Models;
using TechAssignmentUWP.Services;

namespace TechAssignmentUWP.ViewModels
{
    public class CartViewModel : ObservableObject
    {
        public IAsyncRelayCommand DecrementCommand { get; }
        public IAsyncRelayCommand IncrementCommand { get; }
        public IAsyncRelayCommand RemoveCommand { get; }
        public ObservableCollection<KeyValuePair<Product,int>> Source { get; } = new ObservableCollection<KeyValuePair<Product,int>>();
        private double totalPrice = 0;
        public double TotalPrice {
            get => totalPrice;
            set => SetProperty(ref totalPrice, value);
        }
        private int totalQuantity = 0;
        public int TotalQuantity {
            get => totalQuantity;
            set => SetProperty(ref totalQuantity, value);
        }
        public CartViewModel()
        {
            DecrementCommand = new AsyncRelayCommand<long>(async (productID) => await DecreaseAsync(productID));
            IncrementCommand = new AsyncRelayCommand<long>(async (productID) => await IncreaseAsync(productID));
            RemoveCommand = new AsyncRelayCommand<long>(async (productID) => await RemoveAsync(productID));
        }
        private async Task DecreaseAsync(long productID)
        {
            await DataService.DecreaseAsync(productID);
            await LoadDataAsync();
        }
        private async Task IncreaseAsync(long productID)
        {
            await DataService.AddProductToCartAsync(productID);
            await LoadDataAsync();
        }
        private async Task RemoveAsync(long productID)
        {
            await DataService.RemoveProductFromCartAsync(productID);
            await LoadDataAsync();
        }
        public async Task LoadDataAsync()
        {
            Source.Clear();
            TotalPrice = 0;
            TotalQuantity = 0;
            var productsCountDictionary = await DataService.GetProductsCountDictionaryInCart();
            foreach (var product in await DataService.GetCartProductsAsync())
            {
                Source.Add(new KeyValuePair<Product,int>(product, productsCountDictionary[product.Id]));
                TotalPrice += product.Price * productsCountDictionary[product.Id];
                TotalQuantity += productsCountDictionary[product.Id];
            }
           
        }
    }
}
