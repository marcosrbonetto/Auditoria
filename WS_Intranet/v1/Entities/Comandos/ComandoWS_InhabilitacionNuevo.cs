using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS_Intranet.v1.Entities.Comandos
{
    public class ComandoWS_InhabilitacionNuevo
    {
        public int IdUsuario { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string DtoRes { get; set; }
        public string Expediente { get; set; }
        public string Observaciones { get; set; }
        public string ObservacionesAutoChapa { get; set; }
        public string ObservacionesTipoAuto { get; set; }
        public _Model.Enums.TipoInhabilitacion? TipoInhabilitacionKeyValue { get; set; }

        public _Model.Comandos.Comando_InhabilitacionNuevo Convertir()
        {
            return new _Model.Comandos.Comando_InhabilitacionNuevo()
            {
                IdUsuario = IdUsuario,
                FechaInicio = FechaInicio,
                FechaFin = FechaFin,
                DtoRes = DtoRes,
                Expediente = Expediente,
                Observaciones = Observaciones,
                ObservacionesAutoChapa = ObservacionesAutoChapa,
                ObservacionesTipoAuto = ObservacionesTipoAuto,
                TipoInhabilitacionKeyValue = TipoInhabilitacionKeyValue
            };
        }
    }
}