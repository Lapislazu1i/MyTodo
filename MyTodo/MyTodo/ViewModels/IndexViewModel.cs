using MyTodo.Core;
using MyTodo.Core.Models;
using MyTodo.Services;
using MyTodo.SharedLib.Dtos;
using MyTodo.Extensions;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Prism.Events;

namespace MyTodo.ViewModels
{
    public class IndexViewModel : NavigationViewModel
    {
        private readonly ITodoService _todoService;
        private readonly IMemoService _memoService;
        private readonly IDialogHostService _dialog;
        private readonly IRegionManager _regionManager;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        DelegateCommand<TodoDto> _changeTodoStateToTrueCommand;
        DelegateCommand<TodoDto> _addTodoDtoCommand;
        DelegateCommand<MemoDto> _addMemoDtoCommand;
        DelegateCommand<TodoDto> _editTodoDtoCommand;
        DelegateCommand<MemoDto> _editMemoDtoCommand;

        public DelegateCommand ShowDigCommand => new DelegateCommand(() => MessageBox.Show("sd"));

        public DelegateCommand<TodoDto> ChangeTodoStateToTrueCommand { get => _changeTodoStateToTrueCommand ??= new DelegateCommand<TodoDto>(ChangeTodoStateToTrue); }

        public DelegateCommand<TodoDto> AddTodoDtoCommand { get => _addTodoDtoCommand ??= new DelegateCommand<TodoDto>(AddTodo); }

        public DelegateCommand<MemoDto> AddMemoDtoCommand { get => _addMemoDtoCommand ??= new DelegateCommand<MemoDto>(AddMemo); }

        public DelegateCommand<TodoDto> EditTodoDtoCommand { get => _editTodoDtoCommand ??= new DelegateCommand<TodoDto>(AddTodo); }

        public DelegateCommand<MemoDto> EditMemoDtoCommand { get => _editMemoDtoCommand ??= new DelegateCommand<MemoDto>(AddMemo); }

        public IndexViewModel(IContainerProvider provider, IDialogHostService dialog) : base(provider)
        {
            Title = $"你好，{AppSession.UserName} {DateTime.Now.GetDateTimeFormats('D')[1].ToString()}";


            _todoService = provider.Resolve<ITodoService>();
            _memoService = provider.Resolve<IMemoService>();
            _regionManager = provider.Resolve<IRegionManager>();

            _dialog = dialog;

            CreateTaskBar();

        }

        public async void ChangeTodoStateToTrue(TodoDto model)
        {
            UpdateLoading(true);

            var res = await _todoService.UpdateAsync(model);
            RefreshLocation();

            UpdateLoading(false);


        }

        public async void AddTodo(TodoDto model)
        {
            DialogParameters param = new DialogParameters();
            if (model != null)
                param.Add("Value", model);

            string dialogName = "AddTodoDialog";
            var dialogResult = await _dialog.ShowDialog(dialogName, param);
            if (dialogResult.Result == ButtonResult.OK)
            {
                try
                {
                    UpdateLoading(true);
                    var todo = dialogResult.Parameters.GetValue<TodoDto>("Value");
                    if (todo.Id > 0)
                    {
                        var updateResult = await _todoService.UpdateAsync(todo);
                        if (updateResult.Status)
                        {
                            var todoModel = _summary.TodoList.FirstOrDefault(t => t.Id.Equals(todo.Id));
                            if (todoModel != null)
                            {
                                todoModel.Title = todo.Title;
                                todoModel.Content = todo.Content;
                            }
                        }
                    }
                    else
                    {
                        var addResult = await _todoService.AddAsync(todo);
                        if (addResult.Status)
                        {
                            _summary.Sum += 1;
                            _summary.TodoList.Add(addResult.Result);
                            _summary.CompletedRatio = (_summary.CompletedCount / (double)_summary.Sum).ToString("0%");
                            this.Refresh();
                        }
                    }
                }
                finally
                {
                    UpdateLoading(false);
                }
            }

        }
        public async void AddMemo(MemoDto model)
        {
            DialogParameters param = new DialogParameters();
            if (model != null)
                param.Add("Value", model);

            string dialogName = "AddMemoDialog";
            var dialogResult = await _dialog.ShowDialog(dialogName, param);
            if (dialogResult.Result == ButtonResult.OK)
            {
                try
                {
                    UpdateLoading(true);
                    var memo = dialogResult.Parameters.GetValue<MemoDto>("Value");
                    if (memo.Id > 0)
                    {
                        var updateResult = await _memoService.UpdateAsync(memo);
                        if (updateResult.Status)
                        {
                            var memoModel = _summary.MemoList.FirstOrDefault(t => t.Id.Equals(memo.Id));
                            if (memoModel != null)
                            {
                                memoModel.Title = memo.Title;
                                memoModel.Content = memo.Content;
                            }
                        }
                    }
                    else
                    {
                        var addResult = await _memoService.AddAsync(memo);
                        if (addResult.Status)
                        {
                            _summary.MemoList.Add(addResult.Result);

                        }
                    }
                }
                finally
                {
                    UpdateLoading(false);
                }
            }

        }

        public async void UpdateSummary()
        {
            try
            {
                UpdateLoading(true);
                var res = await _todoService.SummaryAsync();
                Summary = res.Result;
                Refresh();
            }
            catch(Exception e)
            {

            }
            finally
            {
                UpdateLoading(false);
            }

        }

        void RefreshLocation(int sum = 0)
        {
            Summary.Sum += sum;
            Summary.CompletedCount = Summary.TodoList.Where(v => v.Status != 0).Count();
            Summary.MemoeCount = Summary.MemoList.Count();
            Summary.CompletedRatio = (Summary.CompletedCount / (double)Summary.Sum).ToString("0%");
            Refresh();
        }
        void Refresh()
        {
            TaskBars[0].Content = _summary.Sum.ToString();
            TaskBars[1].Content = _summary.CompletedCount.ToString();
            TaskBars[2].Content = _summary.CompletedRatio;
            TaskBars[3].Content = _summary.MemoeCount.ToString();
        }

        private ObservableCollection<TaskBar> _taskbars;


        private ObservableCollection<TodoDto> _todoDto;
        private ObservableCollection<MemoDto> _memoDto;


        private SummaryDto _summary;
        private string _title;

        public SummaryDto Summary
        {
            get => _summary;
            set => SetProperty(ref _summary, value);
        }

        public ObservableCollection<TaskBar> TaskBars
        {
            get => _taskbars;
            set => SetProperty(ref _taskbars, value);
        }
        public ObservableCollection<TodoDto> TodoDtos
        {
            get => _todoDto;
            set => SetProperty(ref _todoDto, value);
        }

        public ObservableCollection<MemoDto> MemoDtos
        {
            get => _memoDto;
            set => SetProperty(ref _memoDto, value);
        }

        private void CreateTaskBar(string sum = "", string completedCount = "", string memoeCount = "", string completedRatio = "")
        {
            _taskbars = new ObservableCollection<TaskBar>();
            _taskbars.Add(new TaskBar { Icon = "ClockFast", Title = "汇总", Content = sum, Target = "", Color = "#318ce7" });
            _taskbars.Add(new TaskBar { Icon = "ClockCheckOutline", Title = "已完成", Content = completedCount, Target = "", Color = "#a40000" });
            _taskbars.Add(new TaskBar { Icon = "ChartLineVariant", Title = "完成比例", Content = completedRatio, Target = "", Color = "#007ba7" });
            _taskbars.Add(new TaskBar { Icon = "PlaylistStar", Title = "备忘录", Content = memoeCount, Target = "", Color = "#36454f" });

        }

        private void CreateTestData()
        {
            _todoDto = new ObservableCollection<TodoDto>();
            _memoDto = new ObservableCollection<MemoDto>();
            _todoDto.Add(new TodoDto { Id = 1, Title = "nidfdf", Content = "nihao", Status = 1, CreateData = DateTime.Now, UpdateData = DateTime.Now });
            _todoDto.Add(new TodoDto { Id = 2, Title = "nidfdf", Content = "nihao", Status = 1, CreateData = DateTime.Now, UpdateData = DateTime.Now });
            _memoDto.Add(new MemoDto { Id = 3, Title = "nidfdf", Content = "nihao", CreateData = DateTime.Now, UpdateData = DateTime.Now });

        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            UpdateSummary();
        }


    }
}
