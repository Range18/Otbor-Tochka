using System.Text.RegularExpressions;

namespace TochkaTest;

static class HotelCapacity
{
    static bool CheckCapacity(int maxCapacity, List<Guest> guests)
    {
        var guestsWithDateTimes = guests.Select(guest =>
        {
            var guestNew = new GuestWithDateTime
            {
                Name = guest.Name,
                CheckIn = DateTime.Parse(guest.CheckIn),
                CheckOut = DateTime.Parse(guest.CheckOut)
            };

            return guestNew;
        }).ToList();

        var actions = new List<GuestAction>();
        foreach (var guest in guestsWithDateTimes)
        {
            actions.Add(new GuestAction { Date = guest.CheckIn, GuestChange = 1 });
            actions.Add(new GuestAction { Date = guest.CheckOut, GuestChange = -1 });
        }

        actions.Sort((a, b) =>
        {
            var dateCompare = a.Date.CompareTo(b.Date);
            return dateCompare == 0 ? a.GuestChange.CompareTo(b.GuestChange) : dateCompare;
        });


        var capacity = 0;
        foreach (var action in actions)
        {
            capacity += action.GuestChange;
            if (capacity > maxCapacity)
                return false;
        }

        return true;
    }

    class GuestAction
    {
        public DateTime Date { get; set; }
        public int GuestChange { get; set; }
    }

    class GuestWithDateTime
    {
        public string Name { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
    }

    class Guest
    {
        public string Name { get; set; }
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
    }


    static void Main()
    {
        var maxCapacity = int.Parse(Console.ReadLine());
        var n = int.Parse(Console.ReadLine());


        List<Guest> guests = new List<Guest>();


        for (var i = 0; i < n; i++)
        {
            var line = Console.ReadLine();
            var guest = ParseGuest(line);
            guests.Add(guest);
        }


        var result = CheckCapacity(maxCapacity, guests);


        Console.WriteLine(result ? "True" : "False");
    }


    // Простой парсер JSON-строки для объекта Guest
    static Guest ParseGuest(string json)
    {
        var guest = new Guest();


        // Извлекаем имя
        var nameMatch = Regex.Match(json, "\"name\"\\s*:\\s*\"([^\"]+)\"");
        if (nameMatch.Success)
            guest.Name = nameMatch.Groups[1].Value;


        // Извлекаем дату заезда
        var checkInMatch = Regex.Match(json, "\"check-in\"\\s*:\\s*\"([^\"]+)\"");
        if (checkInMatch.Success)
            guest.CheckIn = checkInMatch.Groups[1].Value;


        // Извлекаем дату выезда
        var checkOutMatch = Regex.Match(json, "\"check-out\"\\s*:\\s*\"([^\"]+)\"");
        if (checkOutMatch.Success)
            guest.CheckOut = checkOutMatch.Groups[1].Value;


        return guest;
    }
}