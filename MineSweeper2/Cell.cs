using System;
using System.Drawing;
using MineSweeper;

namespace MineSweeper2
{
    [Serializable]
    public class Cell
    {

        public bool is_opened = false;
        public Flag flag=new Flag();
        public CellObject cell_object;

        public Cell()
        {

        }

        public void Draw(MainForm mainform, int x, int y)
        {
            int X = x * 40 + 20, Y = y * 40 + 115;  //рассчет координат в пикселях
            SolidBrush dark_gray_brush = new SolidBrush(Color.Gray);
            SolidBrush bright_gray_brush = new SolidBrush(Color.Silver);
            SolidBrush background_brush = new SolidBrush(SystemColors.ControlDark);
            PointF[] backgroundpoints = { new PointF(X, Y), new PointF(X, Y + 40), new PointF(X + 40, Y + 40), new PointF(X + 40, Y) };
            Pen dark_line = new Pen(dark_gray_brush,2);
            

            switch (is_opened)
            {
                case false: //закрыта
                    {    
                        PointF[] darkgray_points = {new PointF(X,Y),new PointF(X,Y+40),new PointF(X+5,Y+35),new PointF(X+5,Y+5),new PointF(X+35,Y+5),new PointF(X+40,Y)};
                        PointF[] bright_gray_points ={new PointF(X,Y+40),new PointF(X+5,Y+35),new PointF(X+35,Y+35),new PointF(X+35,Y+5),new PointF(X+40,Y),new PointF(X+40,Y+40)};

                        if (!flag.TryDraw(mainform, X, Y))
                        {
                            mainform.gr.FillPolygon(background_brush, backgroundpoints);
                            mainform.gr.FillPolygon(dark_gray_brush, bright_gray_points);
                            mainform.gr.FillPolygon(bright_gray_brush, darkgray_points);
                        }
                        break;
                    }
                case true when (!cell_object.ContainsBomb()): //открыта, без бомбы
                    {
                        mainform.gr.FillPolygon(background_brush, backgroundpoints);
                        mainform.gr.DrawLine(dark_line, new PointF(X+1, Y + 40), new PointF(X+1, Y));
                        mainform.gr.DrawLine(dark_line, new PointF(X, Y+1), new PointF(X + 40, Y+1));
                        if (flag.Is_flagged()) flag.TryDraw(mainform,X,Y,true);
                        else
                        if (cell_object.ReturnValue() != 0) cell_object.Draw(mainform,X,Y);
                        break;
                    }
                case true when (cell_object.ContainsBomb()):
                    {
                        mainform.gr.FillPolygon(background_brush, backgroundpoints);
                        mainform.gr.DrawLine(dark_line, new PointF(X + 1, Y + 40), new PointF(X + 1, Y));
                        mainform.gr.DrawLine(dark_line, new PointF(X, Y + 1), new PointF(X + 40, Y + 1));
                        cell_object.Draw(mainform,X,Y);
                        break;
                    }
            }
            mainform.pictureBox1.Image = mainform.bm;
        }
    }
}

