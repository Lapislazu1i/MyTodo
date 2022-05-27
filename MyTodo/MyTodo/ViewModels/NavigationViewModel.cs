using MyTodo.Events;
using MyTodo.Extensions;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTodo.ViewModels
{
    public class NavigationViewModel: BindableBase, INavigationAware
    {
        private readonly IContainerProvider _containerProvider;
        public readonly IEventAggregator _aggregator;

        public NavigationViewModel(IContainerProvider containerProvider)
        {
            this._containerProvider = containerProvider;
            _aggregator = containerProvider.Resolve<IEventAggregator>();
        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {

        }

        public void UpdateLoading(bool IsOpen)
        {
            _aggregator.UpdateLoading(new UpdateModel()
            {
                IsOpen = IsOpen
            });
        }
    }
}
