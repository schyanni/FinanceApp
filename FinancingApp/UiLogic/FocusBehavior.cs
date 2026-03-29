using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FinancingApp.UiLogic;

public class FocusBehavior
{
    public static readonly DependencyProperty SelectTextOnFocusProperty =
        DependencyProperty.RegisterAttached(
            "SelectTextOnFocus",
            typeof(bool),
            typeof(FocusBehavior),
            new PropertyMetadata(false, OnSelectTextOnFocusChanged));

    [AttachedPropertyBrowsableForType(typeof(TextBox))]
    [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
    public static bool GetSelectTextOnFocus(DependencyObject target)
    {
        return (bool)target.GetValue(SelectTextOnFocusProperty);
    }

    public static void SetSelectTextOnFocus(DependencyObject target, bool value)
    {
        target.SetValue(SelectTextOnFocusProperty, value);
    }

    private static void OnSelectTextOnFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not TextBox textBox) return;

        if ((e.NewValue as bool?).GetValueOrDefault(false))
        {
            textBox.GotKeyboardFocus += OnGotKeyboardFocus;
            textBox.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
        }
        else
        {
            textBox.GotKeyboardFocus -= OnGotKeyboardFocus;
            textBox.PreviewMouseLeftButtonDown -= OnPreviewMouseLeftButtonDown;
        }
    }

    private static void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var textBox = MyVisualTreeHelper.GetParentOfType<TextBox>(e.OriginalSource as DependencyObject);
        if (textBox is null) return;

        if (!textBox.IsKeyboardFocusWithin)
        {
            textBox.Focus();
            e.Handled = true;
        }
    }

    private static void OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        if (e.OriginalSource is TextBox textBox) textBox.SelectAll();
    }
}