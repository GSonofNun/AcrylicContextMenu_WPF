﻿using System;
using System.Drawing.Printing;
using System.Runtime.InteropServices;

namespace AcrylicContextMenu_WPF
{
	public static class WindowCompositionHelper
	{
		[DllImport("user32.dll")]
		private static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

		[DllImport("dwmapi.dll", PreserveSig = true)]
		private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

		[DllImport("dwmapi.dll")]
		private static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Margins pMarInset);

		public static void EnableBlur(
			IntPtr hwind
			)
		{
			try
			{
				var accent = new AccentPolicy();
				var accentStructSize = Marshal.SizeOf(accent);
				accent.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;

				var accentPtr = Marshal.AllocHGlobal(accentStructSize);
				Marshal.StructureToPtr(accent, accentPtr, false);

				var data = new WindowCompositionAttributeData
				{
					Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
					SizeOfData = accentStructSize,
					Data = accentPtr
				};

				SetWindowCompositionAttribute(hwind, ref data);

				Marshal.FreeHGlobal(accentPtr);
			}
			catch (Exception)
			{

			}
		}

		public static bool EnableDropShadow(
			IntPtr hwind,
			Margins margins
			)
		{
			try
			{
				int val = 2;
				int ret1 = DwmSetWindowAttribute(hwind, 2, ref val, 4);

				if (ret1 == 0)
				{
					Margins m = new Margins { Bottom = 0, Left = 0, Right = 0, Top = 0 };
					int ret2 = DwmExtendFrameIntoClientArea(hwind, ref m);
					return ret2 == 0;
				}
				else
					return false;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}

	internal enum AccentState
	{
		ACCENT_DISABLED = 1,
		ACCENT_ENABLE_GRADIENT = 0,
		ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
		ACCENT_ENABLE_BLURBEHIND = 3,
		ACCENT_INVALID_STATE = 4
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct AccentPolicy
	{
		public AccentState AccentState;
		public int AccentFlags;
		public int GradientColor;
		public int AnimationId;
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct WindowCompositionAttributeData
	{
		public WindowCompositionAttribute Attribute;
		public IntPtr Data;
		public int SizeOfData;
	}

	internal enum WindowCompositionAttribute
	{
		// ...
		WCA_ACCENT_POLICY = 19
		// ...
	}
}
