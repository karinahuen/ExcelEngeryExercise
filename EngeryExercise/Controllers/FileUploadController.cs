using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EngeryExercise.Models;
using Microsoft.AspNetCore.Components.Forms;
using EngeryExercise.Data;
using Microsoft.EntityFrameworkCore;
using EnergyClassLibrary;
using System.Formats.Tar;
using System.IO;
using System.Data;
using System.Text;
using Microsoft.Extensions.Primitives;
using Microsoft.Data.SqlClient;

namespace EngeryExercise.Controllers
{
    [Route("")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        //set web host
        private static IWebHostEnvironment _webHostEnvironment;
        //set the db context
        private readonly EnergyExerciseContext _db;

        public FileUploadController(IWebHostEnvironment webHostEnvironment, EnergyExerciseContext context)
        {
            _webHostEnvironment = webHostEnvironment;
            _db = context;
        }


        [HttpPost]
        [Route("meter-reading-uploads")]
        public async Task<IActionResult> Upload([FromForm] UploadFile fileContext)
        {
            var resultMessage = "";
            if (fileContext.UploadFiles.Length > 0)
            {
                try
                {
                    //target folder path
                    string targetFolderName = CommonValue.TargetFolderNamePath;
                    var filePath = SaveExcelFile(fileContext, targetFolderName);


                    //insert data if the file path copy success
                    if (filePath == "Copy Fail")
                        resultMessage = "The file is alreaady existing! Please check and rename it";
                    else
                        resultMessage = await InsertExcelDataAsync(fileContext, filePath);



                    return Ok(new { Message = resultMessage }); ;

                    }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal Server Error: {ex.Message}");
                }
            }
            else
            {
                return Ok(new { Message = "Upload File Failed as there are no data or file broken" });
            }
        }


        private string SaveExcelFile([FromForm] UploadFile fileContext, String pathName)
        {
            int success = 0;
            DateTime currentDate = DateTime.Now;

            //Create target folder if it does't exist
            if (!Directory.Exists(_webHostEnvironment.WebRootPath + pathName))
            {
                Directory.CreateDirectory(_webHostEnvironment.WebRootPath + pathName);
            }

            var path = pathName + fileContext.UploadFiles.FileName;

            //check the file is existing or not
            if (!CheckValid.CheckFileAlreadyExisting(_webHostEnvironment.WebRootPath + path))
            {
                //Get the file stream from the post file and store the file in target folder for backup
                using (FileStream filestream = System.IO.File.Create(_webHostEnvironment.WebRootPath + pathName + fileContext.UploadFiles.FileName))
                {
                    //save file
                    fileContext.UploadFiles.CopyTo(filestream);
                    filestream.Flush();

                    success = 1;
                }
            }

            return success == 1 ? path : "Copy Fail";
        }


        private async Task<string> InsertExcelDataAsync([FromForm] UploadFile fileContext, String pathName)
        {
            string resultMsg = "";
            StringBuilder sBuilder = new StringBuilder();
            List<string> isSuccessDataList = new List<string>();
            List<string> isFailDataList = new List<string>();
            List<string> isDuplicatedDataList = new List<string>();
            using (StreamReader reader = new StreamReader(_webHostEnvironment.WebRootPath + pathName))
            {
                string row;

                //Read the first line then
                reader.ReadLine();

                while ((row = reader.ReadLine()) != null)
                {

                    var cells = row.Split(',').ToList();
                    if (cells.Count > 2)
                    {
                        //check data
                        if (CheckValid.CheckValidMeterReadingDateTime(cells[1]) && CheckValid.CheckValidMeterReadingValue(cells[2]))
                        {
                            int accountID = ConvertFormat.ConvertStringToInt(cells[0]);
                            DateTime meterReadingDate = ConvertFormat.ConvertStringToDateTime(cells[1]);
                            string formattedDate = string.Format("{0:yyyy-MM-dd HH:mm:ss}", meterReadingDate);

                            //inset into db and return message
                            var param = new SqlParameter[]
                            {
                                new SqlParameter()
                                {
                                    ParameterName = "@accountId",
                                    SqlDbType = System.Data.SqlDbType.Int,
                                    Value=accountID
                                },
                                new SqlParameter()
                                {
                                    ParameterName = "@meterReadingDate",
                                    SqlDbType = System.Data.SqlDbType.VarChar,
                                    Value=formattedDate
                                },
                                new SqlParameter()
                                {
                                    ParameterName = "@meterReadingValue",
                                    SqlDbType = System.Data.SqlDbType.VarChar,
                                    Value=cells[2]
                                },
                                new SqlParameter()
                                {
                                    ParameterName = "ReturnValue",
                                    SqlDbType = System.Data.SqlDbType.Int,
                                    Direction = System.Data.ParameterDirection.Output,
                                }
                            };

                            await _db.Database.ExecuteSqlRawAsync("EXEC @returnValue = InsertMeterReadingByExcel @accountId, @meterReadingDate, @meterReadingValue", param);
                            int outputValue = (int)param[3].Value;

                            if (outputValue == 1)//success
                                isSuccessDataList.Add("Account ID: " + cells[0] + ", Meter Reading Date: " + cells[1] + ", Meter Value: " + cells[2]);
                            else if (outputValue == 3)// existing duplicated record
                                isDuplicatedDataList.Add("Account ID: " + cells[0] + ", Meter Reading Date: " + cells[1] + ", Meter Value: " + cells[2]);
                            else//no account id match or data insert
                                isFailDataList.Add("Account ID: " + cells[0] + ", Meter Reading Date: " + cells[1] + ", Meter Value: " + cells[2]);
                        }
                        else
                        {
                            isFailDataList.Add("Account ID: " + cells[0] + ", Meter Reading Date: " + cells[1] + ", Meter Value: " + cells[2]);
                        }
                    }
                }

            }

            if (isSuccessDataList != null)
            {
                sBuilder.Append("Result Message: <br />");
                sBuilder.Append("------------------------------------------------------------<br />");
                if (isSuccessDataList.Count > 0)
                {
                    //to append the string
                    sBuilder.Append("The Success Record: <br />");
                    sBuilder.Append("The total success: " + isSuccessDataList.Count.ToString() + "<br />");
                    sBuilder.Append("------------------------------------------------------------<br />");
                }
            }

            if (isFailDataList != null)
            {
                if (isFailDataList.Count > 0)
                {
                    //to append the string
                    sBuilder.Append("The Fail Record: <br />");
                    sBuilder.Append("The total fail: " + isFailDataList.Count.ToString() + "<br />");
                    sBuilder.Append("------------------------------------------------------------<br />");
                }
            }

            if (isDuplicatedDataList != null)
            {
                if (isDuplicatedDataList.Count > 0)
                {
                    //to append the string
                    sBuilder.Append("The Duplicated Record: <br />");
                    sBuilder.Append("The total duplicated: " + isDuplicatedDataList.Count.ToString() + "<br />");
                    sBuilder.Append("------------------------------------------------------------<br />");
                }
            }

            resultMsg = sBuilder.ToString();

            //return result
            return resultMsg;
        }

    }
}
