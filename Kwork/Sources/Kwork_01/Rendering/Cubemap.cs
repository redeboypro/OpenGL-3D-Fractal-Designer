using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace Kwork_01.Rendering
{
    public class Cubemap
    {
        public int Id;

        private TextureTarget[] Targets =
        {
            TextureTarget.TextureCubeMapNegativeX,
            TextureTarget.TextureCubeMapNegativeY,
            TextureTarget.TextureCubeMapNegativeZ,
            TextureTarget.TextureCubeMapPositiveX,
            TextureTarget.TextureCubeMapPositiveY,
            TextureTarget.TextureCubeMapPositiveZ
        };

        public Cubemap(string[] files)
        {
            GL.Enable(EnableCap.TextureCubeMap);
            Id = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.TextureCubeMap, Id);
            
            for (var i = 0; i < files.Length; i++)
            {
                using (var image = new Bitmap(files[i]))
                {
                    var data = image.LockBits(
                        new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly,
                        System.Drawing.Imaging.PixelFormat.Format32bppRgb);


                    GL.TexImage2D(Targets[i], 0, PixelInternalFormat.Luminance6Alpha2,
                        image.Width, image.Height, 0, PixelFormat.Rg,
                        PixelType.UnsignedByte, data.Scan0);
                }
            }
            
            GL.TexParameter(TextureTarget.TextureCubeMap, 
                TextureParameterName.TextureMagFilter,
                (int)TextureMagFilter.Linear);
            
            GL.TexParameter(TextureTarget.TextureCubeMap,
                TextureParameterName.TextureMinFilter,
                (int)TextureMinFilter.Linear);
            
            GL.TexParameter(TextureTarget.TextureCubeMap,
                TextureParameterName.TextureWrapS,
                (int)TextureParameterName.ClampToEdge);
            
            GL.TexParameter(TextureTarget.TextureCubeMap,
                TextureParameterName.TextureWrapT,
                (int)TextureParameterName.ClampToEdge);
            
            GL.TexParameter(TextureTarget.TextureCubeMap,
                TextureParameterName.TextureWrapR,
                (int)TextureParameterName.ClampToEdge);
        }
    }
}