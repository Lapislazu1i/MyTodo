using MyTodo.SharedLib.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTodo.Services
{
    internal class MemoService : BaseService<MemoDto>, IMemoService
    {
        public MemoService(HttpRestClient client ) : base(client, "Memo")
        {
        }
    }
}
