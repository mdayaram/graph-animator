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

		//Get the node corresponding to the given stroke in this collection
		public Node getNode(Stroke s)
		{
			for(int i=0; i<list.Count; i++)
			{
				if(s.Id == ((Node)list[i]).Id)
					return list[i] as Node;
			}
			return null;
		}
		//Get the nodes corresponding to the following strokes
		public Nodes getNodes(Strokes strokes)
		{
			Nodes found = new Nodes();
			for(int i=0; i<strokes.Count; i++)
			{
				Node a = getNode(strokes[i]);
				if(a != null)
				{
					found.Add(a);
				}
			}
			return found;
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
			for(int i=0; i<list.Count; i++)
			{
				if(((Node)list[i]).Id == id)
				{
					list.Remove(list[i]);
				}
			}
		}

		public bool Contains(Node n)
		{
			for(int i=0; i<list.Count; i++)
			{
				if(((Node)list[i]).Equals(n))
					return true;
			}
			return false;
		}

		public int Length()
		{
			return list.Count;
		}

		//Need to use "foreach" loop
		public IEnumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}
	}
}
