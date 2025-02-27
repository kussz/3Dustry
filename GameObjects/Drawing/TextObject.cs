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
        


        internal delegate void ReAlign();
        internal ReAlign Aligner {get; set;}
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
                    Aligner = AlignCenter;
                    break;
                case 'r':
                    Aligner = AlignRight;
                    break;
                case 'l':
                default:
                    Aligner = AlignLeft;
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
            DirectX3DGraphics.Instance.RenderForm.UserResized += Realign;
        }
        private void Realign(object sender, EventArgs e)
        {
            Aligner();
        }
        private void AlignLeft()
        {
            var formsize = DirectX3DGraphics.Instance.RenderForm.ClientSize;
            var asp = (float)formsize.Width / formsize.Height;
            MoveTo(_textPos.X - asp, Position.Y, 0);
            _valigner();
        }
        private void AlignCenter()
        {
            float widthovertwo = Text.Length * Size *_textpadding / 2;
            MoveTo(_textPos.X-widthovertwo, Position.Y, 0);
            _valigner();

        }
        private void AlignRight()
        {
            float widthovertwo = Text.Length * Size * _textpadding;
            var formsize = DirectX3DGraphics.Instance.RenderForm.ClientSize;
            var asp = (float)formsize.Width / formsize.Height;
            MoveTo(_textPos.X +asp - widthovertwo, Position.Y, 0);
            _valigner();
        }
        private void AlignVCenter()
        {
            var asp = 1-Size/2;
            MoveTo(Position.X, _textPos.Y - asp, 0);
        }
        private void AlignVTop()
        {
            MoveTo(Position.X,Position.Y, 0);
        }
        private void AlignVBottom()
        {
            var asp = 2 - Size;
            MoveTo(Position.X, _textPos.Y - asp, 0);
        }
    }
}
