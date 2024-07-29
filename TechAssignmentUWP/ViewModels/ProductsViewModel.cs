using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using TechAssignmentUWP.Core.Models;

namespace TechAssignmentUWP.ViewModels
{
    public class ProductsViewModel : ObservableObject
    {
        public ObservableCollection<Product> Source { get; } = new ObservableCollection<Product>();
        public ProductsViewModel()
        {
            Source = new ObservableCollection<Product>();
            for (int i = 0; i < 250; i++)
            {
                Source.Add(new Product(){Id = i, Name = $"{i}th Product name", Price = i+i/10.0, Image = "Products Image source"});
            }
        }
    }
}
