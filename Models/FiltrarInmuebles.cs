using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliariaPestchanker.Models
{
    public class FiltrarInmuebles
    {
        
		[Required]
        public string Disponibles { get; set; }
        
        [Required]
        public int IdPropietario { get; set; }
        
        [Required]
        public string Ocupados { get; set; }
        
		       
    
}}
