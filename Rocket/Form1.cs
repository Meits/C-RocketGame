using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rocket
{
    public partial class Form1 : Form
    {
        List<Task> taskList;
        Random rand;
        int count = 0;
        public Form1()
        {
            InitializeComponent();
            taskList = new List<Task>();
            rand = new Random();

            this.KeyDown += new KeyEventHandler(Form1_KeyPress);
            this.Height = 725;

            pictureBox1.Image = Image.FromFile("img/rocket.png");
            pictureBox1.Top = 300;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            foreach(var item in Controls)
            {
                if(item is PictureBox && (item as PictureBox) != pictureBox1)
                {
                    (item as PictureBox).Image = Image.FromFile("img/meteor.png");
                    (item as PictureBox).SizeMode = PictureBoxSizeMode.Zoom;
                    (item as PictureBox).Location = new Point(this.Width, (item as PictureBox).Location.Y);
                    taskList.Add(new Task(() => meteorStart((item as PictureBox))));
                }
            }

            DialogResult dialogResult = MessageBox.Show("Начать игру?", "Старт", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                foreach(Task t in taskList)
                {
                    t.Start();
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                this.Close();
            }
        }

        private void meteorStart(PictureBox pictureBox)
        {
            int left = this.Width;
            while(true)
            {
                left = pictureBox.Location.X;
                pictureBox.Invoke(new Action(() => {
                    pictureBox.Location = new Point(pictureBox.Location.X - rand.Next(25,100), pictureBox.Location.Y);
                }));
                Thread.Sleep(rand.Next(200));
                if(left < 0)
                {
               
                    pictureBox.Invoke(new Action(() => {
                        pictureBox.Location = new Point(pictureBox.Location.X + this.Width, pictureBox.Location.Y);
                    }));
                    count++;
                }
                if(pictureBox.Bounds.IntersectsWith(pictureBox1.Bounds))
                {
                    MessageBox.Show("Вы проиграли. Ваш счет - " + count);
                    this.Close();
                }
            }
        }

        private void Form1_KeyPress(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if(pictureBox1.Top - 150 >= 0)
                    {
                        pictureBox1.Top -= 150;
                    }
                    
                    break;
                case Keys.Down:
                    if (pictureBox1.Top + 150 < this.Height)
                    {
                        pictureBox1.Top += 150;
                    }
                    
                    break;
            }
        }
    }
}
