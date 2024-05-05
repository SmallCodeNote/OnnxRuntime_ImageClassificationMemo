﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using WinFormStringCnvClass;


namespace subtool
{

    public partial class Form1 : Form
    {
        string thisExeDirPath;
        public Form1()
        {
            InitializeComponent();
            thisExeDirPath = Path.GetDirectoryName(Application.ExecutablePath);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "TEXT|*.txt";
            if (false && ofd.ShowDialog() == DialogResult.OK)
            {
                WinFormStringCnv.setControlFromString(this, File.ReadAllText(ofd.FileName));
            }
            else
            {
                string paramFilename = Path.Combine(thisExeDirPath, "_param.txt");
                if (File.Exists(paramFilename))
                {
                    WinFormStringCnv.setControlFromString(this, File.ReadAllText(paramFilename));
                }
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            string FormContents = WinFormStringCnv.ToString(this);

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "TEXT|*.txt";

            if (false && sfd.ShowDialog() == DialogResult.OK)
            {

                File.WriteAllText(sfd.FileName, FormContents);
            }
            else
            {
                string paramFilename = Path.Combine(thisExeDirPath, "_param.txt");
                File.WriteAllText(paramFilename, FormContents);
                File.WriteAllText(paramFilename, FormContents);
                File.WriteAllText(paramFilename, FormContents);
            }

        }

        private void PictureBoxUpdate(PictureBox p, Bitmap img)
        {
            if (p.Image != null) p.Image.Dispose();
            p.Image = img;
        }

        private void button_CreateA_Click(object sender, EventArgs e)
        {
            Random rdm = new Random();

            if (!Directory.Exists(textBox_CreateDirPath.Text)) { Directory.CreateDirectory(textBox_CreateDirPath.Text); }

            int imageWidth = int.Parse(textBox_Width.Text);
            int imageHeight = int.Parse(textBox_Height.Text);
            int imageCount = int.Parse(textBox_ImageCount.Text);

            string classDirPath = "";

            classDirPath = Path.Combine(textBox_CreateDirPath.Text, "1");
            if (!Directory.Exists(classDirPath)) { Directory.CreateDirectory(classDirPath); }

            for (int i = 0; i < imageCount; i++)
            {
                Bitmap bitmap = new Bitmap(imageWidth, imageHeight, PixelFormat.Format24bppRgb);
                DrawPattern.drawVerticalStripe(bitmap, Color.White, Color.Black, rdm);
                PictureBoxUpdate(pictureBox1, bitmap);
                bitmap.Save(Path.Combine(classDirPath, i.ToString("0000") + ".jpg"), ImageFormat.Jpeg);
            }

            classDirPath = Path.Combine(textBox_CreateDirPath.Text, "2");
            if (!Directory.Exists(classDirPath)) { Directory.CreateDirectory(classDirPath); }

            for (int i = 0; i < imageCount; i++)
            {
                Bitmap bitmap = new Bitmap(imageWidth, imageHeight, PixelFormat.Format24bppRgb);

                DrawPattern.drawVerticalStripe(bitmap, Color.White, Color.Black, rdm);
                DrawPattern.drawCircle(bitmap, Color.White, rdm);
                PictureBoxUpdate(pictureBox1, bitmap);

                bitmap.Save(Path.Combine(classDirPath, i.ToString("0000") + ".jpg"), ImageFormat.Jpeg);
            }

        }

        private void button_CreateB_Click(object sender, EventArgs e)
        {
            Random rdm = new Random();
            if (!Directory.Exists(textBox_CreateDirPath.Text)) { Directory.CreateDirectory(textBox_CreateDirPath.Text); }

            int imageWidth = int.Parse(textBox_Width.Text);
            int imageHeight = int.Parse(textBox_Height.Text);
            int imageCount = int.Parse(textBox_ImageCount.Text);

            string classDirPath = "";

            classDirPath = Path.Combine(textBox_CreateDirPath.Text, "1");
            if (!Directory.Exists(classDirPath)) { Directory.CreateDirectory(classDirPath); }

            for (int i = 0; i < imageCount; i++)
            {
                Bitmap bitmap = new Bitmap(imageWidth, imageHeight, PixelFormat.Format24bppRgb);
                DrawPattern.drawBackground(bitmap, Color.White);
                DrawPattern.drawCircle(bitmap, Color.Red, rdm);
                PictureBoxUpdate(pictureBox1, bitmap);
                bitmap.Save(Path.Combine(classDirPath, i.ToString("0000") + ".jpg"), ImageFormat.Jpeg);
            }

            classDirPath = Path.Combine(textBox_CreateDirPath.Text, "2");
            if (!Directory.Exists(classDirPath)) { Directory.CreateDirectory(classDirPath); }

            for (int i = 0; i < imageCount; i++)
            {
                Bitmap bitmap = new Bitmap(imageWidth, imageHeight, PixelFormat.Format24bppRgb);
                DrawPattern.drawBackground(bitmap, Color.White);
                DrawPattern.drawTriangle(bitmap, Color.Red, rdm);
                PictureBoxUpdate(pictureBox1, bitmap);

                bitmap.Save(Path.Combine(classDirPath, i.ToString("0000") + ".jpg"), ImageFormat.Jpeg);
            }

            classDirPath = Path.Combine(textBox_CreateDirPath.Text, "3");
            if (!Directory.Exists(classDirPath)) { Directory.CreateDirectory(classDirPath); }

            for (int i = 0; i < imageCount; i++)
            {
                Bitmap bitmap = new Bitmap(imageWidth, imageHeight, PixelFormat.Format24bppRgb);
                DrawPattern.drawBackground(bitmap, Color.White);
                DrawPattern.drawSquare(bitmap, Color.Red, rdm);
                PictureBoxUpdate(pictureBox1, bitmap);

                bitmap.Save(Path.Combine(classDirPath, i.ToString("0000") + ".jpg"), ImageFormat.Jpeg);
            }


        }

        private void button_CreateC_Click(object sender, EventArgs e)
        {
            Random rdm = new Random();
            if (!Directory.Exists(textBox_CreateDirPath.Text)) { Directory.CreateDirectory(textBox_CreateDirPath.Text); }

            int imageWidth = int.Parse(textBox_Width.Text);
            int imageHeight = int.Parse(textBox_Height.Text);
            int imageCount = int.Parse(textBox_ImageCount.Text);

            string classDirPath = "";

            classDirPath = Path.Combine(textBox_CreateDirPath.Text, "1");
            if (!Directory.Exists(classDirPath)) { Directory.CreateDirectory(classDirPath); }

            for (int i = 0; i < imageCount; i++)
            {
                Bitmap bitmap = new Bitmap(imageWidth, imageHeight, PixelFormat.Format24bppRgb);
                DrawPattern.drawVerticalStripe(bitmap, Color.White, Color.Black, rdm);
                PictureBoxUpdate(pictureBox1, bitmap);
                bitmap.Save(Path.Combine(classDirPath, i.ToString("0000") + ".jpg"), ImageFormat.Jpeg);
            }

            classDirPath = Path.Combine(textBox_CreateDirPath.Text, "2");
            if (!Directory.Exists(classDirPath)) { Directory.CreateDirectory(classDirPath); }

            for (int i = 0; i < imageCount; i++)
            {
                Bitmap bitmap = new Bitmap(imageWidth, imageHeight, PixelFormat.Format24bppRgb);
                DrawPattern.drawHorizontalStripe(bitmap, Color.White, Color.Black, rdm);
                PictureBoxUpdate(pictureBox1, bitmap);
                bitmap.Save(Path.Combine(classDirPath, i.ToString("0000") + ".jpg"), ImageFormat.Jpeg);
            }

        }

        private void button_CreateD_Click(object sender, EventArgs e)
        {
            Random rdm = new Random();
            if (!Directory.Exists(textBox_CreateDirPath.Text)) { Directory.CreateDirectory(textBox_CreateDirPath.Text); }

            int imageWidth = int.Parse(textBox_Width.Text);
            int imageHeight = int.Parse(textBox_Height.Text);
            int imageCount = int.Parse(textBox_ImageCount.Text);

            string classDirPath = "";

            classDirPath = Path.Combine(textBox_CreateDirPath.Text, "1");
            if (!Directory.Exists(classDirPath)) { Directory.CreateDirectory(classDirPath); }

            for (int i = 0; i < imageCount; i++)
            {
                Bitmap bitmap = new Bitmap(imageWidth, imageHeight, PixelFormat.Format24bppRgb);
                DrawPattern.drawBackground(bitmap, Color.White);
                DrawPattern.drawRectangle(bitmap, Color.Black, 1, rdm);
                PictureBoxUpdate(pictureBox1, bitmap);
                bitmap.Save(Path.Combine(classDirPath, i.ToString("0000") + ".jpg"), ImageFormat.Jpeg);
            }

            classDirPath = Path.Combine(textBox_CreateDirPath.Text, "2");
            if (!Directory.Exists(classDirPath)) { Directory.CreateDirectory(classDirPath); }

            for (int i = 0; i < imageCount; i++)
            {
                Bitmap bitmap = new Bitmap(imageWidth, imageHeight, PixelFormat.Format24bppRgb);
                DrawPattern.drawBackground(bitmap, Color.White);
                DrawPattern.drawRectangle(bitmap, Color.Black, 2, rdm);
                PictureBoxUpdate(pictureBox1, bitmap);

                bitmap.Save(Path.Combine(classDirPath, i.ToString("0000") + ".jpg"), ImageFormat.Jpeg);
            }

            classDirPath = Path.Combine(textBox_CreateDirPath.Text, "3");
            if (!Directory.Exists(classDirPath)) { Directory.CreateDirectory(classDirPath); }

            for (int i = 0; i < imageCount; i++)
            {
                Bitmap bitmap = new Bitmap(imageWidth, imageHeight, PixelFormat.Format24bppRgb);
                DrawPattern.drawBackground(bitmap, Color.White);
                DrawPattern.drawRectangle(bitmap, Color.Black, 4, rdm);
                PictureBoxUpdate(pictureBox1, bitmap);

                bitmap.Save(Path.Combine(classDirPath, i.ToString("0000") + ".jpg"), ImageFormat.Jpeg);
            }

        }

        private void button_CreateE_Click(object sender, EventArgs e)
        {
            Random rdm = new Random();
            if (!Directory.Exists(textBox_CreateDirPath.Text)) { Directory.CreateDirectory(textBox_CreateDirPath.Text); }

            int imageWidth = int.Parse(textBox_Width.Text);
            int imageHeight = int.Parse(textBox_Height.Text);
            int imageCount = int.Parse(textBox_ImageCount.Text);

            string classDirPath = "";

            classDirPath = Path.Combine(textBox_CreateDirPath.Text, "1");
            if (!Directory.Exists(classDirPath)) { Directory.CreateDirectory(classDirPath); }

            for (int i = 0; i < imageCount; i++)
            {
                Bitmap bitmap = new Bitmap(imageWidth, imageHeight, PixelFormat.Format24bppRgb);
                DrawPattern.drawBackground(bitmap, Color.White);
                DrawPattern.drawCircle(bitmap, Color.Red, rdm);
                PictureBoxUpdate(pictureBox1, bitmap);
                bitmap.Save(Path.Combine(classDirPath, i.ToString("0000") + ".jpg"), ImageFormat.Jpeg);
            }

            classDirPath = Path.Combine(textBox_CreateDirPath.Text, "2");
            if (!Directory.Exists(classDirPath)) { Directory.CreateDirectory(classDirPath); }

            for (int i = 0; i < imageCount; i++)
            {
                Bitmap bitmap = new Bitmap(imageWidth, imageHeight, PixelFormat.Format24bppRgb);
                DrawPattern.drawBackground(bitmap, Color.White);
                DrawPattern.drawCircle(bitmap, Color.YellowGreen, rdm);
                PictureBoxUpdate(pictureBox1, bitmap);
                bitmap.Save(Path.Combine(classDirPath, i.ToString("0000") + ".jpg"), ImageFormat.Jpeg);
            }

            classDirPath = Path.Combine(textBox_CreateDirPath.Text, "3");
            if (!Directory.Exists(classDirPath)) { Directory.CreateDirectory(classDirPath); }

            for (int i = 0; i < imageCount; i++)
            {
                Bitmap bitmap = new Bitmap(imageWidth, imageHeight, PixelFormat.Format24bppRgb);
                DrawPattern.drawBackground(bitmap, Color.White);
                DrawPattern.drawCircle(bitmap, Color.Blue, rdm);
                PictureBoxUpdate(pictureBox1, bitmap);
                bitmap.Save(Path.Combine(classDirPath, i.ToString("0000") + ".jpg"), ImageFormat.Jpeg);
            }

        }
    }
}
