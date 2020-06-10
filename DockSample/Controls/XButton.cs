using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DockSample.Controls
{
	public class XButton : Control
	{
		bool mouseIn = false;
		bool mouseDown = false;

		public XButton()
		{
			ResizeRedraw = true;
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			mouseIn = true;
			Invalidate();
			base.OnMouseEnter(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			mouseIn = false;
			Invalidate();
			base.OnMouseLeave(e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				mouseDown = true;
				Invalidate();
			}
			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				mouseDown = false;
				Invalidate();
			}
			base.OnMouseUp(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			// Imitate usual behaviour of button which is to show as unpressed when the mouse button
			// is pressed down then dragged away.
			if (mouseIn != ClientRectangle.Contains(e.X, e.Y))
			{
				mouseIn = ClientRectangle.Contains(e.X, e.Y);
				Invalidate();
			}
			base.OnMouseMove(e);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.Clear(BackColor);

			// Draw a sunken border if the mouse is in the control and pressed, draw a raised border
			// if the mouse is in the control but not pressed.
			if (mouseIn || mouseDown)
				ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle,
					mouseDown && mouseIn ? Border3DStyle.SunkenOuter : Border3DStyle.RaisedInner);

			// Deflate our client rectangle then draw the X inside it
			Rectangle r = ClientRectangle;
			r.Inflate(-4, -4);
			// A square shape with an odd # of pixels required is to render properly
			r.Width = r.Height = Math.Min(r.Width, r.Height) / 2 * 2 + 1;

			// Draw the 'X'
			using (Pen p = new Pen(Color.Black, 2))
			{
				e.Graphics.DrawLine(p, r.Left, r.Top, r.Right, r.Bottom);
				e.Graphics.DrawLine(p, r.Right, r.Top, r.Left, r.Bottom);
			}

			base.OnPaint(e);
		}
	}
}
