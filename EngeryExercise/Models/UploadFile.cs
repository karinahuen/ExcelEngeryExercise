using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngeryExercise.Models
{
    public class UploadFile
    {
        
        public IFormFile UploadFiles { get; set; }

    }
}
