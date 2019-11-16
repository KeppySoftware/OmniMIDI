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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace KnobControl
{

    // A delegate type for hooking up ValueChanged notifications. 
    public delegate void ValueChangedEventHandler(object Sender);

    /// <summary>
    /// Summary description for KnobControl.
    /// </summary>
    public class KnobControl : System.Windows.Forms.UserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        /// <summary>
        /// Styles of pointer button
        /// </summary>
        public enum knobPointerStyle
        {
            circle,
            line,
        }


        #region private properties

        private knobPointerStyle _knobPointerStyle = knobPointerStyle.circle;
        
        private int _minimum = 0;
        private int _maximum = 25;
        private int _LargeChange = 5;
        private int _SmallChange = 1;

        private int _scaleDivisions;
        private int _scaleSubDivisions;
        private Color _scaleColor;
        private Color _knobBackColor = Color.Transparent;
        private bool _drawDivInside;

        private bool _showSmallScale = false;
        private bool _showLargeScale = true;
                
        private float _startAngle = 135;
        private float _endAngle = 405;
        private float deltaAngle;
        private int _mouseWheelBarPartitions = 10;
        

        private float drawRatio;

        // Color of the pointer
        private Color _PointerColor = Color.SlateBlue;


        private int _Value = 0;
        private bool isFocused = false;
        private bool isKnobRotating = false;
        private Rectangle rKnob;
        private Point pKnob;        
        private Pen DottedPen;

        Brush bKnob;
        Brush bKnobPoint;

        private Font knobFont;
                
        //-------------------------------------------------------
        // declare Off screen image and Offscreen graphics       
        //-------------------------------------------------------
        private Image OffScreenImage;
        private Graphics gOffScreen;

        #endregion


        #region event
        //-------------------------------------------------------
        // An event that clients can use to be notified whenever 
        // the Value is Changed.                                 
        //-------------------------------------------------------
        public event ValueChangedEventHandler ValueChanged;
        
        //-------------------------------------------------------
        // Invoke the ValueChanged event; called  when value     
        // is changed                                            
        //-------------------------------------------------------
        protected virtual void OnValueChanged(object sender)
        {
            if (ValueChanged != null)
                ValueChanged(sender);
        }

        #endregion


        #region (* public Properties *)

        /// <summary>
        /// Start angle to display graduations
        /// </summary>
        /// <value>The start angle to display graduations.</value>
        [Description("Set the start angle to display graduations (min 90)")]
        [Category("KnobControl")]
        [DefaultValue(135)]
        public float StartAngle
        {
            get { return _startAngle; }
            set {
                if (value >= 90 && value < _endAngle)
                {
                    _startAngle = value;
                    deltaAngle = _endAngle - StartAngle;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// End angle to display graduations
        /// </summary>
        /// <value>The end angle to display graduations.</value>
        [Description("Set the end angle to display graduations (max 450)")]
        [Category("KnobControl")]
        [DefaultValue(405)]
        public float EndAngle
        {
            get { return _endAngle; }
            set {
                if (value <= 450 && value > _startAngle)
                {
                    _endAngle = value;
                    deltaAngle = _endAngle - _startAngle;
                    Invalidate();
                }
            }
        }


        /// <summary>
        /// Style of pointer: circle or line
        /// </summary>
        [Description("Set the style of the knob pointer: a circle or a line")]
        [Category("KnobControl")]        
        public knobPointerStyle KnobPointerStyle
        {
            get { return _knobPointerStyle; }
            set { _knobPointerStyle = value;
                Invalidate();
            }
        }


        /// <summary>
        /// Gets or sets the mouse wheel bar partitions.
        /// </summary>
        /// <value>The mouse wheel bar partitions.</value>
        /// <exception cref="T:System.ArgumentOutOfRangeException">exception thrown when value isn't greather than zero</exception>
        [Description("Set to how many parts is bar divided when using mouse wheel")]
        [Category("KnobControl")]
        [DefaultValue(10)]
        public int MouseWheelBarPartitions
        {
            get { return _mouseWheelBarPartitions; }
            set
            {
                if (value > 0)
                    _mouseWheelBarPartitions = value;
                else throw new ArgumentOutOfRangeException("MouseWheelBarPartitions has to be greather than zero");
            }
        }

        /// <summary>
        /// Draw string graduations inside or outside knob circle
        /// </summary>
        /// 
        [Description("Draw graduation strings inside or outside the knob circle")]
        [Category("KnobControl")]
        [DefaultValue(false)]
        public bool DrawDivInside
        {
            get { return _drawDivInside; }
            set
            {
                _drawDivInside = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Color of graduations
        /// </summary>
        [Description("Color of graduations")]
        [Category("KnobControl")]
        public Color ScaleColor
        {
            get { return _scaleColor; }
            set
            {
                _scaleColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Color of graduations
        /// </summary>
        [Description("Color of knob")]
        [Category("KnobControl")]
        public Color knobBackColor
        {
            get { return _knobBackColor; }
            set
            {
                _knobBackColor = value;
                setDimensions();
                Invalidate();
            }
        }

        /// <summary>
        /// How many divisions of maximum?
        /// </summary>
        [Description("Set the number of intervals between minimum and maximum")]
        [Category("KnobControl")]
        public int ScaleDivisions
        {
            get { return _scaleDivisions; }
            set
            {
                _scaleDivisions = value;
                Invalidate();
                
            }
        }

        /// <summary>
        /// How many subdivisions for each division
        /// </summary>
        [Description("Set the number of subdivisions between main divisions of graduation.")]
        [Category("KnobControl")]
        public int ScaleSubDivisions
        {
            get { return _scaleSubDivisions; }
            set
            {
                if (value > 0 && _scaleDivisions > 0 &&  (_maximum - _minimum) / (value * _scaleDivisions) > 0)
                {
                    _scaleSubDivisions = value;
                    Invalidate();
                }                
            }
        }

        /// <summary>
        /// Shows Small Scale marking.
        /// </summary>
        [Description("Show or hide subdivisions of graduations")]
        [Category("KnobControl")]
        public bool ShowSmallScale
		{
			get{return _showSmallScale;}
			set
			{
                if (value == true)
                {
                    if ( _scaleDivisions > 0 && _scaleSubDivisions > 0 &&   (_maximum - _minimum) / (_scaleSubDivisions * _scaleDivisions) > 0)
                    {
                        _showSmallScale = value;
                        Invalidate();
                    }
                }
                else
                {
                    _showSmallScale = value;
                    // need to redraw 
                    Invalidate();
                }
			}
		}

        /// <summary>
        /// Shows Large Scale marking
        /// </summary>
        [Description("Show or hide graduations")]
        [Category("KnobControl")]
        public bool ShowLargeScale
		{
			get{return _showLargeScale;}
			set
			{
				_showLargeScale = value;
                // need to redraw
                setDimensions();

                Invalidate();
			}
		}

        /// <summary>
        /// Minimum Value for knob Control
        /// </summary>
        [Description("set the minimum value for the knob control")]
        [Category("KnobControl")]
        public int Minimum 
		{
			get{return _minimum;}
			set{_minimum = value;
                Invalidate();
            }
		}
        /// <summary>
        /// Maximum value for knob control
        /// </summary>
        [Description("set the maximum value for the knob control")]
        [Category("KnobControl")]
        public int Maximum 
		{
			get{return _maximum;}
			set{

                if (value > _minimum)
                {

                    _maximum = value;
                    
                        
                    if (_scaleSubDivisions > 0 && _scaleDivisions > 0 && (_maximum - _minimum)/ (_scaleSubDivisions * _scaleDivisions) <= 0 )
                    {
                        _showSmallScale = false;
                    }

                    setDimensions();
                    Invalidate();
                }
            }
		}

        /// <summary>
        /// value set for large change
        /// </summary>
        [Description("set the value for the large changes")]
        [Category("KnobControl")]
        public int LargeChange 
		{
			get{return _LargeChange;}
			set
			{
				_LargeChange = value;
				Invalidate();
			}
		}
        /// <summary>
        /// value set for small change.
        /// </summary>
        [Description("set the minimum value for the small changes")]
        [Category("KnobControl")]
        public int SmallChange 
		{
			get{return _SmallChange;}
			set
			{
				_SmallChange = value;
				Invalidate();
			}
		}

        /// <summary>
        /// Current Value of knob control
        /// </summary>
        [Description("set the current value of the knob control")]
        [Category("KnobControl")]
        public int Value
		{
			get{return _Value;}
			set
			{
				
				_Value = value;
				// need to redraw 
				Invalidate();
				// call delegate  
				OnValueChanged(this); 
			}
		}

        /// <summary>
        /// Color of the button
        /// </summary>
        [Description("set the color of the pointer")]
        [Category("KnobControl")]
        public Color PointerColor
        {
            get { return _PointerColor; }
            set { _PointerColor = value;
                Invalidate();
            }
        }

        #endregion properties


        public KnobControl()
		{
			
			// This call is required by the Windows.Forms Form Designer.
			DottedPen = new Pen(Utility.getDarkColor(this.BackColor,40));
			DottedPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
			DottedPen.DashCap = System.Drawing.Drawing2D.DashCap.Flat;
			
			InitializeComponent();

            knobFont = new Font(this.Font.FontFamily, this.Font.Size);

            // Properties initialisation

            // "start angle" and "end angle" possible values:

            // 90 = bottom (minimum value for "start angle")
            // 180 = left
            // 270 = top
            // 360 = right
            // 450 = bottom again (maximum value for "end angle")

            // So the couple (90, 450) will give an entire circle and the couple (180, 360) will give half a circle.

            _startAngle = 135;
            _endAngle = 405;            
            deltaAngle = _endAngle - _startAngle;

            _minimum = 0;
            _maximum = 100;
            _scaleDivisions = 11;
            _scaleSubDivisions = 4;
            _mouseWheelBarPartitions = 10;

            _scaleColor = Color.Black;
            _knobBackColor = Color.White;

			setDimensions();								
		}


        #region override

        /// <summary>
        /// Paint event: draw all
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
		{
            
            Graphics g = e.Graphics;
			// Set background color of Image...            
			gOffScreen.Clear(this.BackColor);
			// Fill knob Background to give knob effect....
			gOffScreen.FillEllipse(bKnob, rKnob);
			// Set antialias effect on                     
			gOffScreen.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias ;
			// Draw border of knob                         
			gOffScreen.DrawEllipse(new Pen(this.BackColor),rKnob);

			//if control is focused 
			if (this.isFocused)
			{
				gOffScreen.DrawEllipse(DottedPen, rKnob);
			}

            // DrawPointer
            DrawPointer(gOffScreen);
            
            //---------------------------------------------
            // darw small and large scale                  
            //---------------------------------------------
            DrawDivisions(gOffScreen, rKnob);
            
            // Drawimage on screen                    
            g.DrawImage(OffScreenImage,0,0);
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			// Empty To avoid Flickring due do background Drawing.
		}

        /// <summary>
        /// Mouse down event: select control
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
		{
            if (e.Button == MouseButtons.Right) return;

			if (Utility.isPointinRectangle(new Point(e.X,e.Y),rKnob))
			{
                if (isFocused)
                {                                       
                    // was already selected
                    // Start Rotation of knob only if it was selected before        
                    isKnobRotating = true;
                }
                else
                {
                    // Was not selected before => select it
                    Focus();
                    isFocused = true;
                    isKnobRotating = false; // disallow rotation, must click again
                    // draw dotted border to show that it is selected
                    Invalidate();
                }
			}
		
		}
		

		//----------------------------------------------------------
		// we need to override IsInputKey method to allow user to   
		// use up, down, right and bottom keys other wise using this
		// keys will change focus from current object to another    
		// object on the form                                       
		//----------------------------------------------------------
		protected override bool IsInputKey(Keys key) 
		{ 
			switch(key) 
			{ 
				case Keys.Up: 
				case Keys.Down: 
				case Keys.Right: 
				case Keys.Left: 
				return true; 
			} 
			return base.IsInputKey(key); 
		}

        /// <summary>
        /// Mouse up event: display new value
        /// </summary>
        /// <param name="e"></param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
            if (e.Button == MouseButtons.Right) return;

            if (Utility.isPointinRectangle(new Point(e.X,e.Y),rKnob))
			{
                if (isFocused == true && isKnobRotating == true)
                {                    
                    // change value is allowed only only after 2nd click                   
                    this.Value = this.getValueFromPosition(new Point(e.X, e.Y));
                   
                }
                else
                {
                    // 1st click = only focus
                    isFocused = true;
                    isKnobRotating = true;
                }

			}
			this.Cursor = Cursors.Default;
		}

        /// <summary>
        /// Mouse move event: drag the pointer to the mouse position
        /// </summary>
        /// <param name="e"></param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			//--------------------------------------
			//  Following Handles Knob Rotating     
			//--------------------------------------
			if (e.Button == MouseButtons.Left &&  this.isKnobRotating == true)
			{
				this.Cursor = Cursors.Hand;
				Point p = new Point(e.X, e.Y);
				int posVal = this.getValueFromPosition(p);
				Value = posVal;
			}
		
		}

        /// <summary>
        /// Mousewheel: change value
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {           
            base.OnMouseWheel(e);

            if ( isFocused && isKnobRotating && Utility.isPointinRectangle(new Point(e.X, e.Y), rKnob))
            {                
                // the Delta value is always 120, as explained in MSDN
                int v = (e.Delta / 120) * (_maximum - _minimum) / _mouseWheelBarPartitions;
                SetProperValue(Value + v);

                // Avoid to send MouseWheel event to the parent container
                ((HandledMouseEventArgs)e).Handled = true;
            }
        }

     
        /*
        protected override void OnEnter(EventArgs e)
		{
			
            Invalidate();

			base.OnEnter(new EventArgs());
		}
        */

        /// <summary>
        /// Leave event: disallow knob rotation
        /// </summary>
        /// <param name="e"></param>
		protected override void OnLeave(EventArgs e)
		{
            // unselect the control (remove dotted border)
            isFocused = false;
            isKnobRotating = false;
            Invalidate();

			base.OnLeave(new EventArgs());
		}

        /// <summary>
        /// Key down event: change value
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
		{
            if (isFocused)
            {
                //--------------------------------------------------------
                // Handles knob rotation with up,down,left and right keys 
                //--------------------------------------------------------
                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Right)
                {
                    if (_Value < _maximum) Value = _Value + 1;
                    this.Refresh();
                }
                else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Left)
                {
                    if (_Value > _minimum) Value = _Value - 1;
                    this.Refresh();
                }
            }
		}
        

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}


        #endregion


        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
		{
			// 
			// KnobControl
			// 
			this.ImeMode = System.Windows.Forms.ImeMode.On;
			this.Name = "KnobControl";
			this.Resize += new System.EventHandler(this.KnobControl_Resize);

		}
        #endregion


        #region Draw

        /// <summary>
        /// Draw the pointer of the knob (a small button inside the main button)
        /// </summary>
        /// <param name="Gr"></param>
        private void DrawPointer(Graphics Gr)
        {
                                     
            try
            {
                if (_knobPointerStyle == knobPointerStyle.line)
                {
                    float radius = (float)(rKnob.Width / 2);

                    int l = (int)radius / 2;
                    int w = l / 8;
                    Point[] pt = getKnobLine(l);
                   
                    Gr.DrawLine(new Pen(_PointerColor, w), pt[0] , pt[1]);

                }
                else
                {
                    int w = 0;
                    int h = 0;                    

                    // Size of pointer
                    w = rKnob.Width / 10;
                    if (w < 7)
                        w = 7;

                    h = w;

                    Point Arrow = this.getKnobPosition(w);

                    // Draw pointer arrow that shows knob position             
                    Rectangle rPointer = new Rectangle(Arrow.X - w / 2, Arrow.Y - w / 2, w, h);
                    

                    Utility.DrawInsetCircle(ref Gr, rPointer, new Pen(_PointerColor));
                    Gr.FillEllipse(bKnobPoint, rPointer);

                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        /// <summary>
        /// Draw graduations
        /// </summary>
        /// <param name="Gr"></param>
        /// <param name="rc"></param>
        /// <returns></returns>
        private bool DrawDivisions(Graphics Gr, RectangleF rc)
        {
            if (this == null)
                return false;

            float cx = pKnob.X; 
            float cy = pKnob.Y; 

            float w = rc.Width;
            float h = rc.Height;

            float tx;
            float ty;

            float incr = Utility.GetRadian((_endAngle - _startAngle) / ((_scaleDivisions - 1) * (_scaleSubDivisions + 1)));
            float currentAngle = Utility.GetRadian(_startAngle);
            
            float radius = (float)(rKnob.Width / 2 );
            float rulerValue = (float)_minimum;

            
            Pen penL = new Pen(_scaleColor, (2 * drawRatio));
            Pen penS = new Pen(_scaleColor, (1 * drawRatio));

            SolidBrush br = new SolidBrush(_scaleColor);

            PointF ptStart = new PointF(0, 0);
            PointF ptEnd = new PointF(0, 0);
            int n = 0;

            if (_showLargeScale)
            {
                for (; n < _scaleDivisions; n++)
                {
                    // draw divisions
                    ptStart.X = (float)(cx + (radius) * Math.Cos(currentAngle));
                    ptStart.Y = (float)(cy + (radius) * Math.Sin(currentAngle));

                    ptEnd.X = (float)(cx + (radius + w/50) * Math.Cos(currentAngle));
                    ptEnd.Y = (float)(cy + (radius + w/50) * Math.Sin(currentAngle));

                    Gr.DrawLine(penL, ptStart, ptEnd);


                    //Draw graduations Strings                    
                    float fSize = (float)(6F * drawRatio);
                    if (fSize < 6)
                        fSize = 6;                   
                    Font font = new Font(this.Font.FontFamily, fSize);

                    double val = Math.Round(rulerValue);
                    String str = String.Format("{0,0:D}", (int)val);
                    SizeF size = Gr.MeasureString(str, font);

                    if (_drawDivInside)
                    {
                        // graduations strings inside the knob
                        tx = (float)(cx + (radius - (11 * drawRatio)) * Math.Cos(currentAngle));
                        ty = (float)(cy + (radius - (11 * drawRatio )) * Math.Sin(currentAngle));


                    }
                    else
                    {
                        // graduation strings outside the knob
                        tx = (float)(cx + (radius + (11 * drawRatio)) * Math.Cos(currentAngle));
                        ty = (float)(cy + (radius + (11 * drawRatio)) * Math.Sin(currentAngle));

                    }                   
                    
                    Gr.DrawString(str,
                                    font,
                                    br,
                                    tx - (float)(size.Width * 0.5),
                                    ty - (float)(size.Height * 0.5));

                    rulerValue += (float)((_maximum - _minimum) / (_scaleDivisions - 1));

                    if (n == _scaleDivisions - 1)
                    {
                        font.Dispose();
                        break;
                    }


                    // Subdivisions
                    
                    if (_scaleDivisions <= 0)
                        currentAngle += incr;
                    else 
                    {

                        for (int j = 0; j <= _scaleSubDivisions; j++)
                        {
                            currentAngle += incr;

                            // if user want to display small graduations
                            if (_showSmallScale)
                            {
                                ptStart.X = (float)(cx + radius * Math.Cos(currentAngle));
                                ptStart.Y = (float)(cy + radius * Math.Sin(currentAngle));
                                ptEnd.X = (float)(cx + (radius + w / 50) * Math.Cos(currentAngle));
                                ptEnd.Y = (float)(cy + (radius + w / 50) * Math.Sin(currentAngle));

                                Gr.DrawLine(penS, ptStart, ptEnd);
                            }
                        }
                    }
                    

                    font.Dispose();
                }
            }

            return true;
        }

        /// <summary>
        /// Set position of button inside its rectangle to insure that divisions will fit.
        /// </summary>
        private void setDimensions()
		{
                      
            int size = this.Width;
            Height = size;


            // Rectangle
            float x, y, w, h;
            x = 0;
            y = 0;
            w = Size.Width;
            h = Size.Height;

            // Calculate ratio
            drawRatio = (Math.Min(w, h)) / 200;
            if (drawRatio == 0.0)
                drawRatio = 1;

            if (_showLargeScale)
            {
                float fSize = (float)(6F * drawRatio);
                if (fSize < 6)
                    fSize = 6;
                knobFont = new Font(this.Font.FontFamily, fSize);
                double val = _maximum;
                String str = String.Format("{0,0:D}", (int)val);


                Graphics Gr = this.CreateGraphics();
                SizeF strsize = Gr.MeasureString(str, knobFont);
                int strw = (int)strsize.Width + 4;
                int strh = (int)strsize.Height;

                // allow 10% gap on all side to determine size of knob    
                //this.rKnob = new Rectangle((int)(size * 0.10), (int)(size * 0.15), (int)(size * 0.80), (int)(size * 0.80));
                x = (int)strw;
                //y = x;
                y = 2 * strh;
                w = (int)(size - 2 * strw);
                if (w <= 0)
                    w = 1;
                h = w;
                this.rKnob = new Rectangle((int)x, (int)y, (int)w, (int)h);
                Gr.Dispose();
            }
            else
            {
                this.rKnob = new Rectangle(0, 0, Width, Height);
            }


            // Center of knob
            this.pKnob = new Point(rKnob.X + rKnob.Width/2, rKnob.Y + rKnob.Height/2);

			// create offscreen image                                 
			this.OffScreenImage = new Bitmap(this.Width,this.Height);
			// create offscreen graphics                              
			this.gOffScreen = Graphics.FromImage(OffScreenImage);	

			// create LinearGradientBrush for creating knob            
			bKnob = new System.Drawing.Drawing2D.LinearGradientBrush(rKnob, Color.FromArgb(255, 121, 0, 182), Color.FromArgb(255, 53, 0, 119), LinearGradientMode.BackwardDiagonal);

            // create LinearGradientBrush for knobPoint                
            bKnobPoint = new System.Drawing.Drawing2D.LinearGradientBrush(
				rKnob,Utility.getLightColor(_PointerColor,55),Utility.getDarkColor(_PointerColor,55),LinearGradientMode.BackwardDiagonal);
		}

        #endregion


        #region resize

        /// <summary>
        /// Resize event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KnobControl_Resize(object sender, System.EventArgs e)
		{
			setDimensions();
            //Refresh();
            Invalidate();
		}

        #endregion


        #region private functions

        /// <summary>
        /// Sets the trackbar value so that it wont exceed allowed range.
        /// </summary>
        /// <param name="val">The value.</param>
        private void SetProperValue(int val)
        {
            if (val < _minimum) Value = _minimum;
            else if (val > _maximum) Value = _maximum;
            else Value = val;
        }

        /// <summary>
        /// gets knob position that is to be drawn on control.
        /// </summary>
        /// <returns>Point that describes current knob position</returns>
        private Point getKnobPosition(int l)
		{
            float cx = pKnob.X;
            float cy = pKnob.Y;
            
            
            float radius = (float)(rKnob.Width / 2);

            float degree = deltaAngle * this.Value/(_maximum - _minimum);
            degree = Utility.GetRadian(degree + _startAngle);
            
            Point Pos = new Point(0,0);

            Pos.X = (int)(cx + (radius - (11)* drawRatio) * Math.Cos(degree));
            Pos.Y = (int)(cy + (radius - (11)* drawRatio) * Math.Sin(degree));

            return Pos;
		}
		
        /// <summary>
        /// return 2 points of a line starting from the center of the knob to the periphery
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        private Point[] getKnobLine (int l)
        {
            Point[] pret = new Point[2];

            float cx = pKnob.X;
            float cy = pKnob.Y;

           
            float radius = (float)(rKnob.Width / 2);

            float degree = deltaAngle * this.Value / (_maximum - _minimum);
            degree = Utility.GetRadian(degree + _startAngle);

            Point Pos = new Point(0, 0);

            Pos.X = (int)(cx + (radius - drawRatio * 10) * Math.Cos(degree));
            Pos.Y = (int)(cy + (radius - drawRatio * 10) * Math.Sin(degree));

            pret[0] = new Point(Pos.X, Pos.Y);

            Pos.X = (int)(cx + (radius - drawRatio * 10 - l) * Math.Cos(degree));
            Pos.Y = (int)(cy + (radius - drawRatio * 10 - l) * Math.Sin(degree));

            pret[1] = new Point(Pos.X, Pos.Y);

            return pret;
        }

        /// <summary>
        /// converts geometrical position into value..
        /// </summary>
        /// <param name="p">Point that is to be converted</param>
        /// <returns>Value derived from position</returns>
        private int getValueFromPosition(Point p)
		{
			float degree = 0;
			int v = 0;

            if (p.X <= pKnob.X )
			{
				degree  = (float)(pKnob.Y - p.Y ) /  (float)(pKnob.X - p.X );
				degree = (float)Math.Atan(degree);
                
                degree = (degree) * (float)(180 / Math.PI) + (180 - _startAngle);   

            }
			else if (p.X > pKnob.X )
			{
				degree  = (float)(p.Y - pKnob.Y ) /  (float)(p.X - pKnob.X );
				degree = (float)Math.Atan(degree);

                degree = (degree) * (float)(180 / Math.PI) + 360 - _startAngle;              				
			}
            
            // round to the nearest value (when you click just before or after a graduation!)
            v = (int)Math.Round(degree * (_maximum - _minimum) / deltaAngle);


            if (v > _maximum) v = _maximum;
			if (v < _minimum) v = _minimum;
			return v;
		
		}

        #endregion


    }
}
