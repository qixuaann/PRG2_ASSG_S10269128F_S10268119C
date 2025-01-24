// ===========================
// Student Number: S10268119C
// Student Name: Joyce Lim
// Partner Name: Kang Qi Xuan
// ===========================


using System;

public class BoardingGate
{
    public string GateName { get; set; }
    public bool SupportsCFFT { get; set; }
    public bool SupportsDDJB { get; set; }
    public bool SupportsLWTT{ get; set; }
    public Flight Flight { get; set; }


    public BoardingGate(string gn, bool supportsCFFT, bool supportsDDJB, bool supportsLWTT)
	{
        GateName = gn;
        SupportsCFFT = supportsCFFT;
        SupportsDDJB = supportsDDJB;
        SupportsLWTT = supportsLWTT;
        // since there is not flight assigned yet
        Flight = null; 
    }

    public double CalculateFees()
    {
        double baseFee = 300; 
        if (Flight is CFFTFlight) baseFee += 150;
        if (Flight is DDJBFlight) baseFee += 300;
        if (Flight is LWTTFlight) baseFee += 500;
        return baseFee;
    }

    public override string ToString()
    {
        string flightNumber = "None";
        if (Flight != null)
        {
            flightNumber = Flight.FlightNumber;
        }
        return $"Gate: {GateName}\t Flight: {flightNumber}";
    }
}
