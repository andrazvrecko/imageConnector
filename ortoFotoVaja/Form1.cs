using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ortoFotoVaja
{



    public partial class Form1 : Form
    {
        public string path1 = "";
        public string path2 = "";
        public List<Point> pic1;
        public List<Point> pic2;
        Bitmap transformedImg;
        Bitmap bitmap;
        Bitmap img1;
        List<int> newPosPix;


        public Form1()
        {
            InitializeComponent();
            pic1 = new List<Point>();
            pic2 = new List<Point>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image files(*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            DialogResult result = openFileDialog1.ShowDialog();
            path1 = openFileDialog1.FileName;
            Console.WriteLine(path1);
            pictureBox1.Image = Image.FromFile(path1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image files(*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            DialogResult result = openFileDialog1.ShowDialog();
            path2 = openFileDialog1.FileName;
            Console.WriteLine(path2);
            pictureBox2.Image = Image.FromFile(path2);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (pic1.Count < 2)
            {
                MouseEventArgs me = (MouseEventArgs)e;

                Point myPts = new Point(me.X, me.Y);
                pic1.Add(myPts);
                float cropX = me.X;
                float cropY = me.Y;

                Pen cropPen = new Pen(System.Drawing.Color.Red, 2);
                if (pic1.Count == 2)
                    cropPen.Color = System.Drawing.Color.Green;
                cropPen.DashStyle = DashStyle.DashDotDot;
                Cursor = Cursors.Cross;
                pictureBox1.CreateGraphics().DrawEllipse(cropPen, cropX, cropY, 8, 8);
                if (pic1.Count == 2)
                    pictureBox1.CreateGraphics().FillEllipse(new System.Drawing.SolidBrush(System.Drawing.Color.Green), new Rectangle((int)cropX, (int)cropY, 8, 8));
                if (pic1.Count == 2)
                {
                    cropPen.Color = System.Drawing.Color.Red;
                    pictureBox1.CreateGraphics().DrawLine(cropPen, pic1[0], pic1[1]);
                }
            }

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (pic2.Count < 2)
            {
                MouseEventArgs me = (MouseEventArgs)e;
                Point myPts = new Point(me.X, me.Y);
                pic2.Add(myPts);
                float cropX = me.X;
                float cropY = me.Y;

                Pen cropPen = new Pen(System.Drawing.Color.Red, 2);
                if (pic2.Count == 2)
                    cropPen.Color = System.Drawing.Color.Green;
                cropPen.DashStyle = DashStyle.DashDotDot;
                Cursor = Cursors.Cross;
                pictureBox2.CreateGraphics().DrawEllipse(cropPen, cropX, cropY, 8, 8);
                if (pic2.Count == 2)
                    pictureBox2.CreateGraphics().FillEllipse(new System.Drawing.SolidBrush(System.Drawing.Color.Green), new Rectangle((int)cropX, (int)cropY, 8, 8));
                if (pic2.Count == 2)
                {
                    cropPen.Color = System.Drawing.Color.Red;
                    pictureBox2.CreateGraphics().DrawLine(cropPen, pic2[0], pic2[1]);
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        public List<int> calculateNewPos(Bitmap img1, Bitmap img2, int imgpos1, int imgpos2, int mainImg) {
            List<int> newPos =  new List<int>();
            List<Point> tockeMain;
            List<Point> tockeSec;
            if(mainImg == 1)
            {
                tockeMain = pic1;
                tockeSec = pic2;
            }
            else
            {
                tockeMain = pic2;
                tockeSec = pic1;
            }

            int posX = imgpos1 + tockeMain[0].X;
            int posY = imgpos2 + tockeMain[0].Y;

            newPos.Add(posX - tockeSec[0].X);
            newPos.Add(posY - tockeSec[0].Y);

            System.Windows.Vector v1 = new System.Windows.Vector(tockeMain[0].X, tockeMain[0].Y);
            System.Windows.Vector v12 = new System.Windows.Vector(tockeMain[1].X, tockeMain[1].Y);
            System.Windows.Vector v2 = new System.Windows.Vector(tockeSec[0].X, tockeSec[0].Y);
            System.Windows.Vector v22 = new System.Windows.Vector(tockeSec[1].X, tockeSec[1].Y);

            System.Windows.Vector f1 = v22 - v2;
            System.Windows.Vector f2 = v12 - v1;
            newPos.Add((int)System.Windows.Vector.AngleBetween(f1, f2));
            Console.WriteLine("Angle: " + newPos[2]);
            //newPos.Add(Math.Atan2(tocke.Y - a.Y, b.X - a.X));

            return newPos;
        }

        private void GetPointBounds(PointF[] points,
        out float xmin, out float xmax,
        out float ymin, out float ymax)
        {
            xmin = points[0].X;
            xmax = xmin;
            ymin = points[0].Y;
            ymax = ymin;
            foreach (PointF point in points)
            {
                if (xmin > point.X) xmin = point.X;
                if (xmax < point.X) xmax = point.X;
                if (ymin > point.Y) ymin = point.Y;
                if (ymax < point.Y) ymax = point.Y;

              


            }

            if (ymin < 0)
            {
                ymax = ymax + Math.Abs(ymin);
                //ymin = 0;
            }
            else if (ymin > 0)
            {
                ymax = ymax - ymin;
                //ymin = 0;
            }

            if (xmin < 0)
            {
                xmax = xmax + Math.Abs(xmin);
                //xmin = 0;
            }
            else if (xmin > 0)
            {
                xmax = xmax - xmin;
                //xmin = 0;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(path1 == "" || path2 == "" || pic1.Count != 2 || pic2.Count != 2)
            {
                label2.Text = "Select 2 images and place points!!";
                return;
            }
            //Combine pictures
            pictureBox3.Visible = true;
            img1 = new Bitmap(path1);
            Bitmap img2 = new Bitmap(path2);
            img1.MakeTransparent();
            img2.MakeTransparent();
            img1.SetResolution(96.0F, 96.0F);
            img2.SetResolution(96.0F, 96.0F);


            int mainImg;
            //Find bigger picture and make it main
            if(img1.Height >= img2.Height && img1.Width >= img2.Width)
            {
                mainImg = 1;
            }
            else
            {
                mainImg = 2;               
            }

            bitmap = new Bitmap(pictureBox3.Width, pictureBox3.Height);
            bitmap.SetResolution(96.0F, 96.0F);



            //Draw first image
            using (var g = Graphics.FromImage(bitmap))
            {
                if (mainImg == 1)
                    g.DrawImage(img1, (pictureBox3.Width - img1.Width) / 2, (pictureBox3.Height - img1.Height) / 2, img1.Width, img1.Height);
                else
                    g.DrawImage(img2, (pictureBox3.Width - img2.Width) / 2, (pictureBox3.Height - img2.Height) / 2, img2.Width, img2.Height);
            }

            pictureBox3.Image = bitmap;
            //Calculate starting (X,Y) for second image
            
            if (mainImg == 1)
                newPosPix = calculateNewPos(img1, img2, (pictureBox3.Width - img1.Width) / 2, (pictureBox3.Height - img1.Height) / 2, mainImg);
            else
                newPosPix = calculateNewPos(img2, img1, (pictureBox3.Width - img2.Width) / 2, (pictureBox3.Height - img2.Height) / 2, mainImg);


            //Rotation
            if (mainImg == 1)
            {

                int angle = newPosPix[2];
                int scale = 1;

                Matrix rotate_at_center = new Matrix();

                Matrix rotate_at_origin = new Matrix();
                rotate_at_origin.Rotate((float)angle);
                PointF[] points =
                {
                    new PointF(0, 0),
                    new PointF(img2.Width, 0),
                    new PointF(img2.Width, img2.Height),
                    new PointF(0, img2.Height),
                };
                rotate_at_origin.TransformPoints(points);
                float xmin, xmax, ymin, ymax;
                GetPointBounds(points, out xmin, out xmax,
                    out ymin, out ymax);

                PointF[] newPts =
                {
                    new PointF(pic2[0].X, pic2[0].Y),
                    new PointF(pic2[1].X, pic2[1].Y),
                };



                int wid = (int)Math.Round(xmax);
                int hgt = (int)Math.Round(ymax);

                transformedImg = new Bitmap(wid, hgt);

                rotate_at_center.RotateAt(angle,
                    new PointF(wid / 2f, hgt / 2f));


                //Calculate new points
                Console.WriteLine("OLD POINTS: " + newPts[0].X + " " + newPts[0].Y);
                Console.WriteLine("OLD POINTS2: " + newPts[1].X + " " + newPts[1].Y);
                rotate_at_origin.TransformPoints(newPts);

                newPts[0].X += Math.Abs(xmin);
                newPts[0].Y += Math.Abs(ymin);
                newPts[1].X += Math.Abs(xmin);
                newPts[1].Y += Math.Abs(ymin);

                Console.WriteLine("NEW POINTS: " + newPts[0].X + " " + newPts[0].Y);
                Console.WriteLine("NEW POINTS2: " + newPts[1].X + " " + newPts[1].Y);
                Console.WriteLine("MIN: " + ymin + " " + xmin);

                //swap original points with new points
                pic2.Clear();
                pic2.Add(new Point((int)newPts[0].X, (int)newPts[0].Y));
                pic2.Add(new Point((int)newPts[1].X, (int)newPts[1].Y));


                using (Graphics g = Graphics.FromImage(transformedImg))
                {
                    // Use smooth image interpolation.
                    g.InterpolationMode = InterpolationMode.High;
                    //g.Clear(Color.LightBlue);
                    g.Transform = rotate_at_center;

                    int x = (wid - img2.Width) / 2;
                    int y = (hgt - img2.Height) / 2;
                    g.DrawImage(img2, x, y, img2.Width, img2.Height);
                    //g.DrawImage(img1, wid - img1.Width, hgt - img1.Height, img1.Width, img1.Height);
                }

                if (mainImg == 1)
                    newPosPix = calculateNewPos(img1, img2, (pictureBox3.Width - img1.Width) / 2, (pictureBox3.Height - img1.Height) / 2, mainImg);
                else
                    newPosPix = calculateNewPos(img2, img1, (pictureBox3.Width - img2.Width) / 2, (pictureBox3.Height - img2.Height) / 2, mainImg);
                /*int angle = newPosPix[2];
                int scale = 1;
                transformedImg = new Bitmap(2000, 2000);
                transformedImg.SetResolution(96.0F, 96.0F);

                using (Graphics g = Graphics.FromImage(transformedImg))
                {

                    g.TranslateTransform(pic2[0].X, pic2[0].Y);
                    g.RotateTransform(angle);
                    g.ScaleTransform(scale, scale);
                    g.TranslateTransform(-pic2[0].X, -pic2[0].Y);
                    g.DrawImage(img2, 0, 0, img2.Width*scale, img2.Height*scale);

                }*/
            }
            else
            {

                int angle = newPosPix[2];
                int scale = 1;

                Matrix rotate_at_center = new Matrix();
                
                Matrix rotate_at_origin = new Matrix();
                rotate_at_origin.Rotate((float)angle);
                PointF[] points =
                {
                    new PointF(0, 0),
                    new PointF(img1.Width, 0),
                    new PointF(img1.Width, img1.Height),
                    new PointF(0, img1.Height),
                };
                rotate_at_origin.TransformPoints(points);
                float xmin, xmax, ymin, ymax;
                GetPointBounds(points, out xmin, out xmax,
                    out ymin, out ymax);

                PointF[] newPts =
                {
                    new PointF(pic1[0].X, pic1[0].Y),
                    new PointF(pic1[1].X, pic1[1].Y),
                };

               

                int wid = (int)Math.Round(xmax);
                int hgt = (int)Math.Round(ymax);

                transformedImg = new Bitmap(wid, hgt);

                rotate_at_center.RotateAt(angle,
                    new PointF(wid / 2f, hgt / 2f));

                
                //Calculate new points
                Console.WriteLine("OLD POINTS: " + newPts[0].X + " " + newPts[0].Y);
                Console.WriteLine("OLD POINTS2: " + newPts[1].X + " " + newPts[1].Y);
                rotate_at_origin.TransformPoints(newPts);

                newPts[0].X += Math.Abs(xmin);
                newPts[0].Y += Math.Abs(ymin);
                newPts[1].X += Math.Abs(xmin);
                newPts[1].Y += Math.Abs(ymin);

                Console.WriteLine("NEW POINTS: " + newPts[0].X + " " + newPts[0].Y);
                Console.WriteLine("NEW POINTS2: " + newPts[1].X + " " + newPts[1].Y);
                Console.WriteLine("MIN: " + ymin + " " + xmin);

                //swap original points with new points
                pic1.Clear();
                pic1.Add(new Point((int)newPts[0].X, (int)newPts[0].Y));
                pic1.Add(new Point((int)newPts[1].X, (int)newPts[1].Y));


                using (Graphics g = Graphics.FromImage(transformedImg))
                {
                    // Use smooth image interpolation.
                    g.InterpolationMode = InterpolationMode.High;
                    //g.Clear(Color.LightBlue);
                    g.Transform = rotate_at_center;

                    int x = (wid - img1.Width) / 2;
                    int y = (hgt - img1.Height) / 2;
                    g.DrawImage(img1, x, y, img1.Width, img1.Height);
                    //g.DrawImage(img1, wid - img1.Width, hgt - img1.Height, img1.Width, img1.Height);
                }

                if (mainImg == 1)
                    newPosPix = calculateNewPos(img1, img2, (pictureBox3.Width - img1.Width) / 2, (pictureBox3.Height - img1.Height) / 2, mainImg);
                else
                    newPosPix = calculateNewPos(img2, img1, (pictureBox3.Width - img2.Width) / 2, (pictureBox3.Height - img2.Height) / 2, mainImg);

            }

            using (var g = Graphics.FromImage(bitmap))
            {
                g.DrawImage(transformedImg, newPosPix[0], newPosPix[1], transformedImg.Width, transformedImg.Height);
                //g.DrawImage(transformedImg, 0, 0, transformedImg.Width, transformedImg.Height);
            }

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            Point myPts = new Point(me.X, me.Y);
            Console.WriteLine(myPts.ToString());
        }

        private void button4_Click(object sender, EventArgs e)
        {
           
        }
    }
}
