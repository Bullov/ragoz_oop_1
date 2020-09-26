using System.Windows;
using ragoz_oop_1.ViewModels;

namespace ragoz_oop_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            // DataContext = new FloorViewModel
            // {
            //     HouseNumber = 1,
            //     Street = "Sezam",
            //     ApartmentsNum = 5,
            //     Area = 1000,
            //     FloorNum = 5
            // };
        }
    }
}