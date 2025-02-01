// ===========================
// Student Number: S10269128F
// Student Name: Kang Qi Xuan
// Partner Name: Joyce Lim
// ===========================

using System;
using System.Runtime.InteropServices;

public class Flight	: IComparable<Flight>
{
    public string FlightNumber { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public DateTime ExpectedTime { get; set; }
    public string Status { get; set; } = "On Time"; // default as stated
    public BoardingGate BoardingGate { get; set; } // added for advance feature (b)
    public string SpecialRequestCode { get; set; } // added for advance feature (b)


    //constructor
    public Flight(string flightNumber, string origin, string destination, DateTime expectedTime, string specialRequestCode = null)
	{   
        FlightNumber = flightNumber;
        Origin = origin;
        Destination = destination;
        ExpectedTime = expectedTime;
        SpecialRequestCode = specialRequestCode; // added for advance feature (b)
    }

    // method
    public virtual double CalculateFees()
    {
        double fee = 0;
        if (Destination == "Singapore (SIN)")
        {
            // arriving 
            fee += 500;
        }
        else if (Origin == "Singapore (SIN)")
        {
            // departing
            fee += 800;
        }
        fee += 300;
        
        if (SpecialRequestCode != null)
        {
            if (SpecialRequestCode == "DDJB")
            {
                fee += 300;
            }
            else if (SpecialRequestCode == "CFFT")
            {
                fee += 150;
            }
            else if (SpecialRequestCode == "LWTT")
            {
                fee += 500;
            }
        }
        return fee;
    }

    public int CompareTo(Flight other)
    {
        if (other == null)
        {
            return 1;
        }
        return ExpectedTime.CompareTo(other.ExpectedTime);
    }

    public override string ToString()
    {
        return $"Flight Number: {FlightNumber}, Origin: {Origin}, Destination: {Destination}, Expected: {ExpectedTime:dd/MM/yyyy h:mm tt}, Status: {Status}";
    }
}

