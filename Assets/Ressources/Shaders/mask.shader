shader_type canvas_item;
render_mode unshaded;

uniform float y;
uniform bool hasMaskTexture = true;
uniform sampler2D mask_texture;

void fragment()
{
	vec4 color = texture(TEXTURE, UV);
	if (hasMaskTexture){
		color = COLOR * texture(mask_texture, UV).a;
	}
	
	if (UV.y < y){
		color.a = 0.0;
	}
	COLOR = color;
}


