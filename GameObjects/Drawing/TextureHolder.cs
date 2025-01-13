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
        public ShaderResourceView[] Textures;
        private float _frame =0;
        private static float _frameGlobal;
        private float _frameLocal;
        public ShaderResourceView CurrentFrame { get {
                return GetCurrentFrame();
            } }
        public TextureHolder(Device device, string path)
        {
            List<ShaderResourceView> list = new List<ShaderResourceView> ();
            try
            {
                // Получаем все файлы в указанной папке
                string[] files = Directory.GetFiles(path);

                // Перебираем и выводим имена файлов
                foreach (string file in files)
                {
                    list.Add(new ShaderResourceView(device, TextureLoader.LoadTexture(device, file)));
                }
                Textures = list.ToArray();
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
                _frameGlobal = 0;
        }
        private ShaderResourceView GetCurrentFrame()
        {
            if(_frameLocal>_frameGlobal)
                _frameLocal -= 1000;
            _frame += _frameGlobal - _frameLocal;
            if(_frame>=Textures.Length)
                _frame=0;
            _frameLocal = _frameGlobal;
            return Textures[(int)_frame];
        }
    }
}
