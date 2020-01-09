using System;
using System.Collections.Generic;
using System.Text;

namespace HiberusAPIEntidades
{
    public interface IMiLogger
    {
        void Write(Capa capa, string texto);
    }
}
