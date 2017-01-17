using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace digits
{
    public partial class Form1 : Form
    {
        const int size = 81;
        int[] X1 = new int[size]; // 9x9
        int[] X2 = new int[size];
        int[] X3 = new int[size];
        int[] X4 = new int[size];
        int[] X5 = new int[size];
        //int[] X6 = new int[size];
        //int[] X7 = new int[size];
        //int[] X8 = new int[size];
        //int[] X9 = new int[size];

        int[] Y = new int[size]; // зашумленный вход
        int[,] W = new int[size, size];
        int[] draws = new int[size];
        bool mode = true;
        
        System.IO.StreamReader sr = new System.IO.StreamReader("ideal.txt");
        System.IO.StreamReader sr1 = new System.IO.StreamReader("input.txt");
        System.IO.StreamWriter sw = new System.IO.StreamWriter("output.txt");
        public Form1()
        {
            InitializeComponent();
            label1.Text = "";
            read(sr, X1);
            read(sr, X2);
            read(sr, X3);
            read(sr, X4);
            read(sr, X5);
            //read(sr, X6);
            //read(sr, X7);
            //read(sr, X8);
            //read(sr, X9);
            //read(sr1, Y);
            //train();
            //recognize();
            
        }

        void view(string s)
        {
            label1.Text = s;
        }

        void read(System.IO.StreamReader reader, int[] mass)
        {
            for (int i = 0; i < size; i++)
            {
                char a = (char)reader.Read();
                if (a == '1')
                    mass[i] = 1;
                else mass[i] = -1;
            }
        }

        int f(int s)
        {
            if (s > 0)
                return 1;
            else return( -1);
        }

        void copy(int[] mas1, int[] mas2)
        {
            for (int i = 0; i < size; i++)
                mas2[i] = mas1[i];
        }

        bool is_equal(int[] mas1, int[] mas2)
        {
            int err = 0;
            for (int i = 0; i < size; i++)
            {
                if (mas1[i] != mas2[i])
                    err++;
                if (err > 7)
                    return false;
            }
            return true;
        }

        bool is_equal_exact(int[] mas1, int[] mas2)
        {
            for (int i = 0; i < size; i++)
            {
                if (mas1[i] != mas2[i])
                    return false;
            }
            return true;
        }

        bool train()
        {
            
            // инициализация весов
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i == j)
                        W[i, j] = 0;
                    else W[i, j] = X1[i] * X1[j] + X2[i] * X2[j] + X3[i] * X3[j] + X4[i] * X4[j] + X5[i] * X5[j];// + X6[i] * X6[j] + X7[i] * X7[j] + X8[i] * X8[j] + X9[i] * X9[j];
                }
            }
            // рассчитываем новое состояние нейронов y
            int count_iter = 0;
            while(count_iter < 300)
            {
                
                for (int i = 0; i < size; i++)
                {
                    if (Y[i] == -1)
                        sw.Write('0');
                    else sw.Write('1');
                    if ((i + 1) % 9 == 0)
                        sw.WriteLine();
                }
                sw.WriteLine();
                int s;
                int[] copy_Y = new int[size];
                for (int i = 0; i < size; i++)
                    copy_Y[i] = Y[i];
                for (int j = 0; j < size; j++)
                {
                    s = 0;
                    for (int i = 0; i < size; i++)
                    {
                        s += W[j, i] * Y[i];
                    }
                    Y[j] = f(s);
                }
                count_iter++;
                // изменились ли значения выходов Y?
                if (is_equal_exact(Y, copy_Y))
                {
                    //button2.Text = count_iter.ToString();
                    break;
                }
            }
            if (is_equal_exact(Y, X1))
            {
                view("Один");
                return true;
            }
            if (is_equal(Y, X2))
            {
                view("Два");
                return true;
            }
            if (is_equal(Y, X3))
            {
                view("Три");
                return true;
            }
            if (is_equal(Y, X4))
            {
                view("Четыре");
                return true;
            }
            if (is_equal(Y, X5))
            {
                view("Пять");
                return true;
            }
           /* if (is_equal(Y, X6))
            {
                view("Шесть");
                return true;
            }
            if (is_equal(Y, X7))
            {
                view("Семь");
                return true;
            }
            if (is_equal(Y, X8))
            {
                view("Восемь");
                return true;
            }
            if (is_equal(Y, X9))
            {
                view("Девять");
                return true;
            }*/
            view("Не могу распознать");
            return false;

        }

        private void recognize()
        {
            for (int i = 0; i < size; i++)
            {
                if (Y[i] == -1)
                    sw.Write('0');
                else sw.Write('1');
                if ((i + 1) % 9 == 0)
                    sw.WriteLine();
            }
                    
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sr.Close();
            sr1.Close();
            sw.Close();
            this.Close();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            int i = e.X / 20;
            int j = e.Y / 20;
            if(mode)
                draws[j * 9 + i] = 1;
            else draws[j * 9 + i] = -1;
            panel1.Refresh();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            for (int i = 0; i < size; i++)
            {
                if (draws[i] == 1)
                {
                    int vert = i % 9;
                    int gor = i / 9;
                    g.FillRectangle(System.Drawing.Brushes.Black, vert * 20, gor * 20, 20, 20);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label1.Text = "";
            for (int i = 0; i < size; i++)
            {
                if (draws[i] == 1)
                    Y[i] = 1;
                else Y[i] = -1;
            }
            if (train())
            {
                recognize();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mode = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mode = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            read(sr1, Y);
            train();
            recognize();
        }
    }
}


/*
000000111
000000111
000001111
000011011
000110011
001100011
000000011
000000011
000000011

000111000
111000111
000000111
000000111
000000111
000011100
011100001
110000001
111111110

001110001
110001110
000001110
000001100
001111000
000001110
000001111
100001111
111111101

100000111
100000111
100000111
100000111
111111111
111111110
000000110
000000110
000000111

111111111
100000001
100000001
100000001
111111000
000001110
000001111
100001111
111111100

011111110
100000001
100000001
100000000
100000000
111111110
100000001
100000001
011111110

011111111
100000001
100000001
000000110
000001100
011111110
000110000
000100000
000100000

001111100
010000010
010000010
010000010
001111100
010000010
010000010
010000010
001111100

001111111
010000001
010000001
010000001
001111111
000000001
000000001
010000001
001111111
*/
