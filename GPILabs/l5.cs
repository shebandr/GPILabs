using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPILabs
{
	internal class l5
	{
		public static List<byte> UpscaleBMP(List<byte> data, int scale)
		{
			int currentIndex = 54 + (256 * 4);
			List<byte> result = new List<byte> (data.GetRange(0, currentIndex)); //тут идет костыль, потому что после палитры в конкретном файле с котенком идут 80 байт неизвестно чего, да и по заданию надо только для 256 цветов
			int width = BitConverter.ToInt32(data.GetRange(18, 4).ToArray(), 0);
			int height = BitConverter.ToInt32(data.GetRange(22, 4).ToArray(), 0);
			int originalStride = (((data.Count - currentIndex) / height) % width);
			byte[] newWidth = BitConverter.GetBytes(BitConverter.ToInt32(data.GetRange(18, 4).ToArray(), 0) * scale);
			byte[] newHeight = BitConverter.GetBytes(BitConverter.ToInt32(data.GetRange(22, 4).ToArray(), 0) * scale);
			for (int i = 0; i < 4; i++)
			{
				result[18 + i] = newWidth[i];
				result[22 + i] = newHeight[i];
			}
			//процесс скейлинга

			for(int i = 0; i<height; i++) // прогон каждой оригинальной строки
			{
				for(int j = 0; j<scale; j++) //прогон каждой копии для скейлинга
				{
					for(int k = 0; k < width; k++) // прогон каждого оригинального пикселя
					{
						for(int l = 0; l<scale; l++)
						{
							result.Add(data[currentIndex]);
							currentIndex++;
						}
					}
					currentIndex += originalStride;
				}
				
			}

			return result;
		}
	}
}
