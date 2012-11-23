using System;
using System.Drawing;
using System.Collections;
using Microsoft.Ink;

namespace GraphAnimator
{

	public class StrokeManager
	{
		public static double SMALLEST_N_SIZE = 25,
							CIRCLE_TOLERANCE = 700,
							RECT_TOLERANCE = 1500,
							CLOSED_TOLERANCE = 1000;

		public StrokeManager(){	}
		
		#region Node Analysis Methods
		public static bool FitsCircleProperties(Stroke e, double tolerance)
		{
			if(e == null) return false;
			Rectangle r = e.GetBoundingBox();
			double radius = r.Height / 2.0;
			double perimeter = 2.0*radius*Math.PI;
			Point[] points = e.GetPoints();
			double distance = 0;
			for(int i=0; i<points.Length-1; i++)
			{
				distance += StrokeManager.distance(points[i], points[i+1]);
			}
			return Math.Abs(perimeter-distance) <= tolerance;
		}
		public static bool FitsCircleProperties(Stroke e)
		{
			return FitsCircleProperties(e, CIRCLE_TOLERANCE);
		}

		public static bool FitsRectProperties(Stroke e, double tolerance)
		{
			if (e == null || e.SelfIntersections.Length > 1 || FitsCircleProperties(e,CIRCLE_TOLERANCE)) return false;
			Rectangle r = e.GetBoundingBox();
			double perimeter = r.Height*2 + r.Width*2;
			Point[] points = e.GetPoints();
			double distance = 0;
			for(int i=0; i<points.Length-1; i++)
			{
				distance += StrokeManager.distance(points[i], points[i+1]);
			}
			return Math.Abs(perimeter-distance) <= tolerance;
		}
		public static bool FitsRectProperties(Stroke e)
		{
			return FitsRectProperties(e, RECT_TOLERANCE);
		}

		public static bool isClosed(Stroke s, double tolerance)
		{
			if(s == null) 
				return false;
			Point[] pts = {s.GetPoint(0), s.GetPoint(s.PacketCount-1)};
			return distance(pts[0], pts[1]) <= tolerance;
		}
		public static bool isClosed(Stroke e)
		{
			return isClosed(e, CLOSED_TOLERANCE);
		}

		#endregion
		public static double distance(Point a, Point b)
		{
			double x = a.X - b.X;
			double y = a.Y - b.Y;
			return Math.Abs(Math.Sqrt(x*x + y*y));
		}

		#region Make Circular and Rectangular Strokes
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
			points[100] = new Point(points[0].X, points[0].Y);
			Stroke circle = s.Ink.CreateStroke(points);
			circle.DrawingAttributes = i.DefaultDrawingAttributes;
			return circle;
		}
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

		#region Edge Helper Methods
		public static Node[] ifEdgeGetNodes(Stroke s, Nodes nodes)
		{
			Point[] sPoints = {s.GetPoint(0),s.GetPoint(s.PacketCount-1)};
			Nodes strokeHitNodes = new Nodes();
			foreach(Node n in nodes)
			{
				Rectangle r = n.Stroke.GetBoundingBox();
				
				if(s.HitTest(n.CenterPoint,Math.Max(r.Width/2, r.Height/2)))
				{
					if(r.Contains(sPoints[0]))
					{
						strokeHitNodes.Add(n);
					}
					else if(r.Contains(sPoints[1]))
					{
						strokeHitNodes.Add(n);
					}
				}
			}
			Node[] nodesHit = strokeHitNodes.ToArray();
			if(nodesHit.Length != 2)
			{
				return null;
			}
			return nodesHit;
		}
        #endregion

		#region Random Stroke Helper Methods

		public static bool isStar(Stroke s)
		{
			return s.SelfIntersections.Length > 3 && StrokeManager.isClosed(s, 400);
		}
	
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
