using System;
using System.Linq;
using _Model.Entities;

namespace _DAO.Maps
{
    class MuniOnlineUsuarioMap : BaseEntityMap<MuniOnlineUsuario>
    {
        public MuniOnlineUsuarioMap()
        {
            Table("MuniOnlineUsuario");

            Map(x => x.Nombre, "nombre");
            Map(x => x.Apellido, "apellido");
            Map(x => x.FechaNacimiento, "fechaNacimiento");
            Map(x => x.SexoMasculino, "sexoMasculino");
            Map(x => x.Dni, "dni");
            Map(x => x.Cuil, "cuil");
            Map(x => x.IdentificadorFotoPersonal, "identificadorFotoPersonal");
            Map(x => x.DomicilioLegalPais, "domicilioLegalPais");
            Map(x => x.DomicilioLegalFormateado, "domicilioLegalFormateado");

            //Datos de acceso
            Map(x => x.Username, "username");

            //Datos contacto
            Map(x => x.Email, "email");
            Map(x => x.TelefonoFijo, "telefonoFijo");
            Map(x => x.TelefonoCelular, "telefonoCelular");
            Map(x => x.Facebook, "facebook");
            Map(x => x.Twitter, "twitter");
            Map(x => x.LinkedIn, "linkedIn");
            Map(x => x.Instagram, "instagram");

            //Validaciones
            Map(x => x.FechaValidacionRenaper, "fechaValidacionRenaper");
            Map(x => x.FechaValidacionEmail, "fechaValidacionEmail");

        }
    }
}