using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSPad.ViewModel.ExecutionResult
{
    public class NullResult : BaseResult
    {
        public NullResult(object obj, Contracts.IUIDispatcher uiDispatcherContract) : base(obj, uiDispatcherContract)
        {
        }

        internal override Task GenerateResult()
        {
            // do nothing since we are a null result.

            return Task.Delay(1) ;
        }
    }
}
