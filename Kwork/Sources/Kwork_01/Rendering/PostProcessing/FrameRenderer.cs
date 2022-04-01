using OpenTK.Graphics.OpenGL;

namespace Kwork_01.Rendering.PostProcessing
{
    public class FrameRenderer
    {
        private static float[] VertexData =
        {
            -1, 1, -1, -1, 1, 1, 1, -1
        };

        private ShaderProgram Shader;
        
        public Fbo Fbo;

        private int vao, vbo;
        public FrameRenderer(Fbo fbo, ShaderProgram shader)
        {
            Fbo = fbo;
            Shader = shader;
            
            GL.GenVertexArrays(1, out vao);
            GL.GenBuffers(1, out vbo);
            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, VertexData.Length, VertexData, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
        }

        public void Init()
        {
            Shader.SetUniform("colorTexture", 0);
        }

        public void BindFBO()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, Fbo.GetFrameBufferId());
        }
        
        public void UnbindFBO()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void Draw()
        {
            GL.Disable(EnableCap.DepthTest);
            GL.BindTexture(TextureTarget.Texture2D, Fbo.GetColorTexture());
            GL.DrawArrays(BeginMode.Triangles, 0, 6);
            GL.Enable(EnableCap.DepthTest);
        }
    }
}