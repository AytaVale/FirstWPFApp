using FirstWPFApp.Models;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Threading.Tasks;
using FirstWPFApp.Loaders;
using Microsoft.Win32;



namespace FirstWPFApp
{

    public partial class MainWindow : Window
    {
        private ObservableCollection<Data> _DataList;
        private FileSystem _monitor;
        //string filePath = @"C:\\Users\\User\\Documents\\data.csv"; 
        //Note for myself:
        //путь к файлу пользователь должен указать сам, не забудь поменять код!!



        string _filePath;

        public MainWindow()
        {
            InitializeComponent();
            _DataList = new ObservableCollection<Data>();
            TradeDataGrid.ItemsSource = _DataList;
            _filePath = ConfigurationManager.AppSettings["InitialFilePath"]  ?? @"C:\\Users\\User\\Documents\\data.csv";

            FilePathTextBox.Text = _filePath;

            LoadData();
            InitializeMonitor();
        }

        public async void LoadData()
        {
            if (string.IsNullOrEmpty(_filePath))
            {
                MessageBox.Show("Choose file");
                return;
            }
            var loader = DetermineFileLoader(_filePath);
            if (loader == null)
                return;


            StatusTextBlock.Text = "Loading...";

            var data = await Task.Run(() => loader.Load(_filePath));

            Dispatcher.Invoke(() =>
            {
                _DataList.Clear();
                foreach (var trade in data)
                {
                    _DataList.Add(trade);
                }
                StatusTextBlock.Text = "Loading Completed";
            });
        }

        public void InitializeMonitor()
        {
            var loaders = new List<IFileLoader>
            {
                new CsvFileLoader(),
                new XmlFileLoader(),
                new TxtFileLoader()
            };

            _monitor = new FileSystem(loaders);


            _monitor.OnNewFileProcessed += OnNewDataLoaded;

            _monitor.Start();
        }

        public async void OnNewDataLoaded(string filePath)
        {
            IFileLoader loader = DetermineFileLoader(filePath);

            if (loader == null) return;

            var newData = await Task.Run(() => loader.Load(filePath));

            Dispatcher.Invoke(() =>
            {
                foreach (var trade in newData)
                {
                    _DataList.Add(trade);
                }
                StatusTextBlock.Text = "New data loaded";
            });
        }

        public IFileLoader DetermineFileLoader(string filePath)
        {
            if (filePath.EndsWith(".csv")) return new CsvFileLoader();
            if (filePath.EndsWith(".xml")) return new XmlFileLoader();
            if (filePath.EndsWith(".txt")) return new TxtFileLoader();


            return null;
        }

        public void LoadDataButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|XML files (*.xml)|*.xml|TXT files (*.txt)|*.txt|All files (*.*)|*.*",

                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _filePath = openFileDialog.FileName;
                FilePathTextBox.Text = _filePath;
                LoadData();
            }
        }
    }
}
