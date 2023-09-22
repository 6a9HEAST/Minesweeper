using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MineSweeper2
{
    [Serializable]
    public class Flag
    {
        private bool is_placed=false;
        
        public bool TryDraw(MainForm mainform,int x,int y)
        {
            if (is_placed)
            {
                mainform.gr.DrawImage(Image.FromFile("flag.png"), x + 5, y + 5, 30, 30);
                mainform.pictureBox1.Image = mainform.bm;
                return true;
            }
            return false;

        }
        public void TryDraw(MainForm mainform, int x, int y, bool tr= true)
        {
            mainform.gr.DrawImage(Image.FromFile("FakeBomb.png"), x, y, 40, 40);
        }

        public void Place_flag(bool x=false)
        {
            if (x) is_placed = true;
            else
            is_placed = !is_placed;
        }

        public bool Is_flagged()
        {
            return is_placed;
        }
    }
}
