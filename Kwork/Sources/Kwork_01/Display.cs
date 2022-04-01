using System;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Kwork_01.Rendering;
using Kwork_01.Rendering.PostProcessing;
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
        private GraphicsUserControl guiControl;

        private float mouseInit = 0.0f;

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
            guiControl = new GraphicsUserControl(canvas);
            
            Content = grid;
            
            grid.Children.Add(glControl);
            grid.Children.Add(canvas);

            glControl.Start(glOptions);

            GL.Enable(EnableCap.TextureCubeMap);
            GL.Enable(EnableCap.Texture2D);

            toolbox = new Toolbox();
            GL.Enable(EnableCap.DepthTest);
            

            /*
             * Interpolation Slider
             */
            guiControl.Slider(5,20, toolbox.GetRenderer().Interpolate, 15, 15);
            
            
            /*
             * Red Color
             */
            guiControl.Slider(0, 255, toolbox.GetRenderer().PaintRed, 1, 255);
            
            
            /*
             * Green Color
             */
            guiControl.Slider(0, 255, toolbox.GetRenderer().PaintGreen);
            
            
            /*
             * Blue Color
             */
            guiControl.Slider(0, 255, toolbox.GetRenderer().PaintBlue);
            
            guiControl.Button(toolbox.GetRenderer().ChangeMode, "Change Mode");

            MouseMove += displayControl_OnDrag;

            glControl.Render += glControl_OnRender;
            ShowDialog();
        }

        public void displayControl_OnDrag(object s, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                var delta = (float)GetNormalizedCursor().X - mouseInit;
                Renderer.Rotation += delta;
            }
            mouseInit = (float)GetNormalizedCursor().X;
        }

        private void glControl_OnRender(TimeSpan delta) {
            GL.ClearColor(Color4.White);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            toolbox.GetRenderer().Draw();
            toolbox.GetRoomRenderer().Draw();

        }
        
        public Point GetNormalizedCursor()
        {
            var origin = GetCursorOrigin();
            var x = (2.0f * origin.X) / 800f - 1;
            var y = (2.0f * origin.Y) / 600f - 1;
            return new Point(x,y);
        }
        
        public Point GetCursorOrigin()
        {
            return Mouse.GetPosition(this);
        }
    }
}