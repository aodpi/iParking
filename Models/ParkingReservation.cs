﻿using System;
using System.ComponentModel.DataAnnotations;

namespace iParking.Models
{
    public class ParkingReservation
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public DateTime ParkingDate { get; set; }
        public TimeSpan ParkingTime { get; set; }
        public decimal AmountPaid { get; set; }
        public Guid VerificationCode { get; set; }
    }
}
