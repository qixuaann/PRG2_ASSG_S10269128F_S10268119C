// ===========================
// Student Number: S10269128F
// Student Name: Kang Qi Xuan
// Partner Name: Joyce Lim
// ===========================

using System;
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
        return base.CalculateFees() + RequestFee;
    }

    //tostring() method
    public override string ToString()
    {
        return base.ToString() + $", Request Fee: {RequestFee} (DDJBFlight)";
    }
}
