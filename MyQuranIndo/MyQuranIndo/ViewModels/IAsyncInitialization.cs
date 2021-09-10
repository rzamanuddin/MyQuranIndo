using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyQuranIndo.ViewModels
{
    public interface IAsyncInitialization
    {
        Task Initialization { get; }
    }
}
