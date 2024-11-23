using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrpManagementSystem.DTO
{
    public class ChannelDTO
    {

        

            public int ChannelId { get; set; }

            [Required(ErrorMessage = "Channel Name is required.")]
            [StringLength(100, ErrorMessage = "Channel Name cannot exceed 100 characters.")]
            public string ChannelName { get; set; }

            [Required(ErrorMessage = "Established Year is required.")]
           [Range(1900, int.MaxValue, ErrorMessage = "Established Year must be a valid year.")]
           [CustomValidation(typeof(TrpManagementSystem.DTO.ChannelDTO), "ValidateYear")]
            public int EstablishedYear { get; set; }

            [Required(ErrorMessage = "Country is required.")]
            [StringLength(50, ErrorMessage = "Country name cannot exceed 50 characters.")]
            public string Country { get; set; }

            // Static validation method for EstablishedYear
           public static ValidationResult ValidateYear(int establishedYear, ValidationContext context)
            {
                var currentYear = DateTime.Now.Year;

                // Check if the year is in the future
                if (establishedYear > currentYear)
                {
                    return new ValidationResult($"Established Year cannot be in the future.");
                }

                return ValidationResult.Success;
            }
        }
    



}