using System;

public class Airline
{
	public string Name { get; set; }
	public string Code { get; set; }
    public Dictionary<string, Flight> Flights { get; set; }

    public Airline(string n, string c)
	{
		Name = n;
		Code = c;
		Flights = new Dictionary<string, Flight>();
	}

	public bool AddFlight(Flight flight)
	{
        if (!Flights.ContainsKey(flight.FlightNumber))
        {
            Flights.Add(flight.FlightNumber, flight);
            return true;
        }
        return false;
    }

    public double CalculateFees()
    {
        double totalFees = 0;
        foreach (var flight in Flights.Values)
        {
            totalFees += flight.CalculateFees();
        }
        return totalFees;
    }

    public bool RemoveFlight(Flight flight)
    {
        return Flights.Remove(flight.FlightNumber);
    }

    public override string ToString()
    {
        return $"Airline: {Name}\t Code: {Code}\t Number of Flights: {Flights.Count}";
    }
}
