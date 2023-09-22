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
    public class Timer
    {
        private int time=0;
        private readonly MainForm mainform;
        private readonly int size;
        public Timer(MainForm mainform,int size) { 
            this.mainform = mainform;
            this.size = size;
            Draw();
        }

        void Draw()
        {
            int first_n = time / 100;
            int second_n = (time % 100) / 10;
            int third_n = time % 10;
            mainform.gr.DrawImage(Image.FromFile("Numbers\\" + first_n + ".png"),  size * 40 - 20-54, 30, 27, 50);
            mainform.gr.DrawImage(Image.FromFile("Numbers\\" + second_n + ".png"),  size * 40 - 20-27, 30, 27, 50);
            mainform.gr.DrawImage(Image.FromFile("Numbers\\" + third_n + ".png"), size*40-20, 30, 27, 50);
            mainform.pictureBox1.Image = mainform.bm;
        }

        public void Time_up()
        {
            time +=1;
            Draw();
        }

        public int Return_Time() { return time; }



    }
}
