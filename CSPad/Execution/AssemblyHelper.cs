using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSPad.Execution
{
    public class AssemblyHelper
    {
        //private static string _cachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LINQPad\\AutoCompletionCache" + (Environment.Version.Major == 4 ? "40" : "") + ".2\\namespaces" + (Program.IsFW45Available ? "45" : "") + ".dat");
        private static readonly string _runtimeDir = RuntimeEnvironment.GetRuntimeDirectory();
        private static readonly string ExtraRefs = "PresentationCore\r\nPresentationFramework\r\nPresentationFramework.Aero\r\nPresentationFramework.Classic\r\nPresentationFramework.Luna\r\nPresentationFramework.Royale\r\nPresentationUI\r\nReachFramework\r\nSystem.Activities\r\nSystem.Activities.Core.Presentation\r\nSystem.Activities.DurableInstancing\r\nSystem.Activities.Presentation\r\nSystem.AddIn.Contract\r\nSystem.AddIn\r\nSystem.ComponentModel.Composition\r\nSystem.ComponentModel.DataAnnotations\r\nSystem.configuration\r\nSystem.Data.Services.Client\r\nSystem.Data.Services\r\nSystem.DirectoryServices.AccountManagement\r\nSystem.DirectoryServices\r\nSystem.DirectoryServices.Protocols\r\nSystem.Dynamic\r\nSystem.EnterpriseServices\r\nSystem.IdentityModel\r\nSystem.IdentityModel.Selectors\r\nSystem.IO.Compression\r\nSystem.IO.Compression.FileSystem\r\nSystem.IO.Log\r\nSystem.Net.Http\r\nSystem.Net.Http.WebRequest\r\nSystem.Management\r\nSystem.Management.Instrumentation\r\nSystem.Messaging\r\nSystem.Net\r\nSystem.Numerics\r\nSystem.Printing\r\nSystem.Runtime.Caching\r\nSystem.Runtime.DurableInstancing\r\nSystem.Runtime.Remoting\r\nSystem.Runtime.Serialization\r\nSystem.Runtime.Serialization.Formatters.Soap\r\nSystem.Security\r\nSystem.ServiceModel\r\nSystem.ServiceModel.Activation\r\nSystem.ServiceModel.Activities\r\nSystem.ServiceModel.Channels\r\nSystem.ServiceModel.Discovery\r\nSystem.ServiceModel.Routing\r\nSystem.ServiceModel.Web\r\nSystem.ServiceProcess\r\nSystem.Speech\r\nSystem.Web\r\nSystem.Web.ApplicationServices\r\nSystem.Web.DataVisualization\r\nSystem.Web.DynamicData\r\nSystem.Web.Entity.Design\r\nSystem.Web.Entity\r\nSystem.Web.Extensions\r\nSystem.Web.Mobile\r\nSystem.Web.Mvc\r\nSystem.Web.RegularExpressions\r\nSystem.Web.Services\r\nSystem.Windows.Forms\r\nSystem.Windows.Forms.DataVisualization\r\nSystem.Windows.Input.Manipulations\r\nSystem.Windows.Presentation\r\nSystem.Workflow.Activities\r\nSystem.Workflow.ComponentModel\r\nSystem.Workflow.Runtime\r\nSystem.WorkflowServices\r\nSystem.Xaml\r\nUIAutomationClient\r\nUIAutomationClientsideProviders\r\nUIAutomationProvider\r\nUIAutomationTypes\r\nWindowsBase\r\nWindowsFormsIntegration";
        private static Dictionary<string, string[]> _refLookup;

        private static Assembly GetAssembly(string name)
        {
            try
            {
                string fullPath = FindFullPath(name);
                if (!File.Exists(fullPath))
                    return (Assembly)null;
                return Assembly.LoadFrom(fullPath);
            }
            catch
            {
                return (Assembly)null;
            }
        }

        public static string FindFullPath(string simpleName)
        {
            string path2 = simpleName.Trim() + ".dll";
            string path1 = Path.Combine(_runtimeDir, path2);
            if (File.Exists(path1))
                return path1;
            if (Environment.Version.Major == 2)
            {
                string path1_1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Reference Assemblies\\Microsoft\\Framework");
                string path3 = Path.Combine(path1_1, "v3.0\\" + path2);
                if (File.Exists(path3))
                    return path3;
                string path4 = Path.Combine(path1_1, "v3.5\\" + path2);
                if (File.Exists(path4))
                    return path4;
            }
            else
            {
                string path3 = Path.Combine(_runtimeDir, "WPF\\" + path2);
                if (File.Exists(path3))
                    return path3;
                string path4 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Microsoft ASP.NET\\ASP.NET MVC 3\\Assemblies\\" + path2);
                if (File.Exists(path4))
                    return path4;
                string path5 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Microsoft ASP.NET\\ASP.NET MVC 2\\Assemblies\\" + path2);
                if (File.Exists(path5))
                    return path5;
            }
            return path2;
        }

        public static string[] GetFullPaths(string[] simpleNames)
        {
            List<string> result = new List<string>();

            foreach (var item in simpleNames)
            {
                result.Add(FindFullPath(item));
            }

            return result.ToArray();
        }

        public static void CreateCache()
        {
            try
            {
                if (_refLookup != null)
                    return;
                using (DomainIsolator domainIsolator = new DomainIsolator("AssemblyHelperTypePopulator"))
                    domainIsolator.Domain.DoCallBack(new CrossAppDomainDelegate(Populate));
            }
            catch (Exception ex)
            {

            }
        }

        private static Type[] GetExportedTypes(Assembly a)
        {
            try
            {
                return a.GetExportedTypes();
            }
            catch
            {
                return (Type[])null;
            }
        }

        private static void Populate()
        {
            Dictionary<string, string[]> dictionary =
                AssemblyHelper.ExtraRefs.Split(new string[1] { "\r\n" }, StringSplitOptions.None)
                .Select(f => f.Trim())
                .Select(file => new { file = file, a = AssemblyHelper.GetAssembly(file) })
                .Where(fa => fa.a != null)
                .Select(fa => new { fa = fa, types = AssemblyHelper.GetExportedTypes(fa.a) })
                .Where(fat => fat.types != null)
                .SelectMany(fat => (IEnumerable<Type>)fat.types, (param0, t) => new { fat = param0, t = t })
                .Select(fat => new { fat = fat, name = ((IEnumerable<string>)fat.t.FullName.Split('.')).Last<string>().Replace('+', '.') })
                .GroupBy(fatn => ((IEnumerable<string>)fatn.name.Split('`')).First<string>(), fatn => new { Namespace = fatn.fat.t.Namespace, file = fatn.fat.fat.fa.file })
                .Select(g => new { TypeName = g.Key, Locations = g.Distinct() })
                .OrderBy(g => g.TypeName).ToDictionary(g => g.TypeName, g => g.Locations.Select(l => l.Namespace + ";" + l.file).ToArray<string>());

            string dict = Newtonsoft.Json.JsonConvert.SerializeObject(dictionary, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText("cache.json", dict);

            //using (FileStream fileStream = File.Create(AutoRefManager._cachePath))
            //    new BinaryFormatter().Serialize((Stream)fileStream, (object)dictionary);
        }
    }
}
