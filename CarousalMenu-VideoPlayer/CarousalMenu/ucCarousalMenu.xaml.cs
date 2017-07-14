using CarousalMenu.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CarousalMenu.Controls
{
    /// <summary>
    /// Interaction logic for ucCarousalMenu.xaml
    /// </summary>
    public partial class ucCarousalMenu : UserControl
    {
        public bool Loading { get; set; }
        #region Constructor

        public ucCarousalMenu()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        private void BindTree()
        {
            Loading = true;
            tvMain.ItemsSource = MenuDataSource.GetSampleList();
            for (int i = 0; i < tvMain.Items.Count; i++)
            {
                TreeViewItem child = (TreeViewItem)(tvMain
                    .ItemContainerGenerator
                    .ContainerFromIndex(i));
                ApplyItemColor(child);
            }
            Loading = false;
            TreeViewItem first = (TreeViewItem)(tvMain
                    .ItemContainerGenerator
                    .ContainerFromIndex(0));
            if (first != null)
                DeepExpand(first);
        }

        private void DeepExpand(TreeViewItem item)
        {
            item.IsExpanded = true;
            for (int i = 0; i < item.Items.Count; i++)
            {
                TreeViewItem child = (TreeViewItem)(item
                    .ItemContainerGenerator
                    .ContainerFromIndex(i));
                DeepExpand(child);
            }
        }

        private void ApplyItemColor(TreeViewItem item)
        {
            if (item == null)
                return;
            MenuDataSource dataSource = (MenuDataSource)item.DataContext;
            item.Background = new SolidColorBrush(dataSource.BackgroundColor);
            item.Padding = new Thickness(dataSource.PaddingLeft, 0, 0, 0);
            item.Foreground = new SolidColorBrush(dataSource.ForegroundColor);
            item.Expanded += new RoutedEventHandler(TreeViewItem_Expanded);
            item.MouseLeftButtonDown += new MouseButtonEventHandler(TreeViewItem_MouseLeftButtonDown);
            if (!item.HasItems)
            {
                item.BorderThickness = new Thickness(0, 0, 0, 1);
                item.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 226, 226, 226));
            }
            else
            {
                item.ExpandSubtree();
                for (int i = 0; i < item.Items.Count; i++)
                {
                    TreeViewItem child = (TreeViewItem)(item
                        .ItemContainerGenerator
                        .ContainerFromIndex(i));
                    ApplyItemColor(child);
                }
                item.IsExpanded = false;
            }
        }

        private void RaiseLeafMenuClicked(object sender, MenuDataSource param)
        {
            LeafMenuClickedEventArgs newEventArgs = new LeafMenuClickedEventArgs(param);
            newEventArgs.RoutedEvent = LeafMenuClickedEvent;
            RaiseEvent(newEventArgs);
        }

        #endregion

        #region Events

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BindTree();
        }

        private void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        {
            if (Loading)
                return;
            //TreeViewItem item = (TreeViewItem)sender;
            TreeViewItem item = e.OriginalSource as TreeViewItem;
            if (item == null)
                return;
            ItemsControl parent = ItemsControl.ItemsControlFromItemContainer(item);
            if (parent == null)
                return;
            ItemCollection items = null;
            if (parent is TreeView)
            {
                TreeView tree = parent as TreeView;
                items = tree.Items;
            }
            else
            {
                TreeViewItem treeItem = parent as TreeViewItem;
                items = treeItem.Items;
            }
            if (items == null)
                return;
            for (int i = 0; i < items.Count; i++)
            {
                TreeViewItem child = (TreeViewItem)(parent
                    .ItemContainerGenerator
                    .ContainerFromIndex(i));
                if (child != item)
                    child.IsExpanded = false;
            }
            DeepExpand(item);
        }
        
        private void TreeViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            TreeViewItem item = e.OriginalSource as TreeViewItem;
            if (!item.HasItems)
                item.Background = new SolidColorBrush(Color.FromArgb(255, 102, 176, 213));
        }

        private void TreeViewItem_MouseLeave(object sender, MouseEventArgs e)
        {
            TreeViewItem item = e.OriginalSource as TreeViewItem;
            if (!item.HasItems)
                item.Background = new SolidColorBrush(((MenuDataSource)item.DataContext).BackgroundColor);
        }

        private void TreeViewItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            TreeViewItem item = grid.TemplatedParent as TreeViewItem;
            if (item == null)
                return;
            if (item.HasItems)
                item.IsExpanded = true;
            else
            {
                RaiseLeafMenuClicked(this, item.DataContext as MenuDataSource);
                //if (LeafMenuClicked != null)
                //    LeafMenuClicked(this, item.DataContext as MenuDataSource);
            }
        }

        #endregion

        public static readonly RoutedEvent LeafMenuClickedEvent = EventManager.RegisterRoutedEvent(
                "LeafMenuClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ucCarousalMenu));
        public event RoutedEventHandler LeafMenuClicked
        {
            add { AddHandler(LeafMenuClickedEvent, value); }
            remove { RemoveHandler(LeafMenuClickedEvent, value); }
        }
    }
}
