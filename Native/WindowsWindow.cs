﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Tile.Net
{
    public class WindowsWindow : IWindow
    {
        private IntPtr _handle;

        public WindowsWindow(IntPtr handle)
        {
            _handle = handle;
        }

        public string Title
        {
            get
            {
                var buffer = new StringBuilder(255);
                Win32.GetWindowText(_handle, buffer, buffer.Capacity + 1);
                return buffer.ToString();
            }
        }

        public IWindowLocation Location
        {
            get
            {
                Win32.Rect rect = new Win32.Rect();
                Win32.GetWindowRect(_handle, ref rect);
                return new WindowLocation(rect.Left, rect.Top, rect.Right, rect.Bottom);
            }
        }

        public bool CanLayout
        {
            get
            {
                return !Win32Helper.IsCloaked(_handle)  &&
                    Win32Helper.IsAppWindow(_handle) && 
                    Win32Helper.IsAltTabWindow(_handle) &&
                    !Win32.IsIconic(_handle);
            }
        }

        public bool CanResize
        {
            get
            {
                return Win32.GetWindowStyleLongPtr(_handle).HasFlag(Win32.WS.WS_SIZEBOX);
            }
            set
            {
                var style = Win32.GetWindowStyleLongPtr(_handle);
                style = value ? style | Win32.WS.WS_SIZEBOX : style & ~Win32.WS.WS_SIZEBOX;
                Win32.SetWindowLongPtr(_handle, Win32.GWL_STYLE, (uint)style);
            }
        }

        public override string ToString()
        {
            return Title;
        }
    }
}