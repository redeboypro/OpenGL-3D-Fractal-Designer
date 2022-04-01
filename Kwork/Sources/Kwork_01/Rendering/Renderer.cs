using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Kwork_01.Rendering.Common;

namespace Kwork_01.Rendering
{
    public class Renderer
    {
        private Geometry Data;
        private ShaderProgram Shader;
        private Camera Camera;
        private Cubemap Sky;
        
        private Texture2D backgroundTexture = new Texture2D("Textures\\room.png");
        
        public static float Interpolation = 150f;

        public static float r = 255f;
        public static float g = 0f;
        public static float b = 0f;

        private float shiness = 25.0f;
        private float reflectivity = 0;

        private bool mode = false;

        public void ChangeMode()
        {
            mode = !mode;
        }
        

        public static float Rotation = 0.0f;


        public sliderEvent Interpolate = new sliderEvent(InterpolationEvent);
        private static void InterpolationEvent(float value)
        {
            Interpolation = value;
        }
        
        
        public sliderEvent PaintRed = new sliderEvent(RedColourEvent);
        private static void RedColourEvent(float value)
        {
            r = value;
        }
        
        
        public sliderEvent PaintGreen = new sliderEvent(GreenColourEvent);
        private static void GreenColourEvent(float value)
        {
            g = value;
        }
        
        
        public sliderEvent PaintBlue = new sliderEvent(BlueColourEvent);
        private static void BlueColourEvent(float value)
        {
            b = value;
        }

        public bool isBackground = false;

        public Renderer(Geometry data, ShaderProgram shader, Cubemap sky, Camera viewer, bool isBG)
        {
            Data = data;
            Shader = shader;
            Camera = viewer;
            Sky = sky;
            isBackground = isBG;
        }

        public void Draw()
        {
            GL.DepthFunc(DepthFunction.Lequal);
            Shader.Start();
            GL.BindVertexArray(Data.GetGeometryId());
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);
            
            GL.ActiveTexture(TextureUnit.Texture0);

            if(!isBackground)
            {
                GL.BindTexture(TextureTarget.TextureCubeMap, Sky.Id);
                Shader.SetUniform("mode", mode);
                
                Shader.SetUniform("projectionMatrix", Camera.GetProjection());
                Shader.SetUniform("transformationMatrix", Matrix4.CreateTranslation(Vector3.Zero)*
                                                          Matrix4.CreateRotationX(0) * Matrix4.CreateRotationY(Rotation) * Matrix4.CreateRotationZ(0)
                                                          * Matrix4.CreateScale(Vector3.One));
                Shader.SetUniform("viewMatrix",Camera.GetTransformation());
                
                Shader.SetUniform("cameraPosition",Camera.Position);
                Shader.SetUniform("SkyColour", Vector3.One);
                
                Shader.SetUniform("interpolation",Interpolation);
                Shader.SetUniform("r_value",r/255f);
                Shader.SetUniform("g_value",g/255f);
                Shader.SetUniform("b_value",b/255f);
                
                Shader.SetUniform("shiness", shiness);
                Shader.SetUniform("reflectivity", reflectivity);
                //Shader.SetUniform("modelTexture", backgroundTexture.GetTextureId());
            }
            else
            {
                GL.BindTexture(TextureTarget.Texture2D, backgroundTexture.GetTextureId());
                Shader.SetUniform("projectionMatrix", Camera.GetProjection());
                Shader.SetUniform("transformationMatrix", Matrix4.CreateTranslation(Vector3.Zero)*
                                                          Matrix4.CreateRotationX(0) * Matrix4.CreateRotationY(Rotation) * Matrix4.CreateRotationZ(0)
                                                          * Matrix4.CreateScale(Vector3.One));
                Shader.SetUniform("viewMatrix",Camera.GetTransformation());
            }
            
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            GL.DrawElements(BeginMode.Triangles, Data.GetVertexCount(), DrawElementsType.UnsignedInt, IntPtr.Zero);

            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.DisableVertexAttribArray(2);
            GL.BindVertexArray(0);
            
            
            Shader.Stop();
        }
    }
}