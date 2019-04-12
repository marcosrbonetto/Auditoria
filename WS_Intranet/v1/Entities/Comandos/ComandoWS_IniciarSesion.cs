using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS_Intranet.v1.Entities.Comandos
{
    public class ComandoWS_IniciarSesion
    {
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
    }
}