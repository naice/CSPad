using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSPad.ViewModel
{
    public class AsyncBaseViewModel : BaseViewModel
    {
        protected Contracts.IUIDispatcher UIDispatcherContract { get; private set; }

        public AsyncBaseViewModel(Contracts.IUIDispatcher uiDispatcherContract)
        {
            UIDispatcherContract = uiDispatcherContract;
        }

        protected async void EnsureOnUI(Action callback)
        {
            await EnsureOnUIAsync(callback);
        }
        protected async Task EnsureOnUIAsync(Action callback)
        {
            if (UIDispatcherContract.UIDispatcher.Thread == System.Threading.Thread.CurrentThread)
            {
                callback.Invoke();
            }
            else
            {
                await UIDispatcherContract.UIDispatcher.InvokeAsync(callback);
            }
        }
    }
}
