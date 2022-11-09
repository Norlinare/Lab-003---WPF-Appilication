using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_003___WPF_Appilikation.Model
{
    public class Booking //För skapa och använda objekt av typen "Booking"
    {
        public string fullName { get; set; } //Auto Implemented Properties
        public string bookedTime { get; set; } 
        public string bookedDate { get; set; } 
        public int bookedTable { get; set; }
        
        

        public Booking(string Fullname, string BookedTime, string BookedDate, int BookedTable) //Constructor
        {
            fullName = Fullname;
            bookedTime = BookedTime;
            bookedDate = BookedDate;
            bookedTable = BookedTable;
        }
    }
}
