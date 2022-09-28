using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliariaPestchanker.Models
{
    public class FiltrarContrato
    {
    
    [Display(Name = "Inquilino")]    
		[Required]
         public int IdInquilino { get; set; }
        
        [Display(Name = "Fecha Desde")]
        public DateTime? FechaDesde { get; set; }
        
       [Display(Name = "Fecha Hasta")]
        public DateTime? FechaHasta { get; set; }
        
		       
    
}}


//x.HasValue

