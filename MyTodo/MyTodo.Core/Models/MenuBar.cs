using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTodo.Core.Modals
{ 
    /// <summary>
    /// 系统导航实体类
    /// </summary>
    public class MenuBar : BindableBase
    {
        private string _icon;

        public string Icon
        {
            get { return _icon; }
            set { SetProperty(ref _icon, value); }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _nameSpace;

        public string NameSpace
        {
            get { return _nameSpace; }
            set { SetProperty(ref _nameSpace, value); }
        }


    }
}
