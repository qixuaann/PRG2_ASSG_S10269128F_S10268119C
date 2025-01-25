using System.Runtime.CompilerServices;

// feature 2 -load flight.csv (flights)

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
// --- end of feature 2 ----

// feature 3 - list flights
void DisplayFlights(Dictionary<string, Flight> flightDict)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Flights for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("{0,-16} {1,-16} {2,-20} {3,-21} {4,-21}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");
    foreach (var kvp in flightDict)
    {
        var flight = kvp.Value;

        // add airline name after qx loads data from airlines.csv
        Console.WriteLine("{0,-16} {1,-16} {2,-20} {3,-21} {4,-21}", flight.FlightNumber, "Empty for now", flight.Origin, flight.Destination, flight.ExpectedTime);
    }
}
// --- end of feature 3 ----

// main (options and calling of method)
void MainCall(Dictionary<string, Flight> flightDict)
{
    LoadFlights(filepath, flightDict);

    while (true)
    {
        Console.WriteLine("=============================================");
        Console.WriteLine("Welcome to Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine("1. List All Flights");
        Console.WriteLine("2. List Boarding Gates");
        Console.WriteLine("3. Assign a Boarding Gate to a Flight");
        Console.WriteLine("4. Create Flight");
        Console.WriteLine("5. Display Airline Flights");
        Console.WriteLine("6. Modify Flight Details");
        Console.WriteLine("7. Display Flight Schedule");
        Console.WriteLine("0. Exit\n");

        Console.Write("Please select your option: ");
        string option = Console.ReadLine();
        if (option == "1")
        {
            DisplayFlights(flightDict);
            Console.WriteLine();
        }
        else if (option == "0")
        {
            break;
        }
        else
        {
            Console.WriteLine("Invalid");
        }
    }
}
