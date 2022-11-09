using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Lab_003___WPF_Appilikation.Model;


namespace Lab_003___WPF_Appilikation.Control
{
    public class DataStore
    {

        

        protected List<Booking> BookingInitiator = new List<Booking>    //Används bara för att skriva till fil ifall fil inte finns
        {
            new Booking("Jöns Petterson","18:00","2022-11-13",1),
            new Booking("Peter Nilsson","20:00","2022-11-13",1),
            new Booking("Lena Gärdestedt","21:00","2022-11-14",3),
            new Booking("Jonas Hage","22:00","2022-11-16",4),
            new Booking("Mia Valenze","20:00","2022-11-14",2),
            new Booking("Bertil Viik","19:00","2022-11-14",5),
            new Booking("Gustav Vederbäck","18:00","2022-11-15",5),
            new Booking("Martin Lindvall","18:00","2022-11-14",5),
            new Booking("Therese Wallander","21:00","2022-11-15",3),
            new Booking("Moa Leander","19:00","2022-11-16",3),
            new Booking("Viktoria Sederljung","22:00","2022-11-14",5)
        };


        public string jsonFileLocation = "BookingFile.json"; //Filplats JSon Fil

        protected List<TimeOnly> AvailableTimes = new List<TimeOnly>() //Tider till ComboBox
        {
            new TimeOnly(18,00),
            new TimeOnly(19,00),
            new TimeOnly(20,00),
            new TimeOnly(21,00),
            new TimeOnly(22,00)
        };

        protected List<int> TableNumbers = new List<int>() { 1, 2, 3, 4, 5 }; //Bordsnummer till ComboBox

        protected List<string> ErrorMessages = new List<string>() //Felmeddelanden vid fel inmatade värden vid bokning
        {
            "-Vänligen ange datum i \"YYYY-MM-DD\"",
            "-Vänligen ange fullständigt namn",
            "-Vänligen ange en av tiderna angivna",
            "-Vänligen ange ett av borden angivna"
        };


        public DataStore() //Konstruktor för att komma åt funktionerna
        {

        }

        
        public List<Booking> UseBookings() //Skicka ut lista med "Booking" objekt
        {
            return BookingInitiator;
        }


        public List<TimeOnly> UseAvailableTimes() //Skicka ut tider
        {
            return AvailableTimes;
        }

        public List<int> UseTableNumbers() //Skicka ut bord
        {
            return TableNumbers;
        }

        public List<String> UseErrorMessage() //Skicka ut felmeddelanden
        {
            return ErrorMessages;
        }


    }
}
