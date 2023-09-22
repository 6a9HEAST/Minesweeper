using MineSweeper2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineSweeper
{
    public partial class Leaderboards : Form
    {
        public Leaderboards(MainForm mainform)
        {
            InitializeComponent();
            this.Location = new Point(mainform.Location.X + (mainform.Width / 2) - (this.Width / 2), mainform.Location.Y + 50);
            int[] intArray = new int[10];
            try
            {
                intArray = mainform.ReadIntArrayFromFile("save.bin");
                RecNumEasy.Text = intArray[1].ToString();
                RecNumMid.Text = intArray[2].ToString();
                RecNumHard.Text = intArray[3].ToString();
                GamesPlayed.Text = intArray[0].ToString();
                GPWins.Text = (intArray[5] + intArray[7] + intArray[9]).ToString();
                if (intArray[0] != 0) GPWinRate.Text= ((int)((float)(intArray[5] + intArray[7] + intArray[9]) / (float)intArray[0]*100)).ToString()+"%";
                EsPlayed.Text = intArray[4].ToString();
                EsWins.Text = intArray[5].ToString();
                if (intArray[4]!=0) EsWinRate.Text = ((int)((float)intArray[5] / (float)intArray[4]*100)).ToString()+"%";
                MidPlayed.Text = intArray[6].ToString();
                MidWins.Text = intArray[7].ToString();
                if (intArray[6] != 0)  MidPercent.Text = ((int)((float)intArray[7] / (float)intArray[6] * 100)).ToString() + "%";
                HardPlayed.Text = intArray[8].ToString();
                HardWins.Text = intArray[9].ToString();
                if (intArray[8] != 0) HardPercent.Text = ((int)((float)intArray[9] / (float)intArray[8] * 100)).ToString() + "%";

            }
            catch { }
            //В ФАЙЛЕ:
            // Всего игр 0, время легкого 1, время среднего 2, время сложного 3
            // игр на леком 4, побед на леком 5
            //игр на среднем 6, побед на среднем 7
            //игр на сложном 8, побед на сложном 9

        }

        private void Enter_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
