using CarousalMenu.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CarousalMenu.Controls
{
    class LeafMenuClickedEventArgs : RoutedEventArgs
    {
        public MenuDataSource Param { get; private set; }

        internal LeafMenuClickedEventArgs(MenuDataSource param)
        {
            Param = param;
        }
    }
}
