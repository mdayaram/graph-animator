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

		private Nodes popped;
		private IPriorityQueue pq;
		private Nodes nodes, pushing;
		private Canvas c;
		private Thread t;
		private Node popNode, home, destination;
		private int stepCount;
		
		public DijkstraAnimation(Nodes nodes, Node home, Node destination, Canvas c) : base()
		{
			popped = new Nodes();
			pushing = new Nodes();
			pq = new BinaryPriorityQueue(Node.sortAscending());
			setUpNew(nodes.Clone(), home, destination);
			this.c = c;
			t = new Thread(new ThreadStart(Run));
		}
		private void setUpNew(Nodes nodes, Node home, Node destination)
		{
			this.nodes = nodes;
			foreach(Node n in nodes)
			{
				n.Distance = Int32.MaxValue;
				n.Incoming = null;
			}
			this.home = home;
			this.destination = destination;
			this.home.Distance = 0;
			t = new Thread(new ThreadStart(Run));
			stepCount = 0;
			pq.Clear();
			pq.Push(home);
			popped.Clear();
			pushing.Clear();
		}

		#region Random Helper Methods

		private void updateText()
		{
			foreach(Node n in nodes)
			{
				string text = (n.Distance == Int32.MaxValue) ? INFINITY : n.Distance.ToString();
				n.Text = text;
			}
		}

		private string getDistance(int i)
		{
			return (i == Int32.MaxValue) ? INFINITY : i.ToString();
		}

		#endregion

		#region Animation Helper Methods

		private void finalize(bool wasFound)
		{
			if(wasFound) 
			{
				MessageBox.Show("Destination Node Found!","Found!");
			}
			else
			{
				MessageBox.Show("Destination Node not Found!","Not Found!");
			}

		}

		private void step1()
		{
			updateText();
			if(pq.Count <= 0) 
			{
				stepCount = -1;
				finalize(false);
				return;
			}
			popNode = pq.Pop() as Node;
			popNode.Color = Color.Cyan;

			if(popNode.Incoming != null)
				popNode.Incoming.Color = Color.Blue;

			popped.Add(popNode);
			if(popNode.Equals(destination))
			{
				stepCount = -1;
				finalize(true);
				return;
			}
			stepCount = 1;
		}
		private void step2()
		{
			pushing.Clear();
			foreach(Edge edge in popNode.Edges)
			{
				Node n = Node.GetOther(popNode, edge);
				if(popped.Contains(n)) continue;
				edge.Color = Color.Salmon;
				n.Proposed = edge.Weight + Node.GetOther(n,edge).Distance;
				n.Incoming = edge;
				n.Text = edge.Weight.ToString()+"+"+popNode.Distance.ToString();
				n.Color = Color.Salmon;
				pushing.Add(n);
			}
			stepCount = 2;
		}
		private void step3()
		{
			if(pushing.Length() <= 0)
			{
				step4();
				return;
			}
			foreach(Node n in pushing)
			{
				if(n.Proposed < n.Distance)
				{
					n.Text = getDistance(n.Distance)+">"+n.Proposed;
					n.Distance = n.Proposed;
				}
				else
				{
					n.Text = getDistance(n.Distance)+"<="+n.Proposed;
					n.Incoming = null;
				}
			}
			stepCount = 3;
		}
		private void step4()
		{
			updateText();
			foreach(Node n in pushing)
			{
				if(!pq.Contains(n))
				{
					pq.Push(n);
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
			foreach(Node n in nodes)
			{
				n.Color = Node.DEFAULT;
				n.TextColor = Node.TEXT_DEFAULT;
				n.Text = "";
				foreach(Edge e in n.Edges)
				{
					e.Color = Edge.DEFAULT;
				}
			}
			setUpNew(nodes, home, destination);
		}

		public override void StepBack()
		{

		}
		#endregion

	}
}
