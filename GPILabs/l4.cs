using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GPILabs
{
    internal class l4
    {
        public static void printBMP(List<byte> data, Image outputImage)
        {
            int colorsCount = BitConverter.ToInt32(data.GetRange(46, 50).ToArray(), 0);
            Console.WriteLine(colorsCount);
            byte[] width = (data.GetRange(18, 4).ToArray());
            byte[] height = (data.GetRange(22, 4).ToArray());
            int heightInt = BitConverter.ToInt32(height, 0);
            int widthInt = BitConverter.ToInt32(width, 0);
            if (colorsCount == 0)
            {

                int originalStride = (((data.Count - 54) / heightInt) % widthInt);
                List<List<List<byte>>> pixels = new List<List<List<byte>>>();
                int currentIndex = 54;
                for (int i = 0; i < heightInt; i++)
                {
                    pixels.Add(new List<List<byte>>());
                    for (int j = 0; j < widthInt; j++)
                    {
                        pixels[i].Add(new List<byte>());
                        for (int q = 0; q < 3; q++)
                        {
                            pixels[i][j].Add(data[currentIndex]);

                            currentIndex++;
                        }
                    }
                    currentIndex += originalStride;
                }

                WriteableBitmap bitmap = new WriteableBitmap(widthInt, heightInt, 96, 96, System.Windows.Media.PixelFormats.Bgra32, null);
                outputImage.Source = bitmap;
                bitmap.Lock();
                unsafe { 
                    IntPtr pBackBuffer = bitmap.BackBuffer;
                    for (int i = 0; i < heightInt; i++)
                    {
                        for (int j = 0; j < widthInt; j++)
                        {

                            IntPtr pPixel = pBackBuffer + i * bitmap.BackBufferStride + j * 4;

                            byte b = pixels[ pixels.Count-1-i][pixels[i].Count-1-j][0]; 
                            byte g = pixels[pixels.Count - 1 - i][pixels[i].Count - 1 - j][1]; 
                            byte r = pixels[pixels.Count - 1 - i][pixels[i].Count - 1 - j][2]; 
                            *((int*)pPixel) = (255 << 24) | (r << 16) | (g << 8) | b;

                        }
                    }
                }
                bitmap.AddDirtyRect(new Int32Rect(0, 0, widthInt, heightInt));
                bitmap.Unlock();

            }
            if (colorsCount > 16)
            {
                //создание словаря с палитрой
                Dictionary<byte, List<byte>> colors = new Dictionary<byte, List<byte>>();
                for (int i = 0; i < colorsCount; i++)
                {
                    byte R = data[54 + (i * 4)];
                    byte G = data[55 + (i * 4)];
                    byte B = data[56 + (i * 4)];
                    List<byte> color = new List<byte> { R, G, B };
                    colors[(byte)i] = color;

                }

                //запись в матрицу строк
                int currentIndex = 54 + (colorsCount * 4);

                int originalStride = (((data.Count - currentIndex) / heightInt) % widthInt);
                Console.WriteLine(originalStride + " | " + currentIndex + " | " + data.Count + " | " + heightInt + " | " + widthInt);

                currentIndex += 80; // это нужно, потому что у файла по метаданным 236 байт на цвет идет, но по факту почему-то 256
                List<List<byte>> bmp = new List<List<byte>>();
                for (int i = 0; i < heightInt; i++)
                {
                    bmp.Add(new List<byte>());
                    for (int j = 0; j < widthInt; j++)
                    {
                        bmp[i].Add(data[currentIndex]);
                        currentIndex++;
                    }
                    currentIndex += originalStride;
                }
                //отрисовка
                WriteableBitmap bitmap = new WriteableBitmap(widthInt, heightInt, 96, 96, System.Windows.Media.PixelFormats.Bgra32, null);
                outputImage.Source = bitmap;
                bitmap.Lock();
                unsafe
                {
                    IntPtr pBackBuffer = bitmap.BackBuffer;
                    for (int i = 0; i < heightInt; i++)
                    {
                        for (int j = 0; j < widthInt; j++)
                        {

                            IntPtr pPixel = pBackBuffer + i * bitmap.BackBufferStride + j * 4;

                            byte b = colors[bmp[bmp.Count - 1 - i][j]][0];
                            byte g = colors[bmp[bmp.Count - 1 - i][j]][1];
                            byte r = colors[bmp[bmp.Count - 1 - i][j]][2];
                            *((int*)pPixel) = (255 << 24) | (r << 16) | (g << 8) | b;

                        }
                    }
                }
                bitmap.AddDirtyRect(new Int32Rect(0, 0, widthInt, heightInt));
                bitmap.Unlock();

            }
            if (colorsCount <= 16 && colorsCount > 0)
            {
                //создание словаря с палитрой
                Dictionary<byte, List<byte>> colors = new Dictionary<byte, List<byte>>();
                

                for (int i = 0; i < colorsCount; i++)
                {
                    byte R = data[54 + (i * 4)];
                    byte G = data[55 + (i * 4)];
                    byte B = data[56 + (i * 4)];
                    List<byte> color = new List<byte> { R, G, B };
                    colors[(byte)i] = color;

                }

                //запись в матрицу строк
                int currentIndex = 54 + (colorsCount * 4);

                int originalStride = ((((data.Count - currentIndex)*2) / heightInt) % widthInt);
                List<byte> data2 = new List<byte>(data.GetRange(0, currentIndex));
                Console.WriteLine(originalStride + " | " + currentIndex + " | " + data.Count + " | " + heightInt + " | " + widthInt);
                for(int i = 0; i < data.Count-currentIndex; i++)
                {
                    data2.Add((byte)((data[i + currentIndex] >> 4) & 0b00001111));
                    data2.Add((byte)((data[i + currentIndex] >> 4) & 0b00001111));
                }

                List<List<byte>> bmp = new List<List<byte>>();
                for (int i = 0; i < heightInt; i++)
                {
                    bmp.Add(new List<byte>());
                    for (int j = 0; j < widthInt; j++)
                    {
                        bmp[i].Add(data2[currentIndex]);
                        currentIndex++;
                    }
                    currentIndex += originalStride;
                }
                //отрисовка
                WriteableBitmap bitmap = new WriteableBitmap(widthInt, heightInt, 96, 96, System.Windows.Media.PixelFormats.Bgra32, null);
                outputImage.Source = bitmap;
                bitmap.Lock();
                unsafe
                {
                    IntPtr pBackBuffer = bitmap.BackBuffer;
                    for (int i = 0; i < heightInt; i++)
                    {
                        for (int j = 0; j < widthInt; j++)
                        {

                            IntPtr pPixel = pBackBuffer + i * bitmap.BackBufferStride + j * 4;

                            byte b = colors[bmp[bmp.Count - 1 - i][j]][0];
                            byte g = colors[bmp[bmp.Count - 1 - i][j]][1];
                            byte r = colors[bmp[bmp.Count - 1 - i][j]][2];
                            *((int*)pPixel) = (255 << 24) | (r << 16) | (g << 8) | b;

                        }
                    }
                }
                bitmap.AddDirtyRect(new Int32Rect(0, 0, widthInt, heightInt));
                bitmap.Unlock();

            }
        }
    }
}
