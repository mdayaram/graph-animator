using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Timers;
using Microsoft.Ink;


namespace GraphAnimator
{

	public class Canvas : System.Windows.Forms.Form
	{
		private const int PLAY = 7, PAUSE = 8,
						  DIJKSTRA = 0;

		private System.ComponentModel.IContainer components;
		private InkOverlay inkOverlay;
		private string PATH = Directory.GetCurrentDirectory();
		private const int TIME_INTERVAL = 1000;
		private Nodes nodes;
		private Edges edges;
		private Node home, destination;
		private Animation anim;
		private RecognizerContext myRecognizer;
		private bool animStarted;
		private int animType;
		//**********************
		private int weight;
		private Edge prevEdgeHit;
		private System.Timers.Timer edgeTimer;
		//**********************

		#region Menu Items
		private System.Windows.Forms.ImageList imgMenu;
		private System.Windows.Forms.ToolBar toolBar1;
		private System.Windows.Forms.ToolBarButton toolBarButtonNew;
		private System.Windows.Forms.ToolBarButton toolBarButtonOpen;
		private System.Windows.Forms.ToolBarButton toolBarButtonSave;
		private System.Windows.Forms.ToolBarButton toolBarButtonSaveAs;
		private System.Windows.Forms.ToolBarButton toolBarButton5;
		private System.Windows.Forms.ToolBarButton toolBarButtonPen;
		private System.Windows.Forms.ToolBarButton toolBarButtonEraser;
		private System.Windows.Forms.ToolBarButton toolBarButtonSelection;
		private System.Windows.Forms.ToolBarButton toolBarButton9;
		private System.Windows.Forms.ToolBarButton toolBarButtonPlayPause;
		private System.Windows.Forms.ToolBarButton toolBarButtonStop;
		private System.Windows.Forms.ToolBarButton toolBarButtonStepBack;
		private System.Windows.Forms.ToolBarButton toolBarButtonStepForward;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.MainMenu mainMenu1;
		#endregion


		public Canvas()
		{

			InitializeComponent();
			
			nodes = new Nodes();
			edges = new Edges();
			animStarted = false;
			weight = -1;
			animType = -1;
			prevEdgeHit = null;

			// Declare repaint optimizations.
			base.SetStyle(
				ControlStyles.UserPaint|
				ControlStyles.AllPaintingInWmPaint|
				ControlStyles.DoubleBuffer,
				true);

			this.Paint += new PaintEventHandler(Canvas_Paint);
			inkOverlay = new InkOverlay(this.Handle, true); //attach to form, allow use of mouse for input
			inkOverlay.CollectionMode =	CollectionMode.InkOnly;	// allow ink only, no gestures

			inkOverlay.AutoRedraw =	false; // Dynamic rendering	only; we do	all	the	painting.

			DrawingAttributes da = new DrawingAttributes();
			da.AntiAliased = true;
			da.Transparency = 0;
			da.PenTip = Microsoft.Ink.PenTip.Ball;
			da.Width = 70.0f;
			inkOverlay.DefaultDrawingAttributes	= da;

			inkOverlay.Stroke += new InkCollectorStrokeEventHandler(inkOverlay_Stroke);
			inkOverlay.StrokesDeleting += new InkOverlayStrokesDeletingEventHandler(inkOverlay_StrokesDeleting);
			inkOverlay.SelectionMoved += new InkOverlaySelectionMovedEventHandler(inkOverlay_SelectionMoved);
			inkOverlay.SelectionResized +=new InkOverlaySelectionResizedEventHandler(inkOverlay_SelectionResized);
			inkOverlay.Enabled = true;

			/* Ugly hack to do number recognition on Edge only */
			myRecognizer = new RecognizerContext();
			int[] wIds = new int[0];
			myRecognizer.Strokes = inkOverlay.Ink.CreateStrokes(wIds);
			/* End of ugly hack */

			edgeTimer = new System.Timers.Timer(TIME_INTERVAL);
			edgeTimer.AutoReset = true;
			edgeTimer.Elapsed +=new ElapsedEventHandler(edgeTimer_Elapsed);

		}


		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}


		#region Windows Form Designer generated code
		private void InitializeComponent()
		{
			this.Closing +=new CancelEventHandler(Canvas_Closing);

			this.components = new System.ComponentModel.Container();
			this.imgMenu = new System.Windows.Forms.ImageList(this.components);
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.toolBar1 = new System.Windows.Forms.ToolBar();
			this.toolBarButtonNew = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonOpen = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSave = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSaveAs = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton5 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonPen = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonEraser = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSelection = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton9 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonPlayPause = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonStop = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonStepBack = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonStepForward = new System.Windows.Forms.ToolBarButton();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// imgMenu
			// 
			this.imgMenu.ImageSize = new System.Drawing.Size(16, 16);
			this.imgMenu.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// toolBar1
			// 
			this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						this.toolBarButtonNew,
																						this.toolBarButtonOpen,
																						this.toolBarButtonSave,
																						this.toolBarButtonSaveAs,
																						this.toolBarButton5,
																						this.toolBarButtonPen,
																						this.toolBarButtonEraser,
																						this.toolBarButtonSelection,
																						this.toolBarButton9,
																						this.toolBarButtonPlayPause,
																						this.toolBarButtonStop,
																						this.toolBarButtonStepBack,
																						this.toolBarButtonStepForward});
			this.toolBar1.DropDownArrows = true;
			this.toolBar1.ImageList = this.imageList1;
			this.toolBar1.Location = new System.Drawing.Point(0, 0);
			this.toolBar1.Name = "toolBar1";
			this.toolBar1.ShowToolTips = true;
			this.toolBar1.Size = new System.Drawing.Size(456, 28);
			this.toolBar1.TabIndex = 0;
			this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
			// 
			// toolBarButtonNew
			// 
			this.toolBarButtonNew.ImageIndex = 0;
			this.toolBarButtonNew.ToolTipText = "New";
			// 
			// toolBarButtonOpen
			// 
			this.toolBarButtonOpen.ImageIndex = 1;
			this.toolBarButtonOpen.ToolTipText = "Open";
			// 
			// toolBarButtonSave
			// 
			this.toolBarButtonSave.ImageIndex = 2;
			this.toolBarButtonSave.ToolTipText = "Save";
			// 
			// toolBarButtonSaveAs
			// 
			this.toolBarButtonSaveAs.ImageIndex = 3;
			this.toolBarButtonSaveAs.ToolTipText = "Save As";
			// 
			// toolBarButton5
			// 
			this.toolBarButton5.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButtonPen
			// 
			this.toolBarButtonPen.ImageIndex = 4;
			this.toolBarButtonPen.Pushed = true;
			this.toolBarButtonPen.ToolTipText = "Pen";
			// 
			// toolBarButtonEraser
			// 
			this.toolBarButtonEraser.ImageIndex = 5;
			this.toolBarButtonEraser.ToolTipText = "Eraser";
			// 
			// toolBarButtonSelection
			// 
			this.toolBarButtonSelection.ImageIndex = 6;
			this.toolBarButtonSelection.ToolTipText = "Selection";
			// 
			// toolBarButton9
			// 
			this.toolBarButton9.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButtonPlayPause
			// 
			this.toolBarButtonPlayPause.ImageIndex = 7;
			this.toolBarButtonPlayPause.ToolTipText = "Play";
			this.toolBarButtonPlayPause.Enabled = true;
			// 
			// toolBarButtonStop
			// 
			this.toolBarButtonStop.ImageIndex = 9;
			this.toolBarButtonStop.ToolTipText = "Stop";
			this.toolBarButtonStop.Enabled = true;
			// 
			// toolBarButtonStepBack
			// 
			this.toolBarButtonStepBack.ImageIndex = 10;
			this.toolBarButtonStepBack.ToolTipText = "Step Back";
			this.toolBarButtonStepBack.Enabled = true;
			// 
			// toolBarButtonStepForward
			// 
			this.toolBarButtonStepForward.ImageIndex = 11;
			this.toolBarButtonStepForward.ToolTipText = "Step Forward";
			this.toolBarButtonStepForward.Enabled = true;
			// 
			// imageList1
			// 
			this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// comboBox1
			// 
			this.comboBox1.Location = new System.Drawing.Point(288, 5);
			this.comboBox1.Name = "Animation";
			this.comboBox1.Size = new System.Drawing.Size(96, 21);
			this.comboBox1.TabIndex = 1;
			this.comboBox1.Text = "Algorithm";
			this.comboBox1.Items.Add("Dijkstra");  //const DIJKSTRA = 0;

			this.comboBox1.TextChanged += new EventHandler(comboBox1_TextChanged);
			//
			// Image List Files
			//
			imageList1.Images.Add(Image.FromFile(PATH+"\\imgs\\new.png"));
			imageList1.Images.Add(Image.FromFile(PATH+"\\imgs\\open.png"));
			imageList1.Images.Add(Image.FromFile(PATH+"\\imgs\\save.png"));
			imageList1.Images.Add(Image.FromFile(PATH+"\\imgs\\saveas.png"));
			imageList1.Images.Add(Image.FromFile(PATH+"\\imgs\\pen.png"));
			imageList1.Images.Add(Image.FromFile(PATH+"\\imgs\\eraser.png"));
			imageList1.Images.Add(Image.FromFile(PATH+"\\imgs\\lasso.png"));
			imageList1.Images.Add(Image.FromFile(PATH+"\\imgs\\play.png"));
			imageList1.Images.Add(Image.FromFile(PATH+"\\imgs\\pause.png"));
			imageList1.Images.Add(Image.FromFile(PATH+"\\imgs\\stop.png"));
			imageList1.Images.Add(Image.FromFile(PATH+"\\imgs\\stepb.png"));
			imageList1.Images.Add(Image.FromFile(PATH+"\\imgs\\stepf.png"));
			//
			// ToolBar Button Image Indexing
			//
			this.toolBarButtonNew.ImageIndex = 0;
			this.toolBarButtonOpen.ImageIndex = 1;
			this.toolBarButtonSave.ImageIndex = 2;
			this.toolBarButtonSaveAs.ImageIndex = 3;
			this.toolBarButtonPen.ImageIndex = 4;
			this.toolBarButtonEraser.ImageIndex = 5;
			this.toolBarButtonSelection.ImageIndex = 6;
			this.toolBarButtonPlayPause.ImageIndex = 7;
			this.toolBarButtonStop.ImageIndex = 9;
			this.toolBarButtonStepBack.ImageIndex = 10;
			this.toolBarButtonStepForward.ImageIndex = 11;
			// 
			// Canvas
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.ClientSize = new System.Drawing.Size(800, 600);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.toolBar1);
			this.HelpButton = true;
			this.KeyPreview = true;
			this.Menu = this.mainMenu1;
			this.Name = "Canvas";
			this.Text = "Graph Simulator";
			this.ResumeLayout(false);

		}
		#endregion


		[STAThread]
		static void Main() 
		{
			Application.Run(new Canvas());
		}


		#region Button Methods 

		private void newButton(object sender, System.EventArgs e)
		{
			nodes.Clear();
			edges.Clear();
			home = null;
			destination = null;
			this.inkOverlay.Ink.DeleteStrokes(inkOverlay.Ink.Strokes);
			penButton(sender, e);
			Invalidate();
		}
		private void openButton(object sender, System.EventArgs e)
		{
		
		}
		private void saveButton(object sender, System.EventArgs e)
		{
		
		}
		private void saveAsButton(object sender, System.EventArgs e)
		{
		
		}
		private void Canvas_Closing(object sender, CancelEventArgs e)
		{
			inkOverlay.Enabled = false;
			inkOverlay.Dispose();
			nodes.Clear();
			edges.Clear();
			home = destination = null;
			this.Dispose();
		}

		private void stepBackButton(object sender, System.EventArgs e)
		{
			if(anim == null) return;
			anim.StepBack();
			Invalidate();
		}

		private void stepForwardButton(object sender, System.EventArgs e)
		{
			if(anim == null) return;
			anim.Step();
			Invalidate();
		}

		private void NewAnimation()
		{
			switch (animType)
			{
				case DIJKSTRA:
					anim = new DijkstraAnimation(nodes, home, destination,this);
					break;
			}
		}

		private void playPauseButton(object sender, System.EventArgs e)
		{
			if(!isPlayable())
			{
				MessageBox.Show("Some conditions to start the animation are lacking.\n"+
					"Please make sure you have assigned home and destination\n"+
					"nodes and that you have selected an animation type from\n"+
					"the drop down menu.");
				return;
			}

			if(!animStarted)
			{
				NewAnimation();
				animStarted = true;
			}

			if(toolBarButtonPlayPause.ImageIndex == PLAY)
				anim.Play();
			else
				anim.Pause();
			
			togglePlayPause();
		}

		private void togglePlayPause()
		{
			switch(toolBarButtonPlayPause.ImageIndex)
			{
				case PLAY: //Play
					toolBarButtonPlayPause.ImageIndex = PAUSE; //Change to pause
					break;
				case PAUSE: //Pause
					toolBarButtonPlayPause.ImageIndex = PLAY; //Change to play
					break;
			}
		}
		private void togglePlayPause(int i)
		{
			if(i!=PLAY && i!=PAUSE) return;
			toolBarButtonPlayPause.ImageIndex = i;
		}

		private void stopButton(object sender, System.EventArgs e)
		{
			if(!isPlayable()) return;
			anim.Stop();
			animStarted = false;
			togglePlayPause(PLAY);
			Invalidate();
		}

		private void eraserButton(object sender, System.EventArgs e)
		{
			toolBarButtonEraser.Pushed = true;
			toolBarButtonPen.Pushed = false;
			toolBarButtonSelection.Pushed = false;
			setEditingMode(InkOverlayEditingMode.Delete);
		}
		private void penButton(object sender, System.EventArgs e)
		{
			toolBarButtonPen.Pushed = true;
			toolBarButtonEraser.Pushed = false;
			toolBarButtonSelection.Pushed = false;
			setEditingMode(InkOverlayEditingMode.Ink);
		}
		private void selectionButton(object sender, System.EventArgs e)
		{
			toolBarButtonSelection.Pushed = true;
			toolBarButtonPen.Pushed = false;
			toolBarButtonEraser.Pushed = false;
			setEditingMode(InkOverlayEditingMode.Select);
		}

		private void setEditingMode(InkOverlayEditingMode em)
		{
			if(inkOverlay.EditingMode == em) return;
			inkOverlay.Selection = inkOverlay.Ink.CreateStrokes();
			inkOverlay.EditingMode = em;
		}
			
		#endregion

		private void Canvas_Paint(object sender, PaintEventArgs e)
		{
			foreach(Edge edge in edges)
			{
				edge.Render(this.inkOverlay,e.Graphics);
			}
			foreach(Node n in nodes)
			{
				n.Render(this.inkOverlay, e.Graphics);
				if(n.Equals(home))
				{
					e.Graphics.DrawString("*",new Font("Arial",30),new SolidBrush(Color.Green),StrokeManager.InkSpaceToPixelRect(inkOverlay, e.Graphics,n.Stroke.GetBoundingBox()));
				}
				else if(n.Equals(destination))
				{
					e.Graphics.DrawString("X",new Font("Arial",14,FontStyle.Bold) ,new SolidBrush(Color.Red),StrokeManager.InkSpaceToPixelRect(inkOverlay, e.Graphics,n.Stroke.GetBoundingBox()));
				}
			}
			if(myRecognizer.Strokes.Count <= 0) return;
			foreach(Stroke s in myRecognizer.Strokes)
			{
				try
				{
					inkOverlay.Renderer.Draw(e.Graphics,s);
				}
				catch(System.Runtime.InteropServices.COMException err)
				{
					MessageBox.Show("Dammit, not again!\n\n"+ err.ToString());
				}
			}
		}
		#region Node and Edge Handlers
		private void AssignStarNode(Node n)
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

		private void removeNode(Node n)
		{
			
			nodes.Remove(n);
			foreach(Edge edge in n.Edges)
			{
				removeEdge(edge, n);
			}
			n.Edges.Clear();
			n.Stroke.Ink.DeleteStroke(n.Stroke);
			Invalidate();
		}
		private void removeEdge(Edge e)
		{
			removeEdge(e, null);
			Invalidate();
		}
		private void removeEdge(Edge e, Node keepNode)
		{
			edges.Remove(e);
			Node n = Node.GetOther(keepNode,e);
			if(n != null)
			{
				n.Edges.Remove(e);
			}
			else
			{
				e.NodeA.Edges.Remove(e);
				e.NodeB.Edges.Remove(e);
			}
			e.Stroke.Ink.DeleteStroke(e.Stroke);
		}
		#endregion


		#region Stroke Event Handlers

		private void inkOverlay_Stroke(object sender, InkCollectorStrokeEventArgs e)
		{	
			if(this.InvokeRequired) return;

			//edgeTimer.BeginInit();
			edgeTimer.Start();

			Node n = nodes.getTappedNode(e.Stroke);
			if(n != null)
			{
				if(inkOverlay.EditingMode == InkOverlayEditingMode.Delete)
				{
					removeNode(n);
				}
				else
				{
					int[] ids = {n.Stroke.Id};
					selectionButton(sender, e);
					inkOverlay.Selection = e.Stroke.Ink.CreateStrokes(ids);
				}
				e.Stroke.Ink.DeleteStroke(e.Stroke);
				Invalidate();
				return;
			}
	
			if(inkOverlay.EditingMode != InkOverlayEditingMode.Ink) return;
			
			n = nodes.HitNodeTest(e.Stroke);
			if(StrokeManager.isClosed(e.Stroke) && n != null && StrokeManager.isStar(e.Stroke))
			{
				AssignStarNode(n);
			}
			else if(StrokeManager.isClosed(e.Stroke) && n==null && e.Stroke.PacketCount > StrokeManager.SMALLEST_N_SIZE && StrokeManager.FitsCircleProperties(e.Stroke))
			{
				Stroke circle = StrokeManager.makeCircle(inkOverlay, e.Stroke);
				Node circleNode = new Node(circle, false);
				nodes.Add(circleNode);
			}
			else if(StrokeManager.isClosed(e.Stroke) && n==null && e.Stroke.PacketCount > StrokeManager.SMALLEST_N_SIZE && StrokeManager.FitsRectProperties(e.Stroke))
			{
				Stroke rect = StrokeManager.makeRect(inkOverlay, e.Stroke);
				Node rectNode = new Node(rect, true);
				nodes.Add(rectNode);
			}
			else if(!StrokeManager.isClosed(e.Stroke))
			{
				Node[] edgeNodes = StrokeManager.ifEdgeGetNodes(e.Stroke, nodes);
				if(edgeNodes != null)
				{
					for(int i=0; i<edgeNodes.Length-1; i++)
					{
						if(!Edge.hasEdge(edgeNodes[i],edgeNodes[i+1]))
						{
							Edge edge = new Edge(edgeNodes[i],edgeNodes[i+1],inkOverlay);
							edges.Add(edge);
						}
					}
				}
			}
			else
			{
				Edge hitEdge = edges.HitEdgeTest(e.Stroke);
				if(hitEdge != null)
				{
					if(prevEdgeHit == null) prevEdgeHit = hitEdge;
					if(hitEdge.Equals(prevEdgeHit))
					{
						myRecognizer.Strokes.Add(CopyStroke(e.Stroke));
					}
					else
					{
						RecognizeWeight();
						prevEdgeHit = hitEdge;
						myRecognizer.Strokes.Add(CopyStroke(e.Stroke));
					}		
				}
			}
			e.Stroke.Ink.DeleteStroke(e.Stroke);
			Invalidate();
		}

		private void inkOverlay_StrokesDeleting(object sender, InkOverlayStrokesDeletingEventArgs e)
		{
			if(this.InvokeRequired) return;
			Strokes strokes = e.StrokesToDelete;
			foreach(Stroke s in strokes)
			{   
				if(StrokeManager.isClosed(s,0))
				{
					Node n = nodes.getNode(s);
					removeNode(n);
					if(n.Equals(home)) home = null;
					if(n.Equals(destination)) destination = null;
				}
				else
				{
					foreach(Edge edge in edges)
					{
						if(edge.strokeEquals(s))
						{	
							removeEdge(edge);
							break;
						}
					}
				}
			}
			Invalidate();
		}

		private void inkOverlay_SelectionMoved(object sender, InkOverlaySelectionMovedEventArgs e)
		{
			if(this.InvokeRequired) return;
						
			Node[] selectedNodes = nodes.getNodes(inkOverlay.Selection);
			foreach(Node n in selectedNodes)
			{
				Rectangle r = n.Stroke.GetBoundingBox();
				n.CenterPoint = new Point(r.X+r.Width/2, r.Y+r.Height/2);
			}
			Edge[] selectedEdges = edges.getEdges(inkOverlay.Selection);
			foreach(Edge edge in selectedEdges)
			{
				Point[] p = {edge.NodeA.CenterPoint,edge.NodeB.CenterPoint};
				edge.Stroke.Ink.DeleteStroke(edge.Stroke);
				edge.Stroke = edge.Stroke.Ink.CreateStroke(p);
			}
			Invalidate();
		}
		private void inkOverlay_SelectionResized(object sender, InkOverlaySelectionResizedEventArgs e)
		{
			if(this.InvokeRequired) return;
						
			Node[] selectedNodes = nodes.getNodes(inkOverlay.Selection);
			foreach(Node n in selectedNodes)
			{
				Rectangle r = n.Stroke.GetBoundingBox();
				n.CenterPoint = new Point(r.X+r.Width/2, r.Y+r.Height/2);
			}
			Edge[] selectedEdges = edges.getEdges(inkOverlay.Selection);
			foreach(Edge edge in selectedEdges)
			{
				Point[] p = {edge.NodeA.CenterPoint,edge.NodeB.CenterPoint};
				edge.Stroke.Ink.DeleteStroke(edge.Stroke);
				edge.Stroke = edge.Stroke.Ink.CreateStroke(p);
			}
			Invalidate();
		}

		#endregion


		#region toolBar1 Button Handlers
		private void toolBar1_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if(e.Button.Equals(toolBarButtonNew))
				newButton(sender, e);
			else if(e.Button.Equals(toolBarButtonOpen))
				openButton(sender,e);
			else if(e.Button.Equals(toolBarButtonSave))
				saveButton(sender,e);
			else if(e.Button.Equals(toolBarButtonSaveAs))
				saveAsButton(sender,e);
			else if(e.Button.Equals(toolBarButtonPen))
				penButton(sender,e);
			else if(e.Button.Equals(toolBarButtonEraser))
				eraserButton(sender,e);
			else if(e.Button.Equals(toolBarButtonSelection))
				selectionButton(sender,e);
			else if(e.Button.Equals(toolBarButtonPlayPause))
				playPauseButton(sender,e);
			else if(e.Button.Equals(toolBarButtonStop))
				stopButton(sender,e);
			else if(e.Button.Equals(toolBarButtonStepBack))
				stepBackButton(sender,e);
			else if(e.Button.Equals(toolBarButtonStepForward))
				stepForwardButton(sender,e);

		}
		#endregion


		private void comboBox1_TextChanged(object sender, EventArgs e)
		{
			if(comboBox1.Text.Equals("Dijkstra"))
			{	
				animType = comboBox1.Items.IndexOf("Dijkstra");
			}
			else
			{
				animType = -1;
			}
		}

		private void RecognizeWeight()
		{
			edgeTimer.Stop();
			if(myRecognizer.Strokes.Count <=0 || prevEdgeHit == null) return;
			RecognitionStatus status;
			string s = myRecognizer.Recognize(out status).TopString;
			s = s.Replace("l","1");
			s = s.Replace("I","1");
			s = s.Replace("|","1");
			s = s.Replace("\\","1");
			s = s.Replace("/","1");
			s = s.Replace("s","5");
			s = s.Replace("S","5");
			s = s.Replace("o","0");
			s = s.Replace("O","0");
			try 
			{ 
				weight = Int32.Parse(s); 
				prevEdgeHit.Weight = weight;
			}
			catch
			{
				Console.WriteLine("Sorry, "+s+" isn't a number.");
				weight = -1;
			}
			prevEdgeHit = null;
			weight = -1;
			myRecognizer.Strokes.Ink.DeleteStrokes(myRecognizer.Strokes);
			myRecognizer.Strokes.Clear();
		}

		private Stroke CopyStroke(Stroke s)
		{
			Point[] p = s.GetPoints();
			return s.Ink.CreateStroke(p);
		}

		private void edgeTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			RecognizeWeight();
			Invalidate();
		}

		private bool isPlayable()
		{
			return home != null && destination != null && 
				nodes.Length() > 0 && animType >= 0;
		}

	}
}
