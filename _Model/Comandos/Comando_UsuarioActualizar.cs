using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Model.Comandos
{
    public class Comando_UsuarioActualizar: Comando_UsuarioNuevo
    {
        public int? Id { get; set; }
    }
}
