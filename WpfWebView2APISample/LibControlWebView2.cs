using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace WpfWebView2APISample
{
    static class LibControlWebView2
    {
        const string DLL_NAME = "LibControlWebView2";

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void DelegateComplete();

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int PluginInit(IntPtr hwnd, DelegateComplete callback);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr PluginGetControl();

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr PluginGetHost();
    }
}
