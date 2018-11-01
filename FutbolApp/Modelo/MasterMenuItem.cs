using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using System.Threading.Tasks;

namespace FutbolApp.Modelo
{
    public class MasterMenuItem
    {
        public string Title { get; set; }
        public string IconSource { get; set; }
        //public Color BackgroundColor { get; set; }
        public Type TargetType { get; set; }
        public MasterMenuItem(string Title, string Iconsource, Type target)
        {
            this.Title = Title;
            this.IconSource = IconSource;
            this.TargetType = target;
        }
    }
}
