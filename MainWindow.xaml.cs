using Lab_003___WPF_Appilikation.Control;
using Lab_003___WPF_Appilikation.Model;
using Lab_003___WPF_Appilikation.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace Lab_003___WPF_Appilikation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public List<Booking> bookings;

        public bool firstLoadingComplete = false;

        public MainWindow()
        {
            DataStore dataStore = new DataStore();
            UseInput useInput = new UseInput();

            List<TimeOnly> AvailableTimes = dataStore.UseAvailableTimes(); //Sparar tider i en lista för att senare skicka till ComboBox
            List<int> TableNumbers = dataStore.UseTableNumbers();//Sparar bord i en lista för att senare skicka till ComboBox

            InitializeComponent();

            foreach (var time in AvailableTimes) //Lägger till tider i ComboBox
                InputTime.Items.Add(time.ToString());
            foreach (var table in TableNumbers) //Lägger till bord i ComboBox
                InputTable.Items.Add(table.ToString());

            useInput.CreateJSonFile(); //Skapar och lägger in lista i JSon fil OM fil inte finns
        }

        private void Book_Click(object sender, RoutedEventArgs e) //Boka Bord kanpp
        {
            SendVisuals visuals = new SendVisuals();

            bool validDate = (InputDate.Text != "" && Regex.IsMatch(InputDate.Text, @"[0-9]{4}-[0-9]{2}-[0-9]{2}")); //Kontrollerar Inputs
            bool validName = (InputName.Text != "");
            bool validTime = (InputTime.SelectedItem != null);
            bool validTable = (InputTable.SelectedItem != null);

            if (validDate == true && validName == true && validTime == true && validTable == true) //Om sann, fortsätter med bokning
            {
                UseInput useInput = new UseInput();

                int parsedTable;
                bool parseSuccess = int.TryParse(InputTable.Text, out parsedTable);

                bool bookingSuccess = AddNewBooking(InputName.Text, InputTime.Text, InputDate.Text, parsedTable); //Kontroll om det går att boka OCH bokar in om det går

                bool bookingIsFull = BookingIsFull(InputDate.Text, InputTime.Text, useInput.RetrieveFromJSonFile()); //Kontroll om bokning den dag och tid är full

                if (bookingSuccess && parseSuccess) //Om bokning lyckas
                {
                    if (InputName.Text.Length < 50) //Om namn inte är för långt
                    {
                        OutputStatus.Text = "Bokning lyckades!\n" +
                                            "Din bokning är nu regristrerad!";
                        ClearTextAfterSubmit();
                    }
                    else //Vid för långt namn
                    {
                        MessageBox.Show("Bokningen misslyckades! Ange ett kortare namn");
                        OutputStatus.Text = "Din sista bokning misslyckades.\nVar vänlig ange ett kortare namn!";
                    }
                }
                else if (bookingIsFull) //Fullt med bord den dagen och tiden
                {
                    MessageBox.Show("Bokningen misslyckades! Max antal bord är bokade den tiden!", "Error!");
                    OutputStatus.Text = "Din sista bokning misslyckades.\nVar vänlig ange annat datum eller tid.";
                }
                else //Just det datum, tid och bord bokad
                {
                    MessageBox.Show("Bokningen misslyckades! Den angavna bokningen är redan upptagen. Var vänlig välj annat datum, tid eller bord.", "Error!");
                    OutputStatus.Text = "Din sista bokning misslyckades\nVar vänlig ange annat datum, tid eller bord.";
                }
            }
            else //Om textfält inte är rätt ifyllda för bokning
            {
                string errorMessage = visuals.SendErrorMessage(validDate, validName, validTime, validTable);

                OutputStatus.Text = errorMessage;


            }
        }

        private void RemoveBooking_Click(object sender, RoutedEventArgs e)  //Borttagning Bokning knapp
        {
            if (BookedCustomers.SelectedItem != null && bookings.Count > 0)  //Kontroll att ett item är valt samt att det finns något att välja
            {
                UseInput useInput = new UseInput();
                List<Booking> bookingsToView = useInput.RetrieveFromJSonFile(); //Lista för att hämta iitems till ListBox

                bookingsToView = bookingsToView.OrderBy(b => b.bookedDate).ThenBy(b => b.bookedTime).ThenBy(b => b.bookedTable).ToList(); //LINQ för att sortera efter datum, tid sen bord

                bookings = bookingsToView; //För att få in en sorterad lista i JSon filen
                useInput.SendToJSonFile(bookings);



                foreach (Booking booking in bookingsToView) //Visa item i ItemBox i en speciell struktur
                {
                    BookedCustomers.Items.Add($"Namn: {booking.fullName}\n" +
                                              $"Datum: {booking.bookedDate}\n" +
                                              $"Tid: {booking.bookedTime}\n" +
                                              $"Bord nr: {booking.bookedTable}");
                }

                int i = BookedCustomers.SelectedIndex; //Selektor för det item man markerar i ListBox

                if (bookings.Count != 0) //För att motverka att ta bort något när listan är tom
                {

                    bookingsToView = useInput.RetrieveFromJSonFile(); //Hämtar in lista för att säkra att rätt väljs

                    bookingsToView.RemoveAt(i); //Tar bort item från lista på selektorns plats

                    bookings = bookingsToView; //Skriver över till lista och skickar in listan till JSon-filen
                    useInput.SendToJSonFile(bookings);

                    OutputStatus.Text = "Avbokning lyckades!\n" + //Text till användaren
                                        "Den valda bokningen är nu avbokad.";

                    BookedCustomers.Items.RemoveAt(i); //Tar bort item i ListBox
                }

                bookingsToView = useInput.RetrieveFromJSonFile(); //Hämtar in lista från JSon fil

                bookingsToView = bookingsToView.OrderBy(b => b.bookedDate).ThenBy(b => b.bookedTime).ThenBy(b => b.bookedTable).ToList(); //LINQ sortering

                BookedCustomers.Items.Clear(); //Tömmer ListBox

                foreach (Booking booking in bookingsToView) //Lägger till alla items i ListBox från Lista (för att ListBox ska stämma överens med Listans och JSon filens objekt)
                {
                    BookedCustomers.Items.Add($"Namn: {booking.fullName}\n" +
                                              $"Datum: {booking.bookedDate}\n" +
                                              $"Tid: {booking.bookedTime}\n" +
                                              $"Bord nr: {booking.bookedTable}");
                }
            }
            else //Om inget item är vald eller Listan är tom
            {
                OutputStatus.Text = "Ingen bokning vald för avbokning.";
            }
        }

        private async void ViewBookings_Click(object sender, RoutedEventArgs e) //Knapp för att visa bokade tider
        {
            BookedCustomers.Items.Clear(); //Tömmer ListBox

            DataStore dataStore = new DataStore();
            UseInput useInput = new UseInput();

            List<Booking> bookingsToView = useInput.RetrieveFromJSonFile(); //Lista för att lägga in i listbox

            bookingsToView = bookingsToView.OrderBy(b => b.bookedDate).ThenBy(b => b.bookedTime).ThenBy(b => b.bookedTable).ToList(); //LINQ Sortering

            

            if (firstLoadingComplete == false) //Körs första gången programmet startar (Loading time)
            {
                Task<string> setItemDescription = SetItemDescription(bookingsToView); //Task för att skriva ut texten för items till ListBox
                Task<string> loading = Loading(); //Task för loading meddelande
                

                string loadingText = await loading; //Laddar in meddelande vid rätt tidpunkt
                string SetItemDescriptionText = await setItemDescription; //Laddar in meddelande vid rätt tidpunkt

                OutputStatus.Text = loadingText; //Skriver ut text
                OutputStatus.Text = SetItemDescriptionText; //Skriver ut text

                firstLoadingComplete = true; //Bool så den ej repeteras
            }
            else //Nästkommande gånger
            {
                foreach (Booking booking in bookingsToView) //Lägger till alla items i ListBox från Lista (för att ListBox ska stämma överens med Listans och JSon filens objekt)
                {
                    BookedCustomers.Items.Add($"Namn: {booking.fullName}\n" +
                                              $"Datum: {booking.bookedDate}\n" +
                                              $"Tid: {booking.bookedTime}\n" +
                                              $"Bord nr: {booking.bookedTable}");

                    OutputStatus.Text = "Hämtning avklarad!\n" +//Skriver ut text
                                        "Listan är nu hämtad.";

                }
            }

            bookings = bookingsToView;//Skriver över till lista och skickar in listan till JSon-filen
            useInput.SendToJSonFile(bookings);
        }

        private void ClearTextAfterSubmit()//Tömmer textfält efter lyckad bokning
        {
            InputDate.Text = "";
            InputName.Text = "";
            InputTime.SelectedItem = null;
            InputTable.SelectedItem = null;
        }

        private void InputTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void InputTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void InputName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void BookedCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e) 
        {

        }

        private bool AddNewBooking(string name, string time, string date, int Table) //Kontrollerar om plats finns och skapar om plats finns
        {

            DataStore dataStore = new DataStore();
            UseInput useInput = new UseInput();


            List<Booking> updateBookings = useInput.RetrieveFromJSonFile(); //Lista från JSon Fil

            bool BookingIsFree = useInput.CheckIfBookingIsFree(date, time, Table, useInput.RetrieveFromJSonFile()); //Jämför angivna värden med värden i JSon fil

            if (BookingIsFree) //Lägger till OM ingen har bokat datum, tid och bord
            {
                updateBookings.Add(new Booking(name, time, date, Table)); //Lägger till i lista och lägger in i JSon fil

                bookings = updateBookings;

                useInput.SendToJSonFile(bookings);

                return true; //Returnerar värde för att meddela användaren
            }
            else
            {
                return false; //Returnerar värde för att meddela användaren
            }
        }




        private bool BookingIsFull(string date, string time, List<Booking> bookings) //Kontroll om alla bord är bokade den dagen och tiden
        {
            UseInput useInput = new UseInput();
            DataStore dataStore = new DataStore();

            bool bookingIsFull = false; //Off switch

            List<Booking> AlreadyBooked = useInput.RetrieveFromJSonFile();
            List<int> tableCheck = dataStore.UseTableNumbers();

            int counter = 0; // för räkna antal bord upptagna samma dag

            foreach (Booking booking in AlreadyBooked)//Gå igenom lista
            {
                if (booking.bookedDate == date && booking.bookedTime == time)//Se om dag och tid stämmer
                {
                    foreach (var tableNumber in tableCheck) //Går igenom varje nummer
                    {

                        if (booking.bookedTable == tableNumber) //OM nummer stämmer
                        {
                            counter++;
                        }

                        if (counter >= 5)//OM alla bord är upptagna
                        {
                            bookingIsFull = true;
                        }

                    }
                }

            }

            return bookingIsFull;
        }


        async Task<string> SetItemDescription(List<Booking> bookingsToView) //Väntetid på att få ut listan (första gången)
        {
            await Task.Delay(5500);
            foreach (Booking booking in bookingsToView)
            {
                BookedCustomers.Items.Add($"Namn: {booking.fullName}\n" +
                                          $"Datum: {booking.bookedDate}\n" +
                                          $"Tid: {booking.bookedTime}\n" +
                                          $"Bord nr: {booking.bookedTable}");

                 
            }
            return "Hämtning avklarad!\n" +
                   "Listan är nu hämtad.";
        }
        async Task<String> Loading() //Laddningsmeddelande som meddelar användaren under tiden.
        {
            string loading = "Vänligen vänta\n" +
                              "Hämtar";
            OutputStatus.Text = loading;
            await Task.Delay(1000);
            loading += ".";
            OutputStatus.Text = loading;
            await Task.Delay(1000);
            loading += ".";
            OutputStatus.Text = loading;
            await Task.Delay(1000);
            loading += ".";
            OutputStatus.Text = loading;
            await Task.Delay(1000);
            loading += ".";
            OutputStatus.Text = loading;
            await Task.Delay(1000);
            loading += ".";
            return loading;
            
        }
    }
}
