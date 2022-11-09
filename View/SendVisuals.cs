using Lab_003___WPF_Appilikation.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_003___WPF_Appilikation.View
{
    public class SendVisuals
    {
        DataStore dataStore = new DataStore();

        public string SendErrorMessage(bool validDate, bool validName, bool validTime, bool validTable) //tar in värden och skickar ut en string för att visas
        {
                string errorMessage = "";
                List<string> errors = dataStore.UseErrorMessage();
                if (!validDate)
                    errorMessage += errors[0] + "\n";
                if (!validName)
                    errorMessage += errors[1] + "\n";
                if (!validTime)
                    errorMessage += errors[2] + "\n";
                if (!validTable)
                    errorMessage += errors[3] + "\n";
                return errorMessage;
        }
    }
}
