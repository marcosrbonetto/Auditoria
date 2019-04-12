using _Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS_Intranet.v1.Entities.Comandos
{
    public class ComandoWS_InscripcionActualizar
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string Identificador { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public Enums.TipoAuto TipoAutoKeyValue { get; set; }
        public Enums.TipoInscripcion TipoInscripcionKeyValue { get; set; }
        public string FechaTelegrama { get; set; }
        public string FechaVencimientoLicencia { get; set; }
        public string ArtCompañia { get; set; }
        public string ArtFechaVencimiento { get; set; }
        public string Caja { get; set; }
        public string Observaciones { get; set; }
        public Enums.TipoCondicionInscripcion CondicionKeyValue { get; set; }

        public _Model.Comandos.Comando_InscripcionActualizar Convertir()
        {
            return new _Model.Comandos.Comando_InscripcionActualizar()
            {
                Id = Id,
                IdUsuario = IdUsuario,
                Identificador = Identificador,
                FechaInicio = FechaInicio,
                FechaFin = FechaFin,
                TipoAutoKeyValue = TipoAutoKeyValue,
                TipoInscripcionKeyValue = TipoInscripcionKeyValue,
                FechaTelegrama = FechaTelegrama,
                FechaVencimientoLicencia = FechaVencimientoLicencia,
                ArtCompañia = ArtCompañia,
                ArtFechaVencimiento = ArtFechaVencimiento,
                Caja = Caja,
                Observaciones = Observaciones,
                CondicionKeyValue = CondicionKeyValue
            };
        }
    }
}