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


        public CollectionController(ICollectionService collectionService)
        {
            _collectionService = collectionService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, string token)
        {
            var collection = await this._collectionService.GetByIdAsync(id);
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
            string bonitaSessionId = this.HttpContext.Request.Cookies["session-id"];
            string bonitaApiKey = this.HttpContext.Request.Cookies["api-token"];
            var collection = await this._collectionService.Create(createCollectionDTO, bonitaSessionId, bonitaApiKey);
            var bonitaSessionId = this.HttpContext.Request.Cookies["session-id"];
            var bonitaApiKey = this.HttpContext.Request.Cookies["api-token"];
            var collection = await this._collectionService.Create(createCollectionDTO, bonitaApiKey, bonitaSessionId);
            return Ok(collection);
        }
    }
}
