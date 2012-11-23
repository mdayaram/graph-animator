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

		public Edge(Node n1, Node n2, InkOverlay i, int weight)
		{
			//Keep track of the nodes its attached to
			this.a = n1;
			this.b = n2;
			this.weight = weight;
			color = DEFAULT;
			//Make sure the nodes contain this in their edges
			a.Edges.Add(this);
			b.Edges.Add(this);
			Point[] p = {a.CenterPoint,b.CenterPoint};
			//Draw a stroke connecting both nodes
			this.stroke = i.Ink.CreateStroke(p);
		}
		public Edge(Node n1, Node n2, InkOverlay i) : this(n1,n2,i,Edge.DEFAULT_WEIGHT) {}

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
		
		//Checks if the two nodes have an edge connecting them
		public static bool hasEdge(Node a, Node b)
		{
			for(int i=0; i<a.Edges.Length(); i++)
			{
				if(a.Edges[i].NodeA.Equals(b) || a.Edges[i].NodeB.Equals(b))
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
		//Checks if the given stroke is equal to this edge
		public bool strokeEquals(Stroke s)
		{
			return this.Stroke.Id == s.Id;
		}

	}
}
