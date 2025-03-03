using SharpDX;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects.Drawing
{
    public class MenuTile
    {
        private static Loader? _loader = Loader.GetInstance();
        private static Device? _device = DirectX3DGraphics.Instance.Device;
        public MeshObject Mesh;
        public MenuTile(string path,float size,Vector2 position)
        {
            
            var bitmap = TextureLoader.LoadBitmap(path);
            var texture = TextureLoader.CreateTexture2DFromBitmap(_device!, bitmap);

            float aspect = (float)bitmap.Size.Width/bitmap.Size.Height;
            Mesh = _loader!.MakeMenuTile(aspect,size,position);
            Mesh.Texture = new SharpDX.Direct3D11.ShaderResourceView(_device, texture);
        }
    }
}
