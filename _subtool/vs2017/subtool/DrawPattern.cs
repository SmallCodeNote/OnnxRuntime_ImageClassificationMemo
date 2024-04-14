using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;


namespace subtool
{
    public static class DrawPattern
    {
        public static void drawStripe(Bitmap bitmap, Color c1, Color c2)
        {
            Random rdm = new Random(DateTime.Now.Millisecond);

            int rectWidth = (int)(bitmap.Width / (20.0 + 10.0 * rdm.NextDouble()));
            int rectHeight = bitmap.Height;
            int xInterval = rectWidth * 2;
            int yInterval = rectHeight;

            Graphics g = Graphics.FromImage(bitmap);

            g.Clear(c1);

            for (int x = (int)(xInterval * (rdm.NextDouble() - 0.5)); x < bitmap.Width; x += xInterval)
            {
                for (int y = 0; y < bitmap.Height; y += yInterval)
                {
                    g.FillRectangle(new SolidBrush(c2), x, y, rectWidth, rectHeight);
                }
            }
        }

        public static void drawBuble(Bitmap bitmap, Color c1)
        {
            Random rdm = new Random(DateTime.Now.Millisecond);

            int diameter = (int)(bitmap.Width / (5.0 + 5.0 * rdm.NextDouble()));
            int x = (int)(rdm.NextDouble() * (bitmap.Width - diameter));
            int y = (int)(rdm.NextDouble() * (bitmap.Height - diameter));

            Graphics g = Graphics.FromImage(bitmap);

            using (Brush b = new SolidBrush(c1))
            {
                g.FillEllipse(b,x,y,diameter,diameter);
            }

        }
    }
}
