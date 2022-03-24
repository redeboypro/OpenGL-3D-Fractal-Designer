using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SimpleGameStudio.Rendering.Common;

namespace SimpleGameStudio.Rendering
{
    public class Renderer
    {
        private Geometry Data;
        private GeometryShader Shader;
        private Camera Camera;
        
        public float Interpolation = 15f;
        
        public float r = 0.6f, g = 0.9f, b = 0.9f;

        public float Rotation = 0.0f;

        public Renderer(Geometry data, GeometryShader shader, Camera viewer)
        {
            Data = data;
            Shader = shader;
            Camera = viewer;
        }

        public void Draw()
        {
            Shader.Start();
            GL.BindVertexArray(Data.GetGeometryId());
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            {
                Shader.SetUniform("projectionMatrix", Camera.GetProjection());
                Shader.SetUniform("transformationMatrix", Matrix4.CreateTranslation(Vector3.Zero)*
                                                          Matrix4.CreateRotationX(0) * Matrix4.CreateRotationY(Rotation) * Matrix4.CreateRotationZ(0)
                                                          * Matrix4.CreateScale(Vector3.One));
                Shader.SetUniform("viewMatrix",Camera.GetTransformation());
                
                Shader.SetUniform("interpolation",Interpolation);
                Shader.SetUniform("r_value",r);
                Shader.SetUniform("g_value",g);
                Shader.SetUniform("b_value",b);
            }
            
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            GL.DrawElements(BeginMode.Triangles, Data.GetVertexCount(), DrawElementsType.UnsignedInt, IntPtr.Zero);

            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.BindVertexArray(0);
            
            
            Shader.Stop();
        }
    }
}