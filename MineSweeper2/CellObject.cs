using MineSweeper2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper2
{
    [Serializable]
    public abstract class CellObject
    {
        private bool contains_bomb = false;


        public abstract void Draw(MainForm mainform, int x, int y);
        public bool ContainsBomb() { return contains_bomb; }
        public void SetBomb() { contains_bomb = true; }
        public virtual void Ended_game() { }
        public virtual int ReturnValue() { return default(int); }
    }

    [Serializable]
    public class Number: CellObject
    {
        private int value = -1;

        public Number(int value)
        {
            this.value = value;
        }

        public override int ReturnValue()
        {
            return value;
        }

        public override void Draw(MainForm mainform, int x, int y)
        {
            mainform.gr.DrawImage(Image.FromFile("CellNumbers\\" + value + ".png"), x + 5, y + 5, 30, 30);
        }
    }
    [Serializable]
    public class Bomb:CellObject
    {
        public Bomb()
        {
            SetBomb();
        }

        private bool ended_game = false;

        public override void Draw(MainForm mainform, int x, int y)
        {
            SolidBrush red_brush = new SolidBrush(Color.Red);
            PointF[] backgroundpoints = { new PointF(x, y), new PointF(x, y + 40), new PointF(x + 40, y + 40), new PointF(x + 40, y) };
            if (ended_game) mainform.gr.FillPolygon(red_brush, backgroundpoints);
            mainform.gr.DrawImage(Image.FromFile("bomb.png"), x, y, 40, 40);
        }

        public override void Ended_game()
        {
            ended_game = true;
        }
    }
}
