using System;
using Microsoft.Ink;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace GraphAnimator
{
	public class Edge
	{
		public static Color DEFAULT = Color.Black;

		public static int DEFAULT_WEIGHT = 10;
		private int weight;
		private Stroke stroke;
		private Node a, b;
		private Color color;

		public Edge(Node a, Node b, InkOverlay i, int weight)
		{
			this.a = a;
			this.b = b;
			this.weight = weight;
			color = DEFAULT;
			a.Edges.Add(this);
			b.Edges.Add(this);
			Point[] p = {a.CenterPoint,b.CenterPoint};
			this.stroke = i.Ink.CreateStroke(p);
		}
		public Edge(Node a, Node b, InkOverlay i) : this(a,b,i,Edge.DEFAULT_WEIGHT) {}

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
		public Stroke Stroke
		{
			get{return stroke;}
			set{stroke = value;}
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
			int width = weight.ToString().Length*15;
			int height = 18;
			stroke.DrawingAttributes = i.DefaultDrawingAttributes.Clone();
			stroke.DrawingAttributes.Color = color;
			i.Renderer.Draw(g,stroke);
			
			Rectangle rect = StrokeManager.InkSpaceToPixelRect(i,g,stroke.GetBoundingBox());
			Point center = new Point(rect.X+rect.Width/2 - width/2, rect.Y+rect.Height/2 - height/2);
			g.FillRectangle(new SolidBrush(Color.White),center.X,center.Y,width, height);
			g.DrawString(weight.ToString(), new Font("Courier New",14, FontStyle.Bold),new SolidBrush(Color.Black), center);
		}

		public override bool Equals(object obj)
		{
			if(obj == null) return false;
			Edge edge = obj as Edge;
			return (this.Stroke.Id == edge.Stroke.Id);
		}
		public bool strokeEquals(Stroke s)
		{
			return this.Stroke.Id == s.Id;
		}

	}
}
