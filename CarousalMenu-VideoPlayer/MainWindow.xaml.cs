using CarousalMenu.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CarousalMenu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private object objContent;
        private WindowStyle windowStyle;
        private WindowState windowState;
        private ResizeMode resizeMode;
        private double leftPosition;
        private double topPosition;
        private double width;
        private double height;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void Fullscreen(object obj, bool bFullscreen)
        {
            if (bFullscreen)
            {
                IntPtr handle = (new WindowInteropHelper(this)).Handle;

                // Use WindowProc as the callback method
                // to process all native window messages.
                HwndSource.FromHwnd(handle)
                    .AddHook(NativeMethods.MaximizedSizeFixWindowProc);

                Dispatcher.Invoke(() =>
                {
                    objContent = Content;
                    windowStyle = WindowStyle;
                    windowState = WindowState;
                    resizeMode = ResizeMode;
                    leftPosition = Left;
                    topPosition = Top;
                    width = Width;
                    height = Height;

                    WindowStyle = WindowStyle.None;
                    WindowState = WindowState.Maximized;
                    WindowState = WindowState.Normal;
                    ResizeMode = ResizeMode.NoResize;
                    Left = 0;
                    Top = 0;
                    Content = obj;
                });
            }
            else
            {
                IntPtr handle = (new WindowInteropHelper(this)).Handle;

                // Use WindowProc as the callback method
                // to process all native window messages.
                HwndSource.FromHwnd(handle)
                    .RemoveHook(NativeMethods.MaximizedSizeFixWindowProc);

                Dispatcher.Invoke(() =>
                {
                    Content = objContent;
                    WindowStyle = windowStyle;
                    WindowState = windowState;
                    ResizeMode = resizeMode;
                    Left = leftPosition;
                    Top = topPosition;
                    Width = width;
                    Height = height;
                });
            }
        }
    }
}
