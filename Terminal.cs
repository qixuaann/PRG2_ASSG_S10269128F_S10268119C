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
        if (!Airlines.ContainsKey(airline.Code))
        {
            Airlines.Add(airline.Code, airline);
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
        foreach (var airlineEntry in Airlines)
        {
            Airline airline = airlineEntry.Value;
            double totalFees = 0;
            double discount = 0;

            foreach (var flight in airline.Flights.Values)
            {
                totalFees += flight.CalculateFees();

                if (flight.ExpectedTime.Hour < 11 || flight.ExpectedTime.Hour >= 21)
                {
                    discount += 110;
                }
                if (flight.Origin == "Dubai (DXB)" || flight.Origin == "Bangkok (BKK)" || flight.Origin == "Tokyo (NRT)")
                {
                    discount += 25;
                }
                if (string.IsNullOrEmpty(flight.Status))
                {
                    discount += 50;
                }
            }

            int flightCount = airline.Flights.Count;
            discount += (flightCount/3) * 350;
            if (flightCount > 5) 
            {
                discount += totalFees * 0.03; // discount off base fee
            }
            
            double finalFee = totalFees - discount;

            Console.WriteLine($"Airline: {airline.Name}\t Total Base Fees: {totalFees:C2}\t Total Discount: {discount:C2}\t Final Fee: {finalFee:C2} ");
        }
    }

	public override string ToString()
	{
        return $"Terminal: {TerminalName}\t Airlines: {Airlines.Count}\t Boarding Gates: {BoardingGates.Count}";
    }

}
