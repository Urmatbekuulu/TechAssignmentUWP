using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel;

namespace TechAssignmentUWP.Core.Models
{
    public class Product : ObservableObject
    {

        public long Id { get; set; }

        public string Name { get; set; }
        public double Price { get; set; }
        public bool isInCart;
        public bool IsInCart { 
            get => isInCart;
            set => SetProperty(ref isInCart, value);

        }
        public string Image { get; set; }

    }
}