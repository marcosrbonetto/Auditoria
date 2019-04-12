using System;
using System.Linq;
using _Model;
using _Model.Entities;
using NHibernate;
using System.Collections.Generic;

namespace _DAO.DAO
{
    public class DAO_MuniOnlineUsuario : BaseDAO<MuniOnlineUsuario>
    {
        private static DAO_MuniOnlineUsuario instance;

        public static DAO_MuniOnlineUsuario Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DAO_MuniOnlineUsuario();
                }
                return instance;
            }
        }

        public IQueryOver<MuniOnlineUsuario, MuniOnlineUsuario> GetQuery(_Model.Consultas.Consulta_MuniOnlineUsuario consulta)
        {
            var query = GetSession().QueryOver<MuniOnlineUsuario>();

            //Dni
            if (consulta.Dni.HasValue)
            {
                query.Where(x => x.Dni == consulta.Dni.Value);
            }

            //Seox masculino
            if (consulta.SexoMasculino.HasValue)
            {
                query.Where(x => x.SexoMasculino == consulta.SexoMasculino.Value);
            }

            //Validado renpaer
            if (consulta.ValidadoRenaper.HasValue)
            {
                if (consulta.ValidadoRenaper.Value)
                {
                    query.Where(x => x.FechaValidacionRenaper != null);
                }
                else
                {
                    query.Where(x => x.FechaValidacionRenaper == null);
                }
            }

            //Dados de baja
            if (consulta.DadosDeBaja.HasValue)
            {
                if (consulta.DadosDeBaja.Value)
                {
                    query.Where(x => x.FechaBaja != null);
                }
                else
                {
                    query.Where(x => x.FechaBaja == null);
                }
            }

            return query;
        }

        public Resultado<List<MuniOnlineUsuario>> Get(_Model.Consultas.Consulta_MuniOnlineUsuario consulta)
        {
            var resultado = new Resultado<List<MuniOnlineUsuario>>();

            try
            {
                var query = GetQuery(consulta);
                resultado.Return = query.List().ToList();
            }
            catch (Exception e)
            {
                resultado.SetError(e);
            }
            return resultado;

        }

        public Resultado<int> GetCantidad(_Model.Consultas.Consulta_MuniOnlineUsuario consulta)
        {
            var resultado = new Resultado<int>();

            try
            {
                var query = GetQuery(consulta);
                resultado.Return = query.RowCount();
            }
            catch (Exception e)
            {
                resultado.SetError(e);
            }
            return resultado;

        }
    }
}