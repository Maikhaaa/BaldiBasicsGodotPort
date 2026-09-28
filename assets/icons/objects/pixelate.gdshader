shader_type canvas_item;

uniform float pixel_size : hint_range(1.0, 32.0, 1.0) = 4.0;

void fragment() {
    vec2 text_size = vec2(textureSize(TEXTURE, 0));
    vec2 grid_uv = floor(UV * text_size / pixel_size) * pixel_size / text_size;
    COLOR = texture(TEXTURE, grid_uv);
}