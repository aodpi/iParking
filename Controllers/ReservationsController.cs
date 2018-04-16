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

namespace iParking.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
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

        // GET: Reservations/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,ParkingDate,ParkingTime,AmountPaid,VerificationCode")] ParkingReservation parkingReservation)
        {

            var userId = _userManager.GetUserId(User);

            parkingReservation.UserId = userId;
            parkingReservation.VerificationCode = Guid.NewGuid();
            _context.Add(parkingReservation);
            await _context.SaveChangesAsync();
            return View("ReservationDetails", parkingReservation);
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
