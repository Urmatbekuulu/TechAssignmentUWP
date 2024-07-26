using System;

using TechAssignmentUWP.ViewModels;

using Windows.UI.Xaml.Controls;

namespace TechAssignmentUWP.Views
{
    public sealed partial class CartPage : Page
    {
        public CartViewModel ViewModel { get; } = new CartViewModel();

        public CartPage()
        {
            InitializeComponent();
        }
    }
}
