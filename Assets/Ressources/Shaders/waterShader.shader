shader_type canvas_item;
//render_mode unshaded;
// Gonkee's water shader for Godot 3
// If you use this shader, I would prefer if you gave credit to me and my channel
uniform vec4 blue_tint : hint_color;
//uniform vec4 blue_tint : hint_color;

void fragment(){
	
	/*vec4 color = textureLod(SCREEN_TEXTURE, SCREEN_UV, 0.0);
	
	color = mix(color, blue_tint, 0.8);
	color.rgb = mix(vec3(0.5), color.rgb, 1.4);

	vec4 curr_color = texture(TEXTURE,UV);
	if (curr_color.a==0.0){
		COLOR = curr_color;
	}else{
		COLOR = color;
	}*/
}