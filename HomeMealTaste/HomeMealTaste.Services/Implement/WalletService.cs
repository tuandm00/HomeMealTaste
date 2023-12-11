using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Services.Implement
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository  _walletRepository;
        private readonly HomeMealTasteContext _context;

        public WalletService(IWalletRepository walletRepository, HomeMealTasteContext context)
        {
            _walletRepository = walletRepository;
            _context = context;
        }

        public async Task<decimal> GetRevenueSystem()
        {
            var result = _context.Wallets.Where(x => x.UserId == 2).Select(x => x.Balance).FirstOrDefault();
            return result ?? 0m;
        }
    }
}
