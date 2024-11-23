using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TrpManagementSystem.EF;

namespace TrpManagementSystem.DTO
{
    public class ProgramDTO
    {
        public int ProgramId { get; set; }

        [Required(ErrorMessage = "Program Name is required.")]
        [StringLength(150, ErrorMessage = "Program Name cannot exceed 150 characters.")]
        [CustomValidation(typeof(ProgramDTO), "ValidateProgramName")]
        public string ProgramName { get; set; }

        [Required(ErrorMessage = "TRP Score is required.")]
        [Range(0.0, 10.0, ErrorMessage = "TRP Score must be between 0.0 and 10.0.")]
        public decimal TRPScore { get; set; }

        [Required(ErrorMessage = "Channel is required.")]
        public int ChannelId { get; set; }

        [Required(ErrorMessage = "Air Time is required.")]
        [RegularExpression(@"^(?:[01]?\d|2[0-3]):[0-5]?\d:[0-5]?\d$", ErrorMessage = "Air Time must be in HH:mm:ss format.")]
        public string AirTime { get; set; }

        // Static validation method for Program Name uniqueness within a channel
        public static ValidationResult ValidateProgramName(string programName, ValidationContext context)
        {
            var instance = context.ObjectInstance as ProgramDTO;
            if (instance != null)
            {
                using (var db = new DotnetDBEntities())
                {
                    bool exists = db.Programs.Any(p => p.ProgramName == programName && p.ChannelId == instance.ChannelId && p.ProgramId != instance.ProgramId);
                    if (exists)
                    {
                        return new ValidationResult("Program Name must be unique within the same channel.");
                    }
                }
            }
            return ValidationResult.Success;
        }
    }

}