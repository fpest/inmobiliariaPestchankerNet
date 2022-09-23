using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliariaPestchanker.Models
{
    public class Clave
    {
        [Key]
        [Display(Name = "Código")]
        public int Id { get; set; }
		[Required]
        [DataType(DataType.Password)]
		public string ClaveAnterior { get; set; }
		
		[Required]
		public string ClaveNueva { get; set; }
		
		
	}
}
