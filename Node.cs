using System;
using Microsoft.Ink;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GraphAnimator
{

	public class Node
	{
		public static Color DEFAULT = Color.AntiqueWhite;

		public static Color TEXT_DEFAULT = Color.Black;
		
		private int id;
		private Stroke stroke;
		private Point centerPoint;
		private string text;
		private int distance;
		private int proposed;
		private Edge incomingEdge;
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
			distance = Int32.MaxValue;
			incomingEdge = null;
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
			set
			{
				centerPoint = value;
				foreach(Edge e in this.edges)
				{
					Point[] p = new Point[2];
					p[0] = centerPoint;
					p[1] = Node.GetOther(this,e).CenterPoint;
					e.Stroke.Ink.DeleteStroke(e.Stroke);
					e.Stroke = stroke.Ink.CreateStroke(p);
				}
			}
		}
		public string Text
		{
			get{return text;}
			set{text = value;}
		}
		public Color TextColor
		{
			get{return textColor;}
			set{textColor = value;}
		}
		public int Distance
		{
			get{return distance;}
			set{distance = value;}
		}
		public int Proposed
		{
			get{return proposed;}
			set{proposed = value;}
		}
		public Edge Incoming
		{
			get{return incomingEdge;}
			set{incomingEdge = value;}
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
			//if(stroke.Deleted == true) return;
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
			g.DrawString(text, new Font("Arial",10), new SolidBrush(textColor), p[0]);
		}

		public static Node GetOther(Node n, Edge e)
		{
			if(n == null || !n.Edges.Contains(e)) return null;
			return (n.Equals(e.NodeB)) ? e.NodeA : e.NodeB;
		}

		#region IComparer Methods and Classes
		public static IComparer sortAscending()
		{      
			return (IComparer) new sortAscendingDistance();
		}

		private class sortAscendingDistance : IComparer
		{
			public int Compare(object x, object y)
			{
				Node nx = x as Node;
				Node ny = y as Node;
				return nx.Distance - ny.Distance;
			}
		}
		#endregion
	}
}
