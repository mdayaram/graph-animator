using System;
using System.Drawing;
using System.Collections;
using Microsoft.Ink;

namespace GraphAnimator
{

	public class StrokeManager
	{
						//How close a stroke needs to be in order to count as a hit.
		private static int HIT_TEST_THRESHOLD = 2500;
						//Smallest size of a stroke in order to register a node or edge
		public static double SMALLEST_N_SIZE = 25,
						//How close the stroke can look like a circle in order to register as one
							CIRCLE_TOLERANCE = 700,
						//How close the stroke can look as a rectangle in order to register as one
							RECT_TOLERANCE = 1500,
						//How far apart the start of the stroke can be to the end of the stroke to register as a close stroke
							CLOSED_TOLERANCE = 1000,
						//Minimum width of a node
							MIN_WIDTH = 1000;

		public StrokeManager(){	}
		
		#region Node Analysis Methods

		/* Returns true if the stroke resembles a circle given a tolerance.
		 * Accomplishes this by taking the perimeter of a circle that 
		 * would be bounded in the area the stroke is bounded and
		 * comparing it to the length of the stroke.
		 */
		public static bool FitsCircleProperties(Stroke e, double tolerance)
		{
			if(e == null) return false;
			Rectangle r = e.GetBoundingBox();
			double radius = StrokeManager.Avg(r.Height,r.Width) / 2.0;
			if(radius < MIN_WIDTH/2) return false;
			double perimeter = 2.0*radius*Math.PI;
			Point[] points = e.GetPoints();
			double strokeLength = StrokeManager.StrokeLength(e);
			return Math.Abs(perimeter-strokeLength) <= tolerance;
		}
		public static bool FitsCircleProperties(Stroke e)
		{
			return FitsCircleProperties(e, CIRCLE_TOLERANCE);
		}

		/* Returns true if the stroke resembles a rectangle given a tolerance.
		 * Accomplishes this by taking the perimeter of a rectangle that 
		 * would be bounded in the area the stroke is bounded and
		 * comparing it to the length of the stroke.
		 */
		public static bool FitsRectProperties(Stroke e, double tolerance)
		{
			if (e == null || FitsCircleProperties(e,CIRCLE_TOLERANCE)) return false;
			Rectangle r = e.GetBoundingBox();
			if(r.Height < MIN_WIDTH || r.Width < MIN_WIDTH) return false;
			double perimeter = r.Height*2 + r.Width*2;
			double strokeLength = StrokeManager.StrokeLength(e);
			return Math.Abs(perimeter-strokeLength) <= tolerance;
		}
		public static bool FitsRectProperties(Stroke e)
		{
			return FitsRectProperties(e, RECT_TOLERANCE);
		}

		/* Returns true if the stroke is somewhat closed in the
		 * sense that it comes back to itself.  Accomplishes
		 * this by checking the distance between the first point
		 * in the stroke and the last point in the stroke.
		 */
		public static bool isClosed(Stroke s, double tolerance)
		{
			if(s == null) 
				return false;
			Point[] pts = {s.GetPoint(0), s.GetPoint(s.PacketCount-1)};
			return StrokeManager.Distance(pts[0], pts[1]) <= tolerance;
		}
		public static bool isClosed(Stroke e)
		{
			return isClosed(e, CLOSED_TOLERANCE);
		}

		#endregion

		/* Several overloads of an average function that returns
		 * the average of a set of data.
		 */
		#region Average function
		public static double Avg(double[] arr)
		{
			double sum = 0;
			foreach(double i in arr)
			{
				sum += i;
			}
			return sum/arr.Length;
		}
		public static double Avg(int[] arr)
		{ 
			double[] darr = new double[arr.Length];
			for(int i=0; i<arr.Length; i++)
			{
				darr[i] = (double)arr[i];
			}
			return Avg(darr);
		}
		public static double Avg(double a, double b)
		{
			double[] arr = {a, b};
			return Avg(arr);
		}
		public static double Avg(int a, int b)
		{
			double[] arr = {(double)a, (double)b};
			return Avg(arr);
		}
		#endregion

		#region Make Circular and Rectangular Strokes
		
		/* Makes a perfectly circular stroke given another
		 * stroke by using its bounding area as a reference for
		 * radius.
		 */
		public static Stroke makeCircle(InkOverlay i, Stroke s)
		{
			Point[] points = new Point[101];
			double theta = 2*Math.PI/100.0;
			Rectangle r = s.GetBoundingBox();
			double radius = (r.Width + r.Height)/4.0;
			int deltaX = r.X + r.Width/2;
			int deltaY = r.Y + r.Height/2;
			for(int k = 0; k < points.Length - 1; k++)
			{
				int x = (int) (radius*Math.Cos(k*theta) + deltaX);
				int y = (int) (radius*Math.Sin(k*theta) + deltaY);

				points[k] = new Point(x, y);
			}
			//Last point is the first point, i.e. it is a closed stroke
			points[100] = new Point(points[0].X, points[0].Y);
			Stroke circle = s.Ink.CreateStroke(points);
			circle.DrawingAttributes = i.DefaultDrawingAttributes;
			return circle;
		}

		/* Makes a perfectly rectangular stroke out of the 
		 * given stroke by using its bounding area.
		 */
		public static Stroke makeRect(InkOverlay i, Stroke s)
		{
			Rectangle r = s.GetBoundingBox();
			Point[] points = new Point[5];
			points[0] = new Point(r.X, r.Y + r.Height);
			points[1] = new Point(r.X + r.Width, points[0].Y);
			points[2] = new Point(points[1].X, r.Y);
			points[3] = new Point(points[0].X, points[2].Y);
			points[4] = new Point(points[0].X, points[0].Y);
			Stroke rectangle = s.Ink.CreateStroke(points);
			rectangle.DrawingAttributes = i.DefaultDrawingAttributes;
			return rectangle;
		}
		#endregion

		#region Node Helper Methods

		/* Determines whether the given stroke was a tap
		 * on a node in Graph g and if so, returns that node
		 */
		public static Node TappedNode(Stroke s, Graph g)
		{
			if(s.PacketCount > 25) return null;
			Point[] points = {s.GetPoint(0),s.GetPoint(s.PacketCount-1)};
			foreach(Node n in g.Nodes)
			{
				Rectangle r = n.Stroke.GetBoundingBox();
				if(r.Contains(points[0]) && r.Contains(points[1]))
					return n;
			}
			return null;
		}
		
		/* Checks if the given stroke is within the bounds of a 
		 * Node.  It accomplishes this by using the stroke hit test.
		 */
		public static Node HitNodeTest(Stroke s, Graph g)
		{
			Rectangle rectS = s.GetBoundingBox();
			Point sCenter = new Point(rectS.X + rectS.Width/2, rectS.Y + rectS.Height/2);
			for(int i=0; i<g.Nodes.Length(); i++)
			{
				Node n = g.Nodes[i];
				Rectangle rectN =n.Stroke.GetBoundingBox();
				if(s.HitTest(n.CenterPoint, (float)Math.Max(rectN.Height/2.0, rectN.Width/2.0))
					||n.Stroke.HitTest(sCenter, (float)Math.Max(rectS.Height/2.0, rectS.Width/2.0)))
				{
					return n;
				}
			}
			return null;
		}

		#endregion

		#region Edge Helper Methods

		/* Determines whether the following stroke could be interpreted as 
		 * an edge (or edges) and if so, returns all the consecutive nodes that the
		 * stroke has gone through in order.
		 */
		public static Nodes ifEdgeGetNodes(Stroke s, Graph g)
		{
			Point[] sPoints = s.GetPoints();
			Nodes strokeHitNodes = new Nodes();
			Node prev = null;
			for(int i=0; i<sPoints.Length; i++)
			{
				for(int j=0; j<g.Nodes.Length(); j++)
				{
					Node n = g.Nodes[j];
					Rectangle r = n.Stroke.GetBoundingBox();
				
					if(s.HitTest(n.CenterPoint,Math.Max(r.Width/2, r.Height/2)) && r.Contains(sPoints[i]) && !n.Equals(prev))
					{
						strokeHitNodes.Add(n);
						prev = n;
						break;
					}
				}
			}
			//If stroke hit one or less nodes, it is clearly not an edge.
			if(strokeHitNodes.Length() < 2)
			{
				return null;
			}
			return strokeHitNodes;
		}

		/* Checks if the stroke drawn is within the range of a
		 * edge from Graph g.  The range of an edge is roughly
		 * around its center.
		 */
		public static Edge HitEdgeTest(Stroke s, Graph g)
		{
			if(StrokeManager.StrokeLength(s) > HIT_TEST_THRESHOLD) return null;
			float distance = 1300;
			Edge hitedge = null;
			for(int i=0; i<g.Edges.Length(); i++)
			{
				float tmp;
				Edge e = g.Edges[i];
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

		/* Gets all the objects, nodes and edges, from graph g
		 * hit by this stroke and returns an arraylist containing them all
		 */
		public static ArrayList HitObjects(Stroke s, Graph g)
		{
			ArrayList objs = new ArrayList();
			Point[] points = s.GetPoints();			
			for(int j=0; j<g.Edges.Length(); j++)
			{
				for(int i=0; i<points.Length; i++)
				{
					if(g.Edges[j].Stroke.HitTest(points[i],1))
					{
						objs.Add(g.Edges[j]);
					}
				}
			}
			for(int j=0; j<g.Nodes.Length(); j++)
			{
				for(int i=0; i<points.Length; i++)
				{
					if(g.Nodes[j].Stroke.HitTest(points[i],1))
					{
						objs.Add(g.Nodes[j]);
					}
				}
			}
			return objs;
		}

        #endregion

		#region Random Stroke Helper Methods
		
		//Creates a deep copy of the stroke s
		public static Stroke CopyStroke(Stroke s)
		{
			Point[] p = s.GetPoints();
			return s.Ink.CreateStroke(p);
		}
		
		//Gets the distance between two points
		public static double Distance(Point a, Point b)
		{
			double x = a.X - b.X;
			double y = a.Y - b.Y;
			return Math.Abs(Math.Sqrt(x*x + y*y));
		}

		//Gets the length of a stroke
		public static double StrokeLength(Stroke e)
		{
			double dist = 0;
			Point[] points = e.GetPoints();
			for(int i=0; i<points.Length-1; i++)
			{
				dist += StrokeManager.Distance(points[i], points[i+1]);
			}
			return dist;
		}

		/* Determines whether the stroke could be interpreted as
		 * a star by checking if the stroke intersects itself at 
		 * least three times and that it is closed.
		 */
		public static bool isStar(Stroke s)
		{
			Console.WriteLine("Count; "+s.SelfIntersections.Length);
            return s.SelfIntersections.Length > 0;// 3 && s.SelfIntersections.Length < 20 && StrokeManager.isClosed(s, 600);
		}

		/* Determines whether the stroke coule be interpreted as a
		 * scratchout by checking that the length is greater than 
		 * three times the width of its bounding box, that it has 
		 * at least three self intersections, and its height is at 
		 * most three quarters of its width.
		 */
		public static bool isScratchOut(Stroke s)
		{
			Rectangle rect = s.GetBoundingBox();
			double length = StrokeManager.StrokeLength(s);
			return length > 3*rect.Width && rect.Height <= rect.Width*3/4 && s.SelfIntersections.Length > 2 ;
		}
	
		//Necessary conversion for rendering strokes
		public static Rectangle InkSpaceToPixelRect(InkOverlay i, Graphics g, Rectangle rect)
		{
			Point ul = new Point(rect.Left,rect.Top);
			Point lr = new Point(rect.Right,rect.Bottom);
			Point[]	pts	= {	ul,	lr };
			i.Renderer.InkSpaceToPixel(g, ref pts);

			ul = pts[0]; lr	= pts[1];
			Rectangle r = new Rectangle(ul.X, ul.Y, lr.X-ul.X,	lr.Y-ul.Y);
			return r;

		}
		#endregion

	}
}
