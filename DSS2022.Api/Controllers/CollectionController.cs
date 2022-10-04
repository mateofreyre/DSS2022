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
        public async Task<IActionResult> Get(int id)
        {
            var collection = await this._collectionService.GetByIdAsync(id);
            return Ok(collection);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCollectionDTO createCollectionDTO)
        {
            var collection = await this._collectionService.Create(createCollectionDTO);
            return Ok(collection);
        }
    }
}