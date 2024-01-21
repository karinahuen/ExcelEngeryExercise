using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EngeryExercise.Models;

namespace EngeryExercise.Data
{
    public class EnergyExerciseContext : DbContext
    {
        //set internal microsoft SQL server Database
        public EnergyExerciseContext(DbContextOptions<EnergyExerciseContext> options)
            : base(options)
        {
        }


        //Get value from models data
        public DbSet<MeterReading> MeterReadings { get; set; } = default!;
        public DbSet<CustomerUsers> CustomerAccount { get; set; } = default!;

    }
}
