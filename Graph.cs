using System;
using System.Drawing;
using System.Collections;
using Microsoft.Ink;

namespace GraphAnimator
{
	public class Graph
	{
		private Nodes nodes;
		private Edges edges;
		private Node home;
		private Node destination;
		public Graph():this(new Nodes(), new Edges()){}
		public Graph(Nodes n, Edges e)
		{
			nodes = n;
			edges = e;
			home = null;
			destination = null;
		}

		#region Accessor Methods
		public Node Home
		{
			get{return home;}
		}
		public Node Destination
		{
			get{return destination;}
		}
		public Nodes Nodes
		{
			get{return nodes;}
		}
		public Edges Edges
		{
			get{return edges;}
		}
		#endregion

		public void Add(object obj)
		{
			if(obj is Node)
			{
				nodes.Add((Node)obj);
			}
			else if(obj is Edge)
			{
				//Don't add edges that have no parent nodes
				Edge e = obj as Edge;
				if(e.NodeA != null && e.NodeB != null)
					edges.Add(e);
			}
		}

		private void RemoveNode(Node dead)
		{
			if(dead.Equals(home)) home = null;
			if(dead.Equals(destination)) destination = null;
			while(dead.Edges.Length()>0)
			{
				Edge e = dead.Edges[dead.Edges.Length()-1];
				RemoveEdge(e);
			}
			nodes.Remove(dead);
			dead.Stroke.Ink.DeleteStroke(dead.Stroke);
		}
		private void RemoveEdge(Edge e)
		{
			e.NodeA.Edges.Remove(e);
			e.NodeB.Edges.Remove(e);
			edges.Remove(e);
			e.Stroke.Ink.DeleteStroke(e.Stroke);
		}
		public void Remove(object obj)
		{
			if(obj is Node)
			{
				RemoveNode(obj as Node);
			}
			else if(obj is Edge)
			{
				RemoveEdge(obj as Edge);
			}
		}
		
		/* Assigns the following node to either home or destination
		 * depending on what was previously assigned
		 */
		public void AssignNode(Node n)
		{
			if(n.Equals(destination))
			{
				destination = null;
				home = n;
			}
			else if(home == null)
			{
				home = n;
			}
			else if(destination == null)
			{
				destination = n;
			}
			else
			{

				Node tmp = destination;
				destination = n;
				home = tmp;
			}
		}

		public void Clear()
		{
			nodes.Clear();
			edges.Clear();
			home = destination = null;
		}
		
		//Renders each edge and node and also marks the home and destination nodes
		public void Render(InkOverlay i, Graphics g)
		{
			for(int j=0; j<edges.Length(); j++)
			{
				edges[j].Render(i,g);
			}
			for(int j=0; j<nodes.Length(); j++)
			{
				nodes[j].Render(i, g);
				if(nodes[j].Equals(home))
				{
					g.DrawString("*",new Font("Arial",30),new SolidBrush(Color.Green),StrokeManager.InkSpaceToPixelRect(i, g,nodes[j].Stroke.GetBoundingBox()));
				}
				else if(nodes[j].Equals(destination))
				{
					g.DrawString("X",new Font("Arial",14,FontStyle.Bold) ,new SolidBrush(Color.Red),StrokeManager.InkSpaceToPixelRect(i,g,nodes[j].Stroke.GetBoundingBox()));
				}
			}
		}


	}
}
