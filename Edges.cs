using System;
using Microsoft.Ink;
using System.Drawing;
using System.Collections;

namespace GraphAnimator
{
	public class Edges
	{
		private ArrayList list;

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
		
		//Gets the edge corresponding to the given stroke
		public Edge getEdge(Stroke s)
		{
			for(int i=0; i<list.Count; i++)
			{
				Edge e = list[i] as Edge;
				if(e.strokeEquals(s))
					return e;
			}
			return null;
		}
		//Gets the edges corresponding to the given strokes
		public Edges getEdges(Strokes strokes)
		{
			Edges found = new Edges();
			for(int i=0; i<strokes.Count; i++)
			{
				Edge e = getEdge(strokes[i]);
				if(e != null)
				{
					found.Add(e);
				}
			}
			return found;
		}

		public bool Contains(Edge e)
		{
			return list.Contains(e);
		}
		//Required in order to use a "foreach" loop
		public IEnumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}

	}
}
