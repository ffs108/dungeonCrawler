___________________________________________________________

Assignment 4 - ISTA 425
___________________________________________________________

+	Daniel Shevelev					          
+	Koan Zhang						          
+	Lin Shao						          
+	Francisco Figueroa				          
___________________________________________________________


In this assignment we have implemented the following:

* Map Representation 
	The individual tiles in the worldspace are represented by a two-dimensional List of
List holding Object created to hold relevant information such as h, g, f values as well as
the particular objects' position in worldspace and x and y indexes in the 2d List. These 
"GridTileObjects" are added to each tile at runtime where they are also marked as being
either walkable or not and what tile the was last arrived from.

* A* (A Star) Algorithm
	Given both an the x and y indexes for a beginning node and ending node, proceeds to
calculating h, g and f values in relation to the start and end nodes, afterwards looping
through each node, and each neighbor of the current node checking the openlist for the 
optimial route and marking visited nodes in closedList. At the end of the inner loop, once
the current node is the ending node, the fill path is calculated from the tile this tile
came from. This is returned as a List of GridTileObjects.

* Hero Movement
	The Hero is able to move to the exact tile the mouse has clicked in an autonomous
manner. This is done by grabbing the world position of mousePosition at input, represented
as a Ray structure and associating the clicked tile with its GridTileObject. This allows
us to be able to grab the x and y indexes of the grid and call our A* algorithm. Once we
have a List representing our optimized path, a Coroutine is started in which each node 
segment is vistied via linear interpolation in relation to Time.deltaTime. Once the position
of the Hero is the same as the goal, the start postion is then changed to the goal indexes
and the Hero is ready to be moved again. Errors are printed when invalid node is selected.


___________________________________________________________

*Hero Walk Animation
	The Hero has a gate and moves feet in accordance to the autonomous walking following
the movement "speed" of the linear interpolation to simulated the actual steps.
