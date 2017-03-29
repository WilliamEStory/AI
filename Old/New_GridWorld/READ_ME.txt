This application is fairly straight forward, just click start and that will run the program.
The program is initially loaded with the updated Q(S,A) table. The original start
Q(S,A) table is located in the file labeled Original_QSA.

If you want to compare the behavior of the agent with these two QSA files, just move either
into the bin/debug and select to replace the current QSA file already there.

I also included a before and after video for comparison.

The agent starts at the darker, green colored square. Wherever it moves, the program
will color the square a light green color. The board is also updated with a direction
the agent took.

U is for up.
D is for down.
L is for left.
R is for right.

The goal state is colored blue.
Walls are colored black.
If the agent hits a wall, it will color that wall red to let you know the agent
moved onto that wall. If the agent moves onto a border state, there is no change in color 
but the agent does terminate.

After the end of each run, the board is reset to its original state.

The file for holding the QSA table is only updated after every 100 runs, and there is a 150 second sleep
after each move to make it easier to observe the agent's moves.