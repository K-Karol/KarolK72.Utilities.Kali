using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarolK72.Utilities.Kali.Extensions
{
    internal sealed class NullExternalScopeProvider : IExternalScopeProvider
    {
        private NullExternalScopeProvider()
        {
        }
        public static IExternalScopeProvider Instance { get; } = new NullExternalScopeProvider();

        void IExternalScopeProvider.ForEachScope<TState>(Action<object?, TState> callback, TState state)
        {
        }

        IDisposable IExternalScopeProvider.Push(object? state)
        {
            return NullScope.Instance;
        }
    }
}
