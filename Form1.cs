using Lesson_50_WF_5_v1.Properties;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Globalization;

namespace Lesson_50_WF_5_v1
{
    public partial class Form1 : Form
    {
        // размеры лабиринта в ячейках 16х16 пикселей
        int columns = 20;
        int rows = 20;

        int pictureSize = 16; // ширина и высота одной ячейки
        string mazeMedalsOnForm, medalsPlayer, lost;

        Labirint l; // ссылка на логику всего происходящего в лабиринте

        Button button1;
        Button button2;
        Button button3;

        //string txtMsgBox = "It will be a new game";

        public Form1()
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            InitializeComponent();
            Options();
            StartGame();
            timer1.Start();
        }

        public void Options()
        {
            //Text = "Maze";
            BackColor = Color.FromArgb(255, 92, 118, 137);

            // размеры клиентской области формы (того участка, на котором размещаются ЭУ)
            ClientSize = new Size(columns * pictureSize + 700, rows * pictureSize + statusStrip1.Height);

            StartPosition = FormStartPosition.CenterScreen;

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();

            // 
            // button1
            // 
            button1.BackgroundImage = Properties.Resources.greatbritain;
            button1.BackgroundImageLayout = ImageLayout.Stretch;
            resources.ApplyResources(button1, "button1");
            button1.Name = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Location = new Point(columns * pictureSize + 20, 20);
            button1.Size = new Size(105, 105);
            button1.Parent = this;
            button1.TabStop = false;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.BackgroundImage = Properties.Resources.ukraine;
            button2.BackgroundImageLayout = ImageLayout.Stretch;
            resources.ApplyResources(button2, "button2");
            button2.Name = "button2";
            button2.UseVisualStyleBackColor = true;
            button2.Location = new Point(columns * pictureSize + button1.Size.Width + 30, 20);
            button2.Size = new Size(105, 105);
            button2.Parent = this;
            button2.TabStop = false;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.BackgroundImage = Properties.Resources.poland;
            button3.BackgroundImageLayout = ImageLayout.Stretch;
            resources.ApplyResources(button3, "button3");
            button3.Name = "button3";
            button3.UseVisualStyleBackColor = true;
            button3.Location = new Point(columns * pictureSize + button1.Size.Width + button2.Size.Width + 40, 20);
            button3.Size = new Size(105, 105);
            button3.Parent = this;
            button3.TabStop = false;
            button3.Click += button3_Click;
        }



        public void StartGame()
        {
            l = new Labirint(this, columns, rows);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)                // хід вправо
            {
                // проверка на то, свободна ли ячейка справа
                if (l.objects[l.CharY, l.CharX + 1].type == MazeObject.MazeObjectType.MEDAL) // проверяем ячейку правее на 1 позицию, является ли она медалькою
                {
                    SwapHall();         // зміна поточної клітинки на hall
                    l.CharX++;
                    l.countCharMedal++;
                    SwapChar();         // зміна сусідньої клітинки на char
                }
                else if (l.objects[l.CharY, l.CharX + 1].type == MazeObject.MazeObjectType.ENEMY) // проверяем ячейку правее на 1 позицию, является ли она ворогом
                {
                    SwapHall();         // зміна поточної клітинки на hall
                    l.CharX++;
                    l.health -= 20;     // зустріч з ворогом - мінус 20 % здоров'я
                    SwapChar();         // зміна сусідньої клітинки на char
                }
                else if (l.objects[l.CharY, l.CharX + 1].type == MazeObject.MazeObjectType.HEALTH) // проверяем ячейку правее на 1 позицию, является ли она ліками
                {
                    if (l.health < 100)     // якщо здоров'я 100 % - ліки не дозволяються
                    {
                        SwapHall();         // зміна поточної клітинки на hall
                        l.CharX++;
                        if (l.health + 5 > 100)    // приймання ліків - плюс 5 % здоров'я
                            l.health = 100;
                        else
                            l.health += 5;
                        SwapChar();         // зміна сусідньої клітинки на char
                    }
                }
                else if (l.objects[l.CharY, l.CharX + 1].type == MazeObject.MazeObjectType.HALL) // проверяем ячейку правее на 1 позицию, является ли она коридором
                {
                    SwapHall();         // зміна поточної клітинки на hall
                    l.CharX++;
                    SwapChar();         // зміна сусідньої клітинки на char
                }
                Text = Resources.Words.mazeMedalsOnForm + l.countFormMedal + Resources.Words.medalsPlayer + l.countCharMedal;
                CheckHealth();          // перевірка стану здоров'я
                CheckMedalWin();        // перевірка на виграш коли зібрані всі медальки
                CheckFinishWin();       // перевірка на виграш коли знайдено вихід
            }


            else if (e.KeyCode == Keys.Left)                // хід вліво
            {
                if (l.objects[l.CharY, l.CharX - 1].type == MazeObject.MazeObjectType.MEDAL) // проверяем ячейку левее на 1 позицию, является ли она медалькою
                {
                    SwapHall();         // зміна поточної клітинки на hall
                    l.CharX--;
                    l.countCharMedal++;
                    SwapChar();         // зміна сусідньої клітинки на char
                }
                else if (l.objects[l.CharY, l.CharX - 1].type == MazeObject.MazeObjectType.ENEMY) // проверяем ячейку правее на 1 позицию, является ли она ворогом
                {
                    SwapHall();         // зміна поточної клітинки на hall
                    l.CharX--;
                    l.health -= 20;
                    SwapChar();         // зміна сусідньої клітинки на char
                }
                else if (l.objects[l.CharY, l.CharX - 1].type == MazeObject.MazeObjectType.HEALTH) // проверяем ячейку правее на 1 позицию, является ли она ліками
                {
                    if (l.health < 100)
                    {
                        SwapHall();         // зміна поточної клітинки на hall
                        l.CharX--;
                        if (l.health + 5 > 100)
                            l.health = 100;
                        else
                            l.health += 5;
                        SwapChar();         // зміна сусідньої клітинки на char
                    }
                }
                else if (l.objects[l.CharY, l.CharX - 1].type == MazeObject.MazeObjectType.HALL) // проверяем ячейку левее на 1 позицию, является ли она коридором
                {
                    SwapHall();         // зміна поточної клітинки на hall
                    l.CharX--;
                    SwapChar();         // зміна сусідньої клітинки на char
                }
                Text = Resources.Words.mazeMedalsOnForm + l.countFormMedal + Resources.Words.medalsPlayer + l.countCharMedal; ;
                CheckHealth();
                CheckMedalWin();
                CheckFinishWin();
            }


            else if (e.KeyCode == Keys.Up)                // хід вверх
            {
                if (l.objects[l.CharY - 1, l.CharX].type == MazeObject.MazeObjectType.MEDAL) // проверяем ячейку вверх на 1 позицию, является ли она медалькою
                {
                    SwapHall();         // зміна поточної клітинки на hall
                    l.CharY--;
                    l.countCharMedal++;
                    SwapChar();         // зміна сусідньої клітинки на char
                }
                else if (l.objects[l.CharY - 1, l.CharX].type == MazeObject.MazeObjectType.ENEMY) // проверяем ячейку правее на 1 позицию, является ли она ворогом
                {
                    SwapHall();         // зміна поточної клітинки на hall
                    l.CharY--;
                    l.health -= 20;
                    SwapChar();         // зміна сусідньої клітинки на char
                }
                else if (l.objects[l.CharY - 1, l.CharX].type == MazeObject.MazeObjectType.HEALTH) // проверяем ячейку правее на 1 позицию, является ли она ліками
                {
                    if (l.health < 100)
                    {
                        SwapHall();         // зміна поточної клітинки на hall
                        l.CharY--;
                        if (l.health + 5 > 100)
                            l.health = 100;
                        else
                            l.health += 5;
                        SwapChar();         // зміна сусідньої клітинки на char
                    }
                }
                else if (l.objects[l.CharY - 1, l.CharX].type == MazeObject.MazeObjectType.HALL) // проверяем ячейку вверх на 1 позицию, является ли она коридором
                {
                    SwapHall();         // зміна поточної клітинки на hall
                    l.CharY--;
                    SwapChar();         // зміна сусідньої клітинки на char
                }
                Text = Resources.Words.mazeMedalsOnForm + l.countFormMedal + Resources.Words.medalsPlayer + l.countCharMedal; ;
                CheckHealth();
                CheckMedalWin();
                CheckFinishWin();
            }


            else if (e.KeyCode == Keys.Down)                // хід вниз
            {
                if (l.objects[l.CharY + 1, l.CharX].type == MazeObject.MazeObjectType.MEDAL) // проверяем ячейку вниз на 1 позицию, является ли она медалькою
                {
                    SwapHall();         // зміна поточної клітинки на hall
                    l.CharY++;
                    l.countCharMedal++;
                    SwapChar();         // зміна сусідньої клітинки на char
                }
                else if (l.objects[l.CharY + 1, l.CharX].type == MazeObject.MazeObjectType.ENEMY) // проверяем ячейку правее на 1 позицию, является ли она ворогом
                {
                    SwapHall();         // зміна поточної клітинки на hall
                    l.CharY++;
                    l.health -= 20;
                    SwapChar();         // зміна сусідньої клітинки на char
                }
                else if (l.objects[l.CharY + 1, l.CharX].type == MazeObject.MazeObjectType.HEALTH) // проверяем ячейку правее на 1 позицию, является ли она ліками
                {
                    if (l.health < 100)
                    {
                        SwapHall();         // зміна поточної клітинки на hall
                        l.CharY++;
                        if (l.health + 5 > 100)
                            l.health = 100;
                        else
                            l.health += 5;
                        SwapChar();         // зміна сусідньої клітинки на char
                    }
                }
                else if (l.objects[l.CharY + 1, l.CharX].type == MazeObject.MazeObjectType.HALL) // проверяем ячейку вниз на 1 позицию, является ли она коридором
                {
                    SwapHall();         // зміна поточної клітинки на hall
                    l.CharY++;
                    SwapChar();         // зміна сусідньої клітинки на char
                }
                Text = Resources.Words.mazeMedalsOnForm + l.countFormMedal + Resources.Words.medalsPlayer + l.countCharMedal; ;
                CheckHealth();
                CheckMedalWin();
                CheckFinishWin();
            }
            if (l.steps == 1)               // запуск секундоміра гри
                l.timeStart = System.DateTime.Now;
        }

        private void CheckHealth()           // перевірка чи в героя залишилося здоров'я
        {
            if (l.health <= 0)
            {
                toolStripStatusLabel2.Text = Resources.Words.tSSLabel2_text + l.steps + ",";
                timer1.Stop();
                MessageBox.Show(Resources.Words.lost);
                this.Close();
            }
        }
        private void CheckFinishWin()           // перевірка чи досяг герой виходу з лабіринта
        {
            if ((l.CharX == 0 || l.CharX == columns - 1 || l.CharY == 0 || l.CharY == rows - 1) // якщо досягнуто краю лабіринту, і ця клітинка - не стартова
                && (l.CharX != l.StartX && l.CharY != l.StartY))
            {
                toolStripStatusLabel2.Text = Resources.Words.tSSLabel2_text + l.steps + ",";
                timer1.Stop();
                MessageBox.Show(Resources.Words.win_exit);
                this.Close();
            }
        }
        private void CheckMedalWin()           // перевірка чи зібрав герой всі медальки
        {
            if (l.countFormMedal != 0 && l.countCharMedal == l.countFormMedal)
            {
                toolStripStatusLabel2.Text = Resources.Words.tSSLabel2_text + l.steps + ",";
                timer1.Stop();
                MessageBox.Show(Resources.Words.win_medals);
                this.Close();
            }
        }
        private void SwapHall()                 // метод для зміни поточної клітинки на hall - замість char
        {
            l.objects[l.CharY, l.CharX].texture = MazeObject.images[(int)MazeObject.MazeObjectType.HALL]; // hall
            l.images[l.CharY, l.CharX].BackgroundImage = l.objects[l.CharY, l.CharX].texture;
            l.objects[l.CharY, l.CharX].type = MazeObject.MazeObjectType.HALL;
            l.steps++;
        }
        private void SwapChar()                 // метод для зміни поточної клітинки на char
        {
            l.objects[l.CharY, l.CharX].texture = MazeObject.images[(int)MazeObject.MazeObjectType.CHAR]; // character
            l.images[l.CharY, l.CharX].BackgroundImage = l.objects[l.CharY, l.CharX].texture;
            l.objects[l.CharY, l.CharX].type = MazeObject.MazeObjectType.CHAR;
        }

        private void timer1_Tick(object sender, System.EventArgs e)         // подія таймеру - відображення інформація statusStrip 
        {
            toolStripStatusLabel1.Text = Resources.Words.tSSLabel1_text + l.health + "%,";
            toolStripStatusLabel2.Text = Resources.Words.tSSLabel2_text + l.steps + ",";
            if (l.steps >= 1)
                toolStripStatusLabel3.Text = Resources.Words.tSSLabel3_text1 + (System.DateTime.Now - l.timeStart).Seconds + Resources.Words.tSSLabel3_text2;
        }

        private void button1_Click(object sender, EventArgs e)  // eng
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            MessageBox.Show(Resources.Words.txtMsgBox);
            this.Controls.Clear();
            InitializeComponent();
            Options();
            StartGame();
            timer1.Start();
            pictureBox1.Image = Properties.Resources.map_gb;
        }

        private void button2_Click(object sender, EventArgs e)  // ukr
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("uk-UA");
            MessageBox.Show(Resources.Words.txtMsgBox);
            this.Controls.Clear();
            InitializeComponent();
            Options();
            StartGame();
            timer1.Start();
            pictureBox1.Image = Properties.Resources.map_ukraine;
        }

        private void button3_Click(object sender, EventArgs e)  // pl
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("pl-PL");
            MessageBox.Show(Resources.Words.txtMsgBox);
            Controls.Clear();
            InitializeComponent();
            Options();
            StartGame();
            timer1.Start();
            pictureBox1.Image = Properties.Resources.map_poland;

        }

        // метод - обробник події, для запобігання переміщення фокусу з форми на елементи форми
        private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            var keys = new[] { Keys.Left, Keys.Right, Keys.Up, Keys.Down };
            if (keys.Contains(e.KeyData))
                e.IsInputKey = true;
        }
    }
}