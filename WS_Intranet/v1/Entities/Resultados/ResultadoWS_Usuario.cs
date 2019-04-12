using _Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WS_Intranet.v1.Entities.Resultados
{
    [Serializable]
    public class ResultadoWS_Usuario
    {
        public int Id { get; set; }

        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int? Dni { get; set; }
        public bool? SexoMasculino { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string DomicilioBarrio { get; set; }
        public string DomicilioCalle { get; set; }
        public string DomicilioAltura { get; set; }
        public string DomicilioObservaciones { get; set; }
        public string Observaciones { get; set; }
        public string DomicilioPiso { get; set; }
        public string DomicilioDepto { get; set; }
        public string DomicilioCodigoPostal { get; set; }
        public string Error { get; set; }

        public ResultadoWS_Usuario(Usuario usuario)
        {
            if (usuario == null) return;

            Id = usuario.Id;
            Nombre = usuario.Nombre;
            Apellido = usuario.Apellido;
            Dni = usuario.Dni;
            SexoMasculino = usuario.SexoMasculino;
            FechaNacimiento = usuario.FechaNacimiento;
            DomicilioBarrio = usuario.DomicilioBarrio;
            DomicilioCalle = usuario.DomicilioCalle;
            DomicilioAltura = usuario.DomicilioAltura;
            DomicilioObservaciones = usuario.DomicilioObservaciones;
            Observaciones = usuario.Observaciones;
            DomicilioPiso = usuario.DomicilioPiso;
            DomicilioDepto = usuario.DomicilioDepto;
            DomicilioCodigoPostal = usuario.DomicilioCodigoPostal;
            Error = usuario.Error;
        }

        public static List<ResultadoWS_Usuario> ToList(IList<Usuario> list)
        {
            return list.Select(x => new ResultadoWS_Usuario(x)).ToList();
        }
    }
}