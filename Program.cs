using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Windows.Markup;

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
            // changed to code as key
            airlineDict.Add(c, airline); 
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

// feature 2 - load flights.csv (flights)
Dictionary<string, string> requestCodeDict = new Dictionary<string, string>();

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
            string requestCode = values[4];

            Flight flight = new Flight(flightNumber, origin, destination, expectedTime);
            flightDict.Add(flightNumber, flight);
            requestCodeDict.Add(flightNumber, requestCode);
        }
    }
}
// --- end of feature 2 ----

// feature 3 - list flights
void DisplayFlights(Terminal terminal, Dictionary<string, Flight> flightDict)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Flights for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("{0,-16} {1,-20} {2,-20} {3,-21} {4,-21}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");
    foreach (var kvp in flightDict)
    {
        var flight = kvp.Value;
        var airline = terminal.GetAirlineFromFlight(flight);
        Console.WriteLine("{0,-16} {1,-20} {2,-20} {3,-21} {4,-21}", flight.FlightNumber, airline.Name, flight.Origin, flight.Destination, flight.ExpectedTime);
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
void AssignGateToFlight(Dictionary<string, Flight> flightDict, Dictionary<string, BoardingGate> boardinggateDict, Dictionary<string, string> requestCodeDict)
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
    foreach (var kvp in requestCodeDict)
    {
        string flightID = kvp.Key;
        string requestCode = kvp.Value;
        if (flightID == flight.FlightNumber)
        {
            Console.WriteLine($"Flight Number: {flight.FlightNumber}\nOrigin: {flight.Origin}\nDestination: {flight.Destination}\nExpectedTime: {flight.ExpectedTime}\nSpecial Request Code: {requestCode}");
        }
        else 
        {
            Console.WriteLine($"Flight Number: {flight.FlightNumber}\nOrigin: {flight.Origin}\nDestination: {flight.Destination}\nExpectedTime: {flight.ExpectedTime}\nSpecial Request Code: None");
        }
    }
    if (!boardinggateDict.ContainsKey(gateName))
    {
        Console.WriteLine("Boarding gate name not found.");
        return;
    }

    BoardingGate selectedGate = boardinggateDict[gateName];
    // assign boarding gate to the flight
    flight.BoardingGate = selectedGate;

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
void CreateFlight(Dictionary<string, Flight> flightDict, Dictionary<string, string> requestCodeDict) 
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
        Flight newFlight = new Flight(flightNo, origin, destination, time);
        flightDict.Add(flightNo, newFlight);
        requestCodeDict.Add(flightNo, requestCode);
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

void DisplayScheduledFlights(Terminal terminal, Dictionary<string, Flight> flightDict, Dictionary<string,string> requestCodeDict)
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
    
    Dictionary<string, string> predefinedGates = new Dictionary<string, string>
    {
        { "SQ 693", "A13" },
        { "MH 722", "B2" },
        { "CX 312", "C22" }
    };   

    Console.WriteLine("{0,-16} {1,-20} {2,-21} {3,-21} {4,-35} {5,-18} {6,-25} {7,-25}",
                        "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time", "Request Code", "Status", "Boarding Gate");
    
    foreach (var flight in flightList)
    {
        Airline airline = terminal.GetAirlineFromFlight(flight); 
        string airlineName;
        if (airline != null)
        {
            airlineName = airline.Name; 
        }
        else 
        {
            airlineName = "Unknown Airline";
        }

        string assignedGate;
        if (predefinedGates.ContainsKey(flight.FlightNumber))
        {
            assignedGate = "Assigned";
        }
        else 
        {
            assignedGate = "Unassigned";
        }

       string code;
        if (requestCodeDict.Keys.Contains(flight.FlightNumber))
        {
            code = requestCodeDict[flight.FlightNumber];
            Console.WriteLine("{0,-16} {1,-20} {2,-21} {3,-21} {4,-35} {5,-18} {6,-25} {7,-25}",
                    flight.FlightNumber, airlineName, flight.Origin, flight.Destination,
                    flight.ExpectedTime, code, "Scheduled", assignedGate);
        }
        else
        {
            Console.WriteLine("{0,-16} {1,-20} {2,-21} {3,-21} {4,-35} {5,-18} {6,-25} {7,-25}",
                    flight.FlightNumber, airlineName, flight.Origin, flight.Destination,
                    flight.ExpectedTime, "Scheduled", assignedGate);
        }
       
    }
}

// --- end of feature 9 ----
// advanced feature - (b) display the total fee per airline for the day
void DisplayTotalFeePerAirline(Terminal terminal)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("Total Fee Per Airline for the Day");
    Console.WriteLine("=============================================");

   bool allFlightsAssigned = true;
    foreach (var flight in terminal.Flights.Values)
    {
        if (flight.BoardingGate == null)
        {
            allFlightsAssigned = false;
            Console.WriteLine($"Flight {flight.FlightNumber} has not been assigned a boarding gate.");
        }
    }

    if (!allFlightsAssigned)
    {
        Console.WriteLine("Please ensure that all flights have their boarding gates assigned before running this feature again.");
        return;
    }

    Console.WriteLine();
    DisplayOverallTotals(terminal);
}

void DisplayOverallTotals(Terminal terminal)
{
    Console.WriteLine("\n=============================================");
    Console.WriteLine("Overall Totals for All Airlines");
    Console.WriteLine("=============================================");
    terminal.PrintAirlineFees();
}


// main (options and calling of method)
MainCall(flightDict, airlineDict, boardinggateDict);

void MainCall(Dictionary<string, Flight> flightDict, Dictionary<string, Airline> airlineDict, Dictionary<string, BoardingGate> boardinggateDict)
{
    Terminal terminal = new Terminal();

    LoadAirlines(filepath_airline, airlineDict);
    LoadBoardinggate(filepath_gate, boardinggateDict);
    LoadFlights(filepath_flight, flightDict);

    terminal.Airlines = airlineDict;
    terminal.BoardingGates = boardinggateDict;
    terminal.Flights = flightDict;

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
        Console.WriteLine("8. Display Total Fee per Airline for the day");
        Console.WriteLine("0. Exit\n");

        Console.Write("Please select your option: ");
        string option = Console.ReadLine();
        if (option == "1")
        {
            DisplayFlights(terminal, flightDict);
            Console.WriteLine();
        }
        else if (option == "2")
        {
            DisplayBoardinggates(boardinggateDict);
            Console.WriteLine();
        }
        else if (option  == "3") 
        {
            AssignGateToFlight(flightDict, boardinggateDict, requestCodeDict);
            DisplayBoardinggates(boardinggateDict);
            Console.WriteLine();
        }
        else if (option == "4")
        {
            CreateFlight(flightDict, requestCodeDict);
            Console.WriteLine();
        }
        else if (option == "5")
        {
            DisplayFullflightdetails(airlineDict);
            Console.WriteLine();
        }
        else if (option == "7")
        {
            DisplayScheduledFlights(terminal, flightDict, requestCodeDict);  
            Console.WriteLine();

        }
        // advanced feature (b)
        else if (option == "8")
        {
            DisplayTotalFeePerAirline(terminal);
            DisplayOverallTotals(terminal);
            Console.WriteLine();
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

 // double totalSubtotalFees = 0;
    // double totalSubtotalDiscounts = 0;
    // double totalFinalFees = 0;
// Console.WriteLine("Debug: Number of Airlines: " + terminal.Airlines.Count); // Debugging


    // foreach (var airlineEntry in terminal.Airlines)
    // {
    //     Airline airline = airlineEntry.Value;
    //     Console.WriteLine($"Debug: Processing Airline: {airline.Name}"); // Debugging
    //     Console.WriteLine($"Debug: Number of Flights: {airline.Flights.Count}"); // Debugging

    //     double totalFees = 0;
    //     double discount = 0;

    //     // all flights for airline
    //     foreach (var flight in airline.Flights.Values)
    //     {
    //         Console.WriteLine($"Debug: Processing Flight: {flight.FlightNumber}"); // Debugging

    //         totalFees += flight.CalculateFees();

    //         if (requestCodeDict.ContainsKey(flight.FlightNumber))
    //         {
    //             string requestCode = requestCodeDict[flight.FlightNumber];
                
    //             if (requestCode == "CFFT")
    //                 totalFees += 100; 
    //             else if (requestCode == "DDJB")
    //             {
    //                 totalFees += 150; 
    //             }
    //             else if (requestCode == "LWTT")
    //             {
    //                 totalFees += 200; 
    //             }
    //             totalFees += 0;

    //             if (flight.ExpectedTime.Hour < 11 || flight.ExpectedTime.Hour >= 21)
    //             {
    //                 discount += 110;
    //             }
    //             if (flight.Origin == "Dubai (DXB)" || flight.Origin == "Bangkok (BKK)" || flight.Origin == "Tokyo (NRT)")
    //             {
    //                 discount += 25;
    //             }
    //             if (string.IsNullOrEmpty(flight.Status))
    //             {
    //                 discount += 50;
    //             }
    //         }

    //         int flightCount = airline.Flights.Count;
    //         discount += (flightCount / 3) * 350;
    //         if (flightCount > 5)
    //         {
    //             discount += totalFees * 0.03; 
    //         }
    //     }

        // double finalFee = totalFees - discount;

    //     totalSubtotalFees += totalFees;
    //     totalSubtotalDiscounts += discount;
    //     totalFinalFees += finalFee;
    //     Console.WriteLine($"Debug: Airline {airline.Name} - Total Fees: {totalFees:C2}, Discount: {discount:C2}, Final Fee: {finalFee:C2}"); // Debugging


    // }
    // Console.WriteLine("Subtotal of All Airline Fees: {0:C2}", totalSubtotalFees);
    // Console.WriteLine("Subtotal of All Airline Discounts: {0:C2}", totalSubtotalDiscounts);
    // Console.WriteLine("Final Total of Airline Fees: {0:C2}", totalFinalFees);

    // if (totalFinalFees > 0)
    // {
    //     double discountPercentage = (totalSubtotalDiscounts / totalFinalFees) * 100;
    //     Console.WriteLine("Percentage of Discounts: {0:F2}%", discountPercentage);
    // }
    // else
    // {
    //     Console.WriteLine("Percentage of Discounts: N/A (No fees to calculate)");
    // }