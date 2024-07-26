using System;

using TechAssignmentUWP.ViewModels;

using Windows.UI.Xaml.Controls;

namespace TechAssignmentUWP.Views
{
    public sealed partial class ProductsPage : Page
    {
        public ProductsViewModel ViewModel { get; } = new ProductsViewModel();

        public ProductsPage()
        {
            InitializeComponent();
        }
    }
}
