using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Pract_7.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.ComponentModel;
using Pract_7.models;


namespace Pract_7
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HttpClient client = new HttpClient();

        public ObservableCollection<FMovie> Movies { get; set; } = new();

        FMovie C = new();

        public FMovie? CurrentMovie { get; set; } = new();

        public string? filtGenre { get; set; } = null;

        public int? filMinYear { get; set; } = null;
        public MainWindow()
        {
            client.BaseAddress = new Uri("https://localhost:7179/");
            
            InitializeComponent();

            Current.DataContext = C;

            DataContext = this;

            GetAll();
        }

        public async void GetAll()
        {
            if (filtGenre != null && filtGenre != "" && filMinYear != null)//всё разом
            {
                try
                {

                    var response = await client.GetFromJsonAsync<List<FMovie>>($"api/v1/Movie?Genre={filtGenre}&minYear={filMinYear}");

                    Movies.Clear();

                    if (response != null)
                    {
                        foreach (var intem in response)
                        {
                            Movies.Add(intem);
                        }
                    }
                }
                catch (System.Net.Http.HttpRequestException)
                {
                    MessageBox.Show($"Связь с API сервером не установлена");
                }
                catch
                {
                    Movies.Clear();
                }
            }
            else
            {
                if (filMinYear != null)//только год
                {
                    try
                    {
                        var response = await client.GetFromJsonAsync<List<FMovie>>($"api/v1/Movie/?minYear={filMinYear}");

                        Movies.Clear();

                        if (response != null)
                        {
                            foreach (var intem in response)
                            {
                                Movies.Add(intem);
                            }
                        }
                    }
                    catch (System.Net.Http.HttpRequestException)
                    {
                        MessageBox.Show($"Связь с API сервером не установлена");
                    }
                    catch
                    {
                        Movies.Clear();
                    }
                }
                else
                {
                    if (filtGenre != null && filtGenre != "")//только жанр
                    {
                        try
                        {

                            var response = await client.GetFromJsonAsync<List<FMovie>>($"api/v1/Movie?Genre={filtGenre}");

                            Movies.Clear();

                            if (response != null)
                            {
                                foreach (var intem in response)
                                {
                                    Movies.Add(intem);
                                }
                            }
                        }
                        catch (System.Net.Http.HttpRequestException)
                        {
                            MessageBox.Show($"Связь с API сервером не установлена");
                        }
                        catch
                        {
                            Movies.Clear();
                        }
                    }
                    else//всё пустое
                    {
                        try
                        {
                            var response = await client.GetFromJsonAsync<List<FMovie>>("api/v1/Movie");

                            Movies.Clear();

                            if (response != null)
                            {
                                foreach (var intem in response)
                                {
                                    Movies.Add(intem);
                                }
                            }
                        }
                        catch (System.Net.Http.HttpRequestException)
                        {
                            MessageBox.Show($"Связь с API сервером не установлена");
                        }
                    }
                }
            }
        }

        private void Change(object sender, SelectionChangedEventArgs e)
        {
            C=new();

            if (CurrentMovie != null)
            {
                C = new FMovie();
                C.ID = CurrentMovie.ID;
                C.Title = CurrentMovie.Title;
                C.Genre = CurrentMovie.Genre;
                C.ReleaseYear = CurrentMovie.ReleaseYear;
                C.DurationMinutes = CurrentMovie.DurationMinutes;
                C.IsAvailable = CurrentMovie.IsAvailable;
            }
            
             Current.DataContext = C;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)//Patch
        {
            if (C.ID!=null) {
                HttpResponseMessage res = await client.PatchAsJsonAsync(
                    $"api/v1/movie/{C.ID}",
                    new
                    {
                        Title = C.Title,
                        Genre = C.Genre,
                        DurationMinutes =  C.DurationMinutes,
                        ReleaseYear = C.ReleaseYear,
                        IsAvailable = C.IsAvailable
                    }
                    );
                if (res.IsSuccessStatusCode)
                {
                    MessageBox.Show("Успешно изменено");
                    GetAll();
                }
                else
                {

                    MessageBox.Show("Неверные данные");
                }
            }
            else
            {
                MessageBox.Show("Выберите элемент!");
            }
        }

        private async void ChangeWhole(object sender, RoutedEventArgs e)//Put
        {
            if (C.ID != null)
            {
                HttpResponseMessage res = await client.PutAsJsonAsync(
                    $"api/v1/movie/{C.ID}",
                    new
                    {
                        Title = C.Title,
                        Genre = C.Genre,
                        DurationMinutes = C.DurationMinutes,
                        ReleaseYear =C.ReleaseYear,
                        IsAvailable = C.IsAvailable
                    }
                    );
                if (res.IsSuccessStatusCode)
                {
                    MessageBox.Show("Успешно изменено");
                    GetAll();
                }
                else
                {

                    MessageBox.Show("Неверные данные");
                }
            }
            else
            {
                MessageBox.Show("Выберите элемент!");
            }
        }

        private async void Create(object sender, RoutedEventArgs e)
        {
            try
            {
                HttpResponseMessage res = await client.PostAsJsonAsync(
                    $"api/v1/movie",
                    new
                    {
                        Title = C.Title,
                        Genre = C.Genre,
                        DurationMinutes = C.DurationMinutes,
                        ReleaseYear = C.ReleaseYear,
                        IsAvailable = C.IsAvailable
                    }
                    );

                if (res.IsSuccessStatusCode)
                {
                    MessageBox.Show("Успешно добавлено");
                    GetAll();
                }
                else
                {
                    MessageBox.Show("Неправильные данные");
                }
            }
            catch (System.Net.Http.HttpRequestException)
            {
                MessageBox.Show($"Связь с API сервером не установлена");
            }
            catch
            {
                MessageBox.Show("Неверные данные");
            }
        }

        private async void Delete(object sender, RoutedEventArgs e)
        {
            if (C.ID != null)
            {
                HttpResponseMessage res = await client.DeleteAsync($"api/v1/movie/{C.ID}");

                if (res.IsSuccessStatusCode)
                {
                    MessageBox.Show("Успешное удаление");
                }
                else
                {
                    MessageBox.Show("Ошибка удаления");
                }
                GetAll();
            }
            else
            {
                MessageBox.Show("Выберите элемент!");
            }
        }


        private async void Filter2(object sender, RoutedEventArgs e)
        {
            GetAll();
        }
    }
}