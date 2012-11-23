using System;
using Microsoft.Ink;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GraphAnimator
{
	public class Nodes
	{
		
		private ArrayList list;

		public Nodes(){list = new ArrayList();}
		public Nodes(int capacity) {list = new ArrayList(capacity);}
		public Nodes(ArrayList list) {this.list = (ArrayList)list.Clone();}

		public Node[] ToArray()
		{
			return (Node[])list.ToArray (typeof(Node));
		}

		public Nodes Clone()
		{
			return (Nodes) new Nodes(list);
		}

		public Node this[int index]
		{
			get
			{
				return (Node)list[index];
			}
			set
			{
				list[index] = value;
			}
		}

		public Node getNode(Stroke s)
		{
			foreach(Node n in list)
			{
				if(s.Id == n.Id)
					return n;
			}
			return null;
		}
		public Node[] getNodes(Strokes strokes)
		{
			Nodes found = new Nodes();
			foreach(Stroke s in strokes)
			{
				Node a = getNode(s);
				if(a != null)
				{
					found.Add(a);
				}
			}
			return (Node[])found.ToArray();
		}

		public void Add(Node n)
		{
			list.Add(n);
		}

		public void Clear()
		{
			list.Clear();
		}
		
		public void Remove(Node a)
		{
			list.Remove(a);
		}

		public void Remove(int id)
		{
			foreach(Node n in list)
			{
				if(n.Id == id)
				{
					list.Remove(n);
				}
			}
		}

		public bool Contains(Node n)
		{
			foreach(Node a in list)
			{
				if(a.Equals(n))
					return true;
			}
			return false;
		}

		public int Length()
		{
			return list.Count;
		}

		public Node getTappedNode(Stroke s)
		{
			if(s.PacketCount > 25) return null;
			Point[] points = {s.GetPoint(0),s.GetPoint(s.PacketCount-1)};
			foreach(Node n in list)
			{
				Rectangle r = n.Stroke.GetBoundingBox();
				if(r.Contains(points[0]) && r.Contains(points[1]))
					return n;
			}
			return null;
		}

		public Node HitNodeTest(Stroke s)
		{
			Rectangle rectS = s.GetBoundingBox();
			Point sCenter = new Point(rectS.X + rectS.Width/2, rectS.Y + rectS.Height/2);
			foreach(Node n in list)
			{
				Rectangle rectN =n.Stroke.GetBoundingBox();
				if(s.HitTest(n.CenterPoint, (float)Math.Max(rectN.Height/2.0, rectN.Width/2.0))
					||n.Stroke.HitTest(sCenter, (float)Math.Max(rectS.Height/2.0, rectS.Width/2.0)))
				{
					return n;
				}
			}
			return null;
		}

		public IEnumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}
	}
}
