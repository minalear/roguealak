using System;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Roguelike.Engine
{
    public sealed class Shader
    {
        private int shaderProgram;

        public Shader(string vertexSource, string fragmentSource)
        {
            //Vertex Shader
            int vertID = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertID, vertexSource);
            GL.CompileShader(vertID);
            this.checkCompilationErrors(vertID, ShaderType.VertexShader);

            //Fragment Shader
            int fragID = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragID, fragmentSource);
            GL.CompileShader(fragID);
            this.checkCompilationErrors(fragID, ShaderType.FragmentShader);

            //Create Shader Program
            this.shaderProgram = GL.CreateProgram();
            GL.AttachShader(this.shaderProgram, vertID); //Attach vertex shader
            GL.AttachShader(this.shaderProgram, fragID); //Attach fragment shader

            GL.LinkProgram(this.shaderProgram);

            //Delete our shaders as they are linked an are no longer required
            GL.DeleteShader(vertID);
            GL.DeleteShader(fragID);
        }

        public void Use()
        {
            GL.UseProgram(this.shaderProgram);
        }
        public void Use(bool useShader)
        {
            if (useShader)
                GL.UseProgram(this.shaderProgram);
        }

        public void SetFloat(string name, float value, bool useShader = false)
        {
            this.Use(useShader);
            GL.Uniform1(GL.GetUniformLocation(this.shaderProgram, name), value);
        }
        public void SetDouble(string name, double value, bool useShader = false)
        {
            this.Use(useShader);
            GL.Uniform1(GL.GetUniformLocation(this.shaderProgram, name), value);
        }
        public void SetInteger(string name, int value, bool useShader = false)
        {
            this.Use(useShader);
            GL.Uniform1(GL.GetUniformLocation(this.shaderProgram, name), value);
        }

        public void SetVector2(string name, Vector2 value, bool useShader = false)
        {
            this.Use(useShader);
            GL.Uniform2(GL.GetUniformLocation(this.shaderProgram, name), value);
        }
        public void SetVector2(string name, float x, float y, bool useShader = false)
        {
            this.Use(useShader);
            GL.Uniform2(GL.GetUniformLocation(this.shaderProgram, name), x, y);
        }

        public void SetVector3(string name, Vector3 value, bool useShader = false)
        {
            this.Use(useShader);
            GL.Uniform3(GL.GetUniformLocation(this.shaderProgram, name), value);
        }
        public void SetVector3(string name, float x, float y, float z, bool useShader = false)
        {
            this.Use(useShader);
            GL.Uniform3(GL.GetUniformLocation(this.shaderProgram, name), x, y, z);
        }

        public void SetVector4(string name, Vector4 value, bool useShader = false)
        {
            this.Use(useShader);
            GL.Uniform4(GL.GetUniformLocation(this.shaderProgram, name), value);
        }
        public void SetVector4(string name, float x, float y, float z, float w, bool useShader = false)
        {
            this.Use(useShader);
            GL.Uniform4(GL.GetUniformLocation(this.shaderProgram, name), x, y, z, w);
        }

        public void SetMatrix4(string name, Matrix4 matrix, bool useShader = false)
        {
            this.Use(useShader);
            GL.UniformMatrix4(GL.GetUniformLocation(this.shaderProgram, name), false, ref matrix);
        }

        private void checkCompilationErrors(int shaderID, ShaderType type)
        {
            int compileStatus = 0;
            GL.GetShader(shaderID, ShaderParameter.CompileStatus, out compileStatus);

            if (compileStatus == COMPILE_ERROR)
            {
                //Shader compilation failed.  Output compilation error to console and throw new exception
                string shaderInfo = String.Empty;
                GL.GetShaderInfoLog(shaderID, out shaderInfo);

                System.Console.WriteLine(String.Format("ERROR::SHADER: Compile-time error: Type: {0}\n{1}", type.ToString(), shaderInfo));

                throw new ShaderCompilationErrorException(
                    String.Format("Shader #{0} ({1}) failed to compile.  Check debug console for error message.",
                    shaderID, type.ToString()));
            }
        }
        private void checkFileSourceValidity(string sourcePath)
        {
            //Check if the source files are not null nor whitespace.
            if (String.IsNullOrWhiteSpace(sourcePath))
                throw new ArgumentNullException("Please provide a proper file path for the shader.");

            //Check if the shader files exist.
            if (!File.Exists(sourcePath))
                throw new FileNotFoundException(String.Format("Could not find shader file from the provided path ('{0}').", sourcePath));
        }

        public int ID { get { return this.shaderProgram; } }

        private const int COMPILE_ERROR = 0;
    }

    public class ShaderCompilationErrorException : Exception
    {
        public ShaderCompilationErrorException() { }
        public ShaderCompilationErrorException(string message) : base(message) { }
        public ShaderCompilationErrorException(string message, Exception inner) : base(message, inner) { }
    }
}
