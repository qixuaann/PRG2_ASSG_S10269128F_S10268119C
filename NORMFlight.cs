// ===========================
// Student Number: S10269128F
// Student Name: Kang Qi Xuan
// Partner Name: Joyce Lim
// ===========================

using System;
public class NORMFlight : Flight
{
    //constructor
    public NORMFlight(string flightNumber, string origin, string destination, DateTime expectedTime)
        : base(flightNumber, origin, destination, expectedTime) { }

    //method
    public override double CalculateFees()
    { 
        return base.CalculateFees();
    }

    //tostring() method
    public override string ToString()
    {
        return base.ToString() + " (NORMFlight)";
    }
}