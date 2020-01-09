using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HiberusAPIEntidades
{
    public class Logger : IMiLogger
    {
        /// <summary>
        /// Método para escribir en los diferentes ficheros 
        /// de log dependiendo de la capa seleccionada
        /// </summary>
        /// <param name="capa">Enum Capa</param>
        /// <param name="texto">Texto a escribir</param>
        public void Write(Capa capa, string texto)
        {

            switch (capa)
            {
                case Capa.Datos:
                    WriteLog("datos.log", texto);
                    break;
                case Capa.Entidades:
                    WriteLog("entidades.log", texto);
                    break;
                case Capa.Negocio:
                    WriteLog("negocio.log", texto);
                    break;
                case Capa.Presentacion:
                    WriteLog("presentacion.log", texto);
                    break;
                default:
                    break;
            }
        }

        private void WriteLog(string path, string text)
        {
            //Comprobamos que exista el directorio de logs, sino lo creamos
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "/logs/"))
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/logs/");

            using (StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + "/logs/" + path, true))
            {
                sw.WriteLine(DateTime.Now.ToString() + " - " + text);
            }
        }
    }

    public enum Capa
    {
        Negocio,
        Entidades,
        Datos,
        Presentacion
    }
}
