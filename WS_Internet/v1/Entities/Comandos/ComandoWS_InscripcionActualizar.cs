using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS_Internet.v1.Entities.Comandos
{
    public class ComandoWS_InscripcionActualizar
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string Identificador { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public int? AñosExperiencia { get; set; }
        public int TipoAutoKeyValue { get; set; }
        public int TipoInscripcionKeyValue { get; set; }
        public virtual string FechaTelegrama { get; set; }
        public virtual string RcondVce { get; set; }
        public virtual string ArtComp { get; set; }
        public virtual string ArtVce { get; set; }
        public virtual string Caja { get; set; }
        public virtual string Observaciones { get; set; }
    }
}