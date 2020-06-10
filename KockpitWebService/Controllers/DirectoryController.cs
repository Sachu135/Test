using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SFTPBusinessLayer;
using SFTPEntities;

namespace KockpitWebService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DirectoryController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public DirectoryController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            //var oldFile = System.Net.WebUtility.HtmlDecode("C%3A%5CGodrejPOS%3A%5C20.05.2019.txt");
            //var ddd = System.IO.Path.GetFullPath(oldFile);
        }

        async Task<string> ReadStringData(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return await reader.ReadToEndAsync();
            }
        }
       
        [HttpGet("GetDirectoryList", Name = "GetDirectoryList")]
        public IActionResult GetDirectoryList([FromQuery(Name = "dirInfo")] string dirInfo)
        {
            var dirList = Common.GetDirectoryFiles(System.Net.WebUtility.HtmlDecode(dirInfo));
            //return Ok(JsonConvert.SerializeObject(dirList));
            //return new OkObjectResult(dirList);
            //return JsonLoadSettings()
            //JObject obj = (JObject)JToken.FromObject(dirList);
            return Ok(dirList);
        }

        [HttpPost("RenameFile", Name = "RenameFile")]
        public IActionResult RenameFile([FromQuery] string oldFile) 
        {
            try
            {
                var newFile = ReadStringData(this.Request.Body).Result;
                newFile = Uri.UnescapeDataString(newFile);
                
                System.IO.File.Move(oldFile, newFile);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPost("RenameDirectory", Name = "RenameDirectory")]
        public IActionResult RenameDirectory([FromQuery] string oldDir)
        {
            try
            {
                var newDir = ReadStringData(this.Request.Body).Result;
                newDir = Uri.UnescapeDataString(newDir);
                Directory.Move(oldDir, newDir);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("CreateFile", Name = "CreateFile")]
        public IActionResult CreateFile()
        {
            try
            {
                var path = ReadStringData(this.Request.Body).Result;
                path = Uri.UnescapeDataString(path);  
                System.IO.File.Create(path).Dispose();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("CreateDirectory", Name = "CreateDirectory")]
        public IActionResult CreateDirectory()
        {
            try
            {
                var path = ReadStringData(this.Request.Body).Result;
                path = Uri.UnescapeDataString(path);
                System.IO.Directory.CreateDirectory(path);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("RemoveFile", Name = "RemoveFile")]
        public IActionResult RemoveFile()
        {
            try
            {
                var path = ReadStringData(this.Request.Body).Result;
                path = Uri.UnescapeDataString(path);
                System.IO.File.Delete(path);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("RemoveDirectory", Name = "RemoveDirectory")]
        public IActionResult RemoveDirectory()
        {
            try
            {
                var path = ReadStringData(this.Request.Body).Result;
                path = Uri.UnescapeDataString(path);
                System.IO.Directory.Delete(path);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("DownloadFile", Name = "DownloadFile")]
        public IActionResult DownloadFile([FromQuery(Name = "fileName")] string fileName)
        {
            try
            {
                fileName = System.Net.WebUtility.HtmlDecode(fileName);
                return File(System.IO.File.ReadAllBytes(fileName), "application/octet-stream");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
