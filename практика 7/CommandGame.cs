using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace практика_7
{
    public class CommandGame : INotifyPropertyChanged
    {
        private readonly HttpClient _client;
        private const string BaseUrl = "https://localhost:7101/";
        private ObservableCollection<Game> _games;
        private Game _currentGame;
        private string _statusMessage;

        public ObservableCollection<Game> Games
        {
            get => _games;
            set { 
                _games = value; 
                OnPropertyChanged(nameof(Games)); 
            }
        }

        public Game CurrentGame
        {
            get => _currentGame;
            set { 
                _currentGame = value; 
                OnPropertyChanged(nameof(CurrentGame)); 
            }
        }

        private int _searchId;
        public int SearchId
        {
            get => _searchId;
            set { 
                _searchId = value; 
                OnPropertyChanged(nameof(SearchId)); 
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set { 
                _statusMessage = value; 
                OnPropertyChanged(nameof(StatusMessage)); 
            }
        }

        public CommandGame()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(BaseUrl);
            Games = new ObservableCollection<Game>();
            CurrentGame = new Game();
        }

        public async Task GetGameByIdAsync()
        {
            if (SearchId <= 0)
            {
                StatusMessage = "Введите корректный ID";
                return;
            }

            try
            {
                StatusMessage = "Поиск";
                var response = await _client.GetFromJsonAsync<Game>($"api/v1/Game/{SearchId}");

                if (response != null)
                {
                    CurrentGame = response;
                    StatusMessage = $"Игра с ID {SearchId} успешно найдена";
                }
            }
            catch (HttpRequestException)
            {
                StatusMessage = $"Игра с ID {SearchId} не найдена";
                CurrentGame = new Game();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка: {ex.Message}";
            }
        }

        public async Task LoadGamesAsync()
        {
            try
            {
                StatusMessage = "Загрузка";
                var response = await _client.GetFromJsonAsync<ObservableCollection<Game>>("api/v1/Game");
                if (response != null)
                {
                    Games = response;
                    StatusMessage = $"Загружено: {Games.Count}";
                }
            }
            catch
            {
                StatusMessage = "Ошибка: Сервер недоступен";
            }
        }

        public async Task PostGameAsync()
        {
            try
            {
                var response = await _client.PostAsJsonAsync("api/v1/Game", CurrentGame);
                if (response.IsSuccessStatusCode)
                {
                    StatusMessage = "Игра создана";
                    await LoadGamesAsync();
                    CurrentGame = new Game();
                }
                else StatusMessage = "Ошибка создания";
            }
            catch { StatusMessage = "Ошибка сервера"; }
        }

        public async Task PutGameAsync()
        {
            if (CurrentGame.Id == 0) { StatusMessage = "Выберите игру"; return; }
            try
            {
                var response = await _client.PutAsJsonAsync($"api/v1/Game/{CurrentGame.Id}", CurrentGame);
                if (response.IsSuccessStatusCode)
                {
                    StatusMessage = "Игра обновлена";
                    await LoadGamesAsync();
                }
                else StatusMessage = "Ошибка обновления";
            }
            catch { StatusMessage = "Ошибка сервера"; }
        }

        public async Task PatchGameAsync()
        {
            if (CurrentGame.Id == 0)
            {
                StatusMessage = "Выберите игру";
                return;
            }

            try
            {
                var response = await _client.PatchAsJsonAsync($"api/v1/Game/{CurrentGame.Id}", CurrentGame);

                if (response.IsSuccessStatusCode)
                {
                    StatusMessage = "Данные игры частично обновлены";
                    await LoadGamesAsync();
                }
                else
                {
                    StatusMessage = $"Ошибка: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка сервера: {ex.Message}";
            }
        }

        public async Task DeleteGameAsync()
        {
            if (CurrentGame.Id == 0) { 
                StatusMessage = "Выберите игру"; 
                return; 
            }
            
            try
            {
                var response = await _client.DeleteAsync($"api/v1/Game/{CurrentGame.Id}");
                if (response.IsSuccessStatusCode)
                {
                    StatusMessage = "Игра удалена";
                    await LoadGamesAsync();
                    CurrentGame = new Game();
                }
            }
            catch { StatusMessage = "Ошибка сервера"; }
        }

        public void NewGame()
        {
            CurrentGame = new Game();
            StatusMessage = "Форма очищена";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
