using System;
using Microsoft.Ink;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace GraphAnimator
{
	public class Edge
	{
		public static Color DEFAULT = Color.Black,
							ACTIVE = Color.Red,
							DONE = Color.Blue;
		public static int DEFAULT_WEIGHT = 10;
		private Node a, b;
		private int weight;
		private Color color;

		public Edge(Node a, Node b, int weight)
		{
			this.a = a;
			this.b = b;
			this.weight = weight;
			color = DEFAULT;
			a.Edges.Add(this);
			b.Edges.Add(this);
		}
		public Edge(Node a, Node b) : this(a,b,Edge.DEFAULT_WEIGHT) {}

		#region Accessor and Mutator Methods
		public Node NodeA
		{
			get{return a;}
		}
		public Node NodeB
		{
			get{return b;}
		}

		public Color Color
		{
			get{return color;}
			set{color = value;}
		}
		public int Weight
		{
			get{return weight;}
			set{weight = value;}
		}

		#endregion
		
		public static bool hasEdge(Node a, Node b)
		{
			foreach(Edge edge in a.Edges)
			{
				if(edge.NodeA.Equals(b) || edge.NodeB.Equals(b))
				{
					return true;
				}
			}
			return false;
		}

		public void Render(InkOverlay i, Graphics g)
		{
			Point[] p = {a.CenterPoint,b.CenterPoint};
			Stroke stroke = i.Ink.CreateStroke(p);
			stroke.DrawingAttributes = i.DefaultDrawingAttributes;
			stroke.DrawingAttributes.Color = color;
			i.Renderer.Draw(g,stroke);

			Rectangle rect = StrokeManager.InkSpaceToPixelRect(i,g,stroke.GetBoundingBox());
			Point center = new Point(rect.X+rect.Width/2 -15, rect.Y+rect.Height/2 -10);
			if(rect.Width > 100)
				center.Y -= 15;
			if(rect.Height > 100)
				center.X += 15;
			g.DrawString(weight.ToString(), new Font("Arial",14),new SolidBrush(Color.Black), center);
		}

		public override bool Equals(object obj)
		{
			if(obj == null) return false;
			Edge edge = obj as Edge;
			return (this.a.Equals(edge.a) && this.b.Equals(edge.b)) ||
					(this.a.Equals(edge.b) && this.b.Equals(edge.a));
		}
		public bool strokeEquals(Stroke s)
		{	
			if(s == null) return false;
			Point[] p = {s.GetPoint(0), s.GetPoint(s.PacketCount-1)};
			return (p[0].Equals(this.a.CenterPoint) && p[1].Equals(this.b.CenterPoint)) ||
				   (p[0].Equals(this.b.CenterPoint) && p[1].Equals(this.a.CenterPoint));
		}

	}
}
