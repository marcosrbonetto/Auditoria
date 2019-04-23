using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Model.Entities
{
    public class Inscripcion : BaseEntity
    {
        public virtual Usuario Usuario { get; set; }
        public virtual string UsuarioDni { get; set; }
        public virtual TipoInscripcion TipoInscripcion { get; set; }
        public virtual TipoAuto TipoAuto { get; set; }
        public virtual string Identificador { get; set; }
        public virtual DateTime? FechaInicio { get; set; }
        public virtual DateTime? FechaFin { get; set; }
        public virtual DateTime? FechaTelegrama { get; set; }
        public virtual DateTime? FechaVencimientoLicencia { get; set; }
        public virtual string ArtCompañia { get; set; }
        public virtual DateTime? ArtFechaVencimiento { get; set; }
        public virtual string Caja { get; set; }
        public virtual string Observaciones { get; set; }
        public virtual string Error { get; set; }
        public virtual TipoCondicionInscripcion TipoCondicionInscripcion { get; set; }
        public virtual bool Favorito { get; set; }
    }
}
