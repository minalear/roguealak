#version 330 core
in vec2 position;
in vec2 texcoords;
in vec3 foreColor;
in vec3 backColor;

out vec2 Texcoords;
out vec3 ForeColor;
out vec3 BackColor;

uniform mat4 model;
uniform mat4 proj;

void main()
{
	Texcoords = texcoords;
	ForeColor = foreColor;
	BackColor = backColor;

	gl_Position = proj * model * vec4(position, 0.0, 1.0);
}