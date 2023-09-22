using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using MineSweeper;
using System.Diagnostics;

namespace MineSweeper2
{
    
    public partial class MainForm : Form
    {
        public Field field = new Field();
        public BombCounter bomb_counter;
        public Timer timer;
        MenuForm menu;
        Leaderboards leaderboard;
        static public int difficulty=0;
        static public bool entered = false;
        public Graphics gr;
        public Bitmap bm;
        public const int CELL_SIZE = 40;
        public bool GAME_ENDED=false;
        public bool game_started = true;
        public int cells_remaining = 0;
        public int easy_games=0;
        public int medium_games = 0;
        public int hard_games = 0;
        public int easy_wins = 0;
        public int medium_wins = 0;
        public int hard_wins = 0;
        public int easy_time = 0;
        public int medium_time = 0;
        public int hard_time = 0;


        public Image MenuButton = Image.FromFile("MenuButtton.png");
        public Image MenuButtonWin = Image.FromFile("MenuButtonWin.png");
        public Image MenuButtonDead = Image.FromFile("MenuButtonDead.png");
        public Image MenuButtonSurprised = Image.FromFile("MenuButtonSurprised.png");


        public static void Main()
        {
            Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
        public MainForm()
        {
            InitializeComponent();
            bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            gr = Graphics.FromImage(bm);
            bomb_counter = new BombCounter(this);
            field.Generate(this);
            GAME_ENDED = false;
            game_started = true;
            button1.BackgroundImage = MenuButton;
            button1.BackgroundImage = MenuButton;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            menu = new MenuForm(this);
            menu.ShowDialog();

            if (entered)
            {
                bomb_counter = new BombCounter(this);
                field.Generate(this);
                GAME_ENDED = false;
                game_started = true;
                button1.BackgroundImage = MenuButton;
    }
            entered = false;
        }

        public void win_the_game()// ПОБЕДА!!!!!!!!!!!!!!!!!!!
        {
            switch (difficulty)
            {
                case 0: 
                    easy_wins += 1;
                    if (easy_time == 0) easy_time = timer.Return_Time();
                    else if (timer.Return_Time()<easy_time) easy_time=timer.Return_Time();
                    break;
                case 1: 
                    medium_wins += 1;
                    if (medium_time == 0) medium_time = timer.Return_Time();
                    else if (timer.Return_Time() < medium_time) medium_time = timer.Return_Time();
                    break;
                case 2:
                    hard_wins += 1;
                    if (hard_time == 0) hard_time = timer.Return_Time();
                    else if (timer.Return_Time() < hard_time) hard_time = timer.Return_Time();
                    break;
            }
            for (int i = 0; i < field.width; i++)
                for (int j = 0; j < field.height; j++) 
                    if (field.cells_of_field[i,j].cell_object.ContainsBomb()) 
                    { 
                        field.cells_of_field[i, j].flag.Place_flag(true);
                        field.cells_of_field[i, j].Draw(this,i,j);
                        GAME_ENDED = true;
                        button1.BackgroundImage = MenuButtonWin;
                        bomb_counter.set_to_zero();
                    }
        }

        public void Draw(int size) // РАМКА СВЕРХУ
        {
            SolidBrush darkgraybrush = new SolidBrush(Color.Gray);
            SolidBrush brightgraybrush = new SolidBrush(Color.Silver);
            PointF[] darkgraypoints =
            {
                new PointF(15,15),new PointF(15,15+80),new PointF(15+5,15+80-5),new PointF(15+5,15+5),new PointF(15+size*CELL_SIZE+5,15+5),new PointF(15+size*CELL_SIZE+10,15)
            };


            PointF[] brightgraypoints =
            {
                    new PointF(15,15+80),new PointF(15+5,15+80-5),new PointF(15+size*CELL_SIZE+5,15+80-5),new PointF(15+size*CELL_SIZE+5,15+5),new PointF(15+size*CELL_SIZE+10,15),new PointF(15+size*CELL_SIZE+10,15+80)
            };

            gr.FillPolygon(darkgraybrush, darkgraypoints);
            gr.FillPolygon(brightgraybrush, brightgraypoints);
            pictureBox1.Image = bm;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (((e.X >= 20) && (e.X <= field.width * 40 + 20)) && ((e.Y >= 115) && (e.Y <= field.height * 40 + 115))&&(!GAME_ENDED)) 
            {
                int x = (e.X - 20) / 40, y = (e.Y - 115) / 40;
                if(MouseButtons!=MouseButtons.Right) button1.BackgroundImage = MenuButtonSurprised;
                while (game_started)
                {
                    if ((field.cells_of_field[x, y].cell_object.ReturnValue() != 0) || (field.cells_of_field[x, y].cell_object.ContainsBomb())) field.Generate(this);
                    else
                    {
                        switch (difficulty)
                        {
                            case 0: easy_games += 1;break;
                            case 1: medium_games+=1;break;
                            case 2:hard_games +=1;break;
                        }
                        game_started = false;
                    }
                }
                field.open_cell(this,x, y,MouseButtons);
            }


        }
        private void timer1_Tick(object sender, EventArgs e)
        {
                if ((!game_started) && (!GAME_ENDED))
                    timer.Time_up();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (!GAME_ENDED) 
            button1.BackgroundImage = MenuButton;
        }

        //Сохранение данных в бинарный файл при закрытии программы
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            WriteIntArrayToFile( "save.bin");

            //В ФАЙЛЕ:
            // Всего игр 0, время легкого 1, время среднего 2, время сложного 3
            // игр на леком 4, побед на леком 5
            //игр на среднем 6, побед на среднем 7
            //игр на сложном 8, побед на сложном 9

        }

        public int[] ReadIntArrayFromFile(string filePath)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {
                int[] intArray = new int[10];

                for (int i = 0; i < 10; i++)
                {
                    intArray[i] = reader.ReadInt32(); // Читаем каждый объект int
                }
                return intArray;
            }
        }

        public void WriteIntArrayToFile(string filePath)
        {
            FileInfo fileinfo = new FileInfo("save.bin");
            int[] intArray = new int[10];
            if (fileinfo.Exists)
            {
                intArray = ReadIntArrayFromFile("save.bin");
                intArray[0] += easy_games + hard_games + medium_games;
                if ((intArray[1]==0)||((intArray[1] > easy_time)&&(easy_time!=0))) intArray[1] = easy_time;
                if ((intArray[2] == 0)|| ((intArray[2] > medium_time)&&(medium_time!=0))) intArray[2] = medium_time;
                if ((intArray[3] == 0) || ((intArray[3] > hard_time) && (hard_time != 0))) intArray[3] = hard_time;
                intArray[4] += easy_games;
                intArray[5] += easy_wins;
                intArray[6] += medium_games;
                intArray[7] += medium_wins;
                intArray[8] += hard_games;
                intArray[9] += hard_wins;
            }
            else
            {
                intArray[0] = easy_games + hard_games + medium_games;
                intArray[1] = easy_time;
                intArray[2] = medium_time;
                intArray[3] = hard_time;
                intArray[4] = easy_games;
                intArray[5] = easy_wins;
                intArray[6] = medium_games;
                intArray[7] = medium_wins;
                intArray[8] = hard_games;
                intArray[9] = hard_wins;
            }
            using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {
                foreach (int num in intArray)
                {
                    writer.Write(num); // Записываем каждый объект int
                }
            }
         easy_games = 0;
         medium_games = 0;
         hard_games = 0;
         easy_wins = 0;
         medium_wins = 0;
         hard_wins = 0;
         easy_time = 0;
         medium_time = 0;
         hard_time = 0;

        }

        private void LeaderBoardButton_Click(object sender, EventArgs e)
        {
            WriteIntArrayToFile("save.bin");
            leaderboard = new Leaderboards(this);
            leaderboard.ShowDialog();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Process.Start("documentation.txt");
        }
    }
}