using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS_Intranet.v1.Entities.Comandos
{
    public class ComandoWS_UsuarioNuevo
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Dni { get; set; }
        public string FechaNacimiento { get; set; }
        public bool? SexoMasculino { get; set; }
        public string DomicilioBarrio { get; set; }
        public string DomicilioCalle { get; set; }
        public string DomicilioAltura { get; set; }
        public string DomicilioObservaciones { get; set; }
        public string Observaciones { get; set; }
        public string DomicilioPiso { get; set; }
        public string DomicilioDepto { get; set; }
        public string DomicilioCodigoPostal { get; set; }

        public _Model.Comandos.Comando_UsuarioNuevo Convertir()
        {
            return new _Model.Comandos.Comando_UsuarioNuevo()
            {
                Nombre = Nombre,
                Apellido = Apellido,
                Dni = Dni,
                SexoMasculino = SexoMasculino,
                FechaNacimiento = FechaNacimiento,
                DomicilioBarrio = DomicilioBarrio,
                DomicilioCalle = DomicilioCalle,
                DomicilioAltura = DomicilioAltura,
                DomicilioObservaciones = DomicilioObservaciones,
                Observaciones = Observaciones,
                DomicilioPiso = DomicilioPiso,
                DomicilioDepto = DomicilioDepto,
                DomicilioCodigoPostal = DomicilioCodigoPostal
            };
        }
    }
}