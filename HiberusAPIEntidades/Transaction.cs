using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HiberusAPIEntidades
{
    /// <summary>
    /// Clase del objeto Transaction.
    /// </summary>
    public class Transaction
    {
        //El id es PK autonumerico en la BD. 
        [Key]
        public int Id { get; set; }
        public string Sku { get; set; }
        public decimal Amount { get; set; }
        public string  Currency { get; set; }

    }
}
