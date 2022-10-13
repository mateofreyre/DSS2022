﻿using DSS2022.Business;
using DSS2022.DataTransferObjects.Collection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DSS2022.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectionController : ControllerBase
    {

        private ICollectionService _collectionService;
        private IFileManagementService _fileManagementService;


        public CollectionController(ICollectionService collectionService,
                                    IFileManagementService fileManagementService)
        {
            _collectionService = collectionService;
            _fileManagementService = fileManagementService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, string token)
        {
            var collection = await this._collectionService.GetByIdAsync(id, token);
            return Ok(collection);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var collection = await this._collectionService.GetAll();
            return Ok(collection);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCollectionDTO createCollectionDTO)
        {
            var bonitaSessionId = this.HttpContext.Request.Cookies["session-id"];
            var bonitaApiKey = this.HttpContext.Request.Cookies["api-token"];
            var collection = await this._collectionService.Create(createCollectionDTO, bonitaApiKey, bonitaSessionId);
            return Ok(collection);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file, string collectionName)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Please upload a file.");
            }
            var fileStream = file.OpenReadStream();
            await this._fileManagementService.SaveFile(file.FileName, fileStream, "Collections\\"+collectionName+ "\\");
            var result = $"The file {file.FileName} has been uploaded";
            return Ok(result);
        }
    }
}
