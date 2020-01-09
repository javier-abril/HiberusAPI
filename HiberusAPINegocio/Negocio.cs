using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiberusAPIDatosContext;
using HiberusAPIEntidades;

namespace HiberusAPINegocio
{
    public class Negocio:INegocio
    {
        //Inyección de dependencias
        private IMiLogger logger;

        private DatosContext contexto;
        private List<RateEnt> ratesList= new List<RateEnt>();
        private List<Transaction> transactionsList = new List<Transaction>(); 

        public Negocio(IMiLogger log)
        {
            this.logger = log;
            this.contexto = new DatosContext();
            this.DescargaDatosAPI().Wait();
        }

        public Negocio(IMiLogger log, List<RateEnt> rates, List<Transaction> transactions)
        {
            this.logger = log;
            this.contexto = new DatosContext();
            this.ratesList = rates;
            this.transactionsList = transactions;
        }


        /// <summary>
        /// Funcion asincrona que intenta descargar datos de la API de Hiberus.
        /// Si funciona OK rellena los datos recogidos en la API y borra/graba las tablas
        /// Si falla la API lee de la BD
        /// </summary>
        /// <returns></returns>
        async private Task DescargaDatosAPI()
        {
            try
            {
                logger.Write(Capa.Negocio, "Leemos datos de la API");
                
                //Intentamos descargar de la API
                this.ratesList = (List<RateEnt>) await ConexionAPIHiberus.GetRatesAPI();
                this.transactionsList = (List<Transaction>)await ConexionAPIHiberus.GetTransactionAPI();

                logger.Write(Capa.Negocio, "Borramos las tablas y las volvemos a cargar con los datos de la API");

                //Si la carga va bien (no lanza excepcion), borramos y metemos en la DB
                this.DeleteRates();
                this.DeleteTransactions();
                this.AddRates(ratesList);
                this.AddTransactions(transactionsList);

            }
            catch (Exception ex)
            {
                logger.Write(Capa.Negocio, "Se ha producido un error accediendo a la API hiberus:\n" + ex.Message);
                
                //Si da error de conexión cargamos de la DB
                this.ratesList = this.GetRates().ToList();
                this.transactionsList = this.GetTransactions().ToList();
            }
        }


        public IEnumerable<RateEnt> GetRates()
        {
            //No tengo claro si se solicita que refresque los datos en cada petición
            //En principio lo he hecho con ese supuesto. Si unicamente se desea que se carguen
            //al inicio de la aplicación habría que mover esta llamada al constructor de esta clase
            this.DescargaDatosAPI().Wait();

            //Si hay rates en las variables locales nos ahorramos la llamada al contexto porque la ha cogido del JSON
            if (this.ratesList.Count > 0)
                return this.ratesList;
            else
            {
                try
                {

                    List<RateEnt> ratesList = (from RateEnt r in contexto.Rates select r)
                                        .ToList<RateEnt>();
                    return ratesList;

                }
                catch (Exception ex)
                {
                    logger.Write(Capa.Negocio, "Se ha producido un leyendo Rates:\n" + ex.Message);
                    //Lanzamos excepcion a capa superior
                    throw new Exception(ex.Message);
                }
            }
        }

        public IEnumerable<Transaction> GetTransactions()
        {
            //No tengo claro si se solicita que refresquen las transacciones en cada petición
            //Para no relentizar las aplicación le cargamos al inicio de la aplicación y no se actualizan
            //en cada petición. Si se desea que se actualice en cada petición unicamente habría que descomentar
            //la siguiente llamada:
            //this.DescargaDatosAPI().Wait();

            //Si hay transacciones en las variables locales nos ahorramos la llamada al contexto porque la ha cogido del JSON
            if (this.transactionsList.Count > 0)
                return this.transactionsList;
            else
            {
                try
                {

                    List<Transaction> transactionsList = (from Transaction t in contexto.Transactions select t)
                                            .ToList<Transaction>();

                    return transactionsList;
                }
                catch (Exception ex)
                {
                    logger.Write(Capa.Negocio, "Se ha producido un leyendo Transactions:\n" + ex.Message);
                    //Lanzamos excepcion a capa superior
                    throw new Exception(ex.Message);
                }
            }
        }



        #region Métodos de acceso a capa de Datos

        public void AddRates(List<RateEnt> ratesList)
        {
            try
            {

                contexto.Rates.AddRange(ratesList);
                SaveChanges();

            }catch (Exception ex)
            {
                logger.Write(Capa.Negocio, "Se ha producido un grabando Rates:\n" + ex.Message);
                //Lanzamos excepcion a capa superior
                throw new Exception(ex.Message);
            }
        }

        public void AddTransactions(List<Transaction> transactionsList)
        {
            try { 
                contexto.Transactions.AddRange(transactionsList);
                SaveChanges();
            }
            catch (Exception ex)
            {
                logger.Write(Capa.Negocio, "Se ha producido un grabando Transactions:\n" + ex.Message);
                //Lanzamos excepcion a capa superior
                throw new Exception(ex.Message);
            }
        }

        public void DeleteRates()
        {
            try{ 
                contexto.Rates.RemoveRange(contexto.Rates.ToList());
                SaveChanges();
            }
            catch (Exception ex)
            {
                logger.Write(Capa.Negocio, "Se ha producido un borrando Rates:\n" + ex.Message);
                //Lanzamos excepcion a capa superior
                throw new Exception(ex.Message);
            }

        }

        public void DeleteTransactions()
        {
            try
            {
                contexto.Transactions.RemoveRange(contexto.Transactions.ToList());
                SaveChanges();
            }
            catch (Exception ex)
            {
                logger.Write(Capa.Negocio, "Se ha producido un borrando Transactions:\n" + ex.Message);
                //Lanzamos excepcion a capa superior
                throw new Exception(ex.Message);
            }

        }


        public void SaveChanges()
        {
            contexto.SaveChanges();
        }

        #endregion
    }
}
