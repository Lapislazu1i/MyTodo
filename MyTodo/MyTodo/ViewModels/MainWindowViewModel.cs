using MyTodo.Core;
using MyTodo.Core.Modals;
using MyTodo.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;

namespace MyTodo.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public string Title { get; set; } = "Main";
        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            CreateMenuBars();


            NavigationCommand = new DelegateCommand<MenuBar>(Navigate);
            GoBackCommand = new DelegateCommand(() =>
            {
                if (_journal != null && _journal.CanGoBack)
                    _journal.GoBack();
            });

            GoForwardCommand = new DelegateCommand(() =>
            {
                if (_journal != null && _journal.CanGoForward)
                {
                    _journal.GoForward();
                }
            });
        }

        private void Navigate(MenuBar obj)
        {
            if (obj == null || string.IsNullOrEmpty(obj.NameSpace))
                return;
            _regionManager.Regions[RegionNames.MainViewRegionName].RequestNavigate(obj.NameSpace, callback =>
            {
                _journal = callback.Context.NavigationService.Journal;
            });
        }

        public DelegateCommand<MenuBar> NavigationCommand { get; private set; }
        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand GoForwardCommand { get; private set; }


        private ObservableCollection<MenuBar> _menuBars;
        private IRegionNavigationJournal _journal;
        private readonly IRegionManager _regionManager;

        public ObservableCollection<MenuBar> MenuBars
        {
            get { return _menuBars; }
            set { SetProperty(ref _menuBars, value); }
        }

        public void CreateMenuBars()
        {
            _menuBars = new ObservableCollection<MenuBar>();
            _menuBars.Add(new MenuBar { Icon = "Home", Title = "首页", NameSpace = "IndexView" });
            _menuBars.Add(new MenuBar { Icon = "NotebookOutline", Title = "Todo", NameSpace = "TodoView" });
            _menuBars.Add(new MenuBar { Icon = "NotebookPlus", Title = "备忘录", NameSpace = "MemoView" });
            _menuBars.Add(new MenuBar { Icon = "Cog", Title = "设置", NameSpace = "SettingView" });
        }

    }
}
