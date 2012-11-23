using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
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
		private Nodes nodes;
		private Edges edges;
		private Node home, destination;
		private Animation anim;
		private bool animStarted;
		private int animType;


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
			animType = -1;

			// Declare repaint optimizations.
			base.SetStyle(
				ControlStyles.UserPaint|
				ControlStyles.AllPaintingInWmPaint|
				ControlStyles.DoubleBuffer,
				true);

			this.Paint += new PaintEventHandler(Canvas_Paint);
			inkOverlay = new InkOverlay(this.Handle, true); //attach to form, allow use of mouse for input
			inkOverlay.CollectionMode =	CollectionMode.InkOnly;	// do not allow gestures

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
			inkOverlay.Enabled = true;

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
			this.toolBarButtonPlayPause.Enabled = false;
			// 
			// toolBarButtonStop
			// 
			this.toolBarButtonStop.ImageIndex = 9;
			this.toolBarButtonStop.ToolTipText = "Stop";
			this.toolBarButtonStop.Enabled = false;
			// 
			// toolBarButtonStepBack
			// 
			this.toolBarButtonStepBack.ImageIndex = 10;
			this.toolBarButtonStepBack.ToolTipText = "Step Back";
			this.toolBarButtonStepBack.Enabled = false;
			// 
			// toolBarButtonStepForward
			// 
			this.toolBarButtonStepForward.ImageIndex = 11;
			this.toolBarButtonStepForward.ToolTipText = "Step Forward";
			this.toolBarButtonStepForward.Enabled = false;
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
			this.ClientSize = new System.Drawing.Size(456, 297);
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
			if(!animStarted)
			{
				NewAnimation();
			}

			if(home==null || destination == null)
			{
				MessageBox.Show("Please pick a home and destination node\nby drawing a star inside the a node.","Can't Animate Yet!");
				return;
			}
			animStarted = true;
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
		}
		#region Node and Edge Handlers
		private void AssignStarNode(Node n)
		{
			if(home == null)
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

			if(StrokeManager.isClosed(e.Stroke))
			{
				n = nodes.HitNodeTest(e.Stroke);
				if(n != null) 
				{
					if(StrokeManager.isStar(e.Stroke))
					{
						AssignStarNode(n);
					}
					else
					{
						MessageBox.Show("Can't overlap nodes!", "Stroker");
					}
				}
				else if(e.Stroke.PacketCount > StrokeManager.SMALLEST_N_SIZE && StrokeManager.FitsCircleProperties(e.Stroke))
				{
					Stroke circle = StrokeManager.makeCircle(inkOverlay, e.Stroke);
					Node circleNode = new Node(circle, false);
					nodes.Add(circleNode);
				}
				else if(e.Stroke.PacketCount > StrokeManager.SMALLEST_N_SIZE && StrokeManager.FitsRectProperties(e.Stroke))
				{
					Stroke rect = StrokeManager.makeRect(inkOverlay, e.Stroke);
					Node rectNode = new Node(rect, true);
					nodes.Add(rectNode);
				}
			}

			else
			{
				Node[] edgeNodes = StrokeManager.ifEdgeGetNodes(e.Stroke, nodes);
				if(edgeNodes != null && !Edge.hasEdge(edgeNodes[0],edgeNodes[1]))
				{
					Edge edge = new Edge(edgeNodes[0],edgeNodes[1],inkOverlay);
					edges.Add(edge);
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
			if(nodes.Length() < 0)
			{
				MessageBox.Show("You must have at least one node.", "Can't Initiate Animation");
				return;
			}
			else if(home == null || destination == null)
			{
				MessageBox.Show("You must select a home and destination node before selecting an animation.", "Can't Initiate Animation");
				return;
			}
			
			if(comboBox1.Text.Equals("Dijkstra"))
			{	
				toolBarButtonPlayPause.Enabled = true;
				toolBarButtonStop.Enabled = true;
				toolBarButtonStepBack.Enabled = true;
				toolBarButtonStepForward.Enabled = true;
				animType = comboBox1.Items.IndexOf("Dijkstra");
			}
			else
			{
				anim = null;
				toolBarButtonPlayPause.Enabled = false;
				toolBarButtonStop.Enabled = false;
				toolBarButtonStepBack.Enabled = false;
				toolBarButtonStepForward.Enabled = false;
			}
		}
	}
}
