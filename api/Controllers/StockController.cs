using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Dtos.Stock;
using api.Interfaces;
using api.Helpers;


namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]

    public class StockController : ControllerBase
    {
        public readonly IStockRepository _stockRepo;

        public StockController(IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));

            var stocks = await _stockRepo.GetAllAsync(query);
            var stocksDto = stocks.Select(stock => stock.ToStockDto());
            return Ok(stocks);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));

            Stock? stock = await _stockRepo.GetByIdAsync(id);
            if (stock == null)
                return NotFound();
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockDto stockDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));

            Stock stock = stockDto.toStockFromCreateStockDto();
            await _stockRepo.CreateAsync(stock);
            return CreatedAtAction(nameof(GetById), new { id = stock.Id }, stock.ToStockDto());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockDto stockDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));

            Stock? stockModel = await _stockRepo.UpdateAsync(id, stockDto);
            if (stockModel == null)
                return NotFound();

            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));

            Stock? stock = await _stockRepo.DeleteAsync(id);
            if (stock == null)
                return NotFound();
            return NoContent();
        }
    }

}
