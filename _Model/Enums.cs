using System;
using System.Linq;

namespace _Model
{
    public class Enums
    {

        public enum InscripcionOrderBy
        {
            Identificador = 1,
            TipoInscripcion = 2,
            TipoAuto = 3,
            FechaInicio = 4,
            FechaFin = 5,
            FechaTelegrama = 6,
            FechaVencimientoLicencia = 7,
            ArtCompañia = 8,
            ArtFechaVencimiento = 9,
            Caja = 10,
            UsuarioApellidoNombre = 11
        }

        public enum InhabilitacionOrderBy
        {
            FechaInicio = 1,
            FechaFin = 2,
            UsuarioApellidoNombre = 3
        }

        public enum UsuarioOrderBy
        {
            Nombre = 1,
            Dni = 2,
            FechaNacimiento = 3
        }


        public enum TipoInscripcion
        {
            Titular = 1,
            Chofer = 2
        }

        public enum TipoAuto
        {
            Taxi = 1,
            Remis = 2,
            TaxiDiscap = 3,
            RemisDiscap = 4,
        }

    }
}