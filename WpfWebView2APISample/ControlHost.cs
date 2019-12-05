// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

#region Using directives

using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;

#endregion

namespace WpfWebView2APISample
{
    public class ControlHost : HwndHost
    {
        internal const int
            WsChild = 0x40000000,
            WsVisible = 0x10000000,
            LbsNotify = 0x00000001,
            HostId = 0x00000002,
            WsVscroll = 0x00200000,
            WsBorder = 0x00800000;

        private readonly int _hostHeight;
        private readonly int _hostWidth;
        private IntPtr _hwndHost;

        public ControlHost(double height, double width)
        {
            _hostHeight = (int)height;
            _hostWidth = (int)width;
        }

        private void OnCompleteWebView2()
        {
            if (true)
            {
            }
        }

        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            _hwndHost = CreateWindowEx(0, "static", "",
                WsChild | WsVisible,
                0, 0,
                _hostHeight, _hostWidth,
                hwndParent.Handle,
                (IntPtr)HostId,
                IntPtr.Zero,
                0);

            LibControlWebView2.PluginInit(_hwndHost, OnCompleteWebView2);

            return new HandleRef(this, _hwndHost);
        }

        protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            IntPtr result;
            handled = LibControlWebView2.PluginHandleWindowMessage(hwnd, msg, wParam, lParam, out result);
            if (handled)
            {
                return result;
            }
            return base.WndProc(hwnd, msg, wParam, lParam, ref handled);
        }

        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            DestroyWindow(hwnd.Handle);
        }

        //PInvoke declarations
        [DllImport("user32.dll", EntryPoint = "CreateWindowEx", CharSet = CharSet.Unicode)]
        internal static extern IntPtr CreateWindowEx(int dwExStyle,
            string lpszClassName,
            string lpszWindowName,
            int style,
            int x, int y,
            int width, int height,
            IntPtr hwndParent,
            IntPtr hMenu,
            IntPtr hInst,
            [MarshalAs(UnmanagedType.AsAny)] object pvParam);

        [DllImport("user32.dll", EntryPoint = "DestroyWindow", CharSet = CharSet.Unicode)]
        internal static extern bool DestroyWindow(IntPtr hwnd);
    }
}