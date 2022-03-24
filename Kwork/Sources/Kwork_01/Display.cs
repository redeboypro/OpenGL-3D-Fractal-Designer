using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Wpf;

namespace Kwork_01
{
    public class Display : Window
    {
        private GLWpfControl glControl;
        private GLWpfControlSettings glOptions;
        private Toolbox toolbox;

        public Display()
        {
            Width = 800;
            Height = 600;
            ResizeMode = ResizeMode.NoResize;

            glControl = new GLWpfControl();
            glOptions = new GLWpfControlSettings
            {
                MajorVersion = 3,
                MinorVersion = 6
            };
            

            var canvas = new Canvas();
            var grid = new Grid();
            
            Content = grid;
            
            grid.Children.Add(glControl);
            grid.Children.Add(canvas);

            glControl.Start(glOptions);

            toolbox = new Toolbox();
            GL.Enable(EnableCap.DepthTest);

            
            /*
             * Rotation Slider
             */
            var rotation_slider = new Slider()
            {
                Minimum = 0, Maximum = 30, Width = 150
            };
            rotation_slider.ValueChanged += (object s, RoutedPropertyChangedEventArgs<double> e) =>
            {
                toolbox.GetRenderer().Rotation = (float)e.NewValue/10;
            };
            canvas.Children.Add(rotation_slider);
            
            
            /*
             * Interpolation Slider
             */
            var interpolation_slider = new Slider()
            {
                Minimum = 10, Maximum = 100, Width = 150, Margin = new Thickness(0,20,0,0)
            };
            interpolation_slider.ValueChanged += (object s, RoutedPropertyChangedEventArgs<double> e) =>
            {
                toolbox.GetRenderer().Interpolation = (float)e.NewValue;
            };
            canvas.Children.Add(interpolation_slider);
            
            
            /*
             * Red Color
             */
            var r_slider = new Slider()
            {
                Minimum = 10, Maximum = 100, Width = 150, Margin = new Thickness(0,40,0,0)
            };
            r_slider.ValueChanged += (object s, RoutedPropertyChangedEventArgs<double> e) =>
            {
                toolbox.GetRenderer().r = (float)e.NewValue/100f;
            };
            canvas.Children.Add(r_slider);
            
            
            /*
             * Red Color
             */
            var g_slider = new Slider()
            {
                Minimum = 10, Maximum = 100, Width = 150, Margin = new Thickness(0,60,0,0)
            };
            g_slider.ValueChanged += (object s, RoutedPropertyChangedEventArgs<double> e) =>
            {
                toolbox.GetRenderer().g = (float)e.NewValue/100f;
            };
            canvas.Children.Add(g_slider);
            
            
            /*
             * Red Color
             */
            var b_slider = new Slider()
            {
                Minimum = 10, Maximum = 100, Width = 150, Margin = new Thickness(0,80,0,0)
            };
            b_slider.ValueChanged += (object s, RoutedPropertyChangedEventArgs<double> e) =>
            {
                toolbox.GetRenderer().b = (float)e.NewValue/100f;
            };
            canvas.Children.Add(b_slider);


            glControl.Render += glControl_OnRender;
            ShowDialog();
        }
        
        /*private void glControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (Key.Up == e.Key)
            {
                toolbox.GetRenderer().Interpolation += 0.1f;
            }
        }*/

        private void glControl_OnRender(TimeSpan delta) {
            GL.ClearColor(Color4.White);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            toolbox.GetRenderer().Draw();
        }
    }
}