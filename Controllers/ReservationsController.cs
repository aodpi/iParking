using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using iParking.Data;
using iParking.Models;
using Microsoft.AspNetCore.Identity;
using iParking.Services;

namespace iParking.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWalletService _walletService;

        public ReservationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            IWalletService wallet)
        {
            _context = context;
            _walletService = wallet;
            _userManager = userManager;
        }

        // GET: Reservations
        public IActionResult Index()
        {
            var applicationDbContext = _context.ParkingReservations.Include(p => p.User)
                .Where(p => p.User.UserName == User.Identity.Name);
            return View(applicationDbContext);
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
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

        public IActionResult GetLatLong(int parkingId)
        {
            var parking = _context.Parkings.FirstOrDefault(f => f.Id == parkingId);
            return Json(parking);
        }
        // GET: Reservations/Create
        public IActionResult Create()
        {
            ReservationCreateViewModel vm = new ReservationCreateViewModel
            {
                AvailableParkings = _context.Parkings.ToList(),
            };
            return View(vm);
        }

        public decimal CalcPrice(int parkingId, int hours)
        {
            var parking = _context.Parkings.FirstOrDefault(f => f.Id == parkingId);

            if(parking != null)
            {
                return hours * parking.PricePerHour;
            }
            return 0;
        }

        public class Slot
        {
            public int SlotNumber { get; set; }
            public bool IsAvailable { get; set; }
        }

        public IActionResult GetAvailableSlots(int parkingId)
        {
            var parking = _context.Parkings.Find(parkingId);

            if(parking != null)
            {
                var reservations = _context.ParkingReservations.Where(f => f.ParkingDate >= DateTime.Now).Select(f => f.SlotNumber);

                var slots = Enumerable.Range(1, parking.ParkingSlots);

                List<Slot> result = new List<Slot>();
                foreach (var item in slots)
                {
                    result.Add(new Slot
                    {
                        SlotNumber = item,
                        IsAvailable = !reservations.Contains(item)
                    });
                }

                return Json(result);
            }
            return Json(Enumerable.Empty<Slot>());
        }
        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservationCreateViewModel model)
        {

            var userId = _userManager.GetUserId(User);
            if(ModelState.IsValid)
            {
                var parking = _context.Parkings.FirstOrDefault(f => f.Id == model.ParkingId);
                var totalAmount = model.ParkingDuration * parking.PricePerHour;
                if (_walletService.Pay(totalAmount, User))
                {
                    var parkingReservation = new ParkingReservation
                    {
                        UserId = userId,
                        CarCategory = model.CarCategory,
                        CarNumber = model.CarNumber,
                        ParkingDate = model.ParkingDate,
                        ParkingTime = model.ParkingDuration,
                        VerificationCode = Guid.NewGuid(),
                        AmountPaid = model.ParkingDuration * parking.PricePerHour,
                        SlotNumber = model.SlotNumber,
                        ParkingId = model.ParkingId
                    };
                    _context.Add(parkingReservation);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), "Reservations", new { id = parkingReservation.Id });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Resurse insuficiente în cont.");
                }
            }
            model.AvailableParkings = _context.Parkings.ToList();
            return View(model);
        }

        public string VerifyCode(Guid verification_code)
        {
            var rz = _context.ParkingReservations.FirstOrDefault(f => f.VerificationCode == verification_code);

            if(rz == null)
            {
                ViewData["Result"] = "Cod invalid :(";
            }
            else
            {
                ViewData["Result"] = "Codul este valid";
            }

            return (string)ViewData["Result"];
        }
        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,ParkingDate,ParkingTime,AmountPaid,VerificationCode")] ParkingReservation parkingReservation)
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
    }
}
