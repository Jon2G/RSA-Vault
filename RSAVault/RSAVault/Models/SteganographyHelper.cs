using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FFImageLoading.Forms;
using ImageProcessing.JPEGCodec;
using ImageProcessing.PNGCodec;
using ImageProcessing.TGACodec;
using Kit;
using Kit.Sql.Attributes;
using MoyskleyTech.ImageProcessing;
using MoyskleyTech.ImageProcessing.Image;
using Xamarin.Forms;

namespace RSAVault.Models
{
    [Preserve]
    public class SteganographyHelper
    {
        public enum State
        {
            Hiding,
            Filling_With_Zeros
        };

        public static async Task<FileImageSource> EmbedText(CachedImage FileInfo, string text)
        {
            await Task.Yield();
            try
            {
                var png = new Hjg.Pngcs.Chunks.PngChunkIHDR(new Hjg.Pngcs.ImageInfo(10, 10, 8, true));
                using (var memory = new MemoryStream(await FileInfo.GetImageAsJpgAsync()))
                {
                    memory.Position = 0;
                    BitmapFactory factory = new BitmapFactory();
                    factory.AddCodec(new BitmapCodec());
                    factory.AddCodec(new PngCodec());
                    factory.AddCodec(new JPEGCodec());
                    factory.AddCodec(new TGACodec());
                    using (Image<Pixel> result = factory.Decode(memory))
                    {
                        using (Image<Pixel> bitmap = result.GetBitmap(0, 0, result.Width, result.Height))
                        {
                            using (Image<Pixel> SBitmap = EmbedText(bmp: bitmap, text))
                            {
                                string path = $"{Tools.Instance.TemporalPath}/{Guid.NewGuid():N}.png";
                                var file = new FileInfo(path);
                                if (!file.Directory.Exists)
                                {
                                    file.Directory.Create();
                                }

                                using (FileStream file_s = new FileStream(file.FullName, FileMode.OpenOrCreate))
                                {
                                    SBitmap.Save(file_s);
                                }
                                return (FileImageSource)ImageSource.FromFile(file.FullName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "BarcodeDecoding.GetImage");
                await Tools.Instance.Dialogs.CustomMessageBox.Show(ex.Message);
            }
            return null;
        }
        public static Image<Pixel> EmbedText(Image<Pixel> bmp, string text)
        {
            // initially, we'll be hiding characters in the image
            State state = State.Hiding;

            // holds the index of the character that is being hidden
            int charIndex = 0;

            // holds the value of the character converted to integer
            int charValue = 0;

            // holds the index of the color element (R or G or B) that is currently being processed
            long pixelElementIndex = 0;

            // holds the number of trailing zeros that have been added when finishing the process
            int zeros = 0;

            // hold pixel elements
            int R = 0, G = 0, B = 0;

            // pass through the rows
            for (int i = 0; i < bmp.Height; i++)
            {
                // pass through each row
                for (int j = 0; j < bmp.Width; j++)
                {
                    // holds the pixel that is currently being processed
                    Pixel pixel = bmp[j, i];

                    // now, clear the least significant bit (LSB) from each pixel element
                    R = pixel.R - pixel.R % 2;
                    G = pixel.G - pixel.G % 2;
                    B = pixel.B - pixel.B % 2;

                    // for each pixel, pass through its elements (RGB)
                    for (int n = 0; n < 3; n++)
                    {
                        // check if new 8 bits has been processed
                        if (pixelElementIndex % 8 == 0)
                        {
                            // check if the whole process has finished
                            // we can say that it's finished when 8 zeros are added
                            if (state == State.Filling_With_Zeros && zeros == 8)
                            {
                                // apply the last pixel on the image
                                // even if only a part of its elements have been affected
                                if ((pixelElementIndex - 1) % 3 < 2)
                                {
                                    bmp[j, i] = Pixel.FromArgb(255, (byte)R, (byte)G, (byte)B);
                                    // bmp.SetPixel(j, i, Color.FromArgb(R, G, B));
                                }

                                // return the bitmap with the text hidden in
                                return bmp;
                            }

                            // check if all characters has been hidden
                            if (charIndex >= text.Length)
                            {
                                // start adding zeros to mark the end of the text
                                state = State.Filling_With_Zeros;
                            }
                            else
                            {
                                // move to the next character and process again
                                charValue = text[charIndex++];
                            }
                        }

                        // check which pixel element has the turn to hide a bit in its LSB
                        switch (pixelElementIndex % 3)
                        {
                            case 0:
                                {
                                    if (state == State.Hiding)
                                    {
                                        // the rightmost bit in the character will be (charValue % 2)
                                        // to put this value instead of the LSB of the pixel element
                                        // just add it to it
                                        // recall that the LSB of the pixel element had been cleared
                                        // before this operation
                                        R += charValue % 2;

                                        // removes the added rightmost bit of the character
                                        // such that next time we can reach the next one
                                        charValue /= 2;
                                    }
                                }
                                break;
                            case 1:
                                {
                                    if (state == State.Hiding)
                                    {
                                        G += charValue % 2;

                                        charValue /= 2;
                                    }
                                }
                                break;
                            case 2:
                                {
                                    if (state == State.Hiding)
                                    {
                                        B += charValue % 2;

                                        charValue /= 2;
                                    }
                                    bmp[j, i] = Pixel.FromArgb(255, (byte)R, (byte)G, (byte)B);
                                    //bmp.SetPixel(j, i, Color.FromArgb(R, G, B));
                                }
                                break;
                        }

                        pixelElementIndex++;

                        if (state == State.Filling_With_Zeros)
                        {
                            // increment the value of zeros until it is 8
                            zeros++;
                        }
                    }
                }
            }

            return bmp;
        }

        public static async Task<string> ExtractText(FileImageSource FileInfo)
        {
            await Task.Yield();
            try
            {
                var png = new Hjg.Pngcs.Chunks.PngChunkIHDR(new Hjg.Pngcs.ImageInfo(10, 10, 8, true));
                using (var memory = new FileStream(FileInfo.File, FileMode.Open))
                {
                    memory.Position = 0;
                    BitmapFactory factory = new BitmapFactory();
                    factory.AddCodec(new BitmapCodec());
                    factory.AddCodec(new PngCodec());
                    factory.AddCodec(new JPEGCodec());
                    factory.AddCodec(new TGACodec());
                    using (Image<Pixel> result = factory.Decode(memory))
                    {
                        using (Image<Pixel> bitmap = result.GetBitmap(0, 0, result.Width, result.Height))
                        {
                            return ExtractText(bitmap);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "BarcodeDecoding.GetImage");
                await Tools.Instance.Dialogs.CustomMessageBox.Show(ex.Message);
            }
            return String.Empty;
        }
        public static string ExtractText(Image<Pixel> bmp)
        {
            int colorUnitIndex = 0;
            int charValue = 0;

            // holds the text that will be extracted from the image
            string extractedText = String.Empty;

            // pass through the rows
            for (int i = 0; i < bmp.Height; i++)
            {
                // pass through each row
                for (int j = 0; j < bmp.Width; j++)
                {
                    var pixel = bmp[j, i];

                    // for each pixel, pass through its elements (RGB)
                    for (int n = 0; n < 3; n++)
                    {
                        switch (colorUnitIndex % 3)
                        {
                            case 0:
                                {
                                    // get the LSB from the pixel element (will be pixel.R % 2)
                                    // then add one bit to the right of the current character
                                    // this can be done by (charValue = charValue * 2)
                                    // replace the added bit (which value is by default 0) with
                                    // the LSB of the pixel element, simply by addition
                                    charValue = charValue * 2 + pixel.R % 2;
                                }
                                break;
                            case 1:
                                {
                                    charValue = charValue * 2 + pixel.G % 2;
                                }
                                break;
                            case 2:
                                {
                                    charValue = charValue * 2 + pixel.B % 2;
                                }
                                break;
                        }

                        colorUnitIndex++;

                        // if 8 bits has been added,
                        // then add the current character to the result text
                        if (colorUnitIndex % 8 == 0)
                        {
                            // reverse? of course, since each time the process occurs
                            // on the right (for simplicity)
                            charValue = reverseBits(charValue);

                            // can only be 0 if it is the stop character (the 8 zeros)
                            if (charValue == 0)
                            {
                                return extractedText;
                            }

                            // convert the character value from int to char
                            char c = (char)charValue;

                            // add the current character to the result text
                            extractedText += c.ToString();
                        }
                    }
                }
            }

            return extractedText;
        }

        public static int reverseBits(int n)
        {
            int result = 0;

            for (int i = 0; i < 8; i++)
            {
                result = result * 2 + n % 2;

                n /= 2;
            }

            return result;
        }
    }
}
