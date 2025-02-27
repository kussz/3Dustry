using SharpDX;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects.Drawing
{
    public class TextObject : MeshObject
    {
        private Vector2 _textPos;
        public string Text {  get; set; }
        public float Size { get; private set; }
        private float _textpadding;
        


        internal delegate float ReAlign();
        private ReAlign _haligner;
        private ReAlign _valigner;
        public TextObject(DirectX3DGraphics directX3DGraphics,
            Vector4 position, float yaw, float pitch, float roll,
            Renderer.VertexDataStruct[] vertices, uint[] indices, ShaderResourceView texture, Vector2 textPos,string align,string text,float size,float textpadding)
            : base(directX3DGraphics, position, yaw, pitch, roll, vertices, indices, texture)
        {
            _textPos = textPos;
            Text = text;
            Size = size;
            _textpadding = textpadding;
            switch (align[0])
            {
                case 'c':
                    _haligner = AlignCenter;
                    break;
                case 'r':
                    _haligner = AlignRight;
                    break;
                case 'l':
                default:
                    _haligner = AlignLeft;
                    break;
            }
            if(align.Length>1)
            switch (align[1])
            {
                case 'c':
                    _valigner = AlignVCenter;
                    break;
                case 'b':
                    _valigner = AlignVBottom;
                    break;
                case 't':
                default:
                    _valigner = AlignVTop;
                    break;
            }
            else
                _valigner = AlignVTop;
            DirectX3DGraphics.Instance.RenderForm.UserResized += AlignEvent;
        }
        public void Align()
        {
            MoveTo(_haligner(), _valigner(), 0); 
        }
        private void AlignEvent(object sender, EventArgs e)
        {
            Align();
        }
        private float AlignLeft()
        {
            var formsize = DirectX3DGraphics.Instance.RenderForm.ClientSize;
            var asp = (float)formsize.Width / formsize.Height;
            return _textPos.X - asp;
        }
        private float AlignCenter()
        {
            float widthovertwo = Text.Length * Size *_textpadding / 2;
            return _textPos.X - widthovertwo;

        }
        private float AlignRight()
        {
            float widthovertwo = Text.Length * Size * _textpadding;
            var formsize = DirectX3DGraphics.Instance.RenderForm.ClientSize;
            var asp = (float)formsize.Width / formsize.Height;
            return _textPos.X + asp - widthovertwo;
        }
        private float AlignVCenter()
        {
            var asp = 1-Size/2;
            return _textPos.Y - asp;
        }
        private float AlignVTop()
        {
            return Position.Y;
        }
        private float AlignVBottom()
        {
            var asp = 2 - Size;
            return _textPos.Y - asp;
        }
    }
}
