using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CSPad
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static GlobalContractHost ContractHost { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            ContractHost = new CSPad.GlobalContractHost(App.Current.Dispatcher);
            // Execution.AssemblyHelper.CreateCache();

            Execution.CSharpExpressionEvaluator eval = new Execution.CSharpExpressionEvaluator();
            eval.Expression = "DateTime.Now";

            var test = eval.Execute();
            eval.Expression = "new string[] {\"a\",\"b\" }[1]";
            var test1 = eval.Execute();
            eval.Expression = "DateTime.Now";
            var test2 = eval.Execute();
            var test3 = eval.Execute();

            base.OnStartup(e);
        }
    }
}
