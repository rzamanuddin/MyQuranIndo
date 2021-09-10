using System;
using System.Collections.Generic;
using System.Text;

namespace MyQuranIndo.Controls
{
    public interface IConfigurableScrollItem
    {
        ScrollToConfiguration Config { get; set; }
    }

    public interface IScrollItem : IConfigurableScrollItem
    {
    }


    public interface IGroupScrollItem : IConfigurableScrollItem
    {
        object GroupValue { get; set; }
    }
}
