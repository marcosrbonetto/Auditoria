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

        public enum TipoInhabilitacion
        {
            Definitivo = 1,
            Provisorio = 2,
            Prescripto = 3,
            Fallecido = 4,
            Sobreseido = 5
        }

        public enum TipoAuto
        {
            AutoTaxi = 1,
            AutoRemis = 2,
            AutoTaxiDiscap = 3,
            AutoRemisDiscap = 4,
            AutoTaxiLujo = 5,
            AutoRemisLujo = 6,
            Escolar = 7,
            EscolarPrivado = 8,
            Privado = 9,
            E = 10,
            S = 11,
            L = 12
        }

        public enum TipoCondicionInscripcion
        {
            Aspirante = 1,
            Caducidad = 2,
            Habilitado = 3,
            TRPFALLE = 4,
            Vacante = 5,
            Deposito = 6,
            Suspendido = 7,
            Transferen = 8
        }


    }
}