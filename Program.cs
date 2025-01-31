using System.Runtime.CompilerServices;
using System.Windows.Markup;

// start of feature 1

// load the airlines.csv file
string filepath_airline = "C:\\Users\\Qi Xuan\\PRG2_ASSG_S10269128F_S10268119C\\airlines.csv";
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
string filepath_gate = "C:\\Users\\Qi Xuan\\PRG2_ASSG_S10269128F_S10268119C\\boardinggates.csv";
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

string filepath_flight = "C:\\Users\\Qi Xuan\\PRG2_ASSG_S10269128F_S10268119C\\flights.csv";
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
            string requestCode = values[4];

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
    Console.WriteLine("{0,-16} {1,-20} {2,-20} {3,-21} {4,-21}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");
    foreach (var kvp in flightDict)
    {
        var flight = kvp.Value;
        foreach (var kvp2 in airlineDict) {
            var airline = kvp2.Value;
            Console.WriteLine("{0,-16} {1,-20} {2,-20} {3,-21} {4,-21}", flight.FlightNumber, airline.Name, flight.Origin, flight.Destination, flight.ExpectedTime);
        }
    }
}
// --- end of feature 3 ----

// feature 4 - list all boarding gates 
void DisplayBoardinggates (Dictionary<string, BoardingGate> boardinggateDict)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("{0,-16} {1,-20} {2,-20} {3,-21}", "Gate Name", "DDJB", "CFFT", "LWTT");
    foreach (var boardinggateEntry in boardinggateDict)
    {
        var boardinggate = boardinggateEntry.Value;
        Console.WriteLine("{0,-16} {1,-20} {2,-20} {3,-21}", boardinggate.GateName, boardinggate.SupportsDDJB, boardinggate.SupportsCFFT, boardinggate.SupportsLWTT);
    }
}
// --- end of feature 4 ---

// feature 5 - assign a boarding gate to a flight
void AssignGateToFlight(Dictionary<string, Flight> flightDict, Dictionary<string, BoardingGate> boardinggateDict)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("Assign a Boarding Gate to a Flight");
    Console.WriteLine("=============================================");
    Console.WriteLine("Enter Flight Number:");
    string flightNo = Console.ReadLine();
    Console.WriteLine("Enter Boarding Gate Name:");
    string gateName = Console.ReadLine();

    if (!flightDict.ContainsKey(flightNo))
    {
        Console.WriteLine("Flight number not found.");
        return;
    }

    Flight flight = flightDict[flightNo];
    // need add \nSpecial Request Code: 
    LoadFlights(filepath_flight, flightDict);
    // string requestCode;
    Console.WriteLine($"Flight Number: {flight.FlightNumber}\nOrigin: {flight.Origin}\nDestination: {flight.Destination}\nExpectedTime: {flight.ExpectedTime}");
    // Console.WriteLine($"Special Request Code: {requestCode}");
    if (!boardinggateDict.ContainsKey(gateName))
    {
        Console.WriteLine("Boarding gate name not found.");
        return;
    }

    BoardingGate selectedGate = boardinggateDict[gateName];

    if (selectedGate.Flight != null)
    {
        Console.WriteLine($"Boarding Gate {gateName} is already assigned to another flight.");
        return;
    }

    Console.WriteLine($"Boarding Gate Name: {selectedGate.GateName}\nSupports DDJB: {selectedGate.SupportsDDJB}\nSupports CFFT: {selectedGate.SupportsCFFT}\nSupports LWTT: {selectedGate.SupportsLWTT}");
    Console.WriteLine("Would you like to update the status of the flight? (Y/N)");
    string status = Console.ReadLine();
    if (status == "Y")
    {
        Console.WriteLine("1. Delayed\n2. Boarding\n3. On Time");
        Console.WriteLine("Please select the new status of the flight:");
        string newStatus = Console.ReadLine();
        flight.Status = newStatus;
        Console.WriteLine($"Flight {flightNo} has been assigned to Boarding Gate {gateName}\n!");
    }
    else if (status == "N")
    {
        flight.Status = "On Time";
        Console.WriteLine($"Status of the flight will be set to default of {"On Time"}\n");
    }
}
// --- end of feature 5 ----

// feature 6 - create a new flight
void CreateFlight(Dictionary<string, Flight> flightDict) 
{
    while (true) 
    {
        Console.Write("Enter Flight Number: ");
        string flightNo = Console.ReadLine();
        Console.Write("Enter Origin: ");
        string origin = Console.ReadLine();
        Console.Write("Enter Destination: ");
        string destination = Console.ReadLine();
        Console.Write("Enter Expected Departure/Arrival Time (dd/m/yyyy hh:mm): ");
        DateTime time = DateTime.ParseExact(Console.ReadLine(), "d/M/yyyy HH:mm", null);
        Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
        string requestCode = Console.ReadLine();
        // will figure out where to store code
        Flight newFlight = new Flight(flightNo, origin, destination, time);
        flightDict.Add(flightNo, newFlight);
        Console.WriteLine($"Flight {flightNo} has been added!");
        Console.WriteLine("Would you like to add another flight? (Y/N)");
        string permission = Console.ReadLine();
        if (permission == "N")
        {
            break;
        }
        else 
        {
            continue;
        }
    }
}
// --- end of feature 6 ----

// feature 7 - display full flight details from an airline
void DisplayFullflightdetails (Dictionary<string, Airline> airlineDict)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("{0,-16} {1,-20}", "Airline Code", "Airline Name");
    foreach (var flightEntry in airlineDict)
    {
        var flightdetail = flightEntry.Value;
        Console.WriteLine("{0,-16} {1,-20}", flightdetail.Code, flightdetail.Name);
    }
    Console.Write("Enter Airline Code: ");
    string inputCode = Console.ReadLine();
    if (!airlineDict.ContainsKey(inputCode))
    {
        Console.WriteLine("Invalid Airline Code. Please try again.");
        return;
    }
    // Get the selected airline from the dictionary using the input code
    Airline selectedAirline = airlineDict[inputCode];

    // Create an empty list to store the filtered flights
    List<Flight> filteredFlights = new List<Flight>();

    // Go through each flight in the flight dictionary
    foreach (var flightEntry in flightDict)
    {
        Flight flight = flightEntry.Value;

        // If the flight's airline name matches the selected airline's name, add it to the filtered list
        string airlineCode = flight.FlightNumber.Substring(0, 2);
        if (airlineCode == selectedAirline.Name)
        {
            filteredFlights.Add(flight);
        }
    }
    // Display the selected airline's name and code
    Console.WriteLine($"\nFlights for Airline: {selectedAirline.Name} ({inputCode})");
    Console.WriteLine("========================================================");

    // Check if there are any flights for the selected airline
    if (filteredFlights.Count > 0)
    {
        // Display table headers
        Console.WriteLine("{0,-16} {1,-20} {2,-20} {3,-21}", "Flight Number", "Origin", "Destination", "Departure/Arrival Time");

        // Go through the filtered flights and print their details
        foreach (var flight in filteredFlights)
        {
            Console.WriteLine("{0,-16} {1,-20} {2,-20} {3,-21}", flight.FlightNumber, flight.Origin, flight.Destination, flight.ExpectedTime);
        }
    }
    else
    {
        // If no flights are found, display this message
        Console.WriteLine("No flights available for this airline.");
    }
}

// feature 8 - modify flight details

// feature 9 - display scheduled flights in chronological order
// with boarding gates assignments where applicable 
void DisplayScheduledFlights(Dictionary<string, Flight> flightDict, Dictionary<string, BoardingGate> boardinggateDict, Dictionary<string, Airline> airlineDict)
{

    Console.WriteLine("=============================================");
    Console.WriteLine("Flight Schedule for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");

    if (flightDict.Count == 0)
    {
        Console.WriteLine("No flights scheduled for today.");
        return;
    }

    List<Flight> flightList = flightDict.Values.ToList();

    flightList.Sort();
    Dictionary<string, Flight> sortedFlightDict = new Dictionary<string, Flight>();
    foreach (var flight in flightList)
    {
        sortedFlightDict[flight.FlightNumber] = flight;
    }

    Console.WriteLine("{0,-16} {1,-20} {2,-20} {3,-21} {4,-21} {5,-21}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time", "Status");
    foreach (var kvp in sortedFlightDict)
    {
        var flight = kvp.Value;
        foreach (var kvp2 in airlineDict) {
            var airline = kvp2.Value;
            Console.WriteLine("{0,-16} {1,-20} {2,-20} {3,-21} {4,-21} {5, -21}", flight.FlightNumber, airline.Name, flight.Origin, flight.Destination, flight.ExpectedTime, flight.Status);
        }
    }
    
}
// --- end of feature 9 ----

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
        else if (option == "2")
        {
            DisplayBoardinggates(boardinggateDict);
            Console.WriteLine();
        }
        else if (option  == "3") 
        {
            DisplayBoardinggates(boardinggateDict);
            Console.WriteLine();
        }
        else if (option == "4")
        {
            CreateFlight(flightDict);
            Console.WriteLine();
        }
        else if (option == "5")
        {
            DisplayFullflightdetails(airlineDict);
            Console.WriteLine();
        }
        else if (option == "7")
        {
            DisplayScheduledFlights(flightDict, boardinggateDict, airlineDict);
        }
        else if (option == "0")
        {
            Console.WriteLine("Goodbye!");
            break;
        }
        else
        {
            Console.WriteLine("Invalid");
        }
    }
}
