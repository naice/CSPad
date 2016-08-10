using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

using CSPad.Contracts;

namespace CSPad
{
    public class GlobalContractHost : IUIDispatcher
    {
        private Dispatcher _uiDispatcher;
        public Dispatcher UIDispatcher
        {
            get
            {
                return _uiDispatcher;
            }
        }

        public GlobalContractHost(Dispatcher uiDispatcher)
        {
            _uiDispatcher = uiDispatcher;
        }
    }
}
