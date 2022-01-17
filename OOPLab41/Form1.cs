using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOPLab41
{
    public partial class Form1 : Form
    {
        class Shape //базовый класс с виртуальными методами
        {
            virtual public void draw(PictureBox sender, Bitmap bmp, Graphics g)
            {
            }
            virtual public bool isChecked(MouseEventArgs e)
            {
                return false;
            }
            virtual public bool getF()
            {
                return false;
            }
            virtual public void slctT()
            {
            }
            virtual public void slctF()
            {
            }

        }
        class CCircle : Shape
        {
            private int x, y, r; //координаты и радиус
            private bool fl; //булевая переменная, показывающая, "выделен" объект или нет
            public CCircle(int _x, int _y, int _r) //констурктор с параметрами
            {
                x = _x;
                y = _y;
                r = _r;
                fl = true;
            }
            override public void draw(PictureBox sender, Bitmap bmp, Graphics g)
            {
                Rectangle rect = new Rectangle(x - r, y - r, r * 2, r * 2);
                Pen pen = new Pen(Color.Black);
                if (fl == true) //проверка на то, выделен ли объект
                {
                    pen.Color = Color.Red; //выделение
                }
                g.DrawEllipse(pen, rect);
                sender.Image = bmp;
            }
            override public bool isChecked(MouseEventArgs e) //проверка на то, нажат ли объект мышкой
            {
                if (((e.X - x) * (e.X - x) + (e.Y - y) * (e.Y - y)) <= (r * r))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            override public bool getF() //получение значения " выделенный/не выделенный" у объекта
            {
                return fl;
            }
            override public void slctT() //изменение значения f на true
            {
                fl = true;
            }
            override public void slctF() //изменение значения f на false
            {
                fl = false;
            }
        }
        class Storage
        {
            private Shape []stor;
            private int count,maxcount;
            public Storage(int _maxcount)
            {
                maxcount = _maxcount;
                count = 0;
                stor = new Shape[_maxcount];
                for (int i = 0; i < _maxcount; i++)
                    stor[i] = null;
            }
            public void addObj(Shape obj)
            {
                if (count >= maxcount)
                {
                    Array.Resize(ref stor, count + 1);
                    stor[count] = obj;
                    count++;
                    maxcount++;
                }
                else if (count == 0)
                {
                    stor[count] = obj;
                    count++;
                }
                else
                {
                    stor[count] = obj;
                    count++;
                    for (int i = 0; i < count - 1; i++)
                    {
                        stor[i].slctF();
                    }
                }
            }
            public void deleteObj(int ind)
            {
                stor[ind] = null;
                count--;
                for (int i = ind; i < count; i++)
                {
                    stor[i] = stor[i + 1];
                }
                stor[count] = null;
            }
            public void drawAll(PictureBox sender, Bitmap bmp, Graphics g)
            {
                g.Clear(Color.White); //очистка рисунка
                for (int i = 0; i < count; i++)
                {
                    if (stor[i] != null)
                    {
                        stor[i].draw(sender, bmp, g);
                    }
                }
                if (count == 0)
                {
                    sender.Image = bmp;
                }
            }
            public int getCount()
            {
                return count;
            }
            public void allObjUnselect()
            {
                for (int i = count - 1; i >= 0; i--)
                {
                    if (stor[i] != null)
                    {
                        if (stor[i].getF() == true)
                        {
                            stor[i].slctF();
                        }
                    }

                }
            }
            public void delWhenClicedDel()
            {
                for (int i = count - 1; i >= 0; i--)
                {
                    if (stor[i] != null)
                    {
                        if (stor[i].getF() == true)
                        {
                            deleteObj(i);
                        }
                    }
                }
            }
            public bool checkSelectNotCtrl(MouseEventArgs e)
            {
                for (int i = 0; i < count; i++)
                {
                    if (stor[i] != null)
                    {
                        if (stor[i].isChecked(e) == true)
                        {
                            allObjUnselect();
                            stor[i].slctT();
                            return true;
                        }
                    }
                }
                return false;
            }
            public bool checkSelectCtrl(MouseEventArgs e)
            {
                for (int i = 0; i < count; i++)
                {
                    if (stor[i] != null)
                    {
                        if (stor[i].isChecked(e) == true)
                        {
                            stor[i].slctT();
                            return true;
                        }
                    }
                }
                return false;
            }
        }
        Storage strg;
        Bitmap bmp;
        Graphics g;
        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
            strg = new Storage(100);
            bmp = new Bitmap(PictureBox1.Width, PictureBox1.Height);
            g = Graphics.FromImage(bmp);
        }

        private void PictureBox1_Click(object sender, MouseEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control) //если зажат Ctrl
            {
                Graphics g = Graphics.FromImage(bmp);
                PictureBox pb = (PictureBox)sender;
                if (strg.checkSelectCtrl(e) == false) //если мышью нажали на пустое место 
                {
                    strg.addObj(new CCircle(e.X, e.Y, 40));
                }
                else //если мышью нажали на объект на форме
                {
                    
                }
                this.Refresh();
            }
            else //если не зажат Ctrl
            {
                Graphics g = Graphics.FromImage(bmp);
                PictureBox pb = (PictureBox)sender;
                if (strg.checkSelectNotCtrl(e) == false) //если мышью нажали на пустое место 
                {
                    strg.addObj(new CCircle(e.X, e.Y, 40));
                }
                else //если мышью нажали на объект на форме
                {
                   
                }
                this.Refresh();
            }
            strg.drawAll(PictureBox1, bmp, g);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true; //для обработки нажатия клавиши
        }

        private void button1_Click(object sender, EventArgs e)
        {
            strg.delWhenClicedDel();
            strg.drawAll(PictureBox1, bmp, g);
        }
    }
}
