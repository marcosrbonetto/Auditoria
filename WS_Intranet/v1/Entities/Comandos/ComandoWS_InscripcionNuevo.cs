using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS_Intranet.v1.Entities.Comandos
{
    public class ComandoWS_InscripcionNuevo
    {
        public int IdUsuario { get; set; }
        public string Identificador { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public _Model.Enums.TipoAuto TipoAutoKeyValue { get; set; }
        public _Model.Enums.TipoInscripcion TipoInscripcionKeyValue { get; set; }
        public virtual string FechaTelegrama { get; set; }
        public virtual string RcondVce { get; set; }
        public virtual string ArtComp { get; set; }
        public virtual string ArtVce { get; set; }
        public virtual string Caja { get; set; }
        public virtual string Observaciones { get; set; }

        public _Model.Comandos.Comando_InscripcionNuevo Convertir()
        {
            return new _Model.Comandos.Comando_InscripcionNuevo()
            {
                IdUsuario = IdUsuario,
                Identificador = Identificador,
                FechaInicio = FechaInicio,
                FechaFin = FechaFin,
                TipoAutoKeyValue = TipoAutoKeyValue,
                TipoInscripcionKeyValue = TipoInscripcionKeyValue,
                FechaTelegrama = FechaTelegrama,
                FechaVencimientoLicencia = RcondVce,
                ArtCompañia = ArtComp,
                ArtFechaVencimiento = ArtVce,
                Caja = Caja,
                Observaciones = Observaciones
            };
        }
    }
}