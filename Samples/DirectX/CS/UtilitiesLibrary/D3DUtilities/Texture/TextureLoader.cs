// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Runtime.InteropServices;


using Microsoft.WindowsAPICodePack.DirectX.Direct3D10;
using Microsoft.WindowsAPICodePack.DirectX.DXGI;
using Microsoft.WindowsAPICodePack.DirectX.WindowsImagingComponent;

namespace Microsoft.WindowsAPICodePack.DirectX.DirectXUtilities
{
    public class TextureLoader
    {
        /// <summary>
        /// Creates a ShaderResourceView from a bitmap in a Stream. 
        /// </summary>
        /// <param name="device">The Direct3D device that will own the ShaderResourceView</param>
        /// <param name="stream">Any Windows Imaging Component decodable image</param>
        /// <returns>A ShaderResourceView object with the loaded texture.</returns>
        public static ShaderResourceView LoadTexture( D3DDevice device, Stream stream )
        {
            ImagingFactory factory = new ImagingFactory( );

            BitmapDecoder bitmapDecoder =
                factory.CreateDecoderFromStream(
                    stream,
                    DecodeMetadataCacheOptions.OnDemand );

            return LoadFromDecoder(device, factory, bitmapDecoder);
        }

        /// <summary>
        /// Creates a ShaderResourceView from a bitmap in a file. 
        /// </summary>
        /// <param name="device">The Direct3D device that will own the ShaderResourceView</param>
        /// <param name="filePath">Any Windows Imaging Component decodable image</param>
        /// <returns>A ShaderResourceView object with the loaded texture.</returns>
        public static ShaderResourceView LoadTexture(D3DDevice device, String filePath)
        {
            ImagingFactory factory = new ImagingFactory();

            BitmapDecoder bitmapDecoder =
                factory.CreateDecoderFromFilename(
                    filePath,
                    DesiredAccess.Read,
                    DecodeMetadataCacheOptions.OnDemand);

            return LoadFromDecoder(device, factory, bitmapDecoder);
        }
        private static ShaderResourceView LoadFromDecoder(D3DDevice device, ImagingFactory factory, BitmapDecoder bitmapDecoder)
        {
            if (bitmapDecoder.FrameCount == 0)
                throw new ArgumentException("Image file successfully loaded, but it has no image frames.");

            BitmapFrameDecode bitmapFrameDecode = bitmapDecoder.GetFrame(0);
            BitmapSource bitmapSource = bitmapFrameDecode.ToBitmapSource();

            // create texture description
            Texture2DDescription textureDescription = new Texture2DDescription()
            {
                Width = bitmapSource.Size.Width,
                Height = bitmapSource.Size.Height,
                MipLevels = 1,
                ArraySize = 1,
                Format = Format.R8G8B8A8_UNORM,
                SampleDescription = new SampleDescription()
                {
                    Count = 1,
                    Quality = 0,
                },
                Usage = Usage.Dynamic,
                BindFlags = BindFlag.ShaderResource,
                CpuAccessFlags = CpuAccessFlag.Write,
                MiscFlags = 0
            };

            // create texture
            Texture2D texture = device.CreateTexture2D(textureDescription);

            // Create a format converter
            WICFormatConverter converter = factory.CreateFormatConverter();
            converter.Initialize(
                bitmapSource,
                PixelFormats.Pf32bppRGBA,
                BitmapDitherType.None,
                BitmapPaletteType.Custom);

            // get bitmap data
            byte[] buffer = converter.CopyPixels();

            // Copy bitmap data to texture
            MappedTexture2D texmap = texture.Map(0, Map.WriteDiscard, MapFlag.Unspecified);
            Marshal.Copy(buffer, 0, texmap.Data, buffer.Length);
            texture.Unmap(0);

            // create shader resource view description
            ShaderResourceViewDescription srvDescription = new ShaderResourceViewDescription()
            {
                Format = textureDescription.Format,
                ViewDimension = ShaderResourceViewDimension.Texture2D,
                Texture2D = new Texture2DShaderResourceView()
                {
                    MipLevels = textureDescription.MipLevels,
                    MostDetailedMip = 0
                }
            };

            // create shader resource view from texture
            return device.CreateShaderResourceView(texture, srvDescription);
        }
    }
}
