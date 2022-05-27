using MyTodo.Core;
using MyTodo.Events;
using Prism.Events;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTodo.Extensions
{
    public static class DialogExtension
    {
        public static async Task<IDialogResult> Question(this IDialogHostService dialogHost, string title="消息", string content="消息主体", string dialogHostName = "Root")
        {
            DialogParameters param = new DialogParameters();
            param.Add("Title", title);
            param.Add("Content", content);
            param.Add("DialogHostName", dialogHostName);
            var dialogRes = await dialogHost.ShowDialog("MessageDialog", param);
            return dialogRes;
        }

        public static void UpdateLoading(this IEventAggregator aggregator, UpdateModel model)
        {
            aggregator.GetEvent<UpdateLoadingEvent>().Publish(model);
        }

        public static void Resgiter(this IEventAggregator aggregator, Action<UpdateModel> action)
        {
            aggregator.GetEvent<UpdateLoadingEvent>().Subscribe(action);
        }

        public static void ResgiterMessage(this IEventAggregator aggregator,
    Action<MessageModel> action, string filterName = "Main")
        {
            aggregator.GetEvent<MessageEvent>().Subscribe(action,
                ThreadOption.PublisherThread, true, (m) =>
                {
                    return m.Filter.Equals(filterName);
                });
        }

        public static void SendMessage(this IEventAggregator aggregator, string message, string filterName = "Main")
        {
            aggregator.GetEvent<MessageEvent>().Publish(new MessageModel()
            {
                Filter = filterName,
                Message = message,
            });
        }
    }
}
