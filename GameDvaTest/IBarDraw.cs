using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Game2Test
{
    public interface IBarDraw
    {
        void GiveHealthForBars(ref int health, ref int MaxHealth);
    }
}
