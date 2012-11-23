using System;
using Microsoft.Ink;
using System.Drawing;
using System.Collections;

namespace GraphAnimator
{
	public class Edges
	{
		private ArrayList list;
		private static int HIT_TEST_THRESHOLD = 2500;

		public Edges() {list = new ArrayList();}
		public Edges(int capacity) {list = new ArrayList(capacity);}
		public Edges(ArrayList list) {this.list = (ArrayList)list.Clone();	}
		
		public Edge[] ToArray()
		{
			return (Edge[])list.ToArray (typeof(Edge));
		}

		public Edges Clone()
		{
			return (Edges) new Edges(list);
		}

		public Edge this[int index]
		{
			get
			{
				return (Edge)list[index];
			}
			set
			{
				list[index] = value;
			}
		}

		public void Add(Edge n)
		{
			list.Add(n);
		}

		public void Clear()
		{
			list.Clear();
		}
		
		public void Remove(Edge a)
		{
			list.Remove(a);
		}

		public int Length()
		{
			return list.Count;
		}

		public Edge getEdge(Stroke s)
		{
			foreach(Edge e in list)
			{
				if(s.Id == e.Stroke.Id)
					return e;
			}
			return null;
		}
		public Edge[] getEdges(Strokes strokes)
		{
			Edges found = new Edges();
			foreach(Stroke s in strokes)
			{
				Edge e = getEdge(s);
				if(e != null)
				{
					found.Add(e);
				}
			}
			return (Edge[])found.ToArray();
		}

		public bool Contains(Edge e)
		{
			return list.Contains(e);
		}

		public Edge HitEdgeTest(Stroke s)
		{
			if(StrokeManager.StrokeLength(s) > HIT_TEST_THRESHOLD) return null;
			float distance = 1300;
			Edge hitedge = null;
			foreach(Edge e in this)
			{
				float tmp;
				Rectangle rect = e.Stroke.GetBoundingBox();
				Point p = new Point(rect.X+rect.Width/2, rect.Y+rect.Height/2);
				s.NearestPoint(p, out tmp);
				if(tmp < distance)
				{
					distance = tmp;
					hitedge = e;
				}
			}
			return hitedge;
		}

		public IEnumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}

	}
}
