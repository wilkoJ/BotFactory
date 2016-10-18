using BotFactory.Interface;
using BotFactory.Common.Tools;
using System.Windows;
using System.Windows.Controls;
using BotFactory.Tools;
using BotFactory.Factories;

namespace BotFactory.Pages
{
    /// <summary>
    /// Interaction logic for UnitTest.xaml
    /// </summary>
    public partial class UnitTest : Page
    {
        private UnitDataContext _unitDataContext = new UnitDataContext();

        public UnitTest()
        {
            InitializeComponent();

            DataContext = _unitDataContext;
        }

        public void SetUnitToTest( ITestingUnit unit )
        {
            _unitDataContext.IBot = unit;
        }
        
        private async void ButtonWork_Click(object sender, RoutedEventArgs e)
        {
            if (_unitDataContext.IBot != null)
            {
                var response = await _unitDataContext.IBot.WorkBegins();
                _unitDataContext.Response = response;
                _unitDataContext.Working = _unitDataContext.IBot.IsWorking;
                _unitDataContext.CurrentPos = _unitDataContext.IBot.CurrentPos;
            }
        }

        private async void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            if (_unitDataContext.IBot != null)
            {
                var response = await _unitDataContext.IBot.WorkEnds();
                _unitDataContext.Response = response;
                _unitDataContext.Working = _unitDataContext.IBot.IsWorking;
                _unitDataContext.CurrentPos = _unitDataContext.IBot.CurrentPos;
            }
        }
    }
}
