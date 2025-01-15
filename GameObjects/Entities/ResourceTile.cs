using GameObjects.Drawing;
using GameObjects.Resources;
using SharpDX;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects.Entities
{
    public class ResourceTile
    {
        private static Loader _loader;

        public static void Configure(Loader loader)
        {
            _loader = loader;
        }
        public float Progress { get; set; } = 0;
        public void AddProgress(float progress)
        {
            Progress += progress;
        }
        public GameResource LogicResource { get; set; }
        public MeshObject Mesh {  get; set; }
        public ShaderResourceView Texture { get; set; }
        public ResourceTile(GameResource resource)
        {
            Mesh = _loader.MakeTileSquare(new Vector4(-0.5f,0,-0.5f,1));
            Texture = TextureStorage.GetTexture(resource.Type);
            LogicResource = resource;
        }
    }
}
