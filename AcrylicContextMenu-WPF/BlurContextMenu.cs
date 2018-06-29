using System;
using System.Drawing.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace AcrylicContextMenu_WPF.AttachedProperties
{
	public class BlurContextMenu
	{
		public static readonly DependencyProperty OnProperty =
			DependencyProperty.RegisterAttached(
				"On",
				typeof(bool),
				typeof(BlurContextMenu),
				new PropertyMetadata(false, OnProperty_OnChanged)
				);

		public static bool GetOn(ContextMenu obj) =>
			(bool)obj.GetValue(OnProperty);

		public static void SetOn(ContextMenu obj, bool value) =>
			obj.SetValue(OnProperty, value);

		private static void OnProperty_OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (!(d is ContextMenu contextMenu))
				return;

			if ((bool)e.NewValue == true)
				contextMenu.Opened += Popup_Opened;
			else
				contextMenu.Opened -= Popup_Opened;
		}

		private static void Popup_Opened(object sender, EventArgs e)
		{
			if (!(sender is ContextMenu contextMenu))
				return;

			HwndSource source = (HwndSource)PresentationSource.FromVisual(contextMenu);
			if (source == null)
				return;

			WindowCompositionHelper.EnableBlur(
				source.Handle
				);

			WindowCompositionHelper.EnableDropShadow(
				source.Handle,
				new Margins(0, 0, 0, 0)
				);
		}
	}
}
