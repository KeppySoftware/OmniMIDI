#region License

/* Copyright (c) 2017 Fabrice Lacharme
 * This code was originally written by Jigar Desai 
 * http://www.c-sharpcorner.com/article/knob-control-using-windows-forms-and-gdi/
 * Note that another implementation exists in vb.net by Blong
 * https://www.codeproject.com/Articles/2563/VB-NET-Knob-Control-using-Windows-Forms-and-GDI?msg=1884770#xx1884770xx
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy 
 * of this software and associated documentation files (the "Software"), to 
 * deal in the Software without restriction, including without limitation the 
 * rights to use, copy, modify, merge, publish, distribute, sublicense, and/or 
 * sell copies of the Software, and to permit persons to whom the Software is 
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software. 
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
 * THE SOFTWARE.
 */

#endregion

#region Contact

/*
 * Fabrice Lacharme
 * Email: fabrice.lacharme@gmail.com
 */

#endregion

using System;
using System.Drawing;

namespace KnobControl
{
	/// <summary>
	/// Summary description for Utility.
	/// </summary>
	public class Utility
	{

        public static float GetRadian(float val)
        {
            return (float)(val * Math.PI / 180);
        }


        public static Color getDarkColor(Color c,byte d)
		{
			byte r = 0 ;
			byte g = 0;
			byte b = 0 ;

			if (c.R > d) r = (byte)(c.R - d);
			if (c.G > d) g = (byte)(c.G - d);
			if (c.B > d) b = (byte)(c.B - d);

			Color c1 = Color.FromArgb(r,g,b);
			return c1;
		}
		public static Color getLightColor(Color c,byte d)
		{
			byte r = 255 ;
			byte g = 255 ;
			byte b = 255 ;

			if (c.R + d < 255) r = (byte)(c.R + d);
			if (c.G + d < 255) g = (byte)(c.G + d);
			if (c.B + d < 255) b = (byte)(c.B + d);

			Color c2 = Color.FromArgb(r,g,b);
			return c2;
		}
		
		/// <summary>
		/// Method which checks is particular point is in rectangle
		/// </summary>
		/// <param name="p">Point to be Chaecked</param>
		/// <param name="r">Rectangle</param>
		/// <returns>true is Point is in rectangle, else false</returns>
		public static bool isPointinRectangle(Point p ,Rectangle r)
		{
			bool flag = false;
			if (p.X > r.X && p.X < r.X + r.Width && p.Y > r.Y && p.Y < r.Y + r.Height)
			{
				flag = true;
			}
			return flag;

		}
		public static void DrawInsetCircle(ref Graphics g,Rectangle r,Pen p)
		{

            Pen p1 = new Pen(getDarkColor(p.Color,50));            
            Pen p2 = new Pen(getLightColor(p.Color,50));
            for (int i=0;i<p.Width;i++)
			{
				Rectangle r1 = new Rectangle(r.X +i,r.Y +i,r.Width-i*2,r.Height-i*2);
				g.DrawArc(p2,r1,-45,180);
				g.DrawArc(p1,r1,135,180);
			}
		}




	}
}
