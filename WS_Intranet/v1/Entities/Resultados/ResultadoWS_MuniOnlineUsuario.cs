using _Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WS_Intranet.v1.Entities.Resultados
{
    [Serializable]
    public class ResultadoWS_MuniOnlineUsuario
    {
        public int Id { get; set; }

        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Dni { get; set; }
        public string Cuil { get; set; }
        public bool SexoMasculino { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string IdentificadorFotoPersonal { get; set; }


        //Datos acceso
        public string Username { get; set; }

        
        //Datos contacto
        public string Email { get; set; }
        public string TelefonoCelular { get; set; }
        public string TelefonoFijo { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Twitter { get; set; }
        public string LinkedIn { get; set; }


        public ResultadoWS_MuniOnlineUsuario(MuniOnlineUsuario usuario)
        {
            if (usuario == null)
            {
                return;
            }

            Id = usuario.Id;


            // Datos personales
            Nombre = usuario.Nombre;
            Apellido = usuario.Apellido;
            Dni = usuario.Dni.HasValue ? usuario.Dni.Value : 0;
            Cuil = usuario.Cuil;
            SexoMasculino = usuario.SexoMasculino;
            FechaNacimiento = usuario.FechaNacimiento;
            IdentificadorFotoPersonal = usuario.IdentificadorFotoPersonal;

            // Datos de acceso
            Username = usuario.Username;

            // Datos de contacto
            Email = usuario.Email;
            TelefonoFijo = usuario.TelefonoFijo;
            TelefonoCelular = usuario.TelefonoCelular;
            Facebook = usuario.Facebook;
            Twitter = usuario.Twitter;
            Instagram = usuario.Instagram;
            LinkedIn = usuario.LinkedIn;
        }

        public static List<ResultadoWS_MuniOnlineUsuario> ToList(IList<MuniOnlineUsuario> list)
        {
            return list.Select(x => new ResultadoWS_MuniOnlineUsuario(x)).ToList();
        }
    }
}