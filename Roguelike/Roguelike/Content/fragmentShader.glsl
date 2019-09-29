#version 330 core
in vec2 Texcoords;
in vec3 ForeColor;
in vec3 BackColor;

out vec4 outColor;

uniform sampler2D font;

void main()
{
	vec4 texColor = texture(font, Texcoords);

	if (texColor.a == 0)
		outColor = vec4(BackColor, 1.0);
	else
		outColor = vec4(ForeColor, 1.0) * texColor;
}