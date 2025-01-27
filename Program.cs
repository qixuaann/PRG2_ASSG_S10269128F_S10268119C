using System.Runtime.CompilerServices;

// start of feature 1

// load the airlines.csv file
string filepath_airline = "/Users/joyce/Github/PRG2_ASSG_S10269128F_S10268119C/airlines.csv";
Dictionary<string, Airline> airlineDict = new Dictionary<string, Airline>();

void LoadAirlines(string filepath_airline, Dictionary<string, Airline> airlineDict)
{
    using (StreamReader sr = new StreamReader(filepath_airline))
    {
        string? s = sr.ReadLine();

        while ((s = sr.ReadLine()) != null)
        {
            string[] index = s.Split(',');
            string n = index[0];
            string c = index[1];

            Airline airline = new Airline(n, c);
            airlineDict.Add(n, airline); 
        }
    }
}

// airlines.csv file loaded

// load the boardinggates.csv file
string filepath_gate = "/Users/joyce/Github/PRG2_ASSG_S10269128F_S10268119C/boardinggates.csv";
Dictionary<string, BoardingGate> boardinggateDict = new Dictionary<string, BoardingGate>();

void LoadBoardinggate(string filepath_gate, Dictionary<string, BoardingGate> boardinggateDict)
{
    using (StreamReader sr = new StreamReader(filepath_gate))
    {
        string? s = sr.ReadLine();

        while ((s = sr.ReadLine()) != null)
        {
            string[] values = s.Split(',');
            string gateNumber = values[0];
            bool DJJB = bool.Parse(values[1]);
            bool CFFT = bool.Parse(values[2]);
            bool LWWT = bool.Parse(values[3]);

            BoardingGate gate = new BoardingGate(gateNumber, DJJB, CFFT, LWWT);
            boardinggateDict.Add(gateNumber, gate);
        }
    }
}

// boardinggates.csv file loaded 

// end of feature 1

// feature 2 -load flight.csv (flights)

string filepath_flight = "/Users/joyce/Github/PRG2_ASSG_S10269128F_S10268119C/flights.csv";
Dictionary<string, Flight> flightDict = new Dictionary<string, Flight>();

void LoadFlights(string filepath_flight, Dictionary<string, Flight> flightDict)
{
    using (StreamReader sr = new StreamReader(filepath_flight))
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
MainCall(flightDict, airlineDict, boardinggateDict);

void MainCall(Dictionary<string, Flight> flightDict, Dictionary<string, Airline> airlinesDict, Dictionary<string, BoardingGate> boardinggateDict)
{
    LoadFlights(filepath_flight, flightDict);
    LoadAirlines(filepath_airline, airlineDict);
    LoadBoardinggate(filepath_gate, boardinggateDict);

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
