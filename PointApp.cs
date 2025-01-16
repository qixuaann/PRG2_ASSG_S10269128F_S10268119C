using PointApp;

//create 2 rect objects
Point ptA = new Point(2, 2);
Point ptB = new Point(5, 5);

// Distance between 2 points
Console.WriteLine($"Distance between A and B: {ptA.Distance(ptB):0.00}");
Console.WriteLine($"Distance between B and A: {ptB.Distance(ptA):0.00}");