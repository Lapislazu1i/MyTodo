using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTodo.SharedLib.Parameters
{
    public class TodoParameter : QueryParameter
    {
        public int? Status { get; set; }
    }
}
