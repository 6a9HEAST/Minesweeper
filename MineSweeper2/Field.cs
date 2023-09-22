using System;
using System.Drawing;
using System.Windows.Forms;

namespace MineSweeper2
{
    [Serializable]
    public class Field
    {
        public Cell[,] cells_of_field;
        public int width = 0, height = 0, bombs;

        public void Generate(MainForm mainform)
        {
            int x, y;
            

            switch (MainForm.difficulty)
            {
                case 0:
                    width = 9;
                    height = 9;
                    bombs = 10;
                    mainform.cells_remaining = 71;
                    mainform.button1.Location= new Point(((MainForm.CELL_SIZE * width + 30 + 10) / 2 - 55), 15+40-25);
                    mainform.LeaderBoardButton.Location = new Point(((MainForm.CELL_SIZE * width + 30 + 10) / 2 +10), 15 + 40 - 25);
                    mainform.ClientSize = new System.Drawing.Size(MainForm.CELL_SIZE*width+30+10, MainForm.CELL_SIZE * height + 30 + 10+92);//400;492
                    break;
                case 1:
                    width = 16;
                    height = 16;
                    bombs = 40;
                    mainform.cells_remaining = 16 * 16 - 40;
                    mainform.button1.Location = new Point(((MainForm.CELL_SIZE * width + 30 + 10) / 2 - 55), 15 + 40 - 25);
                    mainform.LeaderBoardButton.Location = new Point(((MainForm.CELL_SIZE * width + 30 + 10) / 2 + 10), 15 + 40 - 25);
                    mainform.ClientSize = new System.Drawing.Size(MainForm.CELL_SIZE * width + 30 + 10, MainForm.CELL_SIZE * height + 30 + 10 + 92);//400;492
                    break;
                case 2:
                    width = 30;
                    height = 16;
                    bombs = 99;
                    mainform.cells_remaining = 30 * 16 - 99;
                    mainform.button1.Location = new Point(((MainForm.CELL_SIZE * width + 30 + 10) / 2 - 55), 15 + 40 - 25);
                    mainform.LeaderBoardButton.Location = new Point(((MainForm.CELL_SIZE * width + 30 + 10) / 2 + 10), 15 + 40 - 25);
                    mainform.ClientSize = new System.Drawing.Size(MainForm.CELL_SIZE * width + 30 + 10, MainForm.CELL_SIZE * height + 30 + 10 + 92);//400;492
                    break;
            } //на основе сложности выбирает размер поля

            cells_of_field = new Cell[width, height];

           for (int i = 0; i < width; i++)
              for (int j = 0; j < height; j++)
                   cells_of_field[i, j] = new Cell();

            Random rand = new Random();
            int temporary_bombs = bombs;

            while (temporary_bombs > 0) //расставляет бомбы
            {
                x = rand.Next(0, width - 1);
                y = rand.Next(0, height - 1);
                if (cells_of_field[x, y].cell_object == null) 
                {
                    
                        cells_of_field[x, y].cell_object = new Bomb();
                        temporary_bombs--;
                }
                
            }

            for (int i = 0; i < height; i++) //запись в пустые клетки количества бомб вокруг них
                for (int j = 0; j < width; j++)
                    if (cells_of_field[j, i].cell_object == null) cells_of_field[j, i].cell_object = new Number(Count_bombs_around(j, i));
            Draw(mainform);
            mainform.bomb_counter.set_bombs(bombs);
            mainform.timer = new Timer(mainform,width);

        }

        public void Draw(MainForm mainform) //РАМКА У ПОЛЯ
        {
            mainform.bm = new Bitmap(mainform.pictureBox1.Width, mainform.pictureBox1.Height);
            mainform.gr = Graphics.FromImage(mainform.bm);


            Pen darkgraypen = new Pen(Color.Gray, 1);
            SolidBrush darkgraybrush = new SolidBrush(Color.Gray);
            SolidBrush brightgraybrush = new SolidBrush(Color.Silver);


            PointF[] darkgraypoints =
            {
                new PointF(15,110), new PointF(15, 110+10+height*MainForm.CELL_SIZE), new PointF(20, 110+10+height*MainForm.CELL_SIZE-5),new PointF(20, 115),new PointF(20+MainForm.CELL_SIZE*width, 115),new PointF(25+MainForm.CELL_SIZE*width, 110)//,new PointF(15,110)
            };


            PointF[] brightgraypoints =
            {
              new PointF(15, 110+10+height*MainForm.CELL_SIZE), new PointF(20, 110+10+height*MainForm.CELL_SIZE-5),new PointF(20+MainForm.CELL_SIZE*width, 115+height*MainForm.CELL_SIZE),new PointF(20+MainForm.CELL_SIZE*width,115),new PointF(25+MainForm.CELL_SIZE*width,110),new PointF(25+MainForm.CELL_SIZE*width,120+height*MainForm.CELL_SIZE)
            };


            mainform.gr.FillPolygon(darkgraybrush, darkgraypoints);
            mainform.gr.FillPolygon(brightgraybrush, brightgraypoints);
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    cells_of_field[j, i].Draw(mainform, j, i);
            mainform.Draw(width);

            mainform.pictureBox1.Image = mainform.bm;
        }

        private int Count_bombs_around(int x, int y)
        {
            int counter = 0;
            for (int i = y - 1; i <= y + 1; i++)
                for (int j = x - 1; j <= x + 1; j++)
                    if ((j >= 0) && (j < width) && (i >= 0) && (i < height) && ((j != x) || (i != y)))
                        if (cells_of_field[j, i].cell_object!=null)
                        if (cells_of_field[j, i].cell_object.ContainsBomb()) counter++;
            return counter;
        }

        private int Count_flags_around(int x,int y)
        {
            int counter = 0;
            for (int i = y - 1; i <= y + 1; i++)
                for (int j = x - 1; j <= x + 1; j++)
                    if ((j >= 0) && (j < width) && (i >= 0) && (i < height) && ((j != x) || (i != y)))
                        if (cells_of_field[j, i].flag.Is_flagged()) counter++;
            return counter;
        }

        private void Open_all_bombs(MainForm mainform)
        {
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    if (cells_of_field[i, j].cell_object.ContainsBomb()) cells_of_field[i, j].is_opened = true;
                        

                    else if (cells_of_field[i, j].flag.Is_flagged()) cells_of_field[i, j].is_opened = true;
                    cells_of_field[i, j].Draw(mainform, i, j);
                }
        }

        private void open_cells_around(MainForm mainform,int x,int y, MouseButtons mousebuttons) 
        {
            for (int i = y - 1; i <= y + 1; i++)
                for (int j = x - 1; j <= x + 1; j++)
                    if ((j >= 0) && (j < width) && (i >= 0) && (i < height) && ((j != x) || (i != y)))
                        if (!cells_of_field[j, i].flag.Is_flagged()) open_cell(mainform,j,i,mousebuttons);
        }

        public void open_cell(MainForm mainform,int x,int y, MouseButtons mousebuttons)
        {
            if (cells_of_field[x, y].is_opened == false)

                if ((mousebuttons == MouseButtons.Right)&& (!cells_of_field[x,y].is_opened)) //Правый клик на закрытую клетку
                {
                    if (cells_of_field[x, y].flag.Is_flagged()) mainform.bomb_counter.count_up();
                    else mainform.bomb_counter.count_down();
                    cells_of_field[x, y].flag.Place_flag();
                    cells_of_field[x, y].Draw(mainform, x, y);
                }
                else if ((mousebuttons == MouseButtons.Left)&&(!cells_of_field[x, y].flag.Is_flagged()))//Левый клик
                    if (!cells_of_field[x, y].cell_object.ContainsBomb())//Если без бомбы
                    {
                        cells_of_field[x, y].is_opened = true;
                        cells_of_field[x, y].Draw(mainform, x, y);
                        if (Count_flags_around(x,y)== cells_of_field[x, y].cell_object.ReturnValue()) open_cells_around(mainform, x, y,mousebuttons);
                        mainform.cells_remaining -= 1;
                        if (mainform.cells_remaining == 0) mainform.win_the_game();

                        if (cells_of_field[x, y].cell_object.ReturnValue() == 0) //открытие соседних клеток
                            for (int i = y - 1; i <= y + 1; i++)
                                for (int j = x - 1; j <= x + 1; j++)
                                    if ((j >= 0) && (j < width) && (i >= 0) && (i < height) && ((j != x) || (i != y)))
                                        if ((!cells_of_field[j, i].is_opened) && (mousebuttons != MouseButtons.Right)) open_cell(mainform, j, i, mousebuttons);
                    }
                    else //Если с бомбой
                    {
                        mainform.button1.BackgroundImage = mainform.MenuButtonDead;
                        mainform.GAME_ENDED = true;
                        cells_of_field[x, y].is_opened = true;
                        cells_of_field[x, y].cell_object.Ended_game();
                        Open_all_bombs(mainform);
                    }

           
        }
    }
}


//9x9 16x16 16x30
//10    40    99
//размер клетки 30x30
//по бокам 25 пикселей пространства
//на верх оставляется 80 пикселей
//координаты рассчитывать относительно размера picturebox
//шрифт Franclin Gothic Demi
