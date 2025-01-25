using System.Runtime.CompilerServices;

// feature 2, load flight.csv (flights)

string filepath = "C:\\Users\\joyce\\source\\repos\\PRG2_ASSG_S10269128F_S10268119C\\flights.csv";
Dictionary<string, Flight> flightDict = new Dictionary<string, Flight>();
MainCall(flightDict);

void LoadFlights(string filepath, Dictionary<string, Flight> flightDict)
{
    using (StreamReader sr = new StreamReader(filepath))
    {
        string? s = sr.ReadLine();
    
        while ((s = sr.ReadLine()) != null)
        {
            string[] values = s.Split(',');
            string flightNumber = values[0];
            string origin = values[1];
            string destination = values[2];
            DateTime expectedTime = DateTime.Parse(values[3]);

            Flight flight = new Flight(flightNumber, origin, destination, expectedTime);
            flightDict.Add(flightNumber, flight);
        }
    }
}

// feature 2, display flights
void DisplayFlights(Dictionary<string, Flight> flightDict)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Flights for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");

    foreach (var kvp in flightDict)
    {
        var flight = kvp.Value;

        // add airline name after qx loads data from airlines.csv
        Console.WriteLine("{0,10} {1,10} {2,10} {3,10}", flight.FlightNumber, flight.Origin, flight.Destination, flight.ExpectedTime);
    }
}
void MainCall(Dictionary<string, Flight> flightDict)
{
    LoadFlights(filepath, flightDict);

    while (true)
    {
        Console.Write("Enter option: ");
        string option = Console.ReadLine();
        if (option == "1")
        {
            DisplayFlights(flightDict);
        }
        else
        {
            Console.WriteLine("Invalid");
        }
    }
}
