Graph Animator Final Report
===========================

# Dijkstra's Algorithm Tablet Simulation

**by <a href="https://www.google.com/recaptcha/mailhide/d?k=01vKBXg12XP9GeR5bxrmga_w==&amp;c=rZBSr-L6v-4LgCyK8GcSBw==" onclick="window.open('https://www.google.com/recaptcha/mailhide/d?k\07501vKBXg12XP9GeR5bxrmga_w\75\75\46c\75rZBSr-L6v-4LgCyK8GcSBw\75\075', '', 'toolbar=0,scrollbars=0,location=0,statusbar=0,menubar=0,resizable=0,width=500,height=300'); return false;">Manoj Dayaram</a>**

## Purpose

The purpose of Graph Animator is to simulate graph algorithms in an interactive manner on a tablet setting. Dijkstra's algorithm is used in this instance of Graph Animator; however, the program is built to make it easy to add more graph traversing algorithms. The program makes use of the tablet features by allowing the user to be able to fully interact with it by using strokes and some buttons. A graph can be simply drawn by drawing nodes and edges and home and destination nodes are assigns the same way as are the edge weights. This forms a very easy and intuitive design where the user finds comfortable and easy to use.

## Applications

Several applications for this program have arisen, the first of being the educational aspect which was emphasized in the beginning proposal. The program could easily be used to display each step of Dijkstra's Algorithm or any other algorithm being implemented in a classroom. The program also allows students the freedom to create their own graphs and test out the algorithms in their unique settings. Other than educations aspects, however, other applications for this program are only bounded by the applications for Dijkstra's algorithm and could work in any setting where a visual representation of Dijkstra's algorithm is necessary (i.e. schedule planning or flight cost trips).

## The Interface

The user interface was kept as simple as possible offering minimal options other than the essentials. Though the original File menu was included in the beginning proposal, the idea was scrapped during implementation because of the lack of functionality and aesthetics. Instead, the user can navigate through all the options necessary through the simple button interface located at the top.

![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image002.jpg)

From left to right, the buttons represent New, Open, Save, Save As, Pen, Eraser, Lasso (Selection), Play, Stop, Step Back, and Step Forward. The drop down box is used to select the type of animation to implement. As of this moment, the only choice is Dijkstra's algorithm.

Though it may not seem like enough, these are the only buttons necessary in order to fully interact with Graph Animator. In order to draw a node, the user simple draws a square or a circle representative of a node and the program then converts it to a node. The stroke is recognized as a circle if the starting position is relatively close to its ending position and if the length of the stroke is somewhat similar to the circumference of a circle in the same area. Similar measures are taken in order to determine if the stroke drawn is a rectangle.

![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image005.jpg) ![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image003.gif) ![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image007.jpg)

To make an edge, one merely connects two or more nodes together in one stroke. If the stroke goes over several nodes, the program will make an edge connected each pair of consecutive nodes with a default weight of ten.

![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image009.jpg) ![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image003.gif) ![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image011.jpg)

Of course, the graph isn't very useful if the only edge weight possible is ten, so if the user wants to change an edge weight, he just has to write the new desired weight on top of the current one. The program will recognize that the user is attempting to write an edge weight and will wait a second after every stroke in order for him to finish writing the desired weight and once done, will recognize it.

![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image014.jpg) ![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image012.gif) ![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image016.jpg)

If the user makes a mistake, he or she can use the eraser tool to erase nodes and/or edges that he doesn't find a need for anymore. The scratch out gesture has also been implemented to delete all nodes and edges that touch a scratch out. A scratch out is determined by a single stroke that intersects itself at least three times, whose length is at least three times its width, and whose height is at most three quarters its width.


![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image019.jpg) ![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image017.gif) ![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image021.jpg)

Using the lasso or selection tool, one can also move or even resize the nodes around. This can be useful if the user is working on one graph and would like to work on another graph but still keeping the current one in perspective. The could potentially shrink the current graph small enough and drag it to a corner, still offering plenty of room for a new graph without having to delete the old one.


![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image024.jpg) ![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image012.gif) ![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image026.jpg) ![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image012.gif) ![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image028.jpg)

When working with graphs, two nodes are usually unique in terms that they stand for home and destination. These nodes can easily be assigned by drawing a star inside the node desired. The first starred node will be interpreted by the program as the home node, and the second starred node will be interpreted as the destination. After that, every additional starred node will be interpreted as a new destination, making the last destination the new home.

![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image030.jpg) ![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image022.gif) ![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image032.jpg)

![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image034.jpg) ![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image022.gif) ![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image036.jpg)

And that's it! Those are all the features used in creating a graph. There is not so many as to overwhelm the user when trying to make a simple graph, but there is more than enough to build complex graphs and being able to experience all sorts of cases of the algorithm. The next step is the actual animation.


## Dijkstra's Algorithm Animation

To begin the animation, the user has to click the play button at the top menu. However, if not all the proper conditions are met, the user is sent a message saying so. The presence of nodes, an animation type being chosen in the drop down box, and the home and destination nodes being assigned to something are the conditions required for an animation to be initialized. Once initialized the animation will go through each step of Dijkstra's algorithm. The user then has a choice of either pausing the animation whenever he wishes and can also go through it step by step by using the step forward button.

![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image038.jpg)

Consider the following graph. All conditions are met and Dijkstra's Algorithm is being selected in the drop down box. Once the user clicks play, the animation will begin and the program will begin the simulation, first by taking the home node and assigning it a distance value of zero and assigning all other nodes a distance of infinity.

![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image040.jpg)

The next step is checking the children of the node. Each edge from the home node is then checked along with the node attached to the other end where a proposed distance for that node is calculated by the home node's distance plus the edge weight.

![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image042.jpg)

The proposed distance is then checked with the current node's distance. These two steps are usually missed in Dijkstra's algorithm simulations, however, I believe that they capture the essence of how Dijkstra's algorithm works and should be included, if not, emphasized.

![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image044.jpg)

Since the two new proposed weights are obviously less than infinity, they are now adopted as the new node distances for the nodes. After this, the process is repeated by selecting the smallest distance node that has not been processed (nodes that have been processed are in blue). This process is continued until the destination node is either found or all nodes have been processed in the graph.

![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image046.jpg)

![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image048.jpg)

Once a node is checked, if the proposed distance is smaller than the node distance, the program also keeps track of the edge where that shortest distance came from. This way, once the node is processed, the program knows which edge gives that node its shortest distance and colors it appropriately as well as the node.

![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image050.jpg)

![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image052.jpg)

In this case, the proposed distance was larger than the node distance for the bottom middle node, and therefore, the incoming edge is ignored and so is the proposed value.

![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image054.jpg)

As mentioned, this process continues until the destination node is found or when all the nodes in the graph have been processed. When the destination node is found, the user will receive a message saying so and will color the shortest path to it. If the destination node is not found, then the user will receive a message saying that it wasn't found and the animation will stop there. The following image is of the destination node being found.

![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image056.jpg)

The majority of the steps of the animation where skipped due to mere repetitiveness, but if you'd like to see the whole animation process, I have created this animated gif which goes over every step.

![](https://raw.github.com/mdayaram/graph-animator/master/docs/finalreport_files/image057.gif)

## Issues to be Resolved

Sadly, not all things discussed in the beginning proposal could be implemented. Most of these could not be done simply because of lack of time, but can easily be implemented in the future. Things such as the save, save as, and open features were left undone because they were of low priority compared to the rest of the program. However, this does not mean that it would be hard to implement. Another feature that could not be finished was the step backwards in the Dijkstra's algorithm animation. This too, could not be finished due to lack of time, but should not be too complicated to implement.

## Final Thoughts

The Graph Animator has a lot of potential and I would like to continue working on it or at least see work being done with it. There are several possibilities and features that could be appended to the project. Things such as node labeling for one could improve the programs functionality. A help menu for getting started was going to be made, but that too could not be finished due to time constraints, but could easily be done in the near future. More gesture recognition could be added in order to make things such as one way edge or the node labeling. As said, this project has a lot of potential and can only grow in functionality over time.

