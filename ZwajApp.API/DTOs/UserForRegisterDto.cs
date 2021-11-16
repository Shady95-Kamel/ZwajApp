using System;
using System.ComponentModel.DataAnnotations;

namespace ZwajApp.API.DTOs
{
    public class UserForRegisterDto
    {
        [Required]
        public string UserName { get; set; }
        [StringLength(8,MinimumLength=4,ErrorMessage="يجب ان لا تزبد كلمة السر عن اربعة احرف و لا تزيد عن ثمانية احرف")]
        [Required]
        public string Password { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string KnownAs { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }

        public UserForRegisterDto()
        {
            Created=DateTime.Now;
            LastActive=DateTime.Now;
        }
    }
}