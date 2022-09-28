using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliariaPestchanker.Models
{
    public class Usuario
    {
        [Key]
        [Display(Name = "Código")]
        public int Id { get; set; }
		[Required]
		public string Nombre { get; set; }
		[Required]
		public string Apellido { get; set; }
		[Required]
		public string Dni { get; set; }
		[Required, EmailAddress]
		public string Email { get; set; }
		[Required]
		public string Clave { get; set; }
		
		// URL a la foto es Avatar
		public string? Avatar { get; set; }
		
		// acá viene el archivo en AvatarFile
		public IFormFile? AvatarFile { get; set; }

		[Required]
		public int IdRol { get; set; }
       
	}
}
