    using System.Collections.Immutable;
    using System.Net.NetworkInformation;
    using System.Runtime.CompilerServices;
    using System.Windows.Markup;

    // dictionary of gates assigned already (for feature 8/9)
    Dictionary<string, string> predefinedGates = new Dictionary<string, string>
    {
        { "SQ 693", "A13" },
        { "MH 722", "B2" },
        { "CX 312", "C22" }
    };   

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

    void LoadFlights(string filepath_flight, Dictionary<string, Flight> flightDict, Dictionary<string, Airline> airlineDict)
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

                // added requestCode for advanced feature(b) - joyce
                Flight flight = new Flight(flightNumber, origin, destination, expectedTime, requestCode);

                flightDict.Add(flightNumber, flight);
                requestCodeDict.Add(flightNumber, requestCode);
                
                string airlineCode = flightNumber.Substring(0,2);
                if (airlineDict.TryGetValue(airlineCode, out Airline? airline))
                {
                    airline.AddFlight(flight);
                }
                else
                {
                    Console.WriteLine($"Airline with code {airlineCode} not found for flight {flightNumber}.");
                }
            }
        }
    }
    // --- end of feature 2 ----

    // validations 
    bool IsValidAirlineCode(string airlineCode)
    {
        // airline code should be exactly 2 letters
        if (airlineCode.Length != 2 || !char.IsLetter(airlineCode[0]) || !char.IsLetter(airlineCode[1]))
            return false;

        // check if the airline code exists in the airline dictionary
        return airlineDict.ContainsKey(airlineCode);
    }

    bool IsValidBoardingGate(string gateName)
    {
        // check if the boarding gate exists in the boarding gate dictionary
        return boardinggateDict.ContainsKey(gateName);
    }

    bool IsValidDateTime(string dateTimeStr, out DateTime result)
    {
        return DateTime.TryParse(dateTimeStr, out result);
    }

    bool IsValidRequestCode(string requestCode)
    {
        string[] validCodes = { "CFFT", "DDJB", "LWTT", "None" };
        return validCodes.Contains(requestCode);
    }

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
        while (true)
        {
            Console.WriteLine("Enter Flight Number:");
            string flightNo = Console.ReadLine();
            if (string.IsNullOrEmpty(flightNo))
            {
                Console.WriteLine("Flight number cannot be empty. Please try again.");
                continue;
            }
        
            if (!flightDict.ContainsKey(flightNo))
            {
                Console.WriteLine("Flight number not found.");
                continue;
            }

            Console.WriteLine("Enter Boarding Gate Name:");
            string gateName = Console.ReadLine();
            if (!boardinggateDict.ContainsKey(gateName))
            {
                Console.WriteLine("Boarding gate name not found.");
                continue;
            }

            Flight flight = flightDict[flightNo];
            BoardingGate selectedGate = boardinggateDict[gateName];
            // assign boarding gate to the flight
            flight.BoardingGate = selectedGate;

            if (selectedGate.Flight != null)
            {
                Console.WriteLine($"Boarding Gate {gateName} is already assigned to another flight.");
                return;
            }

            if (requestCodeDict.TryGetValue(flightNo, out string? requestCode) && !string.IsNullOrEmpty(requestCode))
            {
                Console.WriteLine($"Flight Number: {flight.FlightNumber}\nOrigin: {flight.Origin}\nDestination: {flight.Destination}\nExpectedTime: {flight.ExpectedTime}\nSpecial Request Code: {requestCode}");
            }
            else
            {
                Console.WriteLine($"Flight Number: {flight.FlightNumber}\nOrigin: {flight.Origin}\nDestination: {flight.Destination}\nExpectedTime: {flight.ExpectedTime}\nSpecial Request Code: None");
            }

            Console.WriteLine($"Boarding Gate Name: {selectedGate.GateName}\nSupports DDJB: {selectedGate.SupportsDDJB}\nSupports CFFT: {selectedGate.SupportsCFFT}\nSupports LWTT: {selectedGate.SupportsLWTT}");
            Console.WriteLine("Would you like to update the status of the flight? (Y/N)");
            string status = Console.ReadLine().Trim().ToUpper();
            if (status == "Y")
            {
                Console.WriteLine("1. Delayed\n2. Boarding\n3. On Time");
                Console.WriteLine("Please select the new status of the flight:");
                string newStatus = Console.ReadLine();
                flight.Status = newStatus;
                Console.WriteLine($"Flight {flightNo} has been assigned to Boarding Gate {gateName}\n!");
                break;
            }
            else if (status == "N")
            {
                flight.Status = "On Time";
                Console.WriteLine($"Status of the flight will be set to default of {"On Time"}\n");
                break;
            }
        }
    }
    // --- end of feature 5 ----

    // feature 6 - create a new flight
    void CreateFlight(Dictionary<string, Flight> flightDict, Dictionary<string, string> requestCodeDict)
    {
        while (true)
        {
            Console.Write("Enter Flight Number: ");
            string flightNo = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(flightNo))
            {
                Console.WriteLine("Flight number cannot be empty. Please try again.");
                continue;
            }

            if (flightDict.ContainsKey(flightNo))
            {
                Console.WriteLine("Flight number already exists. Please enter a unique flight number.");
                continue;
            }

            Console.Write("Enter Origin: ");
            string origin = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(origin))
            {
                Console.WriteLine("Origin cannot be empty. Please try again.");
                continue;
            }

            Console.Write("Enter Destination: ");
            string destination = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(destination))
            {
                Console.WriteLine("Destination cannot be empty. Please try again.");
                continue;
            }

            Console.Write("Enter Expected Departure/Arrival Time (dd/m/yyyy hh:mm): ");
            string dateTimeStr = Console.ReadLine().Trim();
            DateTime expectedTime;

            if (!IsValidDateTime(dateTimeStr, out expectedTime))
            {
                Console.WriteLine("Invalid date/time format. Please use the format dd/m/yyyy hh:mm.");
                continue;
            }

            Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
            string requestCode = Console.ReadLine().Trim();

            if (!IsValidRequestCode(requestCode))
            {
                Console.WriteLine("Invalid special request code. Please enter one of: CFFT, DDJB, LWTT, or None.");
                continue;
            }

            Flight newFlight = new Flight(flightNo, origin, destination, expectedTime);
            flightDict.Add(flightNo, newFlight);
            requestCodeDict.Add(flightNo, requestCode);

            Console.WriteLine($"Flight {flightNo} has been added!");

            Console.WriteLine("Would you like to add another flight? (Y/N)");
            string permission = Console.ReadLine().Trim().ToUpper();

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
    void DisplayFullflightdetails(Dictionary<string, Airline> airlineDict, Dictionary<string, Flight> flightDict)
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
        string inputCode = Console.ReadLine().ToUpper();

        if (IsValidAirlineCode(inputCode))
        {
            Airline selectedAirline = airlineDict[inputCode];
            List<Flight> filteredFlights = new List<Flight>();

            foreach (var flightEntry in flightDict)
            {
                Flight flight = flightEntry.Value;

                string airlineCode = flight.FlightNumber.Substring(0, 2);

                if (airlineCode == selectedAirline.Code)
                {
                    filteredFlights.Add(flight);
                }
            }

            Console.WriteLine("=============================================");
            Console.WriteLine($"List of Flights for {selectedAirline.Name}");
            Console.WriteLine("=============================================");
            if (filteredFlights.Count > 0)
            {
                Console.WriteLine("{0,-16} {1,-20} {2,-20} {3,-21} {4,-21}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");

                foreach (var flight in filteredFlights)
                {
                    Console.WriteLine("{0,-16} {1,-20} {2,-20} {3,-21} {4,-21}", flight.FlightNumber, selectedAirline.Name, flight.Origin, flight.Destination, flight.ExpectedTime);
                }
            }
            else 
            {
                Console.WriteLine("No flights available for this airline.");
            }
        }
        else
        {
        Console.WriteLine("Invalid Airline Code. Please try again.");
            return;
        }
    }

    // ---- end of feature 7 ----

    // feature 8 - modify flight details
    void Modifyflightdetails(Dictionary<string, Airline> airlineDict, Dictionary<string, Flight> flightDict)
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

        Console.WriteLine("Enter Airline Code: ");
        string inputCode = Console.ReadLine().ToUpper();

        if (!airlineDict.ContainsKey(inputCode))
        {
            Console.WriteLine("Invalid Airline Code. Please try again.");
            return;
        }

        Airline selectedAirline = airlineDict[inputCode];
        List<Flight> filteredFlights = new List<Flight>();

        foreach (var flightEntry in flightDict)
        {
            Flight flight = flightEntry.Value;
            string airlineCode = flight.FlightNumber.Substring(0, 2);

            if (airlineCode == selectedAirline.Code)
            {
                filteredFlights.Add(flight);
            }
        }

        Console.WriteLine($"List of Flights for {selectedAirline.Name}");
        if (filteredFlights.Count > 0)
        {
            Console.WriteLine("{0,-16} {1,-20} {2,-20} {3,-21} {4,-21}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");

            foreach (var flight in filteredFlights)
            {
                Console.WriteLine("{0,-16} {1,-20} {2,-20} {3,-21} {4,-21}", flight.FlightNumber, selectedAirline.Name, flight.Origin, flight.Destination, flight.ExpectedTime);
            }

            Console.WriteLine("Choose an existing Flight to modify or delete:");
            string inputFlight = Console.ReadLine();

            if (flightDict.ContainsKey(inputFlight))
            {
                Flight flight = flightDict[inputFlight];
                            
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
                }
                else
                {
                code = null;
                }

                Console.WriteLine("1. Modify Flight");
                Console.WriteLine("2. Delete Flight");
                Console.WriteLine("Choose an option:");
                int inputChoice = Convert.ToInt32(Console.ReadLine());

                if (inputChoice == 1)
                {
                    Console.WriteLine("1. Modify Basic Information");
                    Console.WriteLine("2. Modify Status");
                    Console.WriteLine("3. Modify Special Request Code");
                    Console.WriteLine("4. Modify Boarding Gate");
                    Console.WriteLine("Choose an option:");
                    int modifyChoice = Convert.ToInt32(Console.ReadLine());
                    if (modifyChoice == 1)
                    {
                        Console.Write("Enter new Origin: ");
                        string newOrigin = Console.ReadLine();
                        Console.Write("Enter new Destination: ");
                        string newDestination = Console.ReadLine();
                        Console.Write("Enter new Expected Departure/Arrival Time (dd/MM/yyyy HH:mm): ");
                        string newTime = Console.ReadLine();

                        DateTime newExpectedTime;
                        if (IsValidDateTime(newTime, out newExpectedTime))
                        {
                            flightDict[inputFlight].Origin = newOrigin;
                            flightDict[inputFlight].Destination = newDestination;
                            flightDict[inputFlight].ExpectedTime = newExpectedTime;

                            Console.WriteLine("Flight updated!");
                            Flight updatedFlight = flightDict[inputFlight];
                            Console.WriteLine($"Flight Number: {updatedFlight.FlightNumber}");
                            Console.WriteLine($"Airline Name: {airlineDict[updatedFlight.FlightNumber.Substring(0, 2)].Name}");
                            Console.WriteLine($"Origin: {updatedFlight.Origin}");
                            Console.WriteLine($"Destination: {updatedFlight.Destination}");
                            Console.WriteLine($"Expected Departure/Arrival Time: {updatedFlight.ExpectedTime}");
                            Console.WriteLine($"Status: {updatedFlight.Status}");
                            Console.WriteLine($"Special Request Code: {code}");
                            Console.WriteLine($"Boarding Gate: {assignedGate}");
                            
                        }
                        else
                        {
                            Console.WriteLine("\nInvalid date format. Flight details not updated.");
                        }
                    }
                    else if (modifyChoice == 2)
                    {
                        Console.Write("Enter new status: ");
                        string newStatus = Console.ReadLine();

                        Console.WriteLine("Flight updated!");
                        Flight updatedFlight = flightDict[inputFlight];
                        Console.WriteLine($"Flight Number: {updatedFlight.FlightNumber}");
                        Console.WriteLine($"Airline Name: {airlineDict[updatedFlight.FlightNumber.Substring(0, 2)].Name}");
                        Console.WriteLine($"Origin: {updatedFlight.Origin}");
                        Console.WriteLine($"Destination: {updatedFlight.Destination}");
                        Console.WriteLine($"Expected Departure/Arrival Time: {updatedFlight.ExpectedTime}");
                        Console.WriteLine($"Status: {newStatus}");
                        Console.WriteLine($"Special Request Code: {code}");
                        Console.WriteLine($"Boarding Gate: {assignedGate}");

                    }
                    else if (modifyChoice == 3)
                    {
                        Console.Write("Enter new Special Request Code: ");
                        string newCode = Console.ReadLine();
                        
                        if (IsValidRequestCode(newCode))
                        {
                            Console.WriteLine("Flight updated!");
                            Flight updatedFlight = flightDict[inputFlight];
                            Console.WriteLine($"Flight Number: {updatedFlight.FlightNumber}");
                            Console.WriteLine($"Airline Name: {airlineDict[updatedFlight.FlightNumber.Substring(0, 2)].Name}");
                            Console.WriteLine($"Origin: {updatedFlight.Origin}");
                            Console.WriteLine($"Destination: {updatedFlight.Destination}");
                            Console.WriteLine($"Expected Departure/Arrival Time: {updatedFlight.ExpectedTime}");
                            Console.WriteLine($"Status: {updatedFlight.Status}");
                            Console.WriteLine($"Special Request Code: {newCode}");
                            Console.WriteLine($"Boarding Gate: {assignedGate}");
                            requestCodeDict[flight.FlightNumber] = newCode;
                        }
                        else 
                        {
                            Console.WriteLine("Invalid Special Request Code! Try again.");
                        }
                    }
                    else if (modifyChoice == 4)
                    {
                        Console.Write("Enter new boarding gate: ");
                        string newAssignedgate = Console.ReadLine();

                        if (IsValidBoardingGate(newAssignedgate))
                        {
                            Console.WriteLine("Flight updated!");
                            Flight updatedFlight = flightDict[inputFlight];
                            Console.WriteLine($"Flight Number: {updatedFlight.FlightNumber}");
                            Console.WriteLine($"Airline Name: {airlineDict[updatedFlight.FlightNumber.Substring(0, 2)].Name}");
                            Console.WriteLine($"Origin: {updatedFlight.Origin}");
                            Console.WriteLine($"Destination: {updatedFlight.Destination}");
                            Console.WriteLine($"Expected Departure/Arrival Time: {updatedFlight.ExpectedTime}");
                            Console.WriteLine($"Status: {updatedFlight.Status}");
                            Console.WriteLine($"Special Request Code: {code}");
                            Console.WriteLine($"Boarding Gate: {newAssignedgate}");
                            predefinedGates[$"{inputFlight}"] = newAssignedgate;
                        }
                        else 
                        {
                            Console.WriteLine("Invalid boarding gate! Try again.");
                        }
                    }
                }
                else if (inputChoice == 2)
                    {
                        Console.WriteLine("Are you sure you want to delete this flight and its' details?");
                        Console.WriteLine("[Y]es delete it.");
                        Console.WriteLine("[N]o bring me back to the front page.");
                        Console.Write("Your choice (Y/N): ");
                        string choice = Console.ReadLine();
                        if (choice == "Y") 
                        {
                            flightDict.Remove(inputFlight);
                            Console.WriteLine($"Flight details for {inputFlight} removed!");
                        }
                        else if (choice == "X")
                        {
                            Console.WriteLine("Heading back to front page...");
                        }
                        else 
                        {
                            Console.WriteLine("Invalid Choice, bringing back to front page!");
                        }
                    }
                else 
                {
                    Console.WriteLine("Invalid Option! Try again.");
                }
            }
            else
            {
                Console.WriteLine("Invalid flight number! Try again.");
            }
        }
    }

    // --- end of feature 8 ---

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


    // advanced feature - (a) process all unassigned flights to boarding gates in bulk
    // -> Qi Xuan
    void ProcessBulkBoardingGateAssignment(Dictionary<string, Flight> flightDict, Dictionary<string, BoardingGate> boardinggateDict, Terminal terminal)
    {
        Queue<Flight> unassignedFlightsQueue = new Queue<Flight>();
        foreach (var flight in flightDict.Values)
        {
            if (flight.BoardingGate == null)
            {
                unassignedFlightsQueue.Enqueue(flight);
            }
        }
        int totalUnassignedFlights = unassignedFlightsQueue.Count;
        Console.WriteLine("=============================================");
        Console.WriteLine("Bulk Boarding Gate Assignment Process");
        Console.WriteLine("=============================================");
        Console.WriteLine($"Total number of Flights without a Boarding Gate assigned: {totalUnassignedFlights}");

        int totalUnassignedGates = 0;
        foreach (var gate in boardinggateDict.Values)
        {
            if (gate.Flight == null)
            {
                totalUnassignedGates += 1;
            }
        }
        Console.WriteLine($"Total number of Boarding Gates without a Flight assigned: {totalUnassignedGates}");
        Console.WriteLine();

        int flightsProcessedAutomatically = 0;
        int boardingGatesAssignedAutomatically = 0;

        while (unassignedFlightsQueue.Count > 0)
        {
            Flight flight = unassignedFlightsQueue.Dequeue();

            string specialRequest = "None";
            if (requestCodeDict.ContainsKey(flight.FlightNumber))
            {
                specialRequest = requestCodeDict[flight.FlightNumber];
            }

            BoardingGate assignedGate = null;

            // If the Flight has a special request (other than "None"),
            // then search for an unassigned Boarding Gate that supports the request.
            if (specialRequest != null && specialRequest != "" && specialRequest.ToUpper() != "NONE")
            {
                foreach (var gate in boardinggateDict.Values)
                {
                    if (gate.Flight == null)
                    {
                        string request = specialRequest.ToUpper(); 
                        if ((request == "CFFT" && gate.SupportsCFFT) ||
                            (request == "DDJB" && gate.SupportsDDJB) ||
                            (request == "LWTT" && gate.SupportsLWTT))
                        {
                            assignedGate = gate;
                            break; 
                        }
                    }
                }
            }
            else 
            {
                foreach (var gate in boardinggateDict.Values)
                {
                    if (gate.Flight == null)
                    {
                        assignedGate = gate;
                        break;
                    }
                }
            }

            if (assignedGate != null)
            {
                flight.BoardingGate = assignedGate;
                assignedGate.Flight = flight;
                flightsProcessedAutomatically += 1;
                boardingGatesAssignedAutomatically += 1;
            }
            else
            {
                Console.WriteLine($"No available boarding gate found for Flight {flight.FlightNumber}");
            }
        }

        Console.WriteLine();
        // Display all Flight details (Basic Information) with their Boarding Gate assignment.
        Console.WriteLine("=============================================");
        Console.WriteLine("Bulk Assignment Result - Flight Details");
        Console.WriteLine("=============================================");
        Console.WriteLine("{0,-16} {1,-20} {2,-20} {3,-21} {4,-21} {5,-20} {6,-16}",
                            "Flight Number", "Airline Name", "Origin", "Destination", "Expected Time", "Request Code", "Boarding Gate");
        foreach (var flight in flightDict.Values)
        {
            Airline airline = terminal.GetAirlineFromFlight(flight);
            string airlineName = airline != null ? airline.Name : "Unknown Airline";
            string requestCode = requestCodeDict.ContainsKey(flight.FlightNumber) ? requestCodeDict[flight.FlightNumber] : "None";
            string gateName = flight.BoardingGate != null ? flight.BoardingGate.GateName : "Unassigned";

            Console.WriteLine("{0,-16} {1,-20} {2,-20} {3,-21} {4,-21} {5,-20} {6,-16}",
                                flight.FlightNumber, airlineName, flight.Origin, flight.Destination, flight.ExpectedTime, requestCode, gateName);
        }

        Console.WriteLine();
        Console.WriteLine($"Total Flights processed automatically: {flightsProcessedAutomatically}");
        Console.WriteLine($"Total Boarding Gates assigned automatically: {boardingGatesAssignedAutomatically}");

        int totalFlights = flightDict.Count;
        int alreadyAssignedFlights = totalFlights - totalUnassignedFlights;

        double flightAutomaticPercentage = 0.0;
        if (totalFlights > 0)
        {
            flightAutomaticPercentage = (double)flightsProcessedAutomatically / totalFlights * 100;
        }
        int totalBoardingGates = boardinggateDict.Count;
        int alreadyAssignedGates = totalBoardingGates - totalUnassignedGates;

        double gateAutomaticPercentage = 0.0;
        if (totalBoardingGates > 0)
        {
            gateAutomaticPercentage = (double)boardingGatesAssignedAutomatically / totalBoardingGates * 100;
        }
        Console.WriteLine($"Percentage of Flights processed automatically: {flightAutomaticPercentage:F2}%");
        Console.WriteLine($"Percentage of Boarding Gates processed automatically: {gateAutomaticPercentage:F2}%");
    }
    
    // --- end of advanced feature (a) ---
    
    // advanced feature - (b) display the total fee per airline for the day
    // -> Joyce
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
    DisplayMenu(flightDict, airlineDict, boardinggateDict);

    void DisplayMenu(Dictionary<string, Flight> flightDict, Dictionary<string, Airline> airlineDict, Dictionary<string, BoardingGate> boardinggateDict)
    {
        Terminal terminal = new Terminal();
        Airline airline = new Airline();

        LoadAirlines(filepath_airline, airlineDict);
        LoadBoardinggate(filepath_gate, boardinggateDict);
        LoadFlights(filepath_flight, flightDict, airlineDict);

        terminal.Airlines = airlineDict;
        terminal.BoardingGates = boardinggateDict;
        terminal.Flights = flightDict;
        airline.Flights = flightDict;

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
            Console.WriteLine("8. Process all Unassigned Flights to Boarding Gates");
            Console.WriteLine("9. Display Total Fee per Airline for the day");
            Console.WriteLine("0. Exit\n");

            Console.Write("Please select your option: ");
            string option = Console.ReadLine();
            if (option == null)
            {
                option = "";
            }
            else
            {
                option = option.Trim();
            }

            if (string.IsNullOrWhiteSpace(option))
            {
                Console.WriteLine("Input cannot be empty. Please select a valid option.");
                continue;
            }

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
                DisplayFullflightdetails(airlineDict, flightDict);
                Console.WriteLine();
            }
            else if (option == "6")
            {
                Modifyflightdetails(airlineDict, flightDict);
            }
            else if (option == "7")
            {
                DisplayScheduledFlights(terminal, flightDict, requestCodeDict);  
                Console.WriteLine();

            }
            // advanced feature (a) - Qi Xuan
            else if (option == "8")
            {
                ProcessBulkBoardingGateAssignment(flightDict, boardinggateDict, terminal);
                Console.WriteLine();
            }
            // advanced feature (b) - Joyce
            else if (option == "9")
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
                Console.WriteLine("Invalid option. Please select a valid menu option (0-9)");
            }
        }
    }