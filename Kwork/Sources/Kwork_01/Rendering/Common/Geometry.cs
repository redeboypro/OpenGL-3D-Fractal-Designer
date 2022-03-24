using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SimpleGameStudio.Rendering.Common
{
    public class Geometry
    {
        private Vbo<Vector3> Vertices;
        private Vbo<Vector2> TexCoords;
        private Vbo<int> Indices;
        
        private int VertexCount, DataId;
        
        public int GetGeometryId()
        {
            return DataId;
        }

        public int GetVertexId()
        {
            return Vertices.Id;
        }
        
        public int GetTexCoordId()
        {
            return TexCoords.Id;
        }
        
        public int GetIndexId()
        {
            return Indices.Id;
        }
        
        public int GetVertexCount()
        {
            return VertexCount;
        }
        
        public Geometry(Vector3[] verts, Vector2[] textures, int[] tris)
        {
            CreateDataArrayObject();
            Indices = new Vbo<int>(2,1, tris, BufferTarget.ElementArrayBuffer);
            Vertices = new Vbo<Vector3>(0, 3, verts, BufferTarget.ArrayBuffer);
            TexCoords = new Vbo<Vector2>(1, 2, textures, BufferTarget.ArrayBuffer);
            GL.BindVertexArray(0);
            VertexCount = tris.Length;
        }

        private void CreateDataArrayObject()
        {
            GL.GenVertexArrays(1,out DataId);
            GL.BindVertexArray(DataId);
        }
    }
}