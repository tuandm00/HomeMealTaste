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

        [HttpGet("get-transaction-by-transaction-type-with-orderid")]
        public async Task<IActionResult> GetAllTransactionByTransactionTypeWithOrderId()
        {
            var result = await _transactionService.GetAllTransactionByTransactionTypeWithOrderId();
            return Ok(result);
        }
        [HttpGet("get-transaction-by-transaction-type-without-orderid")]
        public async Task<IActionResult> GetAllTransactionByTransactionTypeWithOutOrderId()
        {
            var result = await _transactionService.GetAllTransactionByTransactionTypeWithOutOrderId();
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
