using System;
using System.Linq;

namespace _Model
{
    [Serializable]
    public abstract class BaseEntity
    {
        public virtual int Id { get; set; }
        public virtual DateTime FechaAlta { get; set; }
        public virtual DateTime? FechaBaja { get; set; }
        public virtual DateTime? FechaModificacion { get; set; }
        public virtual int IdUsuarioAlta { get; set; }
        public virtual int? IdUsuarioModificacion { get; set; }
    }
}