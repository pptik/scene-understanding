using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApp1
{
    class Arah
    {
        public string ArahSpasial { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int LebarArea { get; set; }
        public int PanjangArea { get; set; }

        public Point Pusat()
        {
            return new Point(this.X + this.LebarArea / 2, this.Y + this.PanjangArea / 2);
        }
    }
}
