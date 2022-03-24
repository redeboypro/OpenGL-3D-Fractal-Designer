using System;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;

namespace SimpleGameStudio.Rendering.Common
{
    public class Vbo<T> where T : struct
    {
        private int id;

        public int Id
        {
            get => id;
        }

        public Vbo(int num, int size, T[] data, BufferTarget Target)
        {
            GL.GenBuffers(1, out id);
            GL.BindBuffer(Target, id);
            GL.BufferData(Target, (IntPtr) (data.Length * Marshal.SizeOf<T>()), data, BufferUsageHint.DynamicDraw);

            if (Target != BufferTarget.ElementArrayBuffer)
            {
                GL.VertexAttribPointer(num, size, VertexAttribPointerType.Float, false, 0, 0);
                GL.BindBuffer(Target, 0);
            }
        }
    }
}