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
		private const int PLAY = 7, PAUSE = 8,  //Used for toggling Play/Pause Button
						  DIJKSTRA = 0;  //Animation Constant used for Index in comboBox

		private System.ComponentModel.IContainer components;
		private InkOverlay inkOverlay;
		private string PATH = Directory.GetCurrentDirectory();
		private const int TIME_INTERVAL = 1000;  //The time interval used to recognize Edge weights
		private Graph graph;
		private ArrayList animations;
		private Animation anim;
		private RecognizerContext myRecognizer;
		private bool animStarted;
		//*****Used for Edge Recognition*********
		private int weight;
		private Edge prevEdgeHit;
		private System.Timers.Timer edgeTimer;
		//***************************************

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
		private System.Windows.Forms.ToolBarButton helpButton;
		private System.Windows.Forms.MainMenu mainMenu1;
		#endregion


		public Canvas()
		{

			InitializeComponent();
			
			graph = new Graph();
			animations = new ArrayList();
			animStarted = false;
			weight = -1;
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
			da.AntiAliased = true;  //Makes everything nice and smooth lookin'
			da.Transparency = 0;
			da.PenTip = Microsoft.Ink.PenTip.Ball;
			da.Width = 70.0f;
			inkOverlay.DefaultDrawingAttributes	= da;

			//Triggered at each pen stroke
			inkOverlay.Stroke += new InkCollectorStrokeEventHandler(inkOverlay_Stroke);
			//Triggered at each eraser stroke
			inkOverlay.StrokesDeleting += new InkOverlayStrokesDeletingEventHandler(inkOverlay_StrokesDeleting);
			//Triggered when a selection of strokes is moved
			inkOverlay.SelectionMoved += new InkOverlaySelectionMovedEventHandler(inkOverlay_SelectionMoved);
			//Triggered when a selection of strokes is resized
			inkOverlay.SelectionResized +=new InkOverlaySelectionResizedEventHandler(inkOverlay_SelectionResized);
			inkOverlay.Enabled = true;

			myRecognizer = new RecognizerContext();
			myRecognizer.Strokes = inkOverlay.Ink.CreateStrokes();

			edgeTimer = new System.Timers.Timer(TIME_INTERVAL);
			edgeTimer.AutoReset = true;
			edgeTimer.Elapsed +=new ElapsedEventHandler(edgeTimer_Elapsed);
			edgeTimer.Enabled = true;

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
			this.helpButton = new System.Windows.Forms.ToolBarButton();
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
																						this.toolBarButtonStepForward,
																						this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,
																						this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,this.toolBarButton9,
																						this.helpButton});
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
            this.toolBarButtonOpen.Enabled = false;
			// 
			// toolBarButtonSave
			// 
			this.toolBarButtonSave.ImageIndex = 2;
			this.toolBarButtonSave.ToolTipText = "Save";
            this.toolBarButtonSave.Enabled = false;
			// 
			// toolBarButtonSaveAs
			// 
			this.toolBarButtonSaveAs.ImageIndex = 3;
			this.toolBarButtonSaveAs.ToolTipText = "Save As";
            this.toolBarButtonSaveAs.Enabled = false;
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
			// helpButton
			// 
			this.helpButton.ImageIndex = 12;
			this.helpButton.ToolTipText = "Help";
			this.helpButton.Enabled = true;
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
			imageList1.Images.Add(Image.FromFile(PATH+"\\imgs\\help.png"));
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
			this.helpButton.ImageIndex = 12;
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
			Canvas c = new Canvas();
			c.AddAnimation(new DijkstraAnimation(c));
			Application.Run(c);
		}


		#region Button Methods 

		private void newButton(object sender, System.EventArgs e)
		{
			graph.Clear();
			this.inkOverlay.Ink.DeleteStrokes(inkOverlay.Ink.Strokes);
			penButton(sender, e);
			if(anim != null) anim.Stop();
            anim = new DijkstraAnimation(this);
			comboBox1.Text = anim.ToString();
			togglePlayPause(PLAY);
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
			graph.Clear();
			this.Dispose();
		}

		private void stepBackButton(object sender, System.EventArgs e)
		{
			if(anim == null && !anim.isPlayable()) return;
			if(!animStarted)
			{
				NewAnimation();
				animStarted = true;
			}
			anim.StepBack();
			Invalidate();
		}

		private void stepForwardButton(object sender, System.EventArgs e)
		{
			if(anim == null && !anim.isPlayable()) return;
			anim.Step();
			animStarted = true;
			Invalidate();
		}

		//Starts a new animation  depending on what the animType is
		private void NewAnimation()
		{
			if(anim == null)
				MessageBox.Show("Please select an algorithm from the drop down box.");
			else 
				anim.Initialize(graph);
		}

		/* Starts the animation if it hasn't started yet, 
		 * if it has, then it pauses the animation.  If animation 
		 * is paused, then it plays it again from the place it stopped.
		 */
		private void playPauseButton(object sender, System.EventArgs e)
		{
			if(!animStarted)
			{
				NewAnimation();
			}

			if(anim == null) return;

			if(!anim.isPlayable())
			{
				MessageBox.Show("Some conditions to start the animation are lacking.\n"+
					"Please make sure you have assigned home and destination\n"+
					"nodes and that you have selected an animation type from\n"+
					"the drop down menu.");
				return;
			}
			animStarted = true;
			if(toolBarButtonPlayPause.ImageIndex == PLAY)
				anim.Play();
			else
				anim.Pause();
			
			togglePlayPause();
		}

		//Toggles the image on the play/pause button
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

		/* Stops an animation and returns the program into drawing state.
		 */
		private void stopButton(object sender, System.EventArgs e)
		{
			if(!anim.isPlayable()) return;
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

		/* Sets the editing mode to any of the three,
		 * selection, pen, or eraser.  Also initializes the 
		 * selection strokes.
		 */
		private void setEditingMode(InkOverlayEditingMode em)
		{
			if(inkOverlay.EditingMode == em) return;
			inkOverlay.Selection = inkOverlay.Ink.CreateStrokes();
			inkOverlay.EditingMode = em;
		}
			
		#endregion

		private void Canvas_Paint(object sender, PaintEventArgs e)
		{
			graph.Render(inkOverlay, e.Graphics);

			for(int i=0; i<myRecognizer.Strokes.Count; i++)
			{
				inkOverlay.Renderer.Draw(e.Graphics,myRecognizer.Strokes[i]);
			}
		}

		#region Stroke Event Handlers

		private void inkOverlay_Stroke(object sender, InkCollectorStrokeEventArgs e)
		{	
			if(this.InvokeRequired) return;
			
			//in effect, this is reseting the timer to start from the beginning.
			edgeTimer.Stop();
			edgeTimer.Start();
			
			//Check if the stroke was a tap, and if it was, get the node it tapped.
			Node n = StrokeManager.TappedNode(e.Stroke,graph);
			if(n != null)
			{
				//If its eraser mode, delete it.
				if(inkOverlay.EditingMode == InkOverlayEditingMode.Delete)
				{
					graph.Remove(n);
				}
				else
				{
					//Any other mode, select it and change to selection mode
					int[] ids = {n.Stroke.Id};
					selectionButton(sender, e);
					inkOverlay.Selection = e.Stroke.Ink.CreateStrokes(ids);
				}
				e.Stroke.Ink.DeleteStroke(e.Stroke);
				Invalidate();
				return;
			}
			
			//The following code is for pen mode only strokes
			if(inkOverlay.EditingMode != InkOverlayEditingMode.Ink) return;
			
			//If a stroke is inside a node, store it in n
			n = StrokeManager.HitNodeTest(e.Stroke,graph);

			//If the stroke is closed and it's a start, assign a home or destination
			if(StrokeManager.isClosed(e.Stroke) && n != null && StrokeManager.isStar(e.Stroke))
			{
				graph.AssignNode(n);
				RecognizeWeight();  //Attempt at recognizing weight is made after every stroke.
			}
			//If the stroke is closed and it is not enclosed in a node and is a circle, make a circular node
			else if(StrokeManager.isClosed(e.Stroke) && n==null && e.Stroke.PacketCount > StrokeManager.SMALLEST_N_SIZE && StrokeManager.FitsCircleProperties(e.Stroke))
			{
				Stroke circle = StrokeManager.makeCircle(inkOverlay, e.Stroke);
				Node circleNode = new Node(circle);
				graph.Add(circleNode);
				RecognizeWeight();
			}
			//If the stroke is close and it is not enclosed in a node and is a rectangle, make a rectangular node
			else if(StrokeManager.isClosed(e.Stroke) && n==null && e.Stroke.PacketCount > StrokeManager.SMALLEST_N_SIZE && StrokeManager.FitsRectProperties(e.Stroke))
			{
				Stroke rect = StrokeManager.makeRect(inkOverlay, e.Stroke);
				Node rectNode = new Node(rect);
				graph.Add(rectNode);
				RecognizeWeight();
			}
			//if the stroke isn't closed, then it is an edge.  
			else if(!StrokeManager.isClosed(e.Stroke))
			{
				//Get all the nodes hit by this stroke and create edges for them
				Nodes edgeNodes = StrokeManager.ifEdgeGetNodes(e.Stroke, graph);
				if(edgeNodes != null && !StrokeManager.isScratchOut(e.Stroke))
				{
					for(int i=0; i<edgeNodes.Length()-1; i++)
					{
						if(!Edge.hasEdge(edgeNodes[i],edgeNodes[i+1]))
						{
							Edge edge = new Edge(edgeNodes[i],edgeNodes[i+1],inkOverlay);
							graph.Add(edge);
						}
					}
				}
				else if(StrokeManager.isScratchOut(e.Stroke))
				{
					ArrayList objs = StrokeManager.HitObjects(e.Stroke,graph);
					for(int i=0; i<objs.Count; i++)
					{
						graph.Remove(objs[i]);
					}
				}

				RecognizeWeight();
			}
			else
			{
				//if all of the above fails, then the stroke is considered for edge weights
				Edge hitEdge = StrokeManager.HitEdgeTest(e.Stroke,graph);
				if(hitEdge != null)
				{
					/* if the edge hit is the same as the previous one, 
					 * accumulate strokes for it before recognizing,
					 * if it's a different edge, then recognize and add this
					 * stroke to the new edge.
					 */
					if(prevEdgeHit == null) prevEdgeHit = hitEdge;
					if(hitEdge.Equals(prevEdgeHit))
					{
						myRecognizer.Strokes.Add(StrokeManager.CopyStroke(e.Stroke));
					}
					else
					{
						RecognizeWeight();
						prevEdgeHit = hitEdge;
						myRecognizer.Strokes.Add(StrokeManager.CopyStroke(e.Stroke));
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
			//Remove the corresponding nodes and edges for each stroke
			for(int i=0; i<strokes.Count; i++)
			{   
				if(StrokeManager.isClosed(strokes[i],0))
				{
					graph.Remove(graph.Nodes.getNode(strokes[i]));
				}
				else
				{
					graph.Remove(graph.Edges.getEdge(strokes[i]));
				}
			}
			Invalidate();
		}
		
		/* For any selection movement or resizing we need 
		 * to fix the node center points.
		 */
		private void inkOverlay_SelectionMoved(object sender, InkOverlaySelectionMovedEventArgs e)
		{
			if(this.InvokeRequired) return;
						
			Nodes selectedNodes = graph.Nodes.getNodes(inkOverlay.Selection);
			for(int i=0; i<selectedNodes.Length(); i++)
			{
				Rectangle r = selectedNodes[i].Stroke.GetBoundingBox();
				selectedNodes[i].CenterPoint = new Point(r.X+r.Width/2, r.Y+r.Height/2);
			}
			Invalidate();
		}
		private void inkOverlay_SelectionResized(object sender, InkOverlaySelectionResizedEventArgs e)
		{
			if(this.InvokeRequired) return;
						
			Nodes selectedNodes = graph.Nodes.getNodes(inkOverlay.Selection);
			for(int i=0; i<selectedNodes.Length(); i++)
			{
				Rectangle r = selectedNodes[i].Stroke.GetBoundingBox();
				selectedNodes[i].CenterPoint = new Point(r.X+r.Width/2, r.Y+r.Height/2);
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
			else if(e.Button.Equals(helpButton))
				HelpBrowser.run(PATH+"\\help.html");

		}
		#endregion

		//Select animation from comboBox
		private void comboBox1_TextChanged(object sender, EventArgs e)
		{
			for(int i=0; i<animations.Count; i++)
			{
				if(comboBox1.Text.Equals(animations[i].ToString()))
				{
					anim = animations[i] as Animation;
				}
			}
		}

		//Attempt to recognize an edge weight
		private void RecognizeWeight()
		{
			edgeTimer.Stop();
			//If there are no strokes in the recognizer or no edge hit, return
			if(myRecognizer.Strokes.Count <=0 || prevEdgeHit == null) return;
			RecognitionStatus status;
			//Recognize the contents of the recognizer
			string s = myRecognizer.Recognize(out status).TopString;
			//Replace all letters that look like numbers with their appropriate numbers
			s = s.Replace("l","1");
			s = s.Replace("I","1");
			s = s.Replace("|","1");
			s = s.Replace("\\","1");
			s = s.Replace("/","1");
			s = s.Replace("s","5");
			s = s.Replace("S","5");
			s = s.Replace("o","0");
			s = s.Replace("O","0");
			//Attempt to parse into a number
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

		private void edgeTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			RecognizeWeight();
			edgeTimer.Stop();
			Invalidate();
		}

		/* Can only play animation if the home and destination nodes are set and 
		 * there are nodes in the graph
		 */

		public void AddAnimation(Animation anim)
		{
            this.anim = anim;
            this.comboBox1.Text = anim.ToString();
			this.comboBox1.Items.Add(anim.ToString());
			animations.Add(anim);
		}

	}
}
