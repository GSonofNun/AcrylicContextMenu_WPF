using AcrylicContextMenu_WPF;
using System;
using System.Drawing.Printing;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;

namespace AcrylicContextMenu_WPF.AttachedProperties
{
    public class BlurPopup
    {
        public static readonly DependencyProperty OnProperty =
            DependencyProperty.RegisterAttached(
                "On",
                typeof(bool),
                typeof(BlurPopup),
                new PropertyMetadata(false, OnProperty_OnChanged)
                );

        public static bool GetOn(Popup obj) =>
            (bool)obj.GetValue(OnProperty);

        public static void SetOn(Popup obj, bool value) =>
            obj.SetValue(OnProperty, value);

        private static void OnProperty_OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is Popup popup))
                return;

            if ((bool)e.NewValue == true)
                popup.Opened += Popup_Opened;
            else
                popup.Opened -= Popup_Opened;
        }

        private static void Popup_Opened(object sender, EventArgs e)
        {
            if (!(sender is Popup contextMenu))
                return;

            HwndSource source = (HwndSource)HwndSource.FromVisual(contextMenu.Child);
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
