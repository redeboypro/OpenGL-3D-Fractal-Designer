using SimpleGameStudio.Rendering;
using SimpleGameStudio.Rendering.Common;

namespace Kwork_01
{
    public class Toolbox
    {
        private Camera Camera;
        private GeometryShader Shader;
        private Geometry Model;
        private Renderer Renderer;
        

        public Camera GetCamera() => Camera;
        public Renderer GetRenderer() => Renderer;

        public Toolbox()
        {
            Camera = new Camera();
            Shader = new GeometryShader();
            Model = Loader.Import("model.obj");
            Renderer = new Renderer(Model, Shader, Camera);
        }
    }
}