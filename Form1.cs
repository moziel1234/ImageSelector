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

namespace ImageSelector
{
    public partial class Form1 : Form
    {
        string imagePath = @"C:\Users\mosheo\Desktop\ItalyImages";
        string imagePath2 = @"C:\Users\mosheo\Desktop\ItalyImages\yifat_images";

        string albumPath = @"C:\Users\mosheo\Desktop\ItalyImages\album";
        string garbagePath = @"C:\Users\mosheo\Desktop\ItalyImages\garbage";
        string specialPath = @"C:\Users\mosheo\Desktop\ItalyImages\special";

        List<string> paths;
        int ind = 0;

        public Form1()
        {
            InitializeComponent();

            string[] arr1 = Directory.GetFiles(imagePath, "*.jpg");
            string[] arr2 = Directory.GetFiles(imagePath2, "*.jpg");

            paths = arr1.Concat(arr2).ToList();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Maximized;
            DisplayImage();
        }

        private void buttonRotate_Click(object sender, EventArgs e)
        {
            Image flipImage = pictureBox1.Image;
            flipImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
            pictureBox1.Image = flipImage;
        }

        private void buttonRight_Click(object sender, EventArgs e)
        {
            ind++;
            DisplayImage();
        }

        private void buttonLeft_Click(object sender, EventArgs e)
        {
            ind--;
            DisplayImage();
        }

        

        private void DisplayImage()
        {
            if (paths.Count == 0)
            {
                buttonAdd.Enabled = false;
                buttonLeft.Enabled = false;
                buttonRight.Enabled = false;
                buttonRotate.Enabled = false;
                buttonSpecial.Enabled = false;
                buttonRemove.Enabled = false;
            }
            else
            {
                if (ind < 0)
                    ind = paths.Count - 1;

                if (ind == paths.Count)
                    ind = 0;
                try
                {
                    labelIndNumber.Text = string.Format("{0}/{1}", ind, paths.Count().ToString());
                    using (FileStream fs = new FileStream(paths[ind], FileMode.Open, FileAccess.Read))
                    {
                        pictureBox1.Image = Image.FromStream(fs);
                        fs.Close();
                    }
                    DisplaySmallImage(pictureBox2, ind+1);
                    DisplaySmallImage(pictureBox3, ind + 2);
                    DisplaySmallImage(pictureBox4, ind + 3);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                } // ignore displayed errors
                this.Invalidate();


                
            }
            
        }

        private void DisplaySmallImage(PictureBox pictureBox, int index)
        {
            if (index > paths.Count() - 1)
                index = index - paths.Count();
            using (FileStream fs = new FileStream(paths[index], FileMode.Open, FileAccess.Read))
            {
                pictureBox.Image = Image.FromStream(fs);
                fs.Close();
            }
        }

        private void MoveFileAndDisplayImage(string targetPath)
        {
            string target = Path.Combine(targetPath, Path.GetFileName(paths[ind]));
            if (!File.Exists(target))
                File.Move(paths[ind], target);
            else
                File.Move(paths[ind], target.Replace(".jpg", "") +
                    DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".jpg"
                    );

            paths.RemoveAt(ind);
            DisplayImage();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            MoveFileAndDisplayImage(albumPath);
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            MoveFileAndDisplayImage(garbagePath);
        }

        private void buttonSpecial_Click(object sender, EventArgs e)
        {
            MoveFileAndDisplayImage(specialPath);            
        }
    }
}
