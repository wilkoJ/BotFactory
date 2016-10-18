using BotFactory.Pages;
using BotFactory.Tools;
using System.Windows;
using BotFactory.Factories;
using System.Threading;

namespace BotFactory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataContext _dataContext = new DataContext();
        private UnitTest _unitTestingPage = new UnitTest();
        private FactoryTest _factoryTestPage = new FactoryTest();

        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(_factoryTestPage);
            _factoryTestPage.SetTestingFactory( new UnitFactory( 5, 10 ) );
        }
    }
}
