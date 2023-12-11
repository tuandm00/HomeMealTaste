using HomeMealTaste.Data.Helper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Response;
using HomeMealTaste.Services.Helper;
using HomeMealTaste.Services.Interface;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;
using System.Globalization;

namespace HomeMealTaste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KitchenController : ControllerBase
    {
        private readonly IKitchenService _kitchenService;
        private readonly HomeMealTasteContext _context;
        public KitchenController(IKitchenService kitchenService, HomeMealTasteContext context)
        {
            _kitchenService = kitchenService;
            _context = context;
        }

        [HttpGet("get-all-kitchen")]
        public async Task<IActionResult> GetAllKitchen()
        {
            var result = await _kitchenService.GetAllKitchen();
            return Ok(result);
        }
        [HttpGet("get-single-kitchen-by-kitchen-id")]
        public async Task<IActionResult> GetSingleKitchenByKitchenId(int id)
        {
            var result = await _kitchenService.GetSingleKitchenByKitchenId(id);
            return Ok(result);
        }

        [HttpGet("get-all-kitchen-by-session-id")]
        public async Task<IActionResult> GetAllKitchenBySessionId(int sessionid)
        {
            var result = await _kitchenService.GetAllKitchenBySessionId(sessionid);
            return Ok(result);
        }
        [HttpGet("get-single-kitchen-by-user-id")]
        public async Task<IActionResult> GetSingleKitchenByUserId(int userid)
        {
            var result = await _kitchenService.GetSingleKitchenByUserId(userid);
            return Ok(result);
        }
        public static DateTime TranferDateTimeByTimeZone(DateTime dateTime, string timezoneArea)
        {

            ReadOnlyCollection<TimeZoneInfo> collection = TimeZoneInfo.GetSystemTimeZones();
            var timeZone = collection.ToList().Where(x => x.DisplayName.ToLower().Contains(timezoneArea)).First();

            var timeZoneLocal = TimeZoneInfo.Local;

            var utcDateTime = TimeZoneInfo.ConvertTime(dateTime, timeZoneLocal, timeZone);

            return utcDateTime;
        }
        public static DateTime GetDateTimeTimeZoneVietNam()
        {

            return TranferDateTimeByTimeZone(DateTime.Now, "hanoi");
        }
        public static DateTime? StringToDateTimeVN(string dateStr)
        {

            var isValid = System.DateTime.TryParseExact(
                                dateStr,
                                "d'/'M'/'yyyy",
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None,
                                out var date
                            );
            return isValid ? date : null;
        }


        //[HttpGet("withdraw-money-from-chef-wallet-export-file-PDF")]
        //public async Task<IActionResult> WithdrawMoneyFromChefWalletExportFilePDF(int kitchenid, int moneyWithdraw)
        //{
        //    try
        //    {
        //        var datenow = GetDateTimeTimeZoneVietNam().ToString("dd-MM-yyyy HH:mm");
        //        var userId = _context.Kitchens.Where(x => x.KitchenId == kitchenid).Select(x => x.UserId).FirstOrDefault();
        //        var checkUserKitchenAndMoney = _context.Wallets.Where(x => x.UserId == userId).FirstOrDefault();
        //        if (checkUserKitchenAndMoney.Balance > 0)
        //        {
        //            if (moneyWithdraw >= 50000)
        //            {
        //                checkUserKitchenAndMoney.Balance -= moneyWithdraw;
        //                _context.Wallets.Update(checkUserKitchenAndMoney);
        //                _context.SaveChanges();
        //            }
        //            else throw new Exception("Money Withdraw Must Be Equal or Higher than 50000");
        //        }
        //        else throw new Exception("Money In Wallet Not Enough To Withdraw");

        //        //export to PDF Url
        //        var fileName = $"{Guid.NewGuid()}_WithdrawalReceipt.pdf";
        //        var filePath = Path.Combine(@"E:\Capstone2", fileName); // Update this with the desired directory

        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            var writer = new PdfWriter(stream);
        //            var pdf = new PdfDocument(writer);
        //            var document = new Document(pdf);

        //            // Add content to the PDF
        //            document.Add(new Paragraph($"Kitchen ID: {kitchenid}"));
        //            document.Add(new Paragraph($"Money Withdrawn: {moneyWithdraw}"));
        //            document.Add(new Paragraph($"Money Withdrawn DateTime: {datenow}"));

        //            // Add more information if needed...

        //            // Save the document
        //            document.Close();

        //            // Save the stream to a file or return it as a response
        //            var downloadUrllocal = $"https://localhost:7294/api/Kitchen/DownloadPdf?fileName={fileName}";

        //            // For API, you might return the PDF as a FileResult
        //            return Ok(new { DownloadUrl = downloadUrllocal });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }

        //}

        //[HttpGet("DownloadPdf")]
        //public IActionResult DownloadPdf(string fileName)
        //{
        //    // Construct the full path to the PDF file
        //    var filePath = Path.Combine(@"E:\Capstone2", fileName); // Update this with the actual directory

        //    // Check if the file exists
        //    if (System.IO.File.Exists(filePath))
        //    {
        //        // Read the file content
        //        var fileBytes = System.IO.File.ReadAllBytes(filePath);

        //        // Return the file as a response
        //        return File(fileBytes, "application/pdf", fileName);
        //    }
        //    else
        //    {
        //        return NotFound(); // Or handle the case where the file is not found
        //    }
        //}

        [HttpGet("withdraw-money-from-chef-wallet-export-file-PDF")]
        public async Task<IActionResult> WithdrawMoneyFromChefWalletExportFilePDF(int kitchenid, int moneyWithdraw)
        {
            try
            {
                var datenow = GetDateTimeTimeZoneVietNam();
                var userId = _context.Kitchens.Where(x => x.KitchenId == kitchenid).Select(x => x.UserId).FirstOrDefault();
                var UserNameChef = _context.Kitchens.Where(x => x.KitchenId == kitchenid).Select(x => x.Name).FirstOrDefault();
                var checkUserKitchenAndMoney = _context.Wallets.Where(x => x.UserId == userId).FirstOrDefault();

                if (checkUserKitchenAndMoney.Balance > 0)
                {
                    if (moneyWithdraw >= 50000)
                    {
                        checkUserKitchenAndMoney.Balance -= moneyWithdraw;
                        _context.Wallets.Update(checkUserKitchenAndMoney);

                        var transaction = new Transaction
                        {
                            OrderId = null,
                            UserId = userId,
                            Amount = moneyWithdraw,
                            Date = datenow,
                            Description = $"WITHDRAW WITH CHEF: {UserNameChef}",
                            Status = "SUCCEED",
                            TransactionType = "WITHDRAWN",
                            WalletId = checkUserKitchenAndMoney.WalletId,
                        };

                        _context.Transactions.Add(transaction);
                        _context.SaveChanges();

                    }
                    else throw new Exception("Money Withdraw Must Be Equal or Higher than 50000");
                }
                else throw new Exception("Money In Wallet Not Enough To Withdraw");

                // Create a MemoryStream to hold the PDF content
                using (var stream = new MemoryStream())
                {
                    var writer = new PdfWriter(stream);
                    var pdf = new PdfDocument(writer);
                    var document = new Document(pdf);

                    // Add content to the PDF
                    document.Add(new Paragraph($"Kitchen ID: {kitchenid}"));
                    document.Add(new Paragraph($"Money Withdrawn: {moneyWithdraw}"));
                    document.Add(new Paragraph($"Money Withdrawn DateTime: {datenow}"));

                    // Add more information if needed...

                    // Save the document
                    document.Close();

                    // Get the PDF content as byte array
                    var fileBytes = stream.ToArray();

                    // Return the PDF as a FileResult
                    var fileName = $"{Guid.NewGuid()}_WithdrawalReceipt.pdf";
                    return File(fileBytes, "application/pdf", fileName);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // You can remove the DownloadPdf action as it's not needed anymore.


    }
}
