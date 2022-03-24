using System.Collections.Generic;
using System.Globalization;
using System.IO;
using OpenTK;

namespace SimpleGameStudio.Rendering.Common
{
    public class Vertex
    {
        private static int NO_INDEX = -1;
        public Vector3 position;
        public int uv_index = NO_INDEX;
        public int vertex_index;
        public float lenght;
        private Vertex duplicateVertex = null;

        public Vertex(int index, Vector3 position)
        {
            this.vertex_index = index;
            this.position = position;
        }

        public bool isSet()
        {
            return uv_index != NO_INDEX;
        }

        public Vertex getDuplicateVertex()
        {
            return duplicateVertex;
        }

        public void setDuplicateVertex(Vertex duplicateVertex)
        {
            this.duplicateVertex = duplicateVertex;
        }

        public bool hasSameUV(int textureIndexOther)
        {
            return textureIndexOther == uv_index;
        }
    }
    
    public class Loader
    {
        public static Geometry Import(string file)
        {
            List<Vertex> vertices = new List<Vertex>();
            List<Vector2> uv = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            List<int> indices = new List<int>();

            CultureInfo ci = (CultureInfo) CultureInfo.CurrentCulture.Clone();
            ci.NumberFormat.CurrencyDecimalSeparator = ".";

            StreamReader streamReader = new StreamReader(file);
            if (streamReader != null)
            {
                while (!streamReader.EndOfStream)
                {
                    string line = streamReader.ReadLine();
                    if (line.StartsWith("v "))
                    {
                        string[] split = line.Split(' ');
                        float x = float.Parse(split[1], NumberStyles.Any, ci);
                        float y = float.Parse(split[2], NumberStyles.Any, ci);
                        float z = float.Parse(split[3], NumberStyles.Any, ci);
                        vertices.Add(new Vertex(vertices.Count, new Vector3((float) x, y, z)));
                    }

                    if (line.StartsWith("vt "))
                    {
                        string[] split = line.Split(' ');
                        uv.Add(new Vector2(float.Parse(split[1], NumberStyles.Any, ci),
                            float.Parse(split[2], NumberStyles.Any, ci)));
                    }

                    if (line.StartsWith("vn "))
                    {
                        string[] split = line.Split(' ');
                        normals.Add(new Vector3(float.Parse(split[1], NumberStyles.Any, ci),
                            float.Parse(split[2], NumberStyles.Any, ci),
                            float.Parse(split[3], NumberStyles.Any, ci)));
                    }


                    if (line.StartsWith("f "))
                    {
                        string[] currentLine = line.Split(' ');
                        string[] vertex1 = currentLine[1].Split('/');
                        string[] vertex2 = currentLine[2].Split('/');
                        string[] vertex3 = currentLine[3].Split('/');
                        setup(vertex1, vertices, indices);
                        setup(vertex2, vertices, indices);
                        setup(vertex3, vertices, indices);
                    }
                }
            }

            streamReader.Close();

            var TextureCoords = new Vector2[vertices.Count];
            var Vertices = new Vector3[vertices.Count];

            storeData(vertices, uv, Vertices, TextureCoords);

            var Indices = indices.ToArray();
            return new Geometry(Vertices, TextureCoords, Indices);
        }

        public static void setup(string[] vertexData, List<Vertex> vertices, List<int> indices)
        {
            int index = int.Parse(vertexData[0]) - 1;
            Vertex currentVertex = vertices[index];
            int textureIndex = int.Parse(vertexData[1]) - 1;
            if (!currentVertex.isSet())
            {
                currentVertex.uv_index = textureIndex;
                indices.Add(index);
            }
            else
            {
                initVertex(currentVertex, textureIndex, indices,
                    vertices);
            }
        }

        private static void initVertex(Vertex previousVertex, int newTextureIndex, List<int> indices,
            List<Vertex> vertices)
        {
            if (previousVertex.hasSameUV(newTextureIndex))
            {
                indices.Add(previousVertex.vertex_index);
            }
            else
            {
                Vertex anotherVertex = previousVertex.getDuplicateVertex();
                if (anotherVertex != null)
                {
                    initVertex(anotherVertex, newTextureIndex,
                        indices, vertices);
                }
                else
                {
                    Vertex duplicateVertex = new Vertex(vertices.Count, previousVertex.position);
                    duplicateVertex.uv_index = newTextureIndex;
                    previousVertex.setDuplicateVertex(duplicateVertex);
                    vertices.Add(duplicateVertex);
                    indices.Add(duplicateVertex.vertex_index);
                }
            }
        }

        private static void storeData(List<Vertex> vertices, List<Vector2> textures,
            Vector3[] verticesArray, Vector2[] texturesArray)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                Vertex currentVertex = vertices[i];
                Vector3 position = currentVertex.position;
                Vector2 textureCoord = textures[currentVertex.uv_index];
                verticesArray[i] = position;
                texturesArray[i] = textureCoord;
            }
        }
    }
}