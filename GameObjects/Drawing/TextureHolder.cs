using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;

namespace GameObjects.Drawing
{
    public class TextureHolder
    {
        public ShaderResourceView[][]? Textures;
        private static List<float> _frames;
        private static float _frameNext;
        private static float _framePrev;
        public TextureHolder(Device device, string path)
        {
            _frames = [0];
            List<List<ShaderResourceView>> textureBank;// = new List<ShaderResourceView> ();
            try
            {
                // Получаем все файлы в указанной папке
                string[] dirs = Directory.GetDirectories(path);
                textureBank = new List<List<ShaderResourceView>>();
                foreach (string dir in dirs)
                {
                    List<ShaderResourceView> list = new List<ShaderResourceView>();
                    string[] files = Directory.GetFiles(dir);
                    foreach (string file in files)
                    {
                        list.Add(new ShaderResourceView(device, TextureLoader.LoadTexture(device, file)));
                    }
                    textureBank.Add(list);
                }
                Textures = textureBank.Select(list => list.ToArray()).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }
        public static void Update(float DeltaT)
        {
            _framePrev = _frameNext;
            _frameNext += DeltaT * 10;
            _frameNext %= 1000;
            if (_framePrev > _frameNext)
                _framePrev -= 1000;
            for (int i=0;i<_frames.Count;i++)
            {
                _frames[i] += (_frameNext - _framePrev)%1000;
            }

        }
        public ShaderResourceView GetCurrentFrame(int state)
        {
            if (state >= _frames.Count)
                _frames.Add(0);
            var frame = _frames[state] % Textures[state].Length;
            return Textures[state][(int)frame];
        }
    }
}
