using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Model.Entities
{
    public class MuniOnlineUsuario: BaseEntity
    {
        public virtual string Nombre { get; set; }
        public virtual string Apellido { get; set; }
        public virtual DateTime? FechaNacimiento { get; set; }
        public virtual bool SexoMasculino { get; set; }
        public virtual int? Dni { get; set; }
        public virtual string Cuil { get; set; }
        public virtual string IdentificadorFotoPersonal { get; set; }
        public virtual string DomicilioLegalPais { get; set; }
        public virtual string DomicilioLegalFormateado { get; set; }

        //Datos acceso
        public virtual string Username { get; set; }

        //Datos contacto
        public virtual string Email { get; set; }
        public virtual string TelefonoFijo { get; set; }
        public virtual string TelefonoCelular { get; set; }
        public virtual string Facebook { get; set; }
        public virtual string Instagram { get; set; }
        public virtual string Twitter { get; set; }
        public virtual string LinkedIn { get; set; }

        //Validaciones
        public virtual DateTime? FechaValidacionRenaper { get; set; }
        public virtual DateTime? FechaValidacionEmail { get; set; }
    }
}
