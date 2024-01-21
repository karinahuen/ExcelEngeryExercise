using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyClassLibrary
{
    public static class CheckValid
    {
        public static bool CheckFileAlreadyExisting(string filePath)
        {
            var file = filePath;
            return File.Exists(file);
        }

        public static bool CheckValidMeterReadingDateTime(string meterDate)
        {
            if (DateTime.TryParse(meterDate, out DateTime date))
                return true;
            else
                return false;
        }

        public static bool CheckValidMeterReadingValue(string meterValue)
        {

            if (int.TryParse(meterValue, out _))
            {
                int value = ConvertFormat.ConvertStringToInt(meterValue);
                //check the Reading value format "NNNNN" and is positive value
                if (Math.Abs(value).ToString().Length <= 5 && value > 0)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }


        }

    }
}
