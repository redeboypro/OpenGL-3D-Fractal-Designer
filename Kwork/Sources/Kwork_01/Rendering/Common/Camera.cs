using System.Windows.Media.Imaging;
using OpenTK;

namespace Kwork_01.Rendering.Common
{
    public class Camera
    {
        public Vector3 Position = Vector3.UnitZ * 5;
        public Quaternion Orientation = Quaternion.Identity;
        private Matrix4 Projection;

        public Matrix4 GetProjection()
        {
            return Projection;
        }

        public Matrix4 GetTransformation()
        {
            return Matrix4.CreateTranslation(-Position) * Matrix4.CreateFromQuaternion(Orientation);
        }
        

        public Camera()
        {
            Orientation = Matrix4.LookAt(Position, Vector3.Zero, Vector3.UnitY).ExtractRotation();
            GenerateProjection();
        }
        
        /*public Vector3 Forward
        {
            get { return Orientation * -Vector3.UnitZ; }
        }
        
        public Vector3 Up
        {
            get { return Orientation * Vector3.UnitY; }
        }
        
        public Vector3 Right
        {
            get { return Orientation * Vector3.UnitX; }
        }*/

        public void GenerateProjection()
        {
            Projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)(800f / 600), 0.1f, 1000f);
        }
    }
}