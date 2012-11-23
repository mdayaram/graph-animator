using System;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using System.Drawing;
using Microsoft.Ink;

namespace GraphAnimator
{

	public class DijkstraAnimation : Animation
	{
		
		public static int TIME_STEP = 1500;
		public static string INFINITY = "Inf";
		//Color codes for different parts.
		public static Color INQUEUE_N = Color.Salmon,
							INQUEUE_E = Color.Salmon,
							POPPED_N = Color.SteelBlue,
							POPPED_E = Color.SteelBlue,
							FOUND_N = Color.Cyan,
							FOUND_E = Color.Blue;

		private Graph graph;
		private IPriorityQueue pq;
		private Nodes popped, pushing;
		private Hashtable incomingEdge, proposedWeight;
		private Canvas c;
		private Thread t;
		private Node popNode;  //Node that has just been popped.
		private int stepCount;
		
		public DijkstraAnimation(Canvas c) : base()
		{
			popped = new Nodes();
			pushing = new Nodes();
			incomingEdge = new Hashtable();
			proposedWeight = new Hashtable();
			pq = new BinaryPriorityQueue(Node.sortAscending());
			graph = new Graph(); //empty graph until initialized.
			this.c = c;
			t = new Thread(new ThreadStart(Run));
		}
		public override void Initialize(Graph g)
		{
			graph = g;
			pq.Clear();
			popped.Clear();
			pushing.Clear();
			incomingEdge.Clear();
			proposedWeight.Clear();
			for(int i=0; i<graph.Nodes.Length(); i++)
			{
				Node n = graph.Nodes[i];
				n.Distance = int.MaxValue;
				incomingEdge.Add(n,null);
				proposedWeight.Add(n,int.MaxValue);
			}
			//graph.Home.Distance = 0;
			t = new Thread(new ThreadStart(Run));
			stepCount = 0;
			//pq.Push(graph.Home);
		}

		#region Random Helper Methods

		private void updateText()
		{
			foreach(Node n in graph.Nodes)
			{
				string text = (n.Distance == Int32.MaxValue) ? INFINITY : n.Distance.ToString();
				n.Text = text;
			}
		}

		//Returns a string of either the distance of the value of our infinity const
		private string getDistance(int i)
		{
			return (i == int.MaxValue) ? INFINITY : i.ToString();
		}

		#endregion

		#region Animation Helper Methods

		/* Called at the end of the animation, 
		 * determines whether destination was or 
		 * wasn't found and colors nodes appropriately.
		 */
		private void finalize(bool wasFound)
		{
			if(wasFound) 
			{
				Node tmp = graph.Destination;
				do
				{
					tmp.Color = FOUND_N;
					if(incomingEdge[tmp] == null) break;
					Edge edge = incomingEdge[tmp] as Edge;
					edge.Color = FOUND_E;
					tmp = Node.GetOther(tmp, edge);
				}while(tmp != null);
				MessageBox.Show("Destination Node Found!","Found!");
			}
			else
			{
				MessageBox.Show("Destination Node not Found!","Not Found!");
			}

		}

		/* Pops a node from the PQ and checks whether it is 
		 * the destination node or was the last node in the PQ
		 * and terminates the animation if necessary.  Colors
		 * popNode and its incoming edge as appropriate.
		 */
		private void step1()
		{
			if(!pq.Contains(graph.Home) && !popped.Contains(graph.Home))
			{
				graph.Home.Distance = 0;
				pq.Add(graph.Home);
			}
			updateText();
			if(pq.Count <= 0) 
			{
				stepCount = -1;
				finalize(false);
				return;
			}
			popNode = pq.Pop() as Node;
			popNode.Color = POPPED_N;

			if(incomingEdge[popNode] != null)
				((Edge)incomingEdge[popNode]).Color = POPPED_E;

			popped.Add(popNode);
			if(popNode.Equals(graph.Destination))
			{
				stepCount = -1;
				finalize(true);
				return;
			}
			stepCount = 1;
		}

		/* Takes all the edges from popNode and adds the 
		 * connecting nodes to list "pushing" which are nodes
		 * that are about to be pushed to the PQ.  Also adjusts 
		 * incoming edge if necessary and adjusts colors appropriately.
		 */
		private void step2()
		{
			pushing.Clear();
			for(int i=0; i<popNode.Edges.Length(); i++)
			{
				Edge edge = popNode.Edges[i];
				Node n = Node.GetOther(popNode, edge);
				if(popped.Contains(n)) continue;
				edge.Color = INQUEUE_E;
				proposedWeight[n] = edge.Weight + Node.GetOther(n,edge).Distance;
				if((int)proposedWeight[n] < n.Distance)
					incomingEdge[n] = edge;
				n.Text = edge.Weight.ToString()+"+"+popNode.Distance.ToString();
				n.Color = INQUEUE_N;
				pushing.Add(n);
			}
			stepCount = 2;
		}
		
		/* Performs the comparison for the nodes in 
		 * the pushing list.  Checks whether each
		 * node's proposed weight is smaller than its 
		 * current distance and displays the text of
		 * this comparison.
		 */
		private void step3()
		{
			if(pushing.Length() <= 0)
			{
				stepCount = 0;
				step1();
				return;
			}
			for(int i=0; i<pushing.Length(); i++)
			{
				Node n = pushing[i];
				string dist = getDistance(n.Distance);
				if((int)proposedWeight[n] < n.Distance)
				{
					n.Text = dist+">"+proposedWeight[n];
					n.Distance = (int)proposedWeight[n];
				}
				else if((int)proposedWeight[n] > n.Distance)
				{
					n.Text = dist+"<"+proposedWeight[n];
				}
				else
				{
					n.Text = dist+"="+proposedWeight[n];
				}
			}
			stepCount = 3;
		}

		//Adds the nodes in pushing to the PQ.
		private void step4()
		{
			updateText();
			for(int i=0; i<pushing.Length(); i++)
			{
				if(!pq.Contains(pushing[i]))
				{
					pq.Push(pushing[i]);
				}
			}
			stepCount = 0;
		}

		#endregion

		#region Animation methods
		public override void Step()
		{
			if(stepCount == 0)
				step1();
			else if (stepCount == 1)
				step2();
			else if(stepCount == 2)
				step3();
			else if(stepCount == 3)
				step4();
			c.Invalidate();
		}

		public override void Play()
		{
			t.Start();
		}
		public void Run()
		{
			while(stepCount >= 0)
			{
				Step();
				Thread.Sleep(TIME_STEP);
			}
		}

		public override void Pause()
		{
			t.Abort();
			t = new Thread(new ThreadStart(Run));
		}

		public override void Stop()
		{
			t.Abort();
			for(int i=0; i<graph.Nodes.Length(); i++)
			{
				Node n = graph.Nodes[i];
				n.Color = Node.DEFAULT;
				n.TextColor = Node.TEXT_DEFAULT;
				n.Text = "";
				foreach(Edge e in n.Edges)
				{
					e.Color = Edge.DEFAULT;
				}
			}
			Initialize(graph);
		}

		public override void StepBack()
		{
/*			if(stepCount == 0)
				stepBack1();
			else if (stepCount == 1)
				stepBack2();
			else if(stepCount == 2)
				stepBack3();
			else if(stepCount == 3)
				stepBack4();
			c.Invalidate();*/
		}

		public override string ToString()
		{
			return "Dijkstra's";
		}

		public override bool isPlayable()
		{
			return graph.Home != null && graph.Destination != null && graph.Nodes.Length() > 0;
		}



		#endregion

	}
}
