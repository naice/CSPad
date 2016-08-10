using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSPad.Execution
{
    public class CSharpExpressionEvaluator : CSharpExecutor
    {

        private static readonly string ExpressionBaseCode =
@"
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

public class Program
{{
    public static object Execute()
    {{
        return 
{0};
    }}
}}
";
        protected override string Code
        {
            get
            {
                return string.Format(ExpressionBaseCode, Expression);
            }
        }

        public string Expression { get; set; }

        protected override object ExecuteAssembly(CompilerResults result)
        {
            Type programType = result.CompiledAssembly.GetType("Program");
            var methodInfo = programType.GetMethod("Execute");
            
            var delegateFunc = (Func<object>)Delegate.CreateDelegate(typeof(Func<object>), methodInfo);
            var obj = delegateFunc();

            return obj;
        }
    }
}
