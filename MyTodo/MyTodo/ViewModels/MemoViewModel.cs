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
    public class MemoViewModel : NavigationViewModel
    {
        private readonly IMemoService _memoService;
        private readonly IDialogHostService _dialog;

        public MemoViewModel(IContainerProvider provider, IMemoService memoService, IDialogHostService dialogHostService) : base(provider)
        {
            MemoDtos = new ObservableCollection<MemoDto>();
            _memoService = memoService;
            _dialog = dialogHostService;
            GetDataAsync();
        }

        private MemoDto _currentDto;

        /// <summary>
        /// 编辑选中/新增时对象
        /// </summary>
        public MemoDto CurrentDto
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


        private string _search;

        /// <summary>
        /// 搜索条件
        /// </summary>
        public string Search
        {
            get { return _search; }
            set { _search = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<MemoDto> _MemoDtos;

        public ObservableCollection<MemoDto> MemoDtos
        {
            get => _MemoDtos;
            set { _MemoDtos = value; RaisePropertyChanged(); }
        }

        public DelegateCommand<string> _executeCommand;
        public DelegateCommand<MemoDto> _selectedCommand;
        public DelegateCommand<MemoDto> _delectedCommand;

        public DelegateCommand<string> ExecuteCommand { get => _executeCommand ??= new DelegateCommand<string>(Execute); }
        public DelegateCommand<MemoDto> SelectedCommand { get => _selectedCommand ??= new DelegateCommand<MemoDto>(Selected); }
        public DelegateCommand<MemoDto> DeleteCommand { get => _delectedCommand ??= new DelegateCommand<MemoDto>(Delete); }



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
            CurrentDto = new MemoDto();
            IsRightDrawerOpen = true;
        }

        private async void Selected(MemoDto obj)
        {
            try
            {
                UpdateLoading(true);
                var todoResult = await _memoService.GetFirstOfDefaultAsync(obj.Id);
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
                    var updateResult = await _memoService.UpdateAsync(CurrentDto);
                    if (updateResult.Status)
                    {
                        var todo = MemoDtos.FirstOrDefault(t => t.Id == CurrentDto.Id);
                        if (todo != null)
                        {
                            todo.Title = CurrentDto.Title;
                            todo.Content = CurrentDto.Content;
                        }
                    }
                    IsRightDrawerOpen = false;
                }
                else
                {
                    var addResult = await _memoService.AddAsync(CurrentDto);
                    if (addResult.Status)
                    {
                        MemoDtos.Add(addResult.Result);
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


            var todoResult = await _memoService.GetAllAsync(new QueryParameter()
            {
                PageIndex = 0,
                PageSize = 100,
                Search = Search
            });

            if (todoResult.Status)
            {
                MemoDtos.Clear();
                foreach (var item in todoResult.Result.Items)
                {
                    MemoDtos.Add(item);
                }
            }
            UpdateLoading(false);
        }

        private async void Delete(MemoDto obj)
        {
            try
            {
                var dialogResult = await _dialog.Question("温馨提示", $"确认删除待办事项:{obj.Title} ?");
                if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK) return;

                UpdateLoading(true);
                var deleteResult = await _memoService.DeleteAsync(obj.Id);
                if (deleteResult.Status)
                {
                    var model = MemoDtos.FirstOrDefault(t => t.Id.Equals(obj.Id));
                    if (model != null)
                        MemoDtos.Remove(model);
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
