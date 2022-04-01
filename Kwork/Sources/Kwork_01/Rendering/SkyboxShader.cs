namespace Kwork_01.Rendering
{
    public class SkyboxShader : ShaderProgram
    {
        private static string Vertex = @"
#version 150

in vec3 position;
in vec2 textureCoordinates;

out vec2 pass_textureCoordinates;

uniform mat4 transformationMatrix;
uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;

void main(void){
	gl_Position = projectionMatrix * viewMatrix * transformationMatrix * vec4(position,1.0);
	pass_textureCoordinates = textureCoordinates;
}";
        
        private static string Fragment = @"
#version 150

in vec2 pass_textureCoordinates;

out vec4 out_Color;

uniform sampler2D modelTexture;

void main(void){
	out_Color = texture(modelTexture,pass_textureCoordinates);
}";
        
        public SkyboxShader() : base(Vertex, Fragment)
        {
        }

        public override void BindAttributes()
        {
            BindAttribute(0, "position");
            BindAttribute(1, "textureCoordinates");
            BindAttribute(2, "normal");
        }
    }
}