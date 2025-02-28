using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects.Drawing
{
    public class TextureHolder
    {
        public ShaderResourceView[][]? Textures;
        public TextureHolder(Device device, string path)
        {
            List<List<ShaderResourceView>> textureBank;// = new List<ShaderResourceView> ();
            try
            {
                // Получаем все файлы в указанной папке
                string[] dirs = Directory.GetDirectories(path);
                textureBank = new List<List<ShaderResourceView>>();
                foreach (string dir in dirs)
                {
                    List <ShaderResourceView> list = new List<ShaderResourceView>();
                    string[] files = Directory.GetFiles(dir);
                    foreach (string file in files)
                    {
                        list.Add(new ShaderResourceView(device, TextureLoader.LoadTexture(device, file)));
                    }
                    textureBank.Add(list);
                }
                Textures = textureBank.Select(list=>list.ToArray()).ToArray();

                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }
        public ShaderResourceView GetCurrentFrame(TextureMetaData data, float DeltaT)
        {
            data.Frame += (DeltaT * 10)%1000;
            data.Frame%=Textures[data.State].Length;
            //_frameLocal = _frameGlobal;
            return Textures[data.State][(int)data.Frame];
        }
    }
}
