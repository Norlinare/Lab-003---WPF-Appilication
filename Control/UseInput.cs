using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Xml;
using Lab_003___WPF_Appilikation.Model;
using Microsoft.Win32;

namespace Lab_003___WPF_Appilikation.Control
{
    public class UseInput
    {
       DataStore datastore = new DataStore();

        public bool CheckIfBookingIsFree(string date, string time, int table, List<Booking> bookings) //Jämför med tidigare bokningar om datum, tid och bord är bokat
        {
            bool BookingIsFree = true;
            List<Booking> AlreadyBooked = bookings;
            foreach (Booking booking in AlreadyBooked)
            {
                if (booking.bookedDate == date && booking.bookedTime == time && booking.bookedTable == table)
                {
                    BookingIsFree = false;
                }
            }
            return BookingIsFree;
        }

        public void CreateJSonFile() //Skapar JSon fil och fyller den med en lista av objekt av "Booking" OM fil inte finns
        {
            
            if (!File.Exists(datastore.jsonFileLocation))
            {
                File.Create(datastore.jsonFileLocation);
                List<Booking> bookings = datastore.UseBookings();

                SendToJSonFile(bookings);
            }
            
        }

        public void SendToJSonFile(List<Booking> bookings) // Uppdatera JSon fil med ny data
        {
            var options = new JsonSerializerOptions();
            options.WriteIndented = true;

            string json =JsonSerializer.Serialize<List<Booking>>(bookings,options);

            File.WriteAllText(datastore.jsonFileLocation, json);
        }

        public List<Booking> RetrieveFromJSonFile() //Hämta in JSon fil och skicka ut i listformat för att användas
        {
            List<Booking> updatedBookings = new List<Booking>();

            string jsonString = File.ReadAllText(datastore.jsonFileLocation);
            

            updatedBookings = JsonSerializer.Deserialize<List<Booking>>(jsonString);

            return updatedBookings;
        }

    }
}
