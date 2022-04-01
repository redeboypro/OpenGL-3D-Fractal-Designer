using System;
using System.Windows;
using System.Windows.Controls;
using Kwork_01.Rendering.PostProcessing;

namespace Kwork_01
{
    public delegate void sliderEvent(float val);
    
    public class GraphicsUserControl
    {
        private Canvas Canvas;
        private int elementsCount = 0;
        private static int height = 25;
        private static int space = 5;


        public GraphicsUserControl(Canvas can)
        {
            Canvas = can;
        }
        
        private const float defaultValue = -3333;
        public void Slider(float min, float max, sliderEvent action, float multiplier = 1, float value = defaultValue)
        {
            var slider = new Slider()
            {
                Minimum = min, Maximum = max, Width = 150, Height = height,
                Margin = new Thickness(0, height * elementsCount + space * elementsCount, 0, 0)
            };
            
            slider.Value = value.Equals(defaultValue) ? min : value;

            slider.ValueChanged += (object s, RoutedPropertyChangedEventArgs<double> e) =>
            {
                action((float)e.NewValue * multiplier);
            };
            
            Canvas.Children.Add(slider);
            elementsCount++;
        }
        
        

        public void Button(Action action, string label = "Button")
        {
            var button = new Button()
            {
                Width = 150, Height = height, Content = label,
                Margin = new Thickness(0, height * elementsCount + space * elementsCount, 0, 0)
            };
            
            button.Click += (object s, RoutedEventArgs e) =>
            {
                action.Invoke();
            };
            
            Canvas.Children.Add(button);
            elementsCount++;
        }
    }
}