using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace практика_7.Pages
{
    /// <summary>
    /// Логика взаимодействия для GetPages.xaml
    /// </summary>
    public partial class GetPages : Page
    {
        private CommandGame ViewModel => (CommandGame)DataContext;

        public GetPages()
        {
            InitializeComponent();
            DataContext = new CommandGame();
            Loaded += async (s, e) => await ViewModel.LoadGamesAsync();
        }

        private async void SearchById_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.GetGameByIdAsync();
        }

        private async void Create_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.PostGameAsync();
        }

        private async void Update_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.PutGameAsync();
        }

        private async void Patch_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.PatchGameAsync();
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.DeleteGameAsync();
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.NewGame();
        }
    }
}
