using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
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

		private int _Minimum = 0;
		private int _Maximum = 25;
		private int _LargeChange = 5;
		private int _SmallChange = 1;
		private bool _ShowSmallScale = false;
		private bool _ShowLargeScale = true;
		private bool _isFocused = false;
		
		
		private int _Value = 0;
		private bool isKnobRotating = false;
		private Rectangle rKnob ;
		private Point pKnob;
		private Rectangle rScale;
		private Pen DottedPen;

		Brush bKnob;
		Brush bKnobPoint;
		//-------------------------------------------------------
		// declare Off screen image and Offscreen graphics       
		//-------------------------------------------------------
		private Image OffScreenImage;
		private Graphics gOffScreen;
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
		
		/// <summary>
		/// Shows Small Scale marking.
		/// </summary>
		public bool ShowSmallScale
		{
			get{return _ShowSmallScale;}
			set
			{
				_ShowSmallScale = value;
				// need to redraw 
				Refresh();
			}
		}

		/// <summary>
		/// Shows Large Scale marking
		/// </summary>
		public bool ShowLargeScale
		{
			get{return _ShowLargeScale;}
			set
			{
				_ShowLargeScale = value;
				// need to redraw
				Refresh();
			}
		}
		
		/// <summary>
		/// Minimum Value for knob Control
		/// </summary>
		public int Minimum 
		{
			get{return _Minimum;}
			set{_Minimum = value;}
		}
		/// <summary>
		/// Maximum value for knob control
		/// </summary>
		public int Maximum 
		{
			get{return _Maximum;}
			set{_Maximum = value;}
		}
		
		/// <summary>
		/// value set for large change
		/// </summary>
		public int LargeChange 
		{
			get{return _LargeChange;}
			set
			{
				_LargeChange = value;
				Refresh();
			}
		}
		/// <summary>
		/// value set for small change.
		/// </summary>
		public int SmallChange 
		{
			get{return _SmallChange;}
			set
			{
				_SmallChange = value;
				Refresh();
			}
		}

		/// <summary>
		/// Current Value of knob control
		/// </summary>
		public int Value
		{
			get{return _Value;}
			set
			{
				
				_Value = value;
				// need to redraw 
				Refresh();
				// call delegate  
				OnValueChanged(this); 
			}
		}

		public KnobControl()
		{
			
			// This call is required by the Windows.Forms Form Designer.
			DottedPen = new Pen(Utility.getDarkColor(this.BackColor,40));
			DottedPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
			DottedPen.DashCap = System.Drawing.Drawing2D.DashCap.Flat;
			
			InitializeComponent();
			setDimensions();
			
			
			// TODO: Add any initialization after the InitForm call

		}
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			// Set background color of Image...            
			gOffScreen.Clear(this.BackColor);
			// Fill knob Background to give knob effect....
			gOffScreen.FillEllipse(bKnob,rKnob);
			// Set antialias effect on                     
			gOffScreen.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias ;
			// Draw border of knob                         
			gOffScreen.DrawEllipse(new Pen(this.BackColor),rKnob);

			//if control is focused 
			if (this._isFocused)
			{
				gOffScreen.DrawEllipse(DottedPen,rKnob);
			}

			// get current position of pointer             
			Point Arrow = this.getKnobPosition();

			// Draw pointer arrow that shows knob position 
			Utility.DrawInsetCircle(ref gOffScreen,new Rectangle(Arrow.X-3 ,Arrow.Y-3,6,6),new Pen(this.BackColor));
			
			//---------------------------------------------
			// darw small and large scale                  
			//---------------------------------------------
			if(this._ShowSmallScale)
			{
					for (int i= Minimum ; i<=Maximum ;i+= this._SmallChange)
					{
						gOffScreen.DrawLine(new Pen(this.ForeColor),getMarkerPoint(0,i),getMarkerPoint(3,i));
					}
			}
			if(this._ShowLargeScale)
			{
				for (int i= Minimum ; i<=Maximum ;i+= this._LargeChange)
				{
					gOffScreen.DrawLine(new Pen(this.ForeColor),getMarkerPoint(0,i),getMarkerPoint(5,i));
				}
			}
			
			// Drawimage on screen                    
			g.DrawImage(OffScreenImage,0,0);
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			// Empty To avoid Flickring due do background Drawing.
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
            if (e.Button == MouseButtons.Left)
            {
                if (Utility.isPointinRectangle(new Point(e.X, e.Y), rKnob))
                {
                    // Start Rotation of knob         
                    this.isKnobRotating = true;
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

		protected override void OnMouseUp(MouseEventArgs e)
		{
            if (e.Button == MouseButtons.Left)
            {
                // Stop rotation                   
                this.isKnobRotating = false;
                if (Utility.isPointinRectangle(new Point(e.X, e.Y), rKnob))
                {
                    // get value                   
                    this.Value = this.getValueFromPosition(new Point(e.X, e.Y));
                }
                this.Cursor = Cursors.Default;
            }
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
            if (e.Button == MouseButtons.Left)
            {
                //--------------------------------------
                //  Following Handles Knob Rotating     
                //--------------------------------------
                if (this.isKnobRotating == true)
                {
                    this.Cursor = Cursors.Hand;
                    Point p = new Point(e.X, e.Y);
                    int posVal = this.getValueFromPosition(p);
                    Value = posVal;
                }
            }
		}

		protected override void OnEnter(EventArgs e)
		{
			this._isFocused = true;
			this.Refresh();
			base.OnEnter(new EventArgs());
		}

		protected override void OnLeave(EventArgs e)
		{
			this._isFocused = false;
			this.Refresh();
			base.OnLeave(new EventArgs());
		}
		protected override void OnKeyDown(KeyEventArgs e)
		{
			
			//--------------------------------------------------------
			// Handles knob rotation with up,down,left and right keys 
			//--------------------------------------------------------
			if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Right)
			{
				if (_Value < Maximum) Value = _Value +1;
				this.Refresh();
			}
			else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Left)
			{
				if (_Value > Minimum) Value = _Value - 1;
				this.Refresh();
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

		private void setDimensions()
		{
			// get smaller from height and width
			int size = this.Width ;
			if (this.Width > this.Height)
			{
				size = this.Height;
			}
			// allow 10% gap on all side to determine size of knob    
			this.rKnob = new Rectangle((int)(size*0.10),(int)(size*0.10),(int)(size*0.80),(int)(size*0.80));
			
			this.rScale = new Rectangle(2,2,size-4,size-4);

			this.pKnob = new Point(rKnob.X + rKnob.Width/2, rKnob.Y + rKnob.Height/2);
			// create offscreen image                                 
			this.OffScreenImage = new Bitmap(this.Width,this.Height);
			// create offscreen graphics                              
			this.gOffScreen = Graphics.FromImage(OffScreenImage);	

			// create LinearGradientBrush for creating knob            
			bKnob = new System.Drawing.Drawing2D.LinearGradientBrush(rKnob, Color.SkyBlue, Color.RoyalBlue, LinearGradientMode.BackwardDiagonal);
			// create LinearGradientBrush for knobPoint                
			bKnobPoint = new System.Drawing.Drawing2D.LinearGradientBrush(rKnob, Color.Black, Color.DarkGray, LinearGradientMode.BackwardDiagonal);
        }

		private void KnobControl_Resize(object sender, System.EventArgs e)
		{
			setDimensions();
			Refresh();
		}

		/// <summary>
		/// gets knob position that is to be drawn on control.
		/// </summary>
		/// <returns>Point that describes current knob position</returns>
		private Point getKnobPosition()
		{
			double degree = 270* this.Value/(this.Maximum-this.Minimum);
			degree = (degree +135)*Math.PI /180;

			Point Pos = new Point(0,0);
			Pos.X = (int)(Math.Cos(degree)*(rKnob.Width/2-10)  + rKnob.X + rKnob.Width/2);
			Pos.Y = (int)(Math.Sin(degree)*(rKnob.Width/2-10)  + rKnob.Y + rKnob.Height/2);
			return Pos;
		}

		/// <summary>
		/// gets marker point required to draw scale marker.
		/// </summary>
		/// <param name="length">distance from center</param>
		/// <param name="Value">value that is to be marked</param>
		/// <returns>Point that describes marker position</returns>
		private Point getMarkerPoint(int length,int Value)
		{
			double degree = 270* Value/(this.Maximum-this.Minimum);
			degree = (degree +135)*Math.PI /180;

			Point Pos = new Point(0,0);
			Pos.X = (int)(Math.Cos(degree)*(rKnob.Width/2-length + 7)  + rKnob.X + rKnob.Width/2);
			Pos.Y = (int)(Math.Sin(degree)*(rKnob.Width/2-length + 7) + rKnob.Y + rKnob.Height/2);
			return Pos;
		}

		/// <summary>
		/// converts geomatrical position in to value..
		/// </summary>
		/// <param name="p">Point that is to be converted</param>
		/// <returns>Value derived from position</returns>
		private int getValueFromPosition(Point p)
		{
			double degree = 0.0;
			int v = 0;
			if (p.X <= pKnob.X )
			{
				degree  = (double)(pKnob.Y - p.Y ) /  (double)(pKnob.X - p.X );
				degree = Math.Atan(degree);
				degree = (degree) *(180/Math.PI) + 45;
				v = (int)(degree * (this.Maximum-this.Minimum)/ 270);
				
			}
			else if (p.X > pKnob.X )
			{
				degree  = (double)(p.Y - pKnob.Y ) /  (double)(p.X - pKnob.X );
				degree = Math.Atan(degree);
				degree = 225 + (degree) *(180/Math.PI);
				v = (int)(degree * (this.Maximum-this.Minimum)/ 270);
				
			}
			if (v > Maximum) v=Maximum;
			if (v < Minimum) v=Minimum;
			return v;
		
		}

		
		
	}
}
