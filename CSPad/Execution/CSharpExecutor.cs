using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CSPad.DataModel;

namespace CSPad.Execution
{
    public class CSharpExecutorCompileException : Exception
    {
        public CompilerErrorCollection Errors { get; private set; }

        public CSharpExecutorCompileException(CompilerErrorCollection errors)
        {
            Errors = errors;
        }
    }

    public abstract class CSharpExecutor
    {
        protected abstract string Code { get; }
        public UniqueStringCollection ReferencedAssemblys => new UniqueStringCollection(
            new string[] {
                "System.dll",
                "Microsoft.CSharp.dll",
                "System.Core.dll",
                "System.Data.dll",
                "System.Transactions.dll",
                "System.Xml.dll",
                "System.Xml.Linq.dll",
                "System.Data.Linq.dll",
                "System.Drawing.dll",
                "System.Data.DataSetExtensions.dll"
            }
        );
        public UniqueStringCollection UsingDirectives => new UniqueStringCollection(
            new string[] {
                "System",
                "System.IO",
                "System.Text",
                "System.Text.RegularExpressions",
                "System.Diagnostics",
                "System.Threading",
                "System.Reflection",
                "System.Collections",
                "System.Collections.Generic",
                "System.Linq",
                "System.Linq.Expressions",
                "System.Data",
                "System.Data.SqlClient",
                "System.Data.Linq",
                "System.Data.Linq.SqlClient",
                "System.Transactions",
                "System.Xml",
                "System.Xml.Linq",
                "System.Xml.XPath",
            }
        );

        

        public CSharpExecutor()
        {
        }



        private CompilerResults CompileAssembly()
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            var compilerParams = new CompilerParameters(ReferencedAssemblys.ToArray()); // TODO: UniqueStingCollection...
            compilerParams.GenerateInMemory = true;            
            compilerParams.IncludeDebugInformation = true;
            //compilerParams.LinkedResources.AddRange(LinkedResources);
            //compilerParams.ReferencedAssemblies.AddRange(AssemblyHelper.GetFullPaths(new string[] {
            //    "System",
            //    "System.Core",
            //    "System.Data",
            //    //"Newtonsoft.Json.dll",
            //}));

            StringBuilder code = new StringBuilder();
            foreach (var usingDirective in UsingDirectives)
            {
                code.AppendLine($"using {usingDirective};");
            }
            code.AppendLine(); code.AppendLine();
            code.Append(Code);
            return provider.CompileAssemblyFromSource(compilerParams, code.ToString());
        }

        protected abstract object ExecuteAssembly(CompilerResults result);

        public object Execute()
        {
            var compilerResult = CompileAssembly();

            if (compilerResult.Errors != null && compilerResult.Errors.HasErrors)
                throw new CSharpExecutorCompileException(compilerResult.Errors);

            if (compilerResult.CompiledAssembly == null)
                throw new Exception("Unexpected CompiledAssembly is null but no compiler errors.");

            return ExecuteAssembly(compilerResult);
        }
    }
}
