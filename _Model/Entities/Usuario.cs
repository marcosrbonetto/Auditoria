using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Model.Entities
{
    public class Usuario : BaseEntity
    {
        public virtual string Nombre { get; set; }
        public virtual string Apellido { get; set; }
        public virtual DateTime? FechaNacimiento { get; set; }
        public virtual bool SexoMasculino { get; set; }
        public virtual int? Dni { get; set; }
        public virtual string DomicilioBarrio { get; set; }
        public virtual string DomicilioCalle { get; set; }
        public virtual string DomicilioAltura { get; set; }
        public virtual string DomicilioPiso { get; set; }
        public virtual string DomicilioDepto{ get; set; }
        public virtual string DomicilioCodigoPostal { get; set; }
        public virtual string DomicilioObservaciones { get; set; }
        public virtual string Observaciones { get; set; }
        public virtual string Error { get; set; }
    }
}
