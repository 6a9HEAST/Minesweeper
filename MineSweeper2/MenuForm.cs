using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineSweeper2
{
    public partial class MenuForm : Form
    {
        private int temp_difficulty=MainForm.difficulty;
        public MenuForm(MainForm mainform)
        {
            InitializeComponent();
            TitleTextBox.BackgroundImage= Image.FromFile("MenuText.png");
            this.Location = new Point(mainform.Location.X + (mainform.Width / 2) - (this.Width / 2), mainform.Location.Y + 50);
            switch (MainForm.difficulty)
            {
                case 0: this.BombEasy.Visible= true; break;
                case 1: this.BombMid.Visible= true; break;
                case 2:this.BombHard.Visible= true; break;

            }
        }

        private void Exit_click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Enter_Click(object sender, EventArgs e)
        {
            MainForm.difficulty = temp_difficulty;
            MainForm.entered = true;
            this.Close();
        }

        private void Easybox_Click(object sender, EventArgs e)
        {
            temp_difficulty = 0;
            this.BombEasy.Visible = true;
            this.BombMid.Visible = false;
            this.BombHard.Visible = false;
        }

        private void Midbox_Click(object sender, EventArgs e)
        {
            temp_difficulty = 1;
            this.BombEasy.Visible = false;
            this.BombMid.Visible = true;
            this.BombHard.Visible = false;
        }

        private void Hardbox_Click(object sender, EventArgs e)
        {
            temp_difficulty = 2;
            this.BombEasy.Visible = false;
            this.BombMid.Visible = false;
            this.BombHard.Visible = true;
        }
    }
}
