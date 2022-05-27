using MyTodo.SharedLib.Dtos;
using MyTodo.Extensions;
using MyTodo.Events;


using Prism.Events;
using System.Windows;

namespace MyTodo.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(IEventAggregator aggregator)
        {
            InitializeComponent();

            aggregator.Resgiter(arg =>
            {
                DialogHost.IsOpen = arg.IsOpen;
                if (DialogHost.IsOpen)
                    DialogHost.DialogContent = new LoadingDialog();
            });

            buttonClose.Click += (s, e) =>
            {
                Application.Current.Shutdown();
            };
            buttonMin.Click += (s, e) =>
            {
                this.WindowState = WindowState.Minimized;
            };
            buttonMax.Click += (s, e) =>
            {
                if (this.WindowState == WindowState.Normal)
                {
                    WindowState = WindowState.Maximized;
                }
                else
                {
                    WindowState = WindowState.Normal;
                }
            };
            colorZone.MouseMove += (s, e) =>
            {
                if(e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                    this.DragMove();
            };
            menuBar.SelectionChanged += (s, e) =>
            {
                drawerHost.IsLeftDrawerOpen = false;
            };
        }


    }
}
