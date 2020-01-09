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
    public class RatesController : ControllerBase
    {
        //Usamos la inyección de dependencias
        private INegocio negocio;
        private IMiLogger logger;

        public RatesController(INegocio neg, IMiLogger log)
        {
            negocio = neg;
            logger = log;
        }

        // GET api/rates
        [HttpGet]
        public ActionResult<List<RateEnt>> Get()
        {
            List<RateEnt> rateList = new List<RateEnt>();

            try
            {
                rateList = negocio.GetRates().ToList();
                return rateList;
            }
            catch (Exception ex)
            {
                logger.Write(Capa.Presentacion, ex.Message);
                return BadRequest("Error del servidor");
            }
           
        }

        // GET api/rates/5
        [HttpGet("{id}")]
        public ActionResult<RateEnt> Get(int id)
        {
            RateEnt rate = new RateEnt();

            try
            {
                rate = (RateEnt)negocio.GetRates().Where(r=>r.Id == id).SingleOrDefault();
                return rate;
            }
            catch (Exception ex)
            {
                logger.Write(Capa.Presentacion, ex.Message);
                return BadRequest("Error del servidor");
            }
        }

    }
}
