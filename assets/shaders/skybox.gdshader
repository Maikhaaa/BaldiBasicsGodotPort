shader_type sky;

// Drag your 6 separate square textures into these slots in the Inspector!
uniform sampler2D right_tex : source_color, filter_nearest;
uniform sampler2D left_tex : source_color, filter_nearest;
uniform sampler2D top_tex : source_color, filter_nearest;
uniform sampler2D bottom_tex : source_color, filter_nearest;
uniform sampler2D back_tex : source_color, filter_nearest;
uniform sampler2D front_tex : source_color, filter_nearest;

uniform bool flip_x = false;
uniform bool flip_y = true; // Set to true by default as Y is usually inverted in 3D sky projection

// Y-axis rotation angle in degrees (adjust in Inspector or script)
uniform float sky_rotation_y : hint_range(0.0, 360.0) = 0.0;

// Function to rotate a 3D vector around the Y-axis
vec3 rotate_y(vec3 v, float angle_deg) {
    float rad = radians(angle_deg);
    float cos_a = cos(rad);
    float sin_a = sin(rad);
    return vec3(
        v.x * cos_a - v.z * sin_a,
        v.y,
        v.x * sin_a + v.z * cos_a
    );
}

// Helper function to handle flipping logic
vec2 apply_flips(vec2 in_uv) {
    vec2 res = in_uv;
    if (flip_x) {
        res.x = 1.0 - res.x;
    }
    if (flip_y) {
        res.y = 1.0 - res.y;
    }
    return res;
}

void sky() {
    // 1. Rotate the camera view direction around the Y axis
    vec3 dir = rotate_y(EYEDIR, sky_rotation_y);
    vec3 abs_dir = abs(dir);
    
    vec2 uv = vec2(0.0);
    vec3 col = vec3(0.0);

    if (abs_dir.x >= abs_dir.y && abs_dir.x >= abs_dir.z) {
        if (dir.x > 0.0) {
            uv = vec2(-dir.z, dir.y) / abs_dir.x * 0.5 + 0.5;
            col = texture(right_tex, apply_flips(uv)).rgb;
        } else {
            uv = vec2(dir.z, dir.y) / abs_dir.x * 0.5 + 0.5;
            col = texture(left_tex, apply_flips(uv)).rgb;
        }
    } else if (abs_dir.y >= abs_dir.x && abs_dir.y >= abs_dir.z) {
        if (dir.y > 0.0) {
            uv = vec2(dir.x, -dir.z) / abs_dir.y * 0.5 + 0.5;
            col = texture(top_tex, apply_flips(uv)).rgb;
        } else {
            uv = vec2(dir.x, dir.z) / abs_dir.y * 0.5 + 0.5;
            col = texture(bottom_tex, apply_flips(uv)).rgb;
        }
    } else {
        if (dir.z > 0.0) {
            uv = vec2(dir.x, dir.y) / abs_dir.z * 0.5 + 0.5;
            col = texture(back_tex, apply_flips(uv)).rgb;
        } else {
            uv = vec2(-dir.x, dir.y) / abs_dir.z * 0.5 + 0.5;
            col = texture(front_tex, apply_flips(uv)).rgb;
        }
    }

    COLOR = col;
}