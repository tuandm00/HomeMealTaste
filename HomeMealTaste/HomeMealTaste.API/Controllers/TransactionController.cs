using HomeMealTaste.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeMealTaste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("get-all-transaction-by-user-id")]
        public async Task<IActionResult> GetAllTransactionByUserId(int userid)
        {
            var result = await _transactionService.GetAllTransactionByUserId(userid);
            return Ok(result);
        }

        [HttpGet("get-transaction-by-transaction-type-OREDER")]
        public async Task<IActionResult> GetAllTransactionByTransactionTypeORDERED()
        {
            var result = await _transactionService.GetAllTransactionByTransactionTypeORDERED();
            return Ok(result);
        }
        [HttpGet("get-transaction-by-transaction-type-RECHARGED")]
        public async Task<IActionResult> GetAllTransactionByTransactionTypeRECHARGED()
        {
            var result = await _transactionService.GetAllTransactionByTransactionTypeRECHARGED();
            return Ok(result);
        } 
        [HttpGet("save-total-price-after-finish-session")]
        public async Task<IActionResult> SaveTotalPriceAfterFinishSession(int sessionid)
        {
            var result = await _transactionService.SaveTotalPriceAfterFinishSession(sessionid);
            return Ok(result);
        }
        [HttpGet("get-all-transaction")]
        public async Task<IActionResult> GetAllTransaction()
        {
            var result = await _transactionService.GetAllTransaction();
            return Ok(result);
        }

    }
}
