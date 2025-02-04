using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPILabs
{
    internal class l3
    {
        public static List<byte> RotateBMP(List<byte> data)
        {
            List<byte> result = new List<byte>(data.GetRange(0, 54));

            byte[] width = (data.GetRange(18, 4).ToArray());
            byte[] height =(data.GetRange(22, 4).ToArray());
            int heightInt = BitConverter.ToInt32(height, 0);
            int widthInt = BitConverter.ToInt32(width, 0);
            int originalStride = (((data.Count - 54) / heightInt) % widthInt);
            for (int i = 0; i < 4; i++)
            {
                result[18 + i] = height[i];
                result[22 + i] = width[i];
            }

            List<List<List<byte>>> pixels = new List<List<List<byte>>> ();
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
            for(int i = 0; i < widthInt; i++)
            {
                for(int j = 0; j < heightInt; j++)
                {
                    for(int q = 0; q < 3; q++)
                    {
                        result.Add(pixels[j][i][q]);
                    }
                }
                while ((result.Count - 54) % 4 != 0)
                {
                    result.Add(0);
                }
            }
            
                
            return result;
        }
    }
}
