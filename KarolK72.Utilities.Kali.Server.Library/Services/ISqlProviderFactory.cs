using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarolK72.Utilities.Kali.Server.Library.Services
{
    public interface ISqlProviderFactory<T> where T : ISqlProvider
    {
        T CreateNew();
    }
}
