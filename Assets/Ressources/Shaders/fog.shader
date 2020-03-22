shader_type canvas_item;
render_mode unshaded;

uniform vec3 color = vec3(0.33, 0.15, 0.82);
uniform int OCTAVES = 4;
uniform float size = 20;
uniform float mult = 1.0;

uniform sampler2D mask_texture;

float rand(vec2 coord){
	return fract(sin(dot(coord, vec2(56,78)) * 1000.0) * 1000.0);
}

float noise(vec2 coord){
	vec2 i = floor(coord);
	vec2 f = fract(coord);
	
	float a = rand(i);
	float b = rand(i + vec2(1.0,0.0));
	float c = rand(i+ vec2(0.0,1.0));
	float d = rand(i+ vec2(1.0,1.0)); 
	
	vec2 cubic = f * f * (3.0 - 2.0 * f);

	return mix(a, b, cubic.x) + (c - a) * cubic.y * (1.0 - cubic.x) + (d - b) * cubic.x * cubic.y;
}
 
float fbm(vec2 coord){
	float value = 0.0;
	float scale = 0.5;
	
	for (int i = 0; i<OCTAVES; i++){
		value+= noise(coord) * scale;
		coord *= 2.0;
		scale *= 0.5;
	}
	return value;
}

void fragment()
{
	vec2 coord = UV * size;
	
	//vec2 motion = vec2(fbm(coord + TIME * 0.5 * fract(sin(0.005*TIME))));
	//vec2 motion = vec2(fbm(coord + vec2(TIME*3.0*fract(sin(TIME*0.0013)), TIME*0.5*fract(cos(TIME*0.0013)))));
	vec2 motion = vec2(fbm(coord + vec2(TIME*0.5, TIME*0.5)));
	
	
	float final = fbm(coord + motion);
	
	vec4 color1 = vec4(color, final * mult );
	
	color1 = color1 * texture(mask_texture, UV).a;
	
	COLOR = color1;
	
}