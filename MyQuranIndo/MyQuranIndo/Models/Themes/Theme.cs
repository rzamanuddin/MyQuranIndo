using System;
using System.Collections.Generic;
using System.Text;

namespace MyQuranIndo.Models.Themes
{
    public enum ThemeStyle : int
    {
        Blue = 0,
        Green = 1,
        Orange = 2,
        Black = 3,
        Purple = 4,
        Pink = 5,
        Red = 6,
        DarkBlue = 7
    }
    public class Theme
    {
        
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
