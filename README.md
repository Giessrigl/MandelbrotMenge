# MandelbrotMenge
This is the exercise of the Distributed Systems course in the fifth semester of my bachelor in computer science degree.

## Task 1
Create an application which can display the mandelbrot set.

## Task 2
Create a server which calculates the mandelbrot set instead of the application (of task 1) itself.
Because we had to do this as a team, we had to agree upon a uniformly communication protocol.

## Task 3
Implement threading on the client side. 
The image will be split into small chunks and handled by each thread individually. 
These chunks will be sent to the server (for this task one server handles all of the threads).
