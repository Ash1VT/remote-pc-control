using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Client.Hooks {
	
	public class KeyboardHook {
		public delegate int KeyboardHookProc(int code, int wParam, ref KeyboardHookStruct lParam);

		public struct KeyboardHookStruct {
			public int vkCode;
			public int scanCode;
			public int flags;
			public int time;
			public int dwExtraInfo;
		}

		const int WH_KEYBOARD_LL = 13;
		const int WM_KEYDOWN = 0x100;
		const int WM_KEYUP = 0x101;
		const int WM_SYSKEYDOWN = 0x104;
		const int WM_SYSKEYUP = 0x105;

		private IntPtr _hhook = IntPtr.Zero;
		private static KeyboardHookProc _callback;
		public event KeyEventHandler KeyDown;
		public event KeyEventHandler KeyUp;

		public KeyboardHook() {
			Hook();
		}

		~KeyboardHook() {
			Unhook();
		}
		public void Hook() {
			try
			{
				_callback = new KeyboardHookProc(HookProc);
				// IntPtr hInstance = LoadLibrary("User32");
				_hhook = SetWindowsHookEx(WH_KEYBOARD_LL, _callback, IntPtr.Zero, 0);
			}
			catch
			{
				Console.WriteLine("error");
			}
		}

		public void Unhook() {
			UnhookWindowsHookEx(_hhook);
		}

		public int HookProc(int code, int wParam, ref KeyboardHookStruct lParam) {
				if (code >= 0)
				{
					Keys key = (Keys)lParam.vkCode;
					KeyEventArgs kea = new KeyEventArgs(key);
					if ((wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN) && (KeyDown != null))
					{
						KeyDown?.Invoke(this, kea);
					}
					else if ((wParam == WM_KEYUP || wParam == WM_SYSKEYUP) && (KeyUp != null))
					{
						KeyUp?.Invoke(this, kea);
					}
			
					if (kea.Handled)
						return 1;
				}
			return 1;
		}
		[DllImport("user32.dll")]
		static extern IntPtr SetWindowsHookEx(int idHook, KeyboardHookProc callback, IntPtr hInstance, uint threadId);

		[DllImport("user32.dll")]
		static extern bool UnhookWindowsHookEx(IntPtr hInstance);

	}
}
