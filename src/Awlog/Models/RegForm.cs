using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Awlog.Models
{
    public class RegForm
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        [Required]
        public string Password { get; set; }

        public string Email { get; set; }

        [Display(Name="Login")]
        [Required(ErrorMessage = "FUCKASS!!!")]
        public string Login { get; set; }
    }
}
