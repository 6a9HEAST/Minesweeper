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
    public class BombCounter
    {
        private int bombs;
        private MainForm mainform;

        public BombCounter(MainForm mainform)
        {
            this.mainform=mainform;
        }
        private void Draw()
        {
            //MessageBox.Show("Drawing");
            if (bombs>=0)
                {
                int second_n = (bombs % 100) / 10;
                int third_n = bombs % 10;
                mainform.gr.DrawImage(Image.FromFile("Numbers\\0.png"), 30, 30,27,50);
                mainform.gr.DrawImage(Image.FromFile("Numbers\\" + second_n + ".png"), 57, 30, 27, 50);
                mainform.gr.DrawImage(Image.FromFile("Numbers\\" + third_n + ".png"), 84, 30, 27, 50);
            }
            else
            {
                int second_n = (bombs*(-1) % 100) / 10;
                int third_n = bombs * (-1) % 10;
                mainform.gr.DrawImage(Image.FromFile("Numbers\\min.png"), 30, 30, 27, 50);
                mainform.gr.DrawImage(Image.FromFile("Numbers\\" + second_n + ".png"), 57, 30, 27, 50);
                mainform.gr.DrawImage(Image.FromFile("Numbers\\" + third_n + ".png"), 84, 30, 27, 50);
            }
        }

        public void set_bombs(int bombs)
        {
            this.bombs = bombs;
            //MessageBox.Show("Bombs setted to " + this.bombs);
            Draw();
        }
        public void count_up()
        {
            bombs++;
            Draw();
        }

        public void count_down()
        {
            bombs--;
            Draw();
        }

        public void set_to_zero()
        {
            bombs=0; Draw();    
        }
    }
}
