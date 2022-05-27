
using DryIoc;
using MyTodo.Core;
using MyTodo.Services;
using MyTodo.ViewModels;
using MyTodo.Views;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;

namespace MyTodo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {


            containerRegistry.RegisterInstance(new HttpRestClient(@"http://localhost:5000/"));

            containerRegistry.Register<ILoginService, LoginService>();
            containerRegistry.Register<ITodoService, TodoService>();
            containerRegistry.Register<IMemoService, MemoService>();
            containerRegistry.Register<IDialogHostService, DialogHostService>();


            containerRegistry.RegisterForNavigation<LoadingDialog,LoadingDialogViewModel>();
            containerRegistry.RegisterForNavigation<AddTodoDialog,AddTodoDialogViewModel>();
            containerRegistry.RegisterForNavigation<AddMemoDialog,AddMemoDialogViewModel>();
            containerRegistry.RegisterForNavigation<MessageDialog, MessageDialogViewModel>();

            containerRegistry.RegisterForNavigation<AboutView,AboutViewModel>();
            containerRegistry.RegisterForNavigation<SkinView, SkinViewModel>();

            containerRegistry.RegisterForNavigation<IndexView, IndexViewModel>();
            containerRegistry.RegisterForNavigation<MemoView, MemoViewModel>();
            containerRegistry.RegisterForNavigation<SettingView, SettingViewModel>();
            containerRegistry.RegisterForNavigation<TodoView, TodoViewModel>();



        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            //moduleCatalog.AddModule<ModuleNameModule>();
        }
    }
}
