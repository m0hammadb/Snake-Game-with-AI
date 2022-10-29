using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Console_Snake_Game
{
    class ArrayCopy
    {
        public static ScreenPoint[] CopyByValue(ScreenPoint[] inp)
        {
            List<ScreenPoint> tmp = new List<ScreenPoint>();
            foreach(ScreenPoint t  in inp)
            {
                ScreenPoint tx = new ScreenPoint(t.X, t.Y);
                tmp.Add(tx);
            }

            return tmp.ToArray();
        }
    }
}
