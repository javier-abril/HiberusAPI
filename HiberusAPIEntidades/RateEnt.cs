using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HiberusAPIEntidades
{
    /// <summary>
    /// Clase del objeto Rate.
    /// </summary>
    public class RateEnt
    {
        //El id es PK autonumerico en la BD. 
        [Key]
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public decimal Rate { get; set; }

    }
}
