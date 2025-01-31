// ===========================
// Student Number: S10269128F
// Student Name: Kang Qi Xuan
// Partner Name: Joyce Lim
// ===========================

using System;

public class Flight	: IComparable<Flight>
{
    //attributes - capitalized since its in public form (class diagram is in private form) 
    public string FlightNumber { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public DateTime ExpectedTime { get; set; }
    public string Status { get; set; } = "On Time"; // default as stated


    //constructor
    public Flight(string flightNumber, string origin, string destination, DateTime expectedTime)
	{   
        FlightNumber = flightNumber;
        Origin = origin;
        Destination = destination;
        ExpectedTime = expectedTime;	
    }

    // method
    public virtual double CalculateFees()
    {
        double baseFee;
        if (Destination == "SIN")
        {
            // arriving 
            baseFee = 500;
        }
        else
        {
            // departing
            baseFee = 800;
        }
        return baseFee;
        // return 300.00; - base fee for all boarding gates (thats for terminal) 
    }

    public int CompareTo(Flight other)
    {
        if (other == null)
        {
            return 1;
        }
        return this.ExpectedTime.CompareTo(other.ExpectedTime);
    }

    public override string ToString()
    {
        return $"Flight Number: {FlightNumber}, Origin: {Origin}, Destination: {Destination}, Expected: {ExpectedTime:dd/MM/yyyy h:mm tt}, Status: {Status}";
    }
}

