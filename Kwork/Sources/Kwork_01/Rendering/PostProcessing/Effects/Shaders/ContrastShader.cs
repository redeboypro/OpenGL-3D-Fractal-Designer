namespace Kwork_01.Rendering.PostProcessing.Effects.Shaders
{
    public class ContrastShader : ShaderProgram
    {
        private static string Vertex = @"
#version 140

in vec2 position;
out vec2 textureCoords;

out vec2 pass_textureCoords;

void main(void){

	gl_Position = vec4(position, 0.0, 1.0);
	pass_textureCoords = textureCoords;
	
}";
        private static string Fragment = @"
#version 140

in vec2 pass_textureCoords;

out vec4 out_Colour;

uniform sampler2D colorTexture;

const float contrast = 0.05;

void main(void){

	out_Colour = texture(colourTexture, textureCoords);
	//out_Colour.rgb = (out_Colour.rgb -0.5) * (1.0+contrast) + 0.5;

}";
	
        public ContrastShader() : base(Vertex, Fragment) { }

        public override void BindAttributes()
        {
            BindAttribute(0, "position");
        }

    }
}