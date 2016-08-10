using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSPad.ViewModel.ExecutionResult
{
    public abstract class BaseResult : AsyncBaseViewModel
    {
        public object Result { get; protected set; } = null;
        protected object RawResult;

        public BaseResult(object obj, Contracts.IUIDispatcher uiDispatcherContract) : base(uiDispatcherContract)
        {
            RawResult = obj;
        }

        internal abstract Task GenerateResult();
        
    }
}
