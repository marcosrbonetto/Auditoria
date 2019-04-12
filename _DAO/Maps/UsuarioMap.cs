using System;
using System.Linq;
using _Model.Entities;

namespace _DAO.Maps
{
    class UsuarioMap : BaseEntityMap<Usuario>
    {
        public UsuarioMap()
        {
            Table("Usuario");

            Map(x => x.Nombre, "nombre");
            Map(x => x.Apellido, "apellido");
            Map(x => x.FechaNacimiento, "fechaNacimiento");
            Map(x => x.SexoMasculino, "sexoMasculino");
            Map(x => x.Dni, "dni");
            Map(x => x.DomicilioBarrio, "domicilioBarrio");
            Map(x => x.DomicilioCalle, "domicilioCalle");
            Map(x => x.DomicilioAltura, "domicilioAltura");
            Map(x => x.DomicilioPiso, "domicilioPiso");
            Map(x => x.DomicilioDepto, "domicilioDepto");
            Map(x => x.DomicilioCodigoPostal, "domicilioCodigoPostal");
            Map(x => x.DomicilioObservaciones, "domicilioObservaciones");
            Map(x => x.Observaciones, "observaciones");
            Map(x => x.Error, "error");

        }
    }
}