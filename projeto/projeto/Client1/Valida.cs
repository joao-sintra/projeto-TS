using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client1 {
    internal class Valida {

        public static bool IsValidString(string value) {
            bool isValid = true;
            if (string.IsNullOrEmpty(value.Trim())) {
                isValid = false;
            } 
            return isValid;
        }
    }
}
