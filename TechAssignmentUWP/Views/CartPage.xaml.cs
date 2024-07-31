using TechAssignmentUWP.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace TechAssignmentUWP.Views
{
    public sealed partial class CartPage : Page
    {
        public CartViewModel ViewModel { get; } = new CartViewModel();

        public CartPage()
        {
            InitializeComponent();
            DataContext = ViewModel;

        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await ViewModel.LoadDataAsync();
        
        }
    }
}
