// ===========================
// Student Number: S10268119C
// Student Name: Joyce Lim
// Partner Name: Kang Qi Xuan
// ===========================

using System;

public class Airline
{
	public string Name { get; set; }
	public string Code { get; set; }
    public Dictionary<string, Flight> Flights { get; set; }

    public Airline() {}
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

    // added method for advance feature (b)
    public double CalculateDiscounts()
    {
        double discounts = 0;
        int flightCount = Flights.Count;

        // disc for every 3 flights
        if (flightCount >= 3)
        {
            discounts += 350 * (flightCount / 3);
        }

        // 3% off for >5 flights
        if (flightCount > 5)
        {
            discounts += CalculateFees() * 0.03;
        }

        // disc for flights before 11am or after 9pm
        foreach (var flight in Flights.Values)
        {
            if (flight.ExpectedTime.Hour < 11 || flight.ExpectedTime.Hour >= 21)
            {
                discounts += 110;
            }

            if (flight.Origin == "Dubai (DXB)" || flight.Origin == "Bangkok (BKK)" || flight.Origin == "Tokyo (NRT)")
            {
                discounts += 25;
            }

            // disc for no special request codes
           if (string.IsNullOrEmpty(flight.SpecialRequestCode))
           {
                discounts += 50;
           }
        }
        return discounts;
    }

    // added method for advance feature (b)
    public double CalculateTotalFees()
    {
        return CalculateFees() - CalculateDiscounts();
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
