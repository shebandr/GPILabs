using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPILabs
{
	internal class l6
	{
		public static List<byte> SetLogo(List<byte> data, List<byte> logo, float k, int x, int y)
		{




			List<byte> result = new List<byte>(data.GetRange(0, 54));
			int width = BitConverter.ToInt32(data.GetRange(18, 4).ToArray(), 0);
			int height = BitConverter.ToInt32(data.GetRange(22, 4).ToArray(), 0);
			int widthLogo = BitConverter.ToInt32(logo.GetRange(18, 4).ToArray(), 0);
			int heightLogo = BitConverter.ToInt32(logo.GetRange(22, 4).ToArray(), 0);
			int originalStrideData = (((data.Count - 54) / height) % width);
			int originalStrideLogo = (((logo.Count - 54) / heightLogo) % widthLogo);
			int currentIndexData = 54;
			int currentIndexLogo = 54;
			Console.WriteLine(width + " | " + height + " | " + originalStrideData + " | " + widthLogo + " | " + heightLogo + " | " + originalStrideLogo);
			for(int i = 0; i<height; i++)
			{
				for(int j = 0; j<width; j++)
				{
					for(int w = 0; w<3; w++)
					{
						if(i>=y && i < y+heightLogo && j >= x && j < x + widthLogo )
						{
							if(logo[currentIndexLogo] != 0)
							{
								float t = ((float)data[currentIndexData] * (1-k)) + ((float)logo[currentIndexLogo] * k);
								result.Add((byte)t);

							} else
							{
								result.Add(data[currentIndexData]);
							}

							currentIndexData++;
							currentIndexLogo++;
						} else
						{
							result.Add(data[currentIndexData]);
							currentIndexData++;
						}
					}
				}
				currentIndexData += originalStrideData;
				if(i > y && i < y + heightLogo)
				{
					currentIndexLogo += originalStrideLogo;
				}
			}



			return result;
		}
	}
}
