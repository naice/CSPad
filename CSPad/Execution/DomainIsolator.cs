using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSPad.Execution
{
    /// <summary>
    /// Isolate the execution to an own AppDomain for heavy load Tasks.
    /// </summary>
    internal class DomainIsolator : IDisposable
    {
        private readonly AppDomain _domain;

        public AppDomain Domain
        {
            get
            {
                return this._domain;
            }
        }

        public DomainIsolator(string friendlyName)
            : this(AppDomain.CreateDomain(friendlyName))
        {

        }

        public DomainIsolator(AppDomain domain)
        {
            this._domain = domain;
        }

        public T GetInstance<T>() where T : MarshalByRefObject
        {
            return (T)this.GetInstance(typeof(T));
        }

        public object GetInstance(Type t)
        {
            try
            {
                return this._domain.CreateInstanceFromAndUnwrap(t.Assembly.Location, t.FullName);
            }
            catch (FileNotFoundException ex)
            {
                return this._domain.CreateInstanceAndUnwrap(t.Assembly.FullName, t.FullName);
            }
        }

        public void Dispose()
        {
            UnloadAppDomain(this._domain, 3);
        }

        /// <summary>
        /// Attempts to unload the created AppDomain.
        /// </summary>
        /// <param name="d">DOmain to unload.</param>
        /// <param name="attempts">Count of attempts to unload domain, default = 3.</param>
        internal static void UnloadAppDomain(AppDomain d, int attempts = 3)
        {
            for (int index = 0; index < attempts; ++index)
            {
                if (index > 0)
                    Thread.Sleep(100 * index);
                try
                {
                    AppDomain.Unload(d);
                    return;
                }
                catch (AppDomainUnloadedException ex)
                {
                    return;
                }
                catch (CannotUnloadAppDomainException ex)
                {
                }
            }

        }
    }
}
