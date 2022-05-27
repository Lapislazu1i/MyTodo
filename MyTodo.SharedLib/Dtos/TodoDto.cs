using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTodo.SharedLib.Dtos
{
    public class TodoDto : BaseDto
    {
        private string _title;
        private string _content;
        private int status;

        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }

        public string Content
        {
            get { return _content; }
            set { _content = value; OnPropertyChanged(); }
        }

        public int Status
        {
            get { return status; }
            set { status = value; OnPropertyChanged(); }
        }
    }
}
