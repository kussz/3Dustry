using SharpDX;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects.Drawing
{
    public class TextManager
    {
        private delegate void Moving();
        private float _textpadding;
        private ShaderResourceView _letters;
        private DirectX3DGraphics _graphics;
        private struct Metadata
        {
            public Metadata()
            { }
            public int fullWidth = 14;
            public int fullHeight = 4;
            public int letterHeight = 8;
            public int letterWidth = 10;

        }
        private Metadata _md;
        public TextManager()
        {
            _graphics = DirectX3DGraphics.Instance;
            _letters = TextureStorage.GetTextTile();
            _md = new Metadata();
        }
        public TextObject LoadText(string text,Vector2 position,float size, string align,float textpadding = 0.8f)
        {
            _textpadding = textpadding;
            size /= 10;
            float aspect = (float)_md.letterHeight / _md.letterWidth;
            var asp = (float)_graphics.RenderForm.ClientSize.Width / _graphics.RenderForm.ClientSize.Height;
            //position = new Vector2(position.X/100,1-position.Y/100-size);
            position = new Vector2(size*position.X*_textpadding,1-size*(position.Y+1));
            (var vertices, var indices, var result) = SetText(text,size);
            var textob =  new TextObject(_graphics, new Vector4(position.X-asp, position.Y, 0, 1), 0, 0, 0, vertices.ToArray(), indices.ToArray(),_letters,position,align,result,size,textpadding);
            textob.Align();
            return textob;
        }
        private (Renderer.VertexDataStruct[] vertices, uint[] indices,string result) SetText(string text,float size)
        {
            text = text.ToUpper();

            uint offset = 0;
            float aspect = (float)_md.letterHeight / _md.letterWidth;

            int num = 0;
            List<Renderer.VertexDataStruct> vertices = new List<Renderer.VertexDataStruct>();
            var indices = new List<uint>();
            string result = "";
            string insider = "";
            bool isInside = false;
            foreach (char b in text)
            {
                float i = 0;
                float j = 0;
                if (b >= 'А' && b <= 'Я')
                {
                    int a = b - 'А';
                    i = ((float)a % 11) / _md.fullWidth;
                    j = (a / 11) / (float)_md.fullHeight;
                }
                else if (b >= '0' && b <= '9')
                {
                    int a = b - '0';
                    i = ((float)a % 11) / _md.fullWidth;
                    j = (a / 11 + 3) / (float)_md.fullHeight;
                }
                else if (b == '.'||b==',')
                {
                    int a = 'Я' + 1 - 'А';
                    i = ((float)a % 11) / _md.fullWidth;
                    j = (a / 11) / (float)_md.fullHeight;
                }
                else if (b == ':')
                {
                    int a = '9' + 1 - '0';
                    i = ((float)a % 11) / _md.fullWidth;
                    j = (a / 11 + 3) / (float)_md.fullHeight;
                }
                else if (b == '%')
                {
                    isInside = !isInside;
                    if (isInside)
                    {
                        insider = "";
                        continue;
                    }
                    else
                    {
                        ResourceType type;
                        int a=0;
                        if (Enum.TryParse(insider,true, out type))
                        {
                            a=(int)type;
                        }
                        i = ((float)a % 3 + 11) / _md.fullWidth;
                        j = (a / 3) / (float)_md.fullHeight;
                    }
                }
                else if (b == 32)
                {
                    num++;
                    continue;
                }
                if (isInside)
                {
                    insider += b;
                    continue;
                }
                result += b;
                vertices.Add(new Renderer.VertexDataStruct()
                {
                    position = new Vector4(size * num * _textpadding, 0, 1, 1),
                    texCoord = new Vector2(i, j + (float)1 / 4),
                });
                vertices.Add(new Renderer.VertexDataStruct()
                {
                    position = new Vector4(size * aspect + size * num * _textpadding, 0, 1, 1),
                    texCoord = new Vector2(i + (float)1 / _md.fullWidth, j + (float)1 / 4),
                });
                vertices.Add(new Renderer.VertexDataStruct()
                {
                    position = new Vector4(size * num * _textpadding, size, 1, 1),
                    texCoord = new Vector2(i, j),
                });
                vertices.Add(new Renderer.VertexDataStruct()
                {
                    position = new Vector4(size * aspect + size * num * _textpadding, size, 1, 1),
                    texCoord = new Vector2(i + (float)1 / _md.fullWidth, j),
                });
                indices.Add(offset);
                indices.Add(offset + 1);
                indices.Add(offset + 2);
                indices.Add(offset + 1);
                indices.Add(offset + 3);
                indices.Add(offset + 2);
                offset += 4;
                num++;
            }
            return (vertices.ToArray(), indices.ToArray(), result);
        }

        public void Edit(TextObject textobj,string text)
        {
            (var vertices, var indices, var result) = SetText(text,textobj.Size);

            textobj.Vertices = vertices;
            textobj.Indices = indices;
            textobj.Text = result;
        }
    }
}
