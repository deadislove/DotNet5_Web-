using DotNet5_web.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet5_web.Core.Domain.Interface
{
    public interface IDomain : IDisposable
    {
        void DomainExecute(out IEnumerable<Enterprise_MVC_CoreDTO> returnObj);
    }
}
