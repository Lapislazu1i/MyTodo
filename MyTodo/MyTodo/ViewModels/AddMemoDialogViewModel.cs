using MaterialDesignThemes.Wpf;
using MyTodo.Core;
using MyTodo.SharedLib.Dtos;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyTodo.ViewModels
{
    public class AddMemoDialogViewModel : BindableBase, IDialogHostAware
    {
        public AddMemoDialogViewModel()
        {
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
        }


        /// <summary>
        /// 取消
        /// </summary>
        private void Cancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No)); //取消返回NO告诉操作结束
        }

        /// <summary>
        /// 确定
        /// </summary>
        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Model.Title) ||
                string.IsNullOrWhiteSpace(Model.Content)) return;

            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                //确定时,把编辑的实体返回并且返回OK
                DialogParameters param = new DialogParameters();
                param.Add("Value", Model);
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, param));
            }
        }
        private MemoDto _model;

        public MemoDto Model
        {
            get { return _model; }
            set { SetProperty(ref _model, value); }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string DialogHostName { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        public void OnDialogOpend(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Value"))
            {
                Model = parameters.GetValue<MemoDto>("Value");
                Title = "修改备忘录";
            }
            else
            {

                Model = new MemoDto();
                Title = "增加备忘录";

            }
        }
    }
}
