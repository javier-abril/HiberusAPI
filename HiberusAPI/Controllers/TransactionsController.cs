using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiberusAPIEntidades;
using HiberusAPINegocio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HiberusAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        //Usamos la inyección de dependencias
        private INegocio negocio;
        private IMiLogger logger;

        public TransactionsController(INegocio neg,IMiLogger log)
        {
            negocio = neg;
            logger = log;
        }

        // GET: api/transactions
        [HttpGet]
        public ActionResult<List<Transaction>> Get()
        {
            List<Transaction> transactionsList = new List<Transaction>();

            try
            {
                transactionsList = negocio.GetTransactions().ToList();
                return transactionsList;
            }
            catch (Exception ex)
            {
                logger.Write(Capa.Presentacion, ex.Message);
                return BadRequest("Error del servidor");
            }
        }

        // GET: api/transactions/5
        [HttpGet("{id}")]
        public ActionResult<Transaction> Get(int id)
        {
            Transaction transaction;

            try
            {
                transaction = (Transaction) negocio.GetTransactions().Where(t=>t.Id == id).SingleOrDefault();
                return transaction;
            }
            catch (Exception ex)
            {
                logger.Write(Capa.Presentacion, ex.Message);
                return BadRequest("Error del servidor");
            }
        }

    }
}
