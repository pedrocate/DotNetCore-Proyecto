using System;
using System.Net;

namespace Aplicacion.ManejadorError
{
    public class ManejadorExcepcion : Exception
    {
        public HttpStatusCode Codigo { get; set; }
        public object Errores { get; set; }
        public ManejadorExcepcion(HttpStatusCode codigo, object errores = null) {
            Codigo = codigo;
            Errores = errores;
        }
    }
}