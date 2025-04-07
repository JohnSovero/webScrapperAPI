
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers{
    [ApiController]
    [Route("supplier")]
    [Authorize]
    public class SupplierController(ISupplierService service) : ControllerBase{
        private readonly ISupplierService _service = service;

        // Endpoint to get all Suppliers
        [HttpGet]
        public async Task<IActionResult> GetAllSuppliers() => Ok(await _service.GetAllSuppliers());

        // Endpoint to get Supplier by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplierById([FromQuery] Guid id){
            var Supplier = await _service.GetSupplierById(id);
            return Supplier is null ? NotFound() : Ok(Supplier);
        }

        // Endpoint to create a new Supplier
        [HttpPost]
        public async Task<IActionResult> CreateSupplier([FromBody] SupplierDto dto){
            var created = await _service.CreateSupplier(dto);
            return CreatedAtAction(nameof(GetSupplierById), new { id = created.Id }, created);
        }

        // Endpoint to update an existing Supplier
        [HttpPut]
        public async Task<IActionResult> UpdateSupplier([FromQuery] Guid id, [FromBody] SupplierDto dto){
            var updated = await _service.UpdateSupplier(id, dto);
            return updated is null ? NotFound() : Ok(updated);
        }

        // Endpoint to delete a Supplier
        [HttpDelete]
        public async Task<IActionResult> DeleteSupplier([FromQuery] Guid id){
            var deleted = await _service.DeleteSupplier(id);
            return deleted ? Ok() : NotFound();
        }
    }
}