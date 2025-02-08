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
	internal class l8
	{
		public static void printPCX(List<byte> data, Image outputImage)
		{
			List<List<List<byte>>> pixelsDecoded = new List<List<List<byte>>>();

			List<byte> dataDecoded = new List<byte>();
			int currentIndex = 128;
			int colorByte = data[4];
			int colorCount = 256;
			List<List<byte>> colors = new List<List<byte>>();
			if (colorByte == 8)
			{
				colorCount = 256;
			}
			int width = BitConverter.ToInt16(data.GetRange(8, 2).ToArray(), 0) + 1;
			int height = BitConverter.ToInt16(data.GetRange(10, 2).ToArray(), 0) + 1;
			
			//дешифрование RLE шифра
			for(int i = currentIndex; i<(data.Count-colorCount*3); i++)
			{
				if (data[i] < 193)
				{
					dataDecoded.Add(data[i]);
				} else if (data[i] == 193)
				{
					i++;
					dataDecoded.Add(data[i]);
				} else if (data[i] > 193)
				{
					int temp = data[i] - 192;
					i++;
					for(int q = 0; q< temp; q++)
					{
						dataDecoded.Add(data[i]);
					}
				}
			}
			List<List<byte>> pixelsMap = new List<List<byte>>();
			for(int i = 0; i < height; i++)
			{
				pixelsMap.Add(new List<byte>());
				for(int q = 0; q<width; q++)
				{
					pixelsMap[i].Add(dataDecoded[(i*width)+q]);
				}
			}
			int tempIndex = 0;
			
			for(int i = data.Count-(colorCount*3);i<data.Count; i+=3)
			{
				colors.Add(new List<byte>());
				for(int q = 0; q<3; q++)
				{

					colors[tempIndex].Add(data[i+q]);
					
				}
				Console.WriteLine(colors[tempIndex][0] + " | " + colors[tempIndex][1] + " | " + colors[tempIndex][2]);
				tempIndex++;
			}
			int index = 0;
			for(int i = 0; i<height; i++)
			{
				pixelsDecoded.Add(new List<List<byte>>());
				for(int j = 0; j<width; j++)
				{
					pixelsDecoded[i].Add(new List<byte>());

					pixelsDecoded[i][j].Add(colors[dataDecoded[index]][0]);
					pixelsDecoded[i][j].Add(colors[dataDecoded[index]][1]);
					pixelsDecoded[i][j].Add(colors[dataDecoded[index]][2]);
					index++;

				}
			}

			Console.WriteLine(data.Count + " | " + dataDecoded.Count + " | " + width + " | " + height);


			WriteableBitmap bitmap = new WriteableBitmap(width, height, 96, 96, System.Windows.Media.PixelFormats.Bgra32, null);
			outputImage.Source = bitmap;
			bitmap.Lock();
			unsafe
			{
				IntPtr pBackBuffer = bitmap.BackBuffer;
				for (int i = 0; i < height; i++)
				{
					for (int j = 0; j < width; j++)
					{

						IntPtr pPixel = pBackBuffer + i * bitmap.BackBufferStride + j * 4;

						byte r = colors[pixelsMap[i][j]][0];
						byte g = colors[pixelsMap[i][j]][1];
						byte b = colors[pixelsMap[i][j]][2];
						*((int*)pPixel) = (255 << 24) | (r << 16) | (g << 8) | b;

					}
				}
			}
			bitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));
			bitmap.Unlock();



		}
	}
}
