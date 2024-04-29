using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

class TravelPlanner
{
    static string usersFilePath = "users.json";
    static string itinerariesFilePath = "itineraries.json";
    static User currentUser = null;

    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Travel Planner System!");

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\nMain Menu:");
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Exit");

            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    RegisterUser();
                    break;
                case "2":
                    LoginUser();
                    break;
                case "3":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private static void RegisterUser()
    {
        Console.WriteLine("Registering a new user:");
        Console.Write("Enter username: ");
        string username = Console.ReadLine();

        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        User newUser = new User
        {
            Username = username,
            Password = password
        };

        // Save the new user to the users.json file
        List<User> users = LoadUsers();
        users.Add(newUser);
        SaveUsers(users);

        Console.WriteLine("User registered successfully!");
    }

    private static void LoginUser()
    {
        Console.WriteLine("Logging in:");
        Console.Write("Enter username: ");
        string username = Console.ReadLine();

        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        List<User> users = LoadUsers();
        currentUser = users.Find(user => user.Username == username && user.Password == password);

        if (currentUser != null)
        {
            Console.WriteLine("Login successful!");
            DisplayUserMenu();
        }
        else
        {
            Console.WriteLine("Invalid username or password. Please try again.");
        }
    }

    private static List<User> LoadUsers()
    {
        // Load users from a JSON file
        if (File.Exists(usersFilePath))
        {
            string json = File.ReadAllText(usersFilePath);
            return JsonConvert.DeserializeObject<List<User>>(json);
        }
        else
        {
            return new List<User>();
        }
    }

    private static void SaveUsers(List<User> users)
    {
        // Save users to a JSON file
        string json = JsonConvert.SerializeObject(users, Formatting.Indented);
        File.WriteAllText(usersFilePath, json);
    }

    private static void DisplayUserMenu()
    {
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\nUser Menu:");
            Console.WriteLine("1. Create Itinerary");
            Console.WriteLine("2. View/Edit Itinerary");
            Console.WriteLine("3. Save Itinerary");
            Console.WriteLine("4. Load Itinerary");
            Console.WriteLine("5. Logout");

            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateItinerary();
                    break;
                case "2":
                    ViewEditItinerary();
                    break;
                case "3":
                    SaveItinerary();
                    break;
                case "4":
                    LoadItinerary();
                    break;
                case "5":
                    exit = true;
                    currentUser = null;
                    Console.WriteLine("Logged out successfully.");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private static void CreateItinerary()
    {
        Console.Write("Enter itinerary name: ");
        string name = Console.ReadLine();

        Itinerary newItinerary = new Itinerary
        {
            Name = name
        };

        currentUser.Itineraries.Add(newItinerary);
        Console.WriteLine("Itinerary created successfully!");
    }

    private static void ViewEditItinerary()
    {
        if (currentUser.Itineraries.Count == 0)
        {
            Console.WriteLine("No itineraries available.");
            return;
        }

        Console.WriteLine("\nYour Itineraries:");
        for (int i = 0; i < currentUser.Itineraries.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {currentUser.Itineraries[i].Name}");
        }

        Console.Write("Enter the number of the itinerary to view/edit (or 0 to cancel): ");
        int index = int.Parse(Console.ReadLine()) - 1;

        if (index >= 0 && index < currentUser.Itineraries.Count)
        {
            Itinerary selectedItinerary = currentUser.Itineraries[index];
            ManageItinerary(selectedItinerary);
        }
        else if (index != -1)
        {
            Console.WriteLine("Invalid itinerary number. Please try again.");
        }
    }

    private static void ManageItinerary(Itinerary itinerary)
    {
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine($"\nManaging Itinerary: {itinerary.Name}");
            Console.WriteLine("1. Manage Airline Booking");
            Console.WriteLine("2. Manage Hotel Booking");
            Console.WriteLine("3. Manage Points of Interest");
            Console.WriteLine("4. Manage Budget");
            Console.WriteLine("5. Manage Daily Schedule");
            Console.WriteLine("6. Go back");

            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ManageAirlineBooking(itinerary);
                    break;
                case "2":
                    ManageHotelBooking(itinerary);
                    break;
                case "3":
                    ManagePointsOfInterest(itinerary);
                    break;
                case "4":
                    ManageBudget(itinerary);
                    break;
                case "5":
                    ManageDailySchedule(itinerary);
                    break;
                case "6":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private static void ManageAirlineBooking(Itinerary itinerary)
    {
        Console.WriteLine("\nManaging Airline Booking:");
        Console.WriteLine("1. Add a new airline booking");
        Console.WriteLine("2. Edit an existing airline booking");
        Console.WriteLine("3. Go back");

        Console.Write("Choose an option: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                AddAirlineBooking(itinerary);
                break;
            case "2":
                EditAirlineBooking(itinerary);
                break;
            case "3":
                return;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
    }

    private static void AddAirlineBooking(Itinerary itinerary)
    {
        Console.Write("Enter flight number: ");
        string flightNumber = Console.ReadLine();

        Console.Write("Enter departure time (yyyy-mm-dd HH:mm): ");
        DateTime departureTime = DateTime.Parse(Console.ReadLine());

        Console.Write("Enter arrival time (yyyy-mm-dd HH:mm): ");
        DateTime arrivalTime = DateTime.Parse(Console.ReadLine());

        AirlineBooking booking = new AirlineBooking
        {
            FlightNumber = flightNumber,
            DepartureTime = departureTime,
            ArrivalTime = arrivalTime
        };

        itinerary.AirlineBookings.Add(booking);
        Console.WriteLine("Airline booking added successfully!");
    }

    private static void EditAirlineBooking(Itinerary itinerary)
    {
        Console.WriteLine("\nExisting Airline Bookings:");
        for (int i = 0; i < itinerary.AirlineBookings.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {itinerary.AirlineBookings[i].FlightNumber}");
        }

        Console.Write("\nEnter the number of the booking to edit (or 0 to cancel): ");
        int index = int.Parse(Console.ReadLine()) - 1;

        if (index >= 0 && index < itinerary.AirlineBookings.Count)
        {
            AirlineBooking booking = itinerary.AirlineBookings[index];

            Console.Write("Enter new flight number (or press enter to keep current): ");
            string flightNumber = Console.ReadLine();
            if (!string.IsNullOrEmpty(flightNumber))
            {
                booking.FlightNumber = flightNumber;
            }

            Console.Write("Enter new departure time (or press enter to keep current): ");
            string departureTimeInput = Console.ReadLine();
            if (!string.IsNullOrEmpty(departureTimeInput))
            {
                booking.DepartureTime = DateTime.Parse(departureTimeInput);
            }

            Console.Write("Enter new arrival time (or press enter to keep current): ");
            string arrivalTimeInput = Console.ReadLine();
            if (!string.IsNullOrEmpty(arrivalTimeInput))
            {
                booking.ArrivalTime = DateTime.Parse(arrivalTimeInput);
            }

            Console.WriteLine("Airline booking updated successfully!");
        }
        else
        {
            Console.WriteLine("Invalid booking number. Please try again.");
        }
    }

    private static void ManageHotelBooking(Itinerary itinerary)
    {
        Console.WriteLine("\nManaging Hotel Booking:");
        Console.WriteLine("1. Add a new hotel booking");
        Console.WriteLine("2. Edit an existing hotel booking");
        Console.WriteLine("3. Go back");

        Console.Write("Choose an option: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                AddHotelBooking(itinerary);
                break;
            case "2":
                EditHotelBooking(itinerary);
                break;
            case "3":
                return;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
    }

    private static void AddHotelBooking(Itinerary itinerary)
    {
        Console.Write("Enter hotel name: ");
        string hotelName = Console.ReadLine();

        Console.Write("Enter check-in date (yyyy-mm-dd): ");
        DateTime checkInDate = DateTime.Parse(Console.ReadLine());

        Console.Write("Enter check-out date (yyyy-mm-dd): ");
        DateTime checkOutDate = DateTime.Parse(Console.ReadLine());

        HotelBooking booking = new HotelBooking
        {
            HotelName = hotelName,
            CheckInDate = checkInDate,
            CheckOutDate = checkOutDate
        };

        itinerary.HotelBookings.Add(booking);
        Console.WriteLine("Hotel booking added successfully!");
    }

    private static void EditHotelBooking(Itinerary itinerary)
    {
        Console.WriteLine("\nExisting Hotel Bookings:");
        for (int i = 0; i < itinerary.HotelBookings.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {itinerary.HotelBookings[i].HotelName}");
        }

        Console.Write("\nEnter the number of the booking to edit (or 0 to cancel): ");
        int index = int.Parse(Console.ReadLine()) - 1;

        if (index >= 0 && index < itinerary.HotelBookings.Count)
        {
            HotelBooking booking = itinerary.HotelBookings[index];

            Console.Write("Enter new hotel name (or press enter to keep current): ");
            string hotelName = Console.ReadLine();
            if (!string.IsNullOrEmpty(hotelName))
            {
                booking.HotelName = hotelName;
            }

            Console.Write("Enter new check-in date (or press enter to keep current): ");
            string checkInDateInput = Console.ReadLine();
            if (!string.IsNullOrEmpty(checkInDateInput))
            {
                booking.CheckInDate = DateTime.Parse(checkInDateInput);
            }

            Console.Write("Enter new check-out date (or press enter to keep current): ");
            string checkOutDateInput = Console.ReadLine();
            if (!string.IsNullOrEmpty(checkOutDateInput))
            {
                booking.CheckOutDate = DateTime.Parse(checkOutDateInput);
            }

            Console.WriteLine("Hotel booking updated successfully!");
        }
        else
        {
            Console.WriteLine("Invalid booking number. Please try again.");
        }
    }

    private static void ManagePointsOfInterest(Itinerary itinerary)
    {
        Console.WriteLine("\nManaging Points of Interest:");
        Console.WriteLine("1. Add a new point of interest");
        Console.WriteLine("2. Edit an existing point of interest");
        Console.WriteLine("3. Go back");

        Console.Write("Choose an option: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                AddPointOfInterest(itinerary);
                break;
            case "2":
                EditPointOfInterest(itinerary);
                break;
            case "3":
                return;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
    }

    private static void AddPointOfInterest(Itinerary itinerary)
    {
        Console.Write("Enter point of interest name: ");
        string name = Console.ReadLine();

        Console.Write("Enter point of interest location: ");
        string location = Console.ReadLine();

        PointOfInterest poi = new PointOfInterest
        {
            Name = name,
            Location = location
        };

        itinerary.PointsOfInterest.Add(poi);
        Console.WriteLine("Point of interest added successfully!");
    }

    private static void EditPointOfInterest(Itinerary itinerary)
    {
        Console.WriteLine("\nExisting Points of Interest:");
        for (int i = 0; i < itinerary.PointsOfInterest.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {itinerary.PointsOfInterest[i].Name}");
        }

        Console.Write("\nEnter the number of the point of interest to edit (or 0 to cancel): ");
        int index = int.Parse(Console.ReadLine()) - 1;

        if (index >= 0 && index < itinerary.PointsOfInterest.Count)
        {
            PointOfInterest poi = itinerary.PointsOfInterest[index];

            Console.Write("Enter new name (or press enter to keep current): ");
            string name = Console.ReadLine();
            if (!string.IsNullOrEmpty(name))
            {
                poi.Name = name;
            }

            Console.Write("Enter new location (or press enter to keep current): ");
            string location = Console.ReadLine();
            if (!string.IsNullOrEmpty(location))
            {
                poi.Location = location;
            }

            Console.WriteLine("Point of interest updated successfully!");
        }
        else
        {
            Console.WriteLine("Invalid point of interest number. Please try again.");
        }
    }

    private static void ManageBudget(Itinerary itinerary)
    {
        Console.WriteLine("\nManaging Budget:");
        Console.WriteLine("1. Set new budget");
        Console.WriteLine("2. View current budget");
        Console.WriteLine("3. Go back");

        Console.Write("Choose an option: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                SetNewBudget(itinerary);
                break;
            case "2":
                Console.WriteLine($"Current budget: {itinerary.Budget:C}");
                break;
            case "3":
                return;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
    }

    private static void SetNewBudget(Itinerary itinerary)
    {
        Console.Write("Enter new budget amount: ");
        decimal newBudget = decimal.Parse(Console.ReadLine());

        itinerary.Budget = newBudget;
        Console.WriteLine("Budget updated successfully!");
    }

    private static void ManageDailySchedule(Itinerary itinerary)
    {
        Console.WriteLine("\nManaging Daily Schedule:");
        Console.WriteLine("1. Add new schedule");
        Console.WriteLine("2. Edit existing schedule");
        Console.WriteLine("3. Go back");

        Console.Write("Choose an option: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                AddNewSchedule(itinerary);
                break;
            case "2":
                EditSchedule(itinerary);
                break;
            case "3":
                return;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
    }

    private static void AddNewSchedule(Itinerary itinerary)
    {
        Console.Write("Enter date for the schedule (yyyy-mm-dd): ");
        DateTime date = DateTime.Parse(Console.ReadLine());

        Console.WriteLine("Enter activities for the day (enter 'done' to finish):");
        List<string> activities = new List<string>();
        string activity;

        while (true)
        {
            Console.Write("> ");
            activity = Console.ReadLine();
            if (activity.ToLower() == "done")
            {
                break;
            }
            activities.Add(activity);
        }

        Schedule schedule = new Schedule
        {
            Date = date,
            Activities = activities
        };

        itinerary.DailySchedules.Add(schedule);
        Console.WriteLine("Schedule added successfully!");
    }

    private static void EditSchedule(Itinerary itinerary)
    {
        Console.WriteLine("\nExisting Daily Schedules:");
        for (int i = 0; i < itinerary.DailySchedules.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {itinerary.DailySchedules[i].Date.ToString("yyyy-MM-dd")}");
        }

        Console.Write("\nEnter the number of the schedule to edit (or 0 to cancel): ");
        int index = int.Parse(Console.ReadLine()) - 1;

        if (index >= 0 && index < itinerary.DailySchedules.Count)
        {
            Schedule schedule = itinerary.DailySchedules[index];

            Console.Write("Enter new date for the schedule (or press enter to keep current): ");
            string dateInput = Console.ReadLine();
            if (!string.IsNullOrEmpty(dateInput))
            {
                schedule.Date = DateTime.Parse(dateInput);
            }

            Console.WriteLine("Enter new activities for the day (enter 'done' to finish):");
            List<string> activities = new List<string>();
            string activity;

            while (true)
            {
                Console.Write("> ");
                activity = Console.ReadLine();
                if (activity.ToLower() == "done")
                {
                    break;
                }
                activities.Add(activity);
            }

            schedule.Activities = activities;
            Console.WriteLine("Schedule updated successfully!");
        }
        else
        {
            Console.WriteLine("Invalid schedule number. Please try again.");
        }
    }

    private static void SaveItinerary()
    {
        // Save current user's itineraries to the itineraries.json file
        List<Itinerary> itineraries = LoadItineraries();
        itineraries.AddRange(currentUser.Itineraries);
        SaveItineraries(itineraries);
        Console.WriteLine("Itinerary saved successfully!");
    }

    private static void LoadItinerary()
    {
        // Load itineraries from the itineraries.json file
        List<Itinerary> itineraries = LoadItineraries();
        currentUser.Itineraries = itineraries;
        Console.WriteLine("Itinerary loaded successfully!");
    }

    private static List<Itinerary> LoadItineraries()
    {
        // Load itineraries from a JSON file
        if (File.Exists(itinerariesFilePath))
        {
            string json = File.ReadAllText(itinerariesFilePath);
            return JsonConvert.DeserializeObject<List<Itinerary>>(json);
        }
        else
        {
            return new List<Itinerary>();
        }
    }

    private static void SaveItineraries(List<Itinerary> itineraries)
    {
        // Save itineraries to a JSON file
        string json = JsonConvert.SerializeObject(itineraries, Formatting.Indented);
        File.WriteAllText(itinerariesFilePath, json);
    }
}

class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    public List<Itinerary> Itineraries { get; set; }

    public User()
    {
        Itineraries = new List<Itinerary>();
    }
}

class Itinerary
{
    public string Name { get; set; }
    public List<AirlineBooking> AirlineBookings { get; set; }
    public List<HotelBooking> HotelBookings { get; set; }
    public List<PointOfInterest> PointsOfInterest { get; set; }
    public decimal Budget { get; set; }
    public List<Schedule> DailySchedules { get; set; }

    public Itinerary()
    {
        AirlineBookings = new List<AirlineBooking>();
        HotelBookings = new List<HotelBooking>();
        PointsOfInterest = new List<PointOfInterest>();
        DailySchedules = new List<Schedule>();
    }
}

class AirlineBooking
{
    public string FlightNumber { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
}

class HotelBooking
{
    public string HotelName { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
}

class PointOfInterest
{
    public string Name { get; set; }
    public string Location { get; set; }
}

class Schedule
{
    public DateTime Date { get; set; }
    public List<string> Activities { get; set; }

    public Schedule()
    {
        Activities = new List<string>();
    }
}
