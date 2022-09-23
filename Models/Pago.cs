using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliariaPestchanker.Models
{
    public class Pago
    {
        [Key]
        [Display(Name = "Código")]
        public int Id { get; set; }
		[Required]
		public DateTime FechaPago { get; set; }
		
		[Required]
		public decimal Importe { get; set; }
		[Required]
		public int IdContrato { get; set; }
	}
}
