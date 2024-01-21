using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyClassLibrary
{
    public static class ConvertFormat
    {
        public static int ConvertStringToInt(string value)
        {
            return int.Parse(value);
        }

        public static DateTime ConvertStringToDateTime(string value)
        {

            return DateTime.Parse(value);


        }

    }
}
