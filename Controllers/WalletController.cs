using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iParking.Data;
using iParking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace iParking.Controllers
{
    [Authorize]
    public class WalletController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public WalletController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public IActionResult Fill() => View();

        [HttpPost]
        public async Task<IActionResult> Fill(WalletFillModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var wallet =  _context.Wallets.FirstOrDefault(f => f.UserId == userId);

                if(wallet == null)
                {
                    wallet = _context.Wallets.Add(new UserWallet
                    {
                        UserId = userId
                    }).Entity;

                    await _context.SaveChangesAsync();
                }

                wallet.Amount += model.Amount;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
    }
}