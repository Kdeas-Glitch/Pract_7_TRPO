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

        public FMovie? CurrentMovie { get; set; } = new();

        public string? filtGenre { get; set; } = null;

        public int? filMinYear { get; set; } = null;
        public MainWindow()
        {
            client.BaseAddress = new Uri("https://localhost:7179/");
            
            InitializeComponent();

            Current.DataContext = CurrentMovie;

            DataContext = this;

            GetAll();
        }

        public async void GetAll()
        {
            var response = await client.GetFromJsonAsync<List<FMovie>>("api/v1/Movie");

            Movies.Clear();

            if (response != null)
            {
                foreach(var intem in response)
                {
                    Movies.Add(intem);
                }
            }
        }
    }
}