Dijkstra's Algorithm Tablet Simulation
======================================

**by <a href="https://www.google.com/recaptcha/mailhide/d?k=01vKBXg12XP9GeR5bxrmga_w==&amp;c=rZBSr-L6v-4LgCyK8GcSBw==" onclick="window.open('https://www.google.com/recaptcha/mailhide/d?k\07501vKBXg12XP9GeR5bxrmga_w\75\75\46c\75rZBSr-L6v-4LgCyK8GcSBw\75\075', '', 'toolbar=0,scrollbars=0,location=0,statusbar=0,menubar=0,resizable=0,width=500,height=300'); return false;">Manoj Dayaram</a>**

## Purpose

The purpose for this application will be to simulate Dijkstra's shortest path algorithm in an interactive manner making use of tablet features. Though previous simulations have been made, they lacked the interactive features of creating one's own graph structure to run the simulation on. In this application, the user will be able to draw their own nodes and edges between them anywhere within the drawing board. After applying weights to each edge, the user will then be able to select a home node and a destination node and run the Dijkstra's Algorithm simulation.

## Applications

Applications for this program would mostly fall under the educational spectrum.Computer Science classrooms could use this program in order to demonstrate to students the key features of Dijkstra's algorithm, allowing them to make their own graphs and seeing how the algorithm adjusts to their own adjustments and how it would react in different situations that they could come up with. Since the program will have a strong emphasis on intuitiveness and ease of use, there is no limit of who can use it, thus making it the perfect teaching tool for this algorithm.Expandability will also be kept in mind when programming the project, allowing other algorithm demonstrations to be implemented easily in the future.

## Benefits Over Other Programs

As mentioned earlier, this application offers several advantages over current Dijkstra's algorithm animations.The main advantages covered here will be user interaction, ease of use, and effectiveness in demonstrating Dijkstra's algorithm to those who don't know much about it. In this discussion we will be comparing the following implemented applications that demonstrate Dijkstra's algorithm and show how each lack at least one of the qualities mentioned above. 

The first consists of a simple animation than can be found [here](http://www.cs.sunysb.edu/~skiena/combinatorica/animations/dijkstra.html). In this animation, one can see the order in which Dijkstra's algorithm is applied to each individual vertex and edge eventually covering the whole list. However, this example doesn't explain the essence of Dijkstra's algorithm to someone who might not be very familiar with it.Since it's also just an animation, it lacks user interaction. For these reasons, this is not a very good simulator of the algorithm.

The second application consists of a java applet that can be found [here](http://www.unf.edu/~wkloster/foundations/DijkstraApplet/DijkstraApplet.htm). In this applet, the user is presented with several vertices and edges with weights already on them.The applet is extremely easy to use:the user merely clicks anywhere in the applet and the applet shows a step in Dijkstra's algorithm starting at a select vertex set as a default in the applet. Though a lot more interactive than the last example, this applet still lacks the functionality of allowing the user to create their own graphs in order to test different scenarios out. Though it does a good job displaying Dijkstra's algorithm, it constricts its behavior to only the set example given.

The third application is another java applet that can be found [here](http://www.lupinho.de/gishur/html/DijkstraApplet.html). This is definitely a much greater improvement within the last two. Not only does this applet offer built-in examples that one can run a simulation on, but also allows for a user to create their own graph and run a simulation on that.The only problem this applet faces is the ease of use. While creating vertices is not much of a problem, creating edges between those vertices seems to be a mystery that the applet is not willing to share.The process is very unintuitive and a hassle to deal with when trying to demonstrate the algorithm to students or peers.Even after a half an hour of using the program, I was still unable to figure out how to draw edges.That is time that should not have been spent merely learning how to operate the application (which was not even accomplished).

The fourth and final application is a javascript implementation of the algorithm and can be found [here](http://www.carto.net/papers/svg/dijkstra_shortest_path_demo/). In this application, there is simply a default home node and the user can hover his/her mouse over any other vertex to show the shortest highlighted path to that location. This is a very simple and intuitive application to use; however, it does a poor job demonstrating the bits and pieces of Dijkstra's algorithm. An outside observer could look at this application and simply declare “well, of course that's the shortest path, I can see that.”In other words, the application lacks an explanation of *how* it found that shortest path, which is what Dijkstra's algorithm is all about.

All other simulations of Dijkstra's algorithm that can be found are some variation of the ones described above.This tablet implemented simulation of Dijkstra's algorithm will take all three categories into account:ease of use, user interaction, and clearly demonstrate all the steps covered in Dijkstra's algorithm.This project will make use of a tablet's intuitive drawing and sketching abilities to allow the user to create their own graph and then show a simulation that traverses that graph through Dijkstra's algorithm. A more detailed explanation of how these things will be accomplished is listed under *Expectations and Possible Issues*.

## Expectations and Possible Issues

The main expectation of this project is to create a program that is interactive and intuitive for the user that demonstrates Dijkstra's algorithm in a way that is easy to understand but does not conceal the essence of what the algorithm is. These expectations can be broken apart into two categories: The user interface, and the algorithm simulation.

### User Interface

For the user interface to be successful, it needs to present an intuitive process in making a graph for the user. To achieve this, tablet sketch recognition will be used. By limiting ourselves to only recognizing the basic two objects of a graph, a vertex and an edge, the program lends itself to being very simple and easy to use, yet still capable of creating complex graphs through which the algorithm could be displayed on. 

#### Nodes and Vertices

For this project, there will be two shapes which will be identified as a vertex or a node:a circle and a rectangle.Both vertices will maintain identical properties except their shape. 

![](https://raw.github.com/mdayaram/graph-animator/master/docs/proposal_files/image001.gif)

![](https://raw.github.com/mdayaram/graph-animator/master/docs/proposal_files/image002.gif)

The circle node will be recognized as a whole stroke that comes back to itself. The stroke length will be checked against the perimeter of a circle that would fit in the same area and compared in order to attain further assurance. The rectangle node will be recognized by one continuous stroke of four straight lines that comes back to itself. Again, its stroke length will be checked with the perimeter of a rectangle that would fit in the same area.

![](https://raw.github.com/mdayaram/graph-animator/master/docs/proposal_files/image003.gif)

It is worth to note that a rectangle such as this, which is composed of four separate lines, will not be recognized as a rectangular vertex.Instead, each stroke would be recognized as a single edge that connects no vertices. This leads to the next issue of handling an edge that connects no vertices. 

![](https://raw.github.com/mdayaram/graph-animator/master/docs/proposal_files/image004.gif)

An edge will be recognized simply as a line that connects vertices together.If a line is ever drawn where no vertices lie on, the edge is then simply discarded and, for practical uses, erased from the drawing board. 

![](https://raw.github.com/mdayaram/graph-animator/master/docs/proposal_files/image005.gif)

Before analyzing the issues of edge recognition, one last issue of vertex recognition should be covered: overlapping.  If a previous node has been recognized and the user chooses to draw another one on top of it, the most rational thing to do would be to discard it. Because making individual nodes is not a tedious task, it's easier for the user to simply make a new node in another part of the drawing board where it would not overlap any other node than it would be to recognize the new overlapping node and prompt the user to move it to another location.For this reason, overlapping nodes, both circles and rectangles, will be discarded. 

#### Edges

When an edge is drawn, the requirements for it to be recognized as an edge that will be taken into account are one, that it makes a connection between two nodes, and two, that there's a straight line path between the nodes it connects that does not cause the edge to overlap another node. The following image illustrates what would be an ideal drawing of an edge and its conversion:

![](https://raw.github.com/mdayaram/graph-animator/master/docs/proposal_files/image006.gif)

The line drawn in this illustration makes direct contact and, though it slightly overlaps the nodes it plans to connect, does not completely overshoot the line past the other side of the node. This way, the endpoints of the line drawn lie within the node's boundaries making a concrete connection between the two nodes. A less desired, but still correct drawing of an edge is illustrated in the image below:

![](https://raw.github.com/mdayaram/graph-animator/master/docs/proposal_files/image007.gif)

In this image, the edge is drawn in a way that does not result in a straight line connection between the two nodes it is trying to connect. However, since the line drawn makes a concrete connection between the two nodes at each endpoint, then it can be interpreted as an edge connecting the two nodes. Also, unlike the following example, this edge does not go around interfering nodes in its path.

![](https://raw.github.com/mdayaram/graph-animator/master/docs/proposal_files/image008.gif)

In this example, the edge drawn goes around another node to make a connection. Since this would interfere with the conversion of the line into an edge (causing the edge to go through the intermediate node, the line drawn is then simply discarded. To make a connection between the two nodes, it is suggested that the user move the nodes around in a way that will allow for a straight line connection between them without overlapping any other nodes. 

In another similar case, when a line is purposely drawn overlapping a third intermediate node, the line drawn can be interpreted as two lines, one connecting the first node to the middle, and the second connecting the middle to the third.

![](https://raw.github.com/mdayaram/graph-animator/master/docs/proposal_files/image009.gif)

In this case, if the line drawn makes a concrete connection between all the nodes that it passes over, it will interpret a connection between each subsequent node.Therefore, if the user were to draw a line from node 1 that crossed over node 2 and ended in node 3, the edges generated from such input would be an edge between nodes 1 and 2, and an edge between nodes 2 and 3.

Applying weights on an edge will be simply executed by drawing the number on top of that very edge.As demonstrated in the image below, the number must be drawn on top of an edge for it to be recognized.If drawn near an edge or on a node, or empty space, it will simply be ignored and discarded.

![](https://raw.github.com/mdayaram/graph-animator/master/docs/proposal_files/image010.gif)

#### Home and Destination

Finally, declaring the home node or the starting node and declaring the destination or end node will be done by the following drawing assignment: the user would draw a star on the node he/she wishes to make the home node and can then draw a star at the node he/she wished to be the destination node: 

Home
![](https://raw.github.com/mdayaram/graph-animator/master/docs/proposal_files/image011.gif)

Destination
![](https://raw.github.com/mdayaram/graph-animator/master/docs/proposal_files/image012.gif)

If the user then decides that he made an error in picking the home node, he can simply erase the star.By doing this then the destination node now becomes the home node and the next star the user draws on a node will make it the destination node. A home node would be depicted with an asterisk where as a destination node would be depicted with an X.

### Animation:

The animation will be kept relatively simple but will depict every step in Dijkstra's algorithm. Before the animation can take place, however, some conditions must be met.

Firstly, there must be a graph drawn in the drawing space. If there are no nodes or edges, then no animation can take place.Secondly, every edge drawn must have a weight associated to it. If no weight is found in any edge, the user will either be able to choose a default edge weight or add the weight himself. Thirdly, there must be an assigned home node and destination node.This would also entail that a simulation could not be ran with a single node in the drawing board.

One might think that if a graph is drawn, but there is no possible connection between the home node and destination node, that the program should throw and error.However, instead of that approach, the program would continue the simulation until all nodes are covered and show that there are no paths to the destination node and that it cannot be reached (i.e. its distance from the home node can be considered to be infinity).

A representation of the graphical user interface is displayed below. A relatively simple design is being used in order to not overwhelm the user with several features, since those features will be supplied through pen strokes.Only the most essential options were considered when composing the menus.

![](https://raw.github.com/mdayaram/graph-animator/master/docs/proposal_files/image013.gif)

Each submenu is very trivial except perhaps the Animation menu. In the Animation's submenu, the user will encounter a few options regarding the animation process. He/She will be able to choose the animation speed and also the default edge weight if desired (which can always be overwritten by a pen stroke).The options in the toolbar regarding the animation will also be available in the Animation submenu (play, pause, stop, back a step, and forward a step). 

Also in the Animation submenu, the user will be able to choose which algorithm to simulate.As mentioned earlier, this project will be completed keeping expandability in mind. So though now Dijkstra's algorithm would be the only choice, it is not impossible to imagine several other algorithms being implemented such as the Bellman-Ford algorithm, Breadth First Search, A* search, and any others.

![](https://raw.github.com/mdayaram/graph-animator/master/docs/proposal_files/image014.gif)

The other options in the toolbar are fairly trivial; the pen, selection, and eraser tools are self explanatory.The user will also have the option to save his graph and open it up at a later time.

Once the user has a graph drawn and ready to view the simulation, the play and step buttons will become available for use. The pictures below demonstrate what would be a sample simulation of Dijkstra's algorithm implemented in this environment.

![](https://raw.github.com/mdayaram/graph-animator/master/docs/proposal_files/image015.gif)

This would be a screen shot of a graph ready to be simulated. Notice that all edges contain positive weights and that home and destination nodes have been chosen.The next step in Dijkstra's algorithm would be to identify the home node as done so below:

![](https://raw.github.com/mdayaram/graph-animator/master/docs/proposal_files/image016.gif)

In this screen shot the home node has been chosen and added to the priority queue. It has also been identified to contain a distance of zero from the home node. The next step is to dequeue the node with the shortest distance to the home node and identify its children.

![](https://raw.github.com/mdayaram/graph-animator/master/docs/proposal_files/image017.gif)

Because the home node is the only node in the queue, it is the one which we dequeue and thus, process its children. The node at the top is then added to the priority queue with an established length of three units from the home node and the node to the right is enqueued as well with an established ten units away from the home node.Repeating the previous step, now it is time to dequeue the node with the shortest distance to the home node and process its children.

![](https://raw.github.com/mdayaram/graph-animator/master/docs/proposal_files/image018.gif)

This node would be the top node with distance three. Since it has the shortest distance from the home node and has been dequeued, we can now establish that three units is the absolute shortest path to that node from the home node. Now we can add its children to the priority queue.

![](https://raw.github.com/mdayaram/graph-animator/master/docs/proposal_files/image019.gif)

In this screen shot we can see the children of the top node being added to the priority queue.Notice how their distance from the home node is computed by adding the edge weight connecting them to the top node plus the top node's distance from the home node. This is a step that is usually missed in most simulations of Dijkstra's algorithm that leaves lots of minds in a small state of confusion in terms of how the distance was calculated. 

![](https://raw.github.com/mdayaram/graph-animator/master/docs/proposal_files/image020.gif)

Now that we actually have the sum and thus of the distances for each child, we assign them those distances to be their temporary distance away from the home node.As you can see, there are now three nodes in the priority queue. Again, we must dequeue the one with the shortest distance to the home node.Since two of our nodes have a distance of ten units, we can pick any of the two to dequeue. 

![](https://raw.github.com/mdayaram/graph-animator/master/docs/proposal_files/image021.gif)

By dequeuing the node on the right, we have now determined that the shortest path to that node from the home node can be reached with ten units. Now that the node is out of the queue, we can process its children. However, it's only child (that isn't its parent) is already in the queue!

![](https://raw.github.com/mdayaram/graph-animator/master/docs/proposal_files/image022.gif)

In this case, we make a comparison. The distance that the right node proposes for the bottom node is 21 (10 + 11).This distance is then compared with the temporary distance that is set on the bottom left node which just happens to be ten. Since ten is lower than 21, the bottom left node decides to keep its original value and discards the 21.This is the key aspect of Dijkstra's algorithm and why it works, yet, many simulations, if not all, tend to keep this aspect out of the demonstration. 

![](https://raw.github.com/mdayaram/graph-animator/master/docs/proposal_files/image023.gif)

Now that we've established that the bottom left node has a better shorter path with ten rather than 21, it is time to dequeue our next node from our priority queue.Since the only nodes we have left are the ones in the bottom, the bottom left is chosen because of its lower value of ten compared to twelve.

![](https://raw.github.com/mdayaram/graph-animator/master/docs/proposal_files/image024.gif)

Since this node has been dequeued, its shortest path distance from the home node is now guaranteed to be ten.Now it is time to add its children to the queue, however, all its children have been dequeue and processed already except the bottom right node which is already in the queue.So we take the weight of the edge connecting the parent to the child and add that distance to the distance of the parent to get 15. Comparing 15 to the already established distance of 12 on the node, we find out that its current distance is still lower and thus, will remain with it. It is finally time to dequeue our next smallest node which happens to be our destination node.

![](https://raw.github.com/mdayaram/graph-animator/master/docs/proposal_files/image025.gif)

Once this node is dequeued, we are guaranteed that this is the shortest path to this node.Since this is our destination node, we can stop here and declare our victory in finding the shortest distance to our destination. The program will then trace the path which led to the shortest outcome as demonstrated by the following picture.

![](https://raw.github.com/mdayaram/graph-animator/master/docs/proposal_files/image026.gif)

The user will then have a choice to run the animation again, stop the animation which would return him into editing mode, or go back or forward a step in the animation.He could also open a new file and start a new graph or save the graph that he has made.

Below is a final animated simulation of the entire workflow:

![](https://raw.github.com/mdayaram/graph-animator/master/docs/proposal_files/animation.gif) 
