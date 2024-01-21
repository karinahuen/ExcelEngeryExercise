using Microsoft.AspNetCore.Mvc;

namespace EngeryExercise.Models
{
    public class MeterReading
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime MeterReadingDateTime { get; set; }
        public int MeterReadValue { get; set; }

    }
}
