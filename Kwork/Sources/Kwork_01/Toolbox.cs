using Kwork_01.Rendering;
using Kwork_01.Rendering.Common;

namespace Kwork_01
{
    public class Toolbox
    {
        private Camera Camera;
        private GeometryShader Shader;
        private SkyboxShader RoomShader;
        private Geometry Model;
        private Renderer Renderer;
        private Renderer RoomRenderer;
        private Cubemap Skybox;

        public Camera GetCamera() => Camera;
        public Renderer GetRenderer() => Renderer;
        public Renderer GetRoomRenderer() => RoomRenderer;

        public Toolbox()
        {
            Skybox = new Cubemap(new []
            {
                "Cubemap\\left.png",
                "Cubemap\\bottom.png",
                "Cubemap\\back.png",
                "Cubemap\\right.png",
                "Cubemap\\top.png",
                "Cubemap\\front.png"
            });
            Camera = new Camera();
            
            Shader = new GeometryShader();
            RoomShader = new SkyboxShader();
            
            var Room = Loader.Import("Models\\model.obj");
            Model = Loader.Import("Models\\model2.obj");
            
            Renderer = new Renderer(Model, Shader, Skybox, Camera, false);
            RoomRenderer = new Renderer(Room, RoomShader, null, Camera, true);
        }
    }
}