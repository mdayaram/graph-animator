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
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Canvas : System.Windows.Forms.Form
	{
		private System.ComponentModel.IContainer components;
		private InkOverlay inkOverlay;
		private string PATH = Directory.GetCurrentDirectory();
		private Nodes nodes;
		private Edges edges;
		private Node home, destination;


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
		private System.Windows.Forms.MainMenu mainMenu1;
		#endregion

		public Canvas()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			
			nodes = new Nodes();
			edges = new Edges();
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
			inkOverlay.SelectionChanged += new InkOverlaySelectionChangedEventHandler(inkOverlay_SelectionChanged);
			inkOverlay.CursorButtonDown += new InkCollectorCursorButtonDownEventHandler(inkOverlay_CursorButtonDown);
			inkOverlay.Enabled = true;

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
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
			this.components = new System.ComponentModel.Container();
			this.imgMenu = new System.Windows.Forms.ImageList(this.components);
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.toolBar1 = new System.Windows.Forms.ToolBar();

			this.toolBarButtonNew = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonNew.ToolTipText = "New";
			this.toolBarButtonOpen = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonOpen.ToolTipText = "Open";
			this.toolBarButtonSave = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSave.ToolTipText = "Save";
			this.toolBarButtonSaveAs = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSaveAs.ToolTipText = "Save As";
			this.toolBarButton5 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonPen = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonPen.ToolTipText = "Pen";
			this.toolBarButtonEraser = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonEraser.ToolTipText = "Eraser";
			this.toolBarButtonSelection = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSelection.ToolTipText = "Selection";
			this.toolBarButton9 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonPlayPause = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonPlayPause.ToolTipText = "Play";
			this.toolBarButtonStop = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonStop.ToolTipText = "Stop";
			this.toolBarButtonStepBack = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonStepBack.ToolTipText = "Step Back";
			this.toolBarButtonStepForward = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonStepForward.ToolTipText = "Step Forward";

			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
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
			this.toolBar1.Location = new System.Drawing.Point(0, 0);
			this.toolBar1.Name = "Button Toolbar";
			this.toolBar1.ShowToolTips = true;
			this.toolBar1.Size = new System.Drawing.Size(456, 28);
			this.toolBar1.TabIndex = 0;
			this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
			this.toolBar1.ImageList = imageList1;
			//
			//toolBarButtonPen
			//
			toolBarButtonPen.Pushed = true;
			// 
			// toolBarButton5
			// 
			this.toolBarButton5.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButton9
			// 
			this.toolBarButton9.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// imageList1
			// 
			this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			//
			//Image List Files
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
			//ToolBar Button Indexing
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
			this.Controls.Add(this.toolBar1);
			this.HelpButton = true;
			this.KeyPreview = true;
			this.Menu = this.mainMenu1;
			this.Name = "Canvas";
			this.Text = "Graph Simulator";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
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
		private void menuExit_Click(object sender, System.EventArgs e)
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
		
		}

		private void stepForwardButton(object sender, System.EventArgs e)
		{
		
		}

		private void playPauseButton(object sender, System.EventArgs e)
		{
			switch(toolBarButtonPlayPause.ImageIndex)
			{
				case 7: toolBarButtonPlayPause.ImageIndex = 8; break;
				case 8: toolBarButtonPlayPause.ImageIndex = 7; break;
			}
		}

		private void stopButton(object sender, System.EventArgs e)
		{
		
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

		#region Stroke Event Handlers

		private void inkOverlay_Stroke(object sender, InkCollectorStrokeEventArgs e)
		{	
			if(this.InvokeRequired) return;

			Node n = nodes.getTappedNode(e.Stroke);
			if(n != null)
			{
				if(inkOverlay.EditingMode == InkOverlayEditingMode.Delete)
				{
					foreach(Edge edge in n.Edges)
					{
						edges.Remove(edge);
					}
					nodes.Remove(n);
				}
				else
				{
					int[] ids = {n.Stroke.Id};
					selectionButton(sender, e);
					inkOverlay.Selection = e.Stroke.Ink.CreateStrokes(ids);
					e.Stroke.Ink.DeleteStroke(e.Stroke);
				}
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
						e.Stroke.Ink.DeleteStroke(e.Stroke);
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
					Edge edge = new Edge(edgeNodes[0],edgeNodes[1]);
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
					foreach(Edge edge in n.Edges)
					{
						edges.Remove(edge);
					}
					nodes.Remove(n);
				}
				else
				{
					foreach(Edge edge in edges)
					{
						if(edge.strokeEquals(s))
						{	
							edge.NodeA.Edges.Remove(edge);
							edge.NodeB.Edges.Remove(edge);
							edges.Remove(edge);
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

			Node[] objs = nodes.getNodes(inkOverlay.Selection);
			foreach(Node n in objs)
			{
				Rectangle r = n.Stroke.GetBoundingBox();
				n.CenterPoint = new Point(r.X+r.Width/2, r.Y+r.Height/2);
			}
			Invalidate();
		}
		private void inkOverlay_SelectionChanged(object sender, EventArgs e)
		{
			if(this.InvokeRequired) return;
			Invalidate();
		}
	
		private void inkOverlay_CursorButtonDown(object sender, InkCollectorCursorButtonDownEventArgs e)
		{
			if(this.InvokeRequired) return;
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

	}
}
