// ===========================
// Student Number: S10269128F
// Student Name: Kang Qi Xuan
// Partner Name: Joyce Lim
// ====================

using System;

public class Flight	
{
    //attributes
    public string flightNumber { get; set; }
    public string origin { get; set; }
    public string destination { get; set; }
    public DateTime expectedTime { get; set; }
    public string status { get; set; }

    //constructor
    public Flight(string flightNumber, string origin, string destination, DateTime expectedTime, string status)
	{   
        FlightNumber = flightNumber;
        Origin = origin;
        Destination = destination;
        ExpectedTime = expectedTime;
        Status = status;

	}

    // method
    public virtual double CalculateFees()
    {
        return 300.00; // base fee for all boarding gates
    }

    public override string ToString()
    {
        return $"Flight Number: {FlightNumber}, Origin: {Origin}, Destination: {Destination}, Status: {Status}, Expected: {ExpectedTime}";
    }
}

public class NORMFlight : Flight
{
    //constructor
    public NORMFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status)
        : base(flightNumber, origin, destination, expectedTime, status)
    {
    }

    //method
    public override double CalculateFees()
    {
        if (destination == "Singapore (SIN)")
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
        }
    }
}

public class CFFTFlight : Flight
{
    //attribute
    public double RequestFee { get; set; }

    //constructor
    public CFFTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status, double requestFee)
        : base(flightNumber, origin, destination, expectedTime, status)
    {
        RequestFee = requestFee;
    }

    //method
    public override double CalculateFees()
    {
        return base.CalculateFees() + RequestFee;
    }
    
    public override string ToString()
    {
        return base.ToString() + $"Request Fee: {RequestFee}";
    }
}

public class LWTTFlight : Flight
{
    //attribute
    public double RequestFee { get; set; }

    //constructor
    public LWTTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status, double requestFee)
        : base(flightNumber, origin, destination, expectedTime, status)
    {
        RequestFee = requestFee;
    }

    //method
    public override double CalculateFees()
    {
        return base.CalculateFees() + RequestFee;
    }

    public override string ToString()
    {
        return base.ToString() + $"Request Fee: {RequestFee}";
    }
}

public class DDJBFlight : Flight
{
    //attribute
    public double RequestFee { get; set; }

    //constructor
    public DDJBFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status, double requestFee)
        : base(flightNumber, origin, destination, expectedTime, status)
    {
        RequestFee = requestFee;
    }

    //method
    public override double CalculateFees()
    {
        return base.CalculateFees() + RequestFee;
    }

    public override string ToString()
    {
        return base.ToString() + $"Request Fee: {RequestFee}";
    }
}
}
