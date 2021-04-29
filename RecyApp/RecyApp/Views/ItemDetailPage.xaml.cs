using RecyApp.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace RecyApp.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}