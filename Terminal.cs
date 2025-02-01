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

    public Terminal() {}
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
        string airlineCode = flight.FlightNumber.Substring(0, 2);

        if (Airlines.ContainsKey(airlineCode))
        {
            return Airlines[airlineCode];
        }

        return null;
    }

    public void PrintAirlineFees()
	{
        double totalFeesAllAirlines = 0;
        double totalDiscountsAllAirlines = 0;

        foreach (var airline in Airlines.Values)
        {
            double totalFees = airline.CalculateFlightFees(); // 7205
            double totalDiscounts = airline.CalculateDiscounts(); // 28500
            double finalFees = airline.CalculateFees(); /// 21295

            Console.WriteLine($"Airline: {airline.Name} ({airline.Code})");
            Console.WriteLine($"  Subtotal Fees: ${totalFees}");
            Console.WriteLine($"  Subtotal Discounts: ${totalDiscounts}");
            Console.WriteLine($"  Final Fees: ${finalFees}");
            Console.WriteLine();

            totalFeesAllAirlines += totalFees;
            totalDiscountsAllAirlines += totalDiscounts;
        }

        double finalTotalFees = totalFeesAllAirlines - totalDiscountsAllAirlines;
        double discountPercentage = (totalDiscountsAllAirlines / totalFeesAllAirlines) * 100;

        Console.WriteLine("Summary for All Airlines:");
        Console.WriteLine($"  Total Subtotal Fees: ${totalFeesAllAirlines}");
        Console.WriteLine($"  Total Subtotal Discounts: ${totalDiscountsAllAirlines}");
        Console.WriteLine($"  Final Total Fees: ${finalTotalFees}");
        Console.WriteLine($"  Discount Percentage: {discountPercentage:F2}%");
    }

	public override string ToString()
	{
        return $"Terminal: {TerminalName}\t Airlines: {Airlines.Count}\t Boarding Gates: {BoardingGates.Count}";
    }
}
