using System;
using System.ComponentModel.Design;
using OpenTK.Graphics.OpenGL;

namespace Kwork_01.Rendering.PostProcessing
{
    public class Fbo
    {
        private int Id;
        
        private int RenderBuffer;
        
        private int ColorTexture;

        public int GetColorTexture() => ColorTexture;
        public int GetFrameBufferId() => Id;

        public Fbo()
        {
            GL.GenFramebuffers(1, out Id);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, Id);
            
            GL.GenTextures(1, out ColorTexture);
            GL.BindTexture(TextureTarget.Texture2D, ColorTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, 800, 600, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.ClampToEdge);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, ColorTexture, 0);
            
            GL.GenRenderbuffers(1,out RenderBuffer);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, RenderBuffer);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, 800, 600);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, RenderBuffer);
            
        }
    }
}