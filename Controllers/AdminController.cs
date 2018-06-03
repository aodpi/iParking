using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iParking.Data;
using iParking.Models;
using iParking.Models.AccountViewModels;
using iParking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace iParking.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWalletService _walletService;
        private UserManager<ApplicationUser> _userManager;
        public AdminController(ApplicationDbContext context, IWalletService walletService,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _walletService = walletService;
            _userManager = userManager;
        }

        private IEnumerable<ApplicationUser> GetExistingUsers()
        {
            return _userManager.Users.Where(f => User.Identity.Name != f.UserName);
        }

        [HttpDelete, Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
            }

            return Ok();
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> ReservationEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingReservation = await _context.ParkingReservations.SingleOrDefaultAsync(m => m.Id == id);
            if (parkingReservation == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", parkingReservation.UserId);
            return View(parkingReservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReservationEdit(int id, [Bind("Id,UserId,ParkingDate,ParkingTime,AmountPaid,VerificationCode")] ParkingReservation parkingReservation)
        {
            if (id != parkingReservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parkingReservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParkingReservationExists(parkingReservation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", parkingReservation.UserId);
            return View(parkingReservation);
        }

        private bool ParkingReservationExists(int id)
        {
            return _context.ParkingReservations.Any(e => e.Id == id);
        }

        public async Task<IActionResult> ReservationDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingReservation = await _context.ParkingReservations
                .Include(p => p.User)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (parkingReservation == null)
            {
                return NotFound();
            }

            return View(parkingReservation);
        }

        public IActionResult EditAccount(string id)
        {
            var user = _userManager.Users.FirstOrDefault(f => f.Id == id);

            if (user == null)
            {
                return View();
            }
            var editModel = new UserAccountEditViewModel
            {
                Id = user.Id,
                BirthDate = user.BirthDate,
                Email = user.Email,
                IDNP = user.IDNP,
                NickName = user.UserName,
                Nume = user.FirstName,
                PhoneNumber = user.PhoneNumber,
                Prenume = user.LastName
            };
            return View(editModel);
        }

        public IActionResult Reservations()
        {
            var applicationDbContext = _context.ParkingReservations.Include(p => p.User);
            return View(applicationDbContext);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(CreateAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newUser = new ApplicationUser
                {
                    UserName = model.UserName
                };

                var result = await _userManager.CreateAsync(newUser, model.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    if (model.IsAdmin)
                    {
                        var roleResult = await _userManager.AddToRoleAsync(newUser, "Admin");

                        if (!roleResult.Succeeded)
                        {
                            foreach (var error in roleResult.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                        else
                        {
                            return RedirectToAction("CreateAccount");
                        }
                    }
                }
            }
            model.ExistingUserNames = GetExistingUsers();
            model.WalletAmounts = new Dictionary<string, string>();
            foreach (var item in model.ExistingUserNames)
            {
                model.WalletAmounts.Add(item.Id, _walletService.GetCurrentAmount(item.Id));
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditAccount(UserAccountEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.Users.FirstOrDefault(f => f.Id == model.Id);

                if (user != null)
                {
                    user.BirthDate = model.BirthDate;
                    user.FirstName = model.Nume;
                    user.LastName = model.Prenume;
                    user.Email = model.Email;
                    user.UserName = model.NickName;
                    user.PhoneNumber = model.PhoneNumber;
                    user.IDNP = model.IDNP;

                    var result = await _userManager.UpdateAsync(user);
                    if (!result.Succeeded)
                    {

                    }
                }
            }
            return RedirectToActionPermanent(nameof(CreateAccount));
        }

        public IActionResult CreateAccount()
        {
            var users = GetExistingUsers();
            var model = new CreateAccountViewModel
            {
                ExistingUserNames = users,
                WalletAmounts = new Dictionary<string, string>()
            };

            foreach (var item in users)
            {
                model.WalletAmounts.Add(item.Id, _walletService.GetCurrentAmount(item.Id));
            }
            return View(model);
        }

        // GET: Parkings
        public async Task<IActionResult> Index() =>
            View(await _context.Parkings.ToListAsync());

        // GET: Parkings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parking = await _context.Parkings
                .SingleOrDefaultAsync(m => m.Id == id);
            if (parking == null)
            {
                return NotFound();
            }

            return View(parking);
        }

        // GET: Parkings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Parkings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ParkingName,ParkingNumber,ParkingSlots,PricePerHour,Latitude,Longitude")] Parking parking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(parking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(parking);
        }

        // GET: Parkings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parking = await _context.Parkings.SingleOrDefaultAsync(m => m.Id == id);
            if (parking == null)
            {
                return NotFound();
            }
            return View(parking);
        }

        // POST: Parkings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ParkingName,ParkingNumber,ParkingSlots,PricePerHour,Latitude,Longitude")] Parking parking)
        {
            if (id != parking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParkingExists(parking.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(parking);
        }

        // GET: Parkings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parking = await _context.Parkings
                .SingleOrDefaultAsync(m => m.Id == id);
            if (parking == null)
            {
                return NotFound();
            }

            return View(parking);
        }

        // POST: Parkings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parking = await _context.Parkings.SingleOrDefaultAsync(m => m.Id == id);
            _context.Parkings.Remove(parking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParkingExists(int id)
        {
            return _context.Parkings.Any(e => e.Id == id);
        }
    }
}