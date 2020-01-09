using HiberusAPIEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiberusAPINegocio
{
    public interface INegocio
    {
        void AddRates(List<RateEnt> ratesList);

        void DeleteRates();

        IEnumerable<RateEnt> GetRates();

        void AddTransactions(List<Transaction> transactionsList);

        void DeleteTransactions();

        IEnumerable<Transaction> GetTransactions();

        void SaveChanges();

    }
}
