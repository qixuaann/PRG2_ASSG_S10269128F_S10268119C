// ===========================
// Student Number: S10269128F
// Student Name: Kang Qi Xuan
// Partner Name: Joyce Lim
// ===========================

using System;

public class Flight	
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
        ExpectedTime = expectedTime;	}

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

    public override string ToString()
    {
        return $"Flight Number: {FlightNumber}, Origin: {Origin}, Destination: {Destination}, Expected: {ExpectedTime}, Status: {Status}";
    }
}

// put in NORMFlight.cs 
public class NORMFlight : Flight
{
    //constructor
    public NORMFlight(string flightNumber, string origin, string destination, DateTime expectedTime)
        : base(flightNumber, origin, destination, expectedTime) { } 

    //method
    public override double CalculateFees()
    {
        /*if (destination == "Singapore (SIN)")
        {
            return base.CalculateFees() + 500.00;
        }
        else if (origin == "Singapore (SIN)")
        {
            return base.CalculateFees() + 800.00;
        }
        else
        {
            return base.CalculateFees();
        }*/
        return base.CalculateFees(); // no additional fees, see table 6
    }
    // add the override tostring() method
    public override string ToString()
    {
        return base.ToString() + " (NORMFlight)";
    }
}

// put in CFFTFlight.cs 
public class CFFTFlight : Flight
{
    //attribute
    public double RequestFee { get; set; }

    //constructor

    public CFFTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string requestFee)
        : base(flightNumber, origin, destination, expectedTime)
    {
        RequestFee = 150; 
    }
    //method
    public override double CalculateFees()
    {
        return base.CalculateFees() + RequestFee; // additional fee, see table 6
    }
    
    public override string ToString()
    {
        return base.ToString() + $", Request Fee: {RequestFee} (CCFTFlight)";
    }
}

// put in DDJBFlight.cs 
public class DDJBFlight : Flight
{
    //attribute
    public double RequestFee { get; set; }

    //constructor
    public DDJBFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string requestFee)
        : base(flightNumber, origin, destination, expectedTime)
    {
        RequestFee = 300;
    }

    //method
    public override double CalculateFees()
    {
        return base.CalculateFees() + RequestFee; // additional fee, see table 6
    }

    public override string ToString()
    {
        return base.ToString() + $", Request Fee: {RequestFee} (DDJBFlight)";
    }
}


// put in LWTTFLight.cs 
public class LWTTFlight : Flight
{
    //attribute
    public double RequestFee { get; set; }

    //constructor
    public LWTTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string requestFee)
        : base(flightNumber, origin, destination, expectedTime)
    {
        RequestFee = 500;
    }

    //method
    public override double CalculateFees()
    {
        return base.CalculateFees() + RequestFee; // additional fee, see table 6
    }

    public override string ToString()
    {
        return base.ToString() + $", Request Fee: {RequestFee} (LWTTFlight)";
    }
}
