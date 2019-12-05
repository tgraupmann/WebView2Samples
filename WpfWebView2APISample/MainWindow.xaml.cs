// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;

namespace WpfWebView2APISample
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Application _app;
        private IntPtr _hwndControl;
        private ControlHost _win32Control;
        private Window _myWindow;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnCompleteWebView2()
        {
            // ControlHostElement - XAML element to display the browser
            _win32Control = new ControlHost(ControlHostElement.ActualHeight, ControlHostElement.ActualWidth);
            ControlHostElement.Child = _win32Control;
            _win32Control.MessageHook += ControlMsgFilter;
            _hwndControl = _win32Control.HwndControl;
        }

        private void On_UIReady(object sender, EventArgs e)
        {
            _app = Application.Current;
            _myWindow = _app.MainWindow;

            IntPtr windowHandle = new WindowInteropHelper(_myWindow).Handle;
            LibControlWebView2.PluginInit(windowHandle, OnCompleteWebView2);

            _myWindow.SizeToContent = SizeToContent.WidthAndHeight;
        }

        private IntPtr ControlMsgFilter(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            handled = false;
            return IntPtr.Zero;
        }

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Unicode)]
        internal static extern int SendMessage(IntPtr hwnd,
            int msg,
            IntPtr wParam,
            IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Unicode)]
        internal static extern int SendMessage(IntPtr hwnd,
            int msg,
            int wParam,
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Unicode)]
        internal static extern IntPtr SendMessage(IntPtr hwnd,
            int msg,
            IntPtr wParam,
            string lParam);
    }
}
