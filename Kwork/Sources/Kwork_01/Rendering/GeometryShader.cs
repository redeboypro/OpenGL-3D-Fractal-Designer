namespace Kwork_01.Rendering
{
    public class GeometryShader : ShaderProgram
    {
        private static string Vertex = @"
#version 330

in vec3 position;
in vec2 textureCoordinates;
in vec3 normal;

out vec3 pass_position;
out vec2 pass_textureCoordinates;
out vec3 pass_normal;

out vec3 lightVector;
out vec3 cameraVector;

out vec3 position_s;
out vec3 normal_s;

out float visibility;

uniform mat4 transformationMatrix;
uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;

const float density = 0.13;
const float gradient = 5;


void main(void){
    vec4 localPosition = transformationMatrix * vec4(position, 1.0);
    vec4 RelativeToCamera = viewMatrix * localPosition;
	gl_Position = projectionMatrix * RelativeToCamera;
	pass_textureCoordinates = textureCoordinates;
    pass_position = position;
    pass_normal = (transformationMatrix * vec4(normal, 0.0)).xyz;

    normal_s = mat3(transpose(inverse(transformationMatrix))) * normal;
    position_s = vec3(transformationMatrix * vec4(position, 1.0));

    float distance = length(RelativeToCamera.xyz);
    visibility = exp(-pow((distance * density), gradient));
    visibility = clamp(visibility, 0.0, 1.0);

    lightVector = vec3(20000,20000,2000) - localPosition.xyz;
    cameraVector = (inverse(viewMatrix) * vec4(0.0,0.0,0.0,1.0)).xyz - localPosition.xyz;
}
";

        private static string Fragment = @"
#version 330

in vec3 pass_position;
in vec2 pass_textureCoordinates;
in vec3 pass_normal;

in vec3 lightVector;
in vec3 cameraVector;

in vec3 position_s;
in vec3 normal_s;
in float visibility;

out vec4 out_Color;

uniform vec3 cameraPosition;
uniform vec3 SkyColour;

uniform float interpolation;

uniform float r_value;
uniform float g_value;
uniform float b_value;

uniform bool mode;

uniform float shiness;
uniform float reflectivity;

uniform sampler2D modelTexture;
uniform samplerCube skybox;

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

vec4 genRefs(){
    float ratio = 1.00 / 1.52;
    vec3 I = normalize(position_s - cameraPosition);
    vec3 Rl = refract(I, normalize(normal_s), ratio);
    vec3 Rr = reflect(I, normalize(normal_s));
    vec3 reflection = mix(Rl, Rr, 0.5);
    return vec4(texture(skybox, reflection).rgb, 0.5);
}

vec4 genFractalPattern(){
    const vec2 zoomP = vec2(-.7457117,.186142);
    const float zoomTime = 100.0;
    float tTime = 9.0 + abs(mod(interpolation+zoomTime,zoomTime*2.0)-zoomTime);
    tTime = (145.5/(.0005*pow(tTime,5.0)));
    return mapColor(testMandelbrot(zoomP + tTime * pass_textureCoordinates));
}

float genCircle(in vec2 _st, in float _radius) {
    vec2 l = _st-vec2(0.5);
    return 1.-smoothstep(_radius-(_radius*0.01),
                         _radius+(_radius*0.01),
                         dot(l,l)*4.0);
}

vec4 genCirclePattern(){
    vec2 uv = pass_textureCoordinates * interpolation / 5;
    uv = fract(uv);
    vec3 col = vec3(genCircle(uv,0.5));
    return vec4(col,1.0) * vec4(r_value, g_value, b_value, 1);
}

vec4 genCheckersPattern(){
    vec2 Pos = floor(pass_textureCoordinates * interpolation/5);
    float PatternMask = mod(Pos.x + mod(Pos.y, 2.0), 2.0);
    return PatternMask * vec4(r_value, g_value, b_value, 1.0);
}

void main(void){
    vec4 outs;

    if(mode!=false){
       outs = genCheckersPattern();
    }
    else{
       outs = genFractalPattern();
    }

    vec4 sky = genRefs();
     
    out_Color = mix(vec4(0.3), sky, 1);

    if(pass_position.y<1.5 && pass_position.y>0.3){

        if(mode!=false){
           outs = genCheckersPattern();
        }
        else{
           outs = genCirclePattern();
        }

        out_Color = mix(outs, sky, 0.8);
    }

    if(pass_position.y<0.3){
       out_Color = outs;
    }

    vec3 lightColour = vec3(1.0, 1.0, 1.0);

    vec3 unitNormal = normalize(pass_normal);
    vec3 unitLightVector = normalize(lightVector);

    float dotLightNormal = dot(unitNormal, unitLightVector);
    float brightness = max(dotLightNormal, 0.5);
    vec3 diffuse = brightness * lightColour;
  

    vec3 unitCameraVector = normalize(cameraVector);
    vec3 lightDirection = -unitLightVector;
    vec3 reflectedLightDir = reflect(lightDirection, unitNormal);

    float specularFactor = dot(reflectedLightDir, unitCameraVector);
    specularFactor = max(specularFactor, 0.0);
    float dampedFactor = pow(specularFactor, shiness);
    vec3 specular = dampedFactor * 1 * lightColour;

    if(pass_position.y>0.3){
      out_Color = vec4(diffuse, 1) * out_Color + vec4(specular,1);
    }

    out_Color = mix(vec4(SkyColour, 1.0), out_Color, visibility);
}
";

        public GeometryShader() : base(Vertex, Fragment)
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