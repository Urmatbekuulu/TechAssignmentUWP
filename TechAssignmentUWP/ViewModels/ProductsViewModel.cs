using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using TechAssignmentUWP.Core.Models;
using TechAssignmentUWP.Core.Services;
using TechAssignmentUWP.Services;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace TechAssignmentUWP.ViewModels
{
    public class ProductsViewModel : ObservableObject
    {
        public ObservableCollection<Product> Source { get; } = new ObservableCollection<Product>();

        public IAsyncRelayCommand AddToCartCommand { get; }
        public ProductsViewModel()
        {
            AddToCartCommand = new AsyncRelayCommand<Product>(async (product) => await AddToCartAsync(product));

        }
        private async Task AddToCartAsync(Product product)
        {
            if (product.IsInCart) 
                await DataService.RemoveProductFromCartAsync(product.Id);
            
            else
                await DataService.AddProductToCartAsync(product.Id);
            product.IsInCart = !product.IsInCart;
            //await LoadDataAsync();
        }
        public async Task LoadDataAsync()
        {
            Source.Clear();
            foreach (var product in await DataService.GetProductsDataAsync())
            {
                Source.Add(product);
            }
        }
    }
}
