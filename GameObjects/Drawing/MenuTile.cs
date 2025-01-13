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
        public MeshObject Mesh;
        public MenuTile(Loader loader,Device device, string path,float size,Vector2 position)
        {
            var bitmap = TextureLoader.LoadBitmap(path);
            var texture = TextureLoader.CreateTexture2DFromBitmap(device, bitmap);

            float aspect = (float)bitmap.Size.Width/bitmap.Size.Height;
            Mesh = loader.MakeMenuTile(aspect,size,position);
            Mesh.Texture = new SharpDX.Direct3D11.ShaderResourceView(device, texture);
        }
    }
}
