using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LetsTalkWP8.Common
{
    // Due to missing DependencyObject for the SelectedItem, this helper is needed
    // Source: http://dotnet-redzone.blogspot.de/2012/11/windows-phone-8longlistselector.html

    public class LongListSelector : Microsoft.Phone.Controls.LongListSelector
    {
        public LongListSelector()
        {
            SelectionChanged += LongListSelector_SelectionChanged;
        }

        void LongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItem = base.SelectedItem;
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(
                "SelectedItem",
                typeof(object),
                typeof(LongListSelector),
                new PropertyMetadata(null, OnSelectedItemChanged)
            );

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var selector = (LongListSelector)d;
            selector.SelectedItem = e.NewValue;
        }

        public new object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
    }
}
