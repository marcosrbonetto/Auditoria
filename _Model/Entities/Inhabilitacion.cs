using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Model.Entities
{
    public class Inhabilitacion : BaseEntity
    {
        public virtual Usuario Usuario { get; set; }
        public virtual string UsuarioDni { get; set; }
        public virtual TipoInhabilitacion TipoInhabilitacion { get; set; }
        public virtual DateTime? FechaInicio { get; set; }
        public virtual string FechaInicioString { get; set; }
        public virtual DateTime? FechaFin { get; set; }
        public virtual string FechaFinString { get; set; }
        public virtual string DtoRes { get; set; }
        public virtual string Expediente { get; set; }
        public virtual string ObservacionesAutoChapa { get; set; }
        public virtual string ObservacionesTipoAuto { get; set; }
        public virtual string Observaciones { get; set; }
    }
}
