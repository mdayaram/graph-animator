using System;
using Microsoft.Ink;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GraphAnimator
{

	public class Node
	{
		public static Color DEFAULT = Color.AntiqueWhite,
						    ENQUEUED = Color.Beige,
						    DEQUEUED = Color.Aqua;

		public static Color TEXT_DEFAULT = Color.Black,
							TEXT_ACTIVE = Color.Red,
							TEXT_COMPARED = Color.DarkMagenta,
							TEXT_COMPLETE = Color.DarkBlue;
		
		private int id;
		private Stroke stroke;
		private Point centerPoint;
		private string text;
		private Color fillColor, textColor;
		private Edges edges;
		private bool isRect;

		public Node(Stroke stroke, bool isRect)
		{
			this.stroke = stroke;
			this.id = stroke.Id;
			Rectangle r = stroke.GetBoundingBox();
			centerPoint = new Point(r.X + r.Width/2, r.Y + r.Height/2);
			fillColor = DEFAULT;
			this.isRect = isRect;
			edges = new Edges();
			textColor = TEXT_DEFAULT;
			text = "";
		}
		public Node(Stroke stroke) : this(stroke, false) {}

		#region Accessor and Mutators
		public int Id
		{
			get{return id;}
		}
		public Stroke Stroke
		{
			get{return stroke;}
		}
		public Color Color
		{
			get{return fillColor;}
			set{fillColor = value;}
		}
		public Edges Edges
		{
			get{return edges;}
		}
		public Point CenterPoint
		{
			get{return centerPoint;}
			set{centerPoint = value;}
		}
		public string Text
		{
			get{return text;}
			set{text = value;}
		}
		#endregion

		public override bool Equals(object obj)
		{
			if(obj == null) return false;
			Node n = obj as Node;
			return this.Id == n.Id;
		}

		public void Render(InkOverlay i, Graphics g)
		{

			Rectangle rect = stroke.GetBoundingBox();
			rect = StrokeManager.InkSpaceToPixelRect(i, g, rect);

			if(isRect)
				g.FillRectangle(new SolidBrush(fillColor), rect);
			else
				g.FillEllipse(new SolidBrush(fillColor), rect);

			i.Renderer.Draw(g, stroke);
			Point[] p = {new Point(centerPoint.X, centerPoint.Y)};
			i.Renderer.InkSpaceToPixel(g, ref p);
			p[0].X -= 15;
			p[0].Y -= 10;
			g.DrawString(text, new Font("Arial",14), new SolidBrush(textColor), p[0]);
		}
	}
}
