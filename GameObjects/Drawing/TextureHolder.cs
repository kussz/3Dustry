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
        private float _frame =0;
        private static float _frameGlobal;
        private float _frameLocal;
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
        public static void Update(float DeltaT)
        {
            _frameGlobal += DeltaT*10;
            if (_frameGlobal >= 1000)
                _frameGlobal %= 1000;
        }
        public ShaderResourceView GetCurrentFrame(int state)
        {
            if(_frameLocal>_frameGlobal)
                _frameLocal -= 1000;
            _frame += _frameGlobal - _frameLocal;
            if(_frame>=Textures[state]!.Length)
                _frame%=Textures[state].Length;
            _frameLocal = _frameGlobal;
            return Textures[state][(int)_frame];
        }
    }
}
