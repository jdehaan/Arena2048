using System;
using System.Windows.Media;

namespace Wpf2048.ViewModels
{
    public class TileAppearance
    {
        public TileAppearance(String text, String backcolor, String textcolor)
        {
            Text = text;
            BackColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(backcolor));
            TextColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(textcolor));
            FontSize = 30;
        }

        public Brush TextColor { get; set; }
        public Brush BackColor { get; set; }
        public String Text { get; set; }
        public int FontSize { get; set; }
    }
}
