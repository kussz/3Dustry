using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX.Direct3D;
using Device11 = SharpDX.Direct3D11.Device;
using GameObjects.GameLogic;

namespace GameMain
{
    static class Program
    {

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!(Device11.GetSupportedFeatureLevel() == FeatureLevel.Level_11_0))
            {
                MessageBox.Show("DirectX11 Not Supported.");
                return;
            }


            var game = new Game();

            game.Run();
            game.Dispose();
            // Бесконечный цикл, чтобы приложение не завершалось
        }
    }
}
