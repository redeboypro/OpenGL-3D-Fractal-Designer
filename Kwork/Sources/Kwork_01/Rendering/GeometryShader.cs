namespace SimpleGameStudio.Rendering
{
    public class GeometryShader : ShaderProgram
    {
        private static string Vertex = @"
#version 330

in vec3 position;
in vec2 textureCoordinates;

out vec3 pass_position;
out vec2 pass_textureCoordinates;

uniform mat4 transformationMatrix;
uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;

void main(void){
	gl_Position = projectionMatrix * viewMatrix * transformationMatrix * vec4(position, 1.0);
	pass_textureCoordinates = textureCoordinates;
    pass_position = position;
}
";

        private static string Fragment = @"
#version 330

in vec3 pass_position;
in vec2 pass_textureCoordinates;

out vec4 out_Color;

uniform float interpolation;
uniform float r_value;
uniform float g_value;
uniform float b_value;


vec2 complexMult(vec2 a, vec2 b) {
	return vec2(a.x*b.x - a.y*b.y, a.x*b.y + a.y*b.x);
}

float testMandelbrot(vec2 coord) {
    const int iterations = 912;
	vec2 testPoint = vec2(0,0);
	for (int i = 0; i < iterations; i++){
		testPoint = complexMult(testPoint,testPoint) + coord;
        float ndot = dot(testPoint,testPoint);
		if (ndot > 45678.0) {
            float sl = float(i) - log2(log2(ndot))+4.0;
			return sl/float(iterations);
		}
	}
	return 0.0;
}

vec4 mapColor(float mcol) {
    return vec4(0.5 + 0.5*cos(2.7+mcol*30.0 + vec3(r_value,g_value,b_value)),1.0);
}

void main(void){

    const vec2 zoomP = vec2(-.7457117,.186142);
    const float zoomTime = 100.0;
    float tTime = 9.0 + abs(mod(interpolation+zoomTime,zoomTime*2.0)-zoomTime);
    tTime = (145.5/(.0005*pow(tTime,5.0)));
    
    vec4 outs = vec4(0.0);
       
    outs += mapColor(testMandelbrot(zoomP + tTime * pass_textureCoordinates));
     
    out_Color = vec4(0.3);
    if(pass_position.y<-0.8){
       out_Color = outs/float(1);
    }
}
";

        public GeometryShader() : base(Vertex, Fragment)
        {
        }

        protected override void GetAllUniformLocations()
        {
        }

        public override void BindAttributes()
        {
            BindAttribute(0, "position");
            BindAttribute(1, "textureCoordinates");
        }
    }
}