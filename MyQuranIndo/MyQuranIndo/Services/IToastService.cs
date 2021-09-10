using System;
using System.Collections.Generic;
using System.Text;

namespace MyQuranIndo.Services
{
    public interface IToastService
    {
        void Show(string message, bool isLongDuration, bool isCenter = false);
        void Show(string message, bool isLongDuration);

        void Show(string message);
        
    }
}
