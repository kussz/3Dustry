// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using SharpDX.Direct3D11;
using SharpDX.WIC;

namespace GameObjects.Drawing
{
    public class TextureLoader
    {
        /// <summary>
        /// Loads a bitmap using WIC.
        /// </summary>
        /// <param name="deviceManager"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static BitmapSource LoadBitmap(string filename)
        {
            ImagingFactory2 factory = new ImagingFactory2();
            var bitmapDecoder = new BitmapDecoder(
                factory,
                filename,
                DecodeOptions.CacheOnDemand
                );

            var formatConverter = new FormatConverter(factory);

            formatConverter.Initialize(
                bitmapDecoder.GetFrame(0),
                PixelFormat.Format32bppPRGBA,
                BitmapDitherType.None,
                null,
                0.0,
                BitmapPaletteType.Custom);

            return formatConverter;
        }

        /// <summary>
        /// Creates a <see cref="Texture2D"/> from a WIC <see cref="BitmapSource"/>
        /// </summary>
        /// <param name="device">The Direct3D11 device</param>
        /// <param name="bitmapSource">The WIC bitmap source</param>
        /// <returns>A Texture2D</returns>
        public static Texture2D CreateTexture2DFromBitmap(Device device, BitmapSource bitmapSource)
        {
            // Allocate DataStream to receive the WIC image pixels
            int stride = bitmapSource.Size.Width * 4;
            using (var buffer = new SharpDX.DataStream(bitmapSource.Size.Height * stride, true, true))
            {
                // Copy the content of the WIC to the buffer
                bitmapSource.CopyPixels(stride, buffer);
                return new Texture2D(device, new Texture2DDescription()
                {
                    Width = bitmapSource.Size.Width,
                    Height = bitmapSource.Size.Height,
                    ArraySize = 1,
                    BindFlags = BindFlags.ShaderResource,
                    Usage = ResourceUsage.Immutable,
                    CpuAccessFlags = CpuAccessFlags.None,
                    Format = SharpDX.DXGI.Format.R8G8B8A8_UNorm,
                    MipLevels = 1,
                    OptionFlags = ResourceOptionFlags.None,
                    SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                }, new SharpDX.DataRectangle(buffer.DataPointer, stride));
            }
        }
        public static Texture2D LoadTexture(Device device, string filename)
        {
            BitmapSource bitmapSource = LoadBitmap(filename);
            return CreateTexture2DFromBitmap(device, bitmapSource);
        }
        public static ShaderResourceView GetFloorTexture(DirectX3DGraphics graphics, Tile type)
        {
            Texture2D texture = LoadTexture(graphics.Device, TextureStorage.GetTexturePath(type));
            return new ShaderResourceView(graphics.Device, texture);
        }
        public static ShaderResourceView GetResourceTexture(DirectX3DGraphics graphics, ResourceType type)
        {
            Texture2D texture = LoadTexture(graphics.Device, TextureStorage.GetTexturePath(type));
            return new ShaderResourceView(graphics.Device, texture);
        }
        public static ShaderResourceView GetTexture(DirectX3DGraphics graphics, string filename)
        {
            Texture2D texture = LoadTexture(graphics.Device, filename);
            return new ShaderResourceView(graphics.Device, texture);
        }
        public static ShaderResourceView GetEntityTexture(DirectX3DGraphics graphics, EntityType type)
        {
            Texture2D texture = LoadTexture(graphics.Device, TextureStorage.GetTexturePath(type));
            return new ShaderResourceView(graphics.Device, texture);
        }
    }
}
