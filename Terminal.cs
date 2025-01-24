// ===========================
// Student Number: S10268119C
// Student Name: Joyce Lim
// Partner Name: Kang Qi Xuan
// ===========================

using System;

public class Terminal
{
	public string TerminalName { get; set; }
	public Dictionary<string,Airline> Airlines { get; set; }
	public Dictionary<string, Flight> Flights { get; set; }
	public Dictionary<string, BoardingGate> BoardingGates { get; set; }
	public Dictionary<string, double> GateFees { get; set; }

    public Terminal() { };

	public Terminal(string t)
	{
		TerminalName = t;
        Airlines = new Dictionary<string, Airline>();
        Flights = new Dictionary<string, Flight>();
        BoardingGates = new Dictionary<string, BoardingGate>();
        GateFees = new Dictionary<string, double>();
    }

    public bool AddAirline(Airline airline)
    {
        if (!Airlines.ContainsKey(airline.Name))
        {
            Airlines.Add(airline.Name, airline);
            return true;
        }
        return false;
    }

    public bool AddBoardingGate(BoardingGate gate)
    {
        if (!BoardingGates.ContainsKey(gate.GateName))
        {
            BoardingGates.Add(gate.GateName, gate);
            return true;
        }
        return false;
    }

    public Airline GetAirlineFromFlight(Flight flight)
    {
        foreach (var airline in Airlines.Values)
        {
            if (airline.Flights.ContainsKey(flight.FlightNumber))
            {
                return airline;
            }
        }
        return null;
    }

    public void PrintAirlineFees()
	{
        foreach (var airline in Airlines.Values)
        {
            Console.WriteLine($"Airline: {airline.Name}\t Total Fees: {airline.CalculateFees():C}");
        }
    }

	public override string ToString()
	{
        return $"Terminal: {TerminalName}\t Airlines: {Airlines.Count}\t Boarding Gates: {BoardingGates.Count}";
    }

}
