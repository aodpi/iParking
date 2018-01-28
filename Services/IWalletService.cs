using iParking.Data;
using iParking.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace iParking.Services
{
    public interface IWalletService
    {
        string GetCurrentAmount(ClaimsPrincipal user);
    }

    public class WalletService : IWalletService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public WalletService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public string GetCurrentAmount(ClaimsPrincipal user)
        {
            var userId = _userManager.GetUserId(user);
            var wallet = _context.Wallets.FirstOrDefault(f => f.UserId == userId);
            decimal totalAmount = 0;
            if(wallet != null)
            {
                totalAmount = wallet.Amount;
            }

            var result = string.Format("{0:N} Lei", totalAmount);

            return result;
        }
    }
}
