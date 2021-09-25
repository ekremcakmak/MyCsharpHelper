using System;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Image = System.Drawing.Image;

public static class MyImageHelper
{

    public static Bitmap ImageOnImageMake(Bitmap bacgroundImage, Color bacgroundTrasparentColor,
            Bitmap pictureImage, Point pictureImageCordinat)
    {
        bacgroundImage.MakeTransparent(bacgroundTrasparentColor);
        Bitmap retnImage = new Bitmap(bacgroundImage.Width, bacgroundImage.Height);
        Graphics gr = System.Drawing.Graphics.FromImage(retnImage);
        gr.DrawImage(pictureImage, pictureImageCordinat);
        gr.DrawImage(bacgroundImage, new Point(0, 0));
        gr.Dispose();

        return retnImage;
    }


    public static void ImageResizeSize(string imagePath, string savePath, int newWidth, int newHeight)
    {
        int newW = newWidth;
        int newH = newHeight;

        Bitmap uploadedimage = new Bitmap(imagePath);

        decimal width = uploadedimage.Width;
        decimal height = uploadedimage.Height;

        decimal PersentW = (decimal)width / newW;
        decimal PersentH = (decimal)height / newH;

        int PossibleW, PossibleH;

        if (PersentW > PersentH)
        {
            PossibleW = newW;
            PossibleH = (int)(height / PersentW);
        }
        else
        {
            PossibleW = (int)(width / PersentH);
            PossibleH = newH;
        }

        System.Drawing.Bitmap DestImage = new System.Drawing.Bitmap(uploadedimage.Width, uploadedimage.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        System.Drawing.Graphics.FromImage(DestImage).DrawImage(System.Drawing.Image.FromStream(new System.Net.WebClient().OpenRead(imagePath)), new System.Drawing.Rectangle(0, 0, uploadedimage.Width, uploadedimage.Height), new System.Drawing.Rectangle(0, 0, uploadedimage.Width, uploadedimage.Height), System.Drawing.GraphicsUnit.Pixel);
        System.Drawing.Bitmap imgOutput = new System.Drawing.Bitmap(DestImage, PossibleW, PossibleH);

        Graphics myresizer;
        myresizer = Graphics.FromImage(imgOutput);
        myresizer.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        myresizer.DrawImage(DestImage, 0, 0, PossibleW, PossibleH);

        imgOutput.Save(savePath, ImageFormat.Jpeg);
    }

    public static Bitmap RotateCanvas(Bitmap image, float angle)
    {
        if (image == null) throw new ArgumentNullException("image");
        const double pi2 = Math.PI / 2.0;
        double oldWidth = (double)image.Width;
        double oldHeight = (double)image.Height;
        double theta = ((double)angle) * Math.PI / 180.0; double locked_theta = theta;
        while (locked_theta < 0.0) locked_theta += 2 * Math.PI;
        double newWidth, newHeight;
        int nWidth, nHeight;
        double adjacentTop, oppositeTop;
        double adjacentBottom, oppositeBottom;
        if ((locked_theta >= 0.0 && locked_theta < pi2) || (locked_theta >= Math.PI && locked_theta < (Math.PI + pi2)))
        {
            adjacentTop = Math.Abs(Math.Cos(locked_theta)) * oldWidth;
            oppositeTop = Math.Abs(Math.Sin(locked_theta)) * oldWidth;
            adjacentBottom = Math.Abs(Math.Cos(locked_theta)) * oldHeight;
            oppositeBottom = Math.Abs(Math.Sin(locked_theta)) * oldHeight;
        }
        else
        {
            adjacentTop = Math.Abs(Math.Sin(locked_theta)) * oldHeight;
            oppositeTop = Math.Abs(Math.Cos(locked_theta)) * oldHeight;
            adjacentBottom = Math.Abs(Math.Sin(locked_theta)) * oldWidth;
            oppositeBottom = Math.Abs(Math.Cos(locked_theta)) * oldWidth;
        }
        newWidth = adjacentTop + oppositeBottom;
        newHeight = adjacentBottom + oppositeTop;
        nWidth = (int)Math.Ceiling(newWidth);
        nHeight = (int)Math.Ceiling(newHeight);
        Bitmap rotatedBmp = new Bitmap(nWidth, nHeight);
        using (Graphics g = Graphics.FromImage(rotatedBmp))
        {
            Point[] points;
            if (locked_theta >= 0.0 && locked_theta < pi2)
            {
                points = new Point[] { new Point((int)oppositeBottom, 0), new Point(nWidth, (int)oppositeTop), new Point(0, (int)adjacentBottom) };
            }
            else if (locked_theta >= pi2 && locked_theta < Math.PI)
            {
                points = new Point[] { new Point(nWidth, (int)oppositeTop), new Point((int)adjacentTop, nHeight), new Point((int)oppositeBottom, 0) };
            }
            else if (locked_theta >= Math.PI && locked_theta < (Math.PI + pi2))
            {
                points = new Point[] { new Point((int)adjacentTop, nHeight), new Point(0, (int)adjacentBottom), new Point(nWidth, (int)oppositeTop) };
            }
            else
            {
                points = new Point[] { new Point(0, (int)adjacentBottom), new Point((int)oppositeBottom, 0), new Point((int)adjacentTop, nHeight) };
            }
            g.DrawImage(image, points);
        }
        return rotatedBmp;
    }
    private static Color GetColorToHexColor(string colorCode)
    {
        string rStr = colorCode.Substring(1, 2);
        string gStr = colorCode.Substring(3, 2);
        string bStr = colorCode.Substring(5, 2);
        int r = GetIntFromHexadecimal(rStr);
        int g = GetIntFromHexadecimal(gStr);
        int b = GetIntFromHexadecimal(bStr);
        return Color.FromArgb(r, g, b);
    }

    private static int GetIntFromHexadecimal(string hex)
    {
        int retval = 0;
        retval = GetNumber(hex.Substring(0, 1)) * 16;
        retval = retval + GetNumber(hex.Substring(1, 1));
        return retval;
    }

    private static int GetNumber(string i)
    {
        switch (i.ToLower())
        {
            case "f":
                return 15;
            case "e":
                return 14;
            case "d":
                return 13;
            case "c":
                return 12;
            case "b":
                return 11;
            case "a":
                return 10;
            default:
                return Convert.ToInt32(i);
        }
    }

    public static void BmpConvertFotmat(string bmpPath, string savePath, ImageFormat format)
    {
        Bitmap bmp = new Bitmap(bmpPath);
        bmp.Save(savePath, format);
    }



    public static Image AddWatermark(Image image, string watermarkText)
    {
        Bitmap bitmap = new Bitmap(image);
        Font font = new Font("Arial", 40, FontStyle.Bold, GraphicsUnit.Pixel);
        ///Color color = Color.FromArgb(10, 0, 0, 0);
        Color color = Color.FromArgb(100, 255, 255, 255);
        Point atPoint = new Point((image.Width / 2) - 70, (image.Height / 2) - 34);
        SolidBrush brush = new SolidBrush(color);
        Graphics graphics = null;
        try
        {
            graphics = Graphics.FromImage(bitmap);
        }
        catch
        {
            Bitmap temp = bitmap;
            bitmap = new Bitmap(bitmap.Width, bitmap.Height);
            graphics = Graphics.FromImage(bitmap);
            graphics.DrawImage(temp, new Rectangle(0, 0, bitmap.Width, bitmap.Height), 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel);
            temp.Dispose();
        }
        graphics.DrawImage(Image.FromFile(HttpContext.Current.Server.MapPath("/") + "/images/_logo.png"), atPoint);
        //graphics.DrawString(watermarkText, font, brush, atPoint);
        int lineLeft = (bitmap.Width * 0.25).ToInt32();
        int lineRight = (bitmap.Width * 0.75).ToInt32();
        graphics.DrawLine(new Pen(color), new Point(lineLeft, 0), new Point(lineRight, bitmap.Height));
        graphics.DrawLine(new Pen(color), new Point(lineLeft, bitmap.Height), new Point(lineRight, 0));
        graphics.Dispose();
        return (Image)bitmap;
    }
    public static System.Drawing.Image ResimBoyutlandir(System.Drawing.Image img, Size boyut, BoyutlandirmaTuru bt)
    {
        int HedefEn = 0;
        int HedefBoy = 0;
        switch (bt)
        {
            case BoyutlandirmaTuru.TamBoyutlandirma:
                HedefEn = boyut.Width;
                HedefBoy = boyut.Height;
                break;
            case BoyutlandirmaTuru.EnBoyOraniKoru:
                int kaynakEn = img.Width;
                int KaynakBoy = img.Height;

                float nPercent = 0;
                float nPercentW = 0;
                float nPercentH = 0;

                nPercentW = ((float)boyut.Width / (float)kaynakEn);
                nPercentH = ((float)boyut.Height / (float)KaynakBoy);

                if (nPercentH < nPercentW)
                {
                    nPercent = nPercentH;
                }
                else
                {
                    nPercent = nPercentW;
                }
                HedefEn = (int)(kaynakEn * nPercent);
                HedefBoy = (int)(KaynakBoy * nPercent);
                break;
            default:
                break;
        }

        Bitmap b = new Bitmap(HedefEn, HedefBoy);
        Graphics g = Graphics.FromImage((System.Drawing.Image)b);
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        g.DrawImage(img, 0, 0, HedefEn, HedefBoy);
        g.Dispose();

        return (System.Drawing.Image)b;
    }

            public enum BoyutlandirmaTuru
        {
            TamBoyutlandirma = 1,
            EnBoyOraniKoru = 2
        }
}