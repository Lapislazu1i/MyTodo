using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTodo.Core.Models
{
    public class TaskBar : BindableBase
    {
        private string _icon;
        private string _title;
        private string _content;
        private string _color;
        private string _target;
        public string Icon { get => _icon; set => SetProperty(ref _icon, value); }
        public string Title { get => _title; set => SetProperty(ref _title, value); }
        public string Content { get => _content; set => SetProperty(ref _content, value); }
        public string Color { get => _color; set => SetProperty(ref _color,value); }
        public string Target { get => _target; set => SetProperty(ref _target, value); }
    }
}
