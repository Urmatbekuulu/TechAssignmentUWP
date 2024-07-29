using System;
using System.ComponentModel;

namespace TechAssignmentUWP.Core.Models
{
    public class Product : INotifyPropertyChanged
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        private bool addedToCart = false;
        public bool AddedToCart { 
            get => addedToCart; 
            set 
            { 
                addedToCart = value; 
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Product.AddedToCart)));
            } 
        }
        public string Image { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}