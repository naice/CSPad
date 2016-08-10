using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSPad.ViewModel.ExecutionResult
{
    public class CollectionResult : BaseResult
    {
        public CollectionResult(object obj, Contracts.IUIDispatcher uiDispatcherContract) : base(obj, uiDispatcherContract)
        {
        }

        internal override Task GenerateResult()
        {
            return Task.Factory.StartNew(async () => {
                var result = Newtonsoft.Json.JsonConvert.SerializeObject(RawResult);
                await EnsureOnUIAsync(() => Result = result);
            });
        }
    }
}
