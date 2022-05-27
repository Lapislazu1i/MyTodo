using MyTodo.Core;
using MyTodo.Extensions;
using MyTodo.Services;
using MyTodo.SharedLib.Dtos;
using MyTodo.SharedLib.Parameters;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyTodo.ViewModels
{
    public class TodoViewModel : NavigationViewModel
    {
        private readonly ITodoService _todoService;
        private readonly IDialogHostService _dialog;

        public TodoViewModel(IContainerProvider provider, ITodoService todoService, IDialogHostService dialogHostService) : base(provider)
        {
            TodoDtos = new ObservableCollection<TodoDto>();
            _todoService = todoService;
            _dialog = dialogHostService;
            GetDataAsync();
        }

        private TodoDto _currentDto;

        /// <summary>
        /// 编辑选中/新增时对象
        /// </summary>
        public TodoDto CurrentDto
        {
            get => _currentDto;
            set { SetProperty(ref _currentDto, value); }
        }
        private bool _isRightDrawerOpen;

        /// <summary>
        /// 右侧编辑窗口是否展开
        /// </summary>
        public bool IsRightDrawerOpen
        {
            get { return _isRightDrawerOpen; }
            set { SetProperty(ref _isRightDrawerOpen, value); }
        }

        private int _selectedIndex;

        /// <summary>
        /// 下拉列表选中状态值
        /// </summary>
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { _selectedIndex = value; RaisePropertyChanged(); }
        }


        private string _search;

        /// <summary>
        /// 搜索条件
        /// </summary>
        public string Search
        {
            get { return _search; }
            set { _search = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<TodoDto> _todoDtos;

        public ObservableCollection<TodoDto> TodoDtos
        {
            get => _todoDtos;
            set { _todoDtos = value; RaisePropertyChanged(); }
        }

        public DelegateCommand<string> _executeCommand;
        public DelegateCommand<TodoDto> _selectedCommand;
        public DelegateCommand<TodoDto> _delectedCommand;

        public DelegateCommand<string> ExecuteCommand { get => _executeCommand ??= new DelegateCommand<string>(Execute); }
        public DelegateCommand<TodoDto> SelectedCommand { get => _selectedCommand ??= new DelegateCommand<TodoDto>(Selected); }
        public DelegateCommand<TodoDto> DeleteCommand { get => _delectedCommand ??= new DelegateCommand<TodoDto>(Delete); }



        private void Execute(string obj)
        {
            switch (obj)
            {
                case "新增": Add(); break;
                case "查询": GetDataAsync(); break;
                case "保存": Save(); break;
            }
        }

        private void Add()
        {
            CurrentDto = new TodoDto();
            IsRightDrawerOpen = true;
        }

        private async void Selected(TodoDto obj)
        {
            try
            {
                UpdateLoading(true);
                var todoResult = await _todoService.GetFirstOfDefaultAsync(obj.Id);
                if (todoResult.Status)
                {
                    CurrentDto = todoResult.Result;
                    IsRightDrawerOpen = true;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                UpdateLoading(false);
            }
        }

        private async void Save()
        {
            if (string.IsNullOrWhiteSpace(CurrentDto.Title) ||
                string.IsNullOrWhiteSpace(CurrentDto.Content))
                return;

            UpdateLoading(true);

            try
            {
                if (CurrentDto.Id > 0)
                {
                    var updateResult = await _todoService.UpdateAsync(CurrentDto);
                    if (updateResult.Status)
                    {
                        var todo = TodoDtos.FirstOrDefault(t => t.Id == CurrentDto.Id);
                        if (todo != null)
                        {
                            todo.Title = CurrentDto.Title;
                            todo.Content = CurrentDto.Content;
                            todo.Status = CurrentDto.Status;
                        }
                    }
                    IsRightDrawerOpen = false;
                }
                else
                {
                    var addResult = await _todoService.AddAsync(CurrentDto);
                    if (addResult.Status)
                    {
                        TodoDtos.Add(addResult.Result);
                        IsRightDrawerOpen = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                UpdateLoading(false);
            }
        }

        async void GetDataAsync()
        {
            UpdateLoading(true);

            int? Status = SelectedIndex switch
            {
                0 => null,
                1 => 0,
                2 => 1,
                _ => null
            };

            var todoResult = await _todoService.GetAllFilterAsync(new TodoParameter()
            {
                PageIndex = 0,
                PageSize = 100,
                Search = Search,
                Status = Status
            });

            if (todoResult.Status)
            {
                TodoDtos.Clear();
                foreach (var item in todoResult.Result.Items)
                {
                    TodoDtos.Add(item);
                }
            }
            UpdateLoading(false);
        }

        private async void Delete(TodoDto obj)
        {
            try
            {
                var dialogResult = await _dialog.Question("温馨提示", $"确认删除待办事项:{obj.Title} ?");
                if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK) return;

                UpdateLoading(true);
                var deleteResult = await _todoService.DeleteAsync(obj.Id);
                if (deleteResult.Status)
                {
                    var model = TodoDtos.FirstOrDefault(t => t.Id.Equals(obj.Id));
                    if (model != null)
                        TodoDtos.Remove(model);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                UpdateLoading(false);
            }
        }
    }
}
