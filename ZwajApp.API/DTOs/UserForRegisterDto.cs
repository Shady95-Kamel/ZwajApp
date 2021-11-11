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
    }
}