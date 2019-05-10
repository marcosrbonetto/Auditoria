using System;
using System.Linq;
using _Model;
using _Model.Entities;
using NHibernate;
using System.Collections.Generic;
using _Model.Resultados;
using NHibernate.Criterion.Lambda;
using NHibernate.Criterion;

namespace _DAO.DAO
{
    public class DAO_Inscripcion : BaseDAO<Inscripcion>
    {
        private static DAO_Inscripcion instance;

        public static DAO_Inscripcion Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DAO_Inscripcion();
                }
                return instance;
            }
        }


        Dictionary<string, bool> joins = new Dictionary<string, bool>();
        Usuario joinUsuario;
        TipoInscripcion joinTipoInscripcion;
        TipoAuto joinTipoAuto;


        private void ClearJoins()
        {
            joinUsuario = null;
            joinTipoInscripcion = null;
            joinTipoAuto = null;
            joins = new Dictionary<string, bool>();
        }

        private void JoinUsuario(IQueryOver<Inscripcion, Inscripcion> query)
        {
            if (!joins.ContainsKey("usuario"))
            {
                query.Left.JoinAlias(x => x.Usuario, () => joinUsuario);
                joins["usuario"] = true;
            }
        }

        private void JoinTipoInscripcion(IQueryOver<Inscripcion, Inscripcion> query)
        {
            if (!joins.ContainsKey("tipoInscripcion"))
            {
                query.Left.JoinAlias(x => x.TipoInscripcion, () => joinTipoInscripcion);
                joins["tipoInscripcion"] = true;
            }
        }

        private void JoinTipoAuto(IQueryOver<Inscripcion, Inscripcion> query)
        {
            if (!joins.ContainsKey("tipoAuto"))
            {
                query.Left.JoinAlias(x => x.TipoAuto, () => joinTipoAuto);
                joins["tipoAuto"] = true;
            }
        }

        public IQueryOver<Inscripcion, Inscripcion> GetQuery(_Model.Consultas.Consulta_Inscripcion consulta)
        {
            ClearJoins();

            var query = GetSession().QueryOver<Inscripcion>();

            //DNI
            if (consulta.Dni.HasValue)
            {
                JoinUsuario(query);
                query.Where(() => joinUsuario.Dni == consulta.Dni.Value);
            }

            //IdUsuario
            if (consulta.IdUsuario.HasValue)
            {
                query.Where(x => x.Usuario.Id == consulta.IdUsuario.Value);
            }

            //Nombre
            if (!string.IsNullOrEmpty(consulta.Nombre))
            {
                JoinUsuario(query);
                foreach (var palabra in consulta.Nombre.Split(' '))
                {
                    var p = palabra.Trim();
                    query.Where(() => (joinUsuario.Nombre != null && joinUsuario.Nombre.IsLike(p, MatchMode.Anywhere)) || joinUsuario.Apellido != null && joinUsuario.Apellido.IsLike(p, MatchMode.Anywhere));
                }
            }

            //Identificador
            if (!string.IsNullOrEmpty(consulta.Identificador))
            {
                foreach (var palabra in consulta.Identificador.Split(' '))
                {
                    var p = palabra.Trim();
                    query.Where(x => x.Identificador.IsLike(p, MatchMode.Anywhere));
                }
            }

            //Tipo auto
            if (consulta.TipoAuto.HasValue)
            {
                JoinTipoAuto(query);
                query = query.Where(x => joinTipoAuto.KeyValue == consulta.TipoAuto.Value);
            }

            //Tipo inscripcion
            if (consulta.TipoInscripcion.HasValue)
            {
                JoinTipoInscripcion(query);
                query = query.Where(x => joinTipoInscripcion.KeyValue == consulta.TipoInscripcion.Value);
            }

            //Con fecha inicio
            if (consulta.ConFechaInicio.HasValue)
            {
                if (consulta.ConFechaInicio.Value)
                {
                    query.Where(x => x.FechaInicio != null);
                }
                else
                {
                    query.Where(x => x.FechaInicio == null);
                }
            }

            //Activo hasta x fecha (Con fecha fin superior a X fecha)
            if (consulta.ActivoHasta.HasValue)
            {
                query.Where(x => x.FechaFin > consulta.ActivoHasta.Value);
            }

            //Con error
            if (consulta.ConError.HasValue)
            {
                if (consulta.ConError.Value)
                {
                    query.Where(x => x.Error != null && x.Error != "");
                }
                else
                {
                    query.Where(x => x.Error == null || x.Error == "");
                }
            }

            //favorito
            if (consulta.Favorito.HasValue)
            {
                if (consulta.Favorito.Value)
                {
                    query.Where(x => x.Favorito == true);
                }
                else
                {
                    query.Where(x => x.Favorito == false);
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

        public Resultado<List<Inscripcion>> Get(_Model.Consultas.Consulta_Inscripcion consulta)
        {
            var resultado = new Resultado<List<Inscripcion>>();

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

        public Resultado<Resultado_Paginador<Inscripcion>> GetPaginado(_Model.Consultas.Consulta_InscripcionPaginada consulta)
        {
            var resultado = new Resultado<Resultado_Paginador<Inscripcion>>();

            try
            {
                int tamaño = consulta.TamañoPagina;
                if (tamaño == 0)
                {
                    resultado.Error = "Tamaño de página inválido";
                    return resultado;
                }

                if (!Enum.IsDefined(typeof(_Model.Enums.InscripcionOrderBy), consulta.OrderBy))
                {
                    resultado.Error = "Debe enviar el ordenamiento deseado";
                    return resultado;
                }

                int pagina = consulta.Pagina;

                int count = GetQuery(consulta).RowCount();
                int cantidadPaginas = (int)Math.Ceiling((double)count / consulta.TamañoPagina);

                if (count != 0)
                {
                    if (pagina > cantidadPaginas || pagina < 0)
                    {
                        resultado.Error = "Página indicada inválida";
                        return resultado;
                    }
                }

                var query = GetQuery(consulta);

                switch (consulta.OrderBy)
                {
                    case Enums.InscripcionOrderBy.Identificador:
                        {
                            //JoinTipoInscripcion(query);
                            //JoinTipoAuto(query);
                            //JoinUsuario(query);
                            if (consulta.OrderByAsc)
                            {
                                query = query
                                    .OrderBy(x => x.Identificador).Asc
                                    //.ThenBy(() => joinUsuario.Apellido).Asc
                                    //.ThenBy(() => joinUsuario.Nombre).Asc
                                    //.ThenBy(() => joinTipoInscripcion.Nombre).Asc
                                    //.ThenBy(() => joinTipoAuto.Nombre).Asc
                                    //.ThenBy((x) => x.FechaInicio).Asc
                                    //.ThenBy((x) => x.FechaFin).Asc
                                    .ThenBy(x => x.Id).Asc;

                            }
                            else
                            {
                                query = query
                                    .OrderBy(x => x.Identificador).Desc
                                    //.ThenBy(() => joinUsuario.Apellido).Desc
                                    //.ThenBy(() => joinUsuario.Nombre).Desc
                                    //.ThenBy(() => joinTipoInscripcion.Nombre).Desc
                                    //.ThenBy(() => joinTipoAuto.Nombre).Desc
                                    //.ThenBy((x) => x.FechaInicio).Desc
                                    //.ThenBy((x) => x.FechaFin).Desc
                                    .ThenBy(x => x.Id).Desc;

                            }

                        } break;

                    case Enums.InscripcionOrderBy.TipoInscripcion:
                        {
                            JoinTipoInscripcion(query);
                            if (consulta.OrderByAsc)
                            {
                                query = query
                                    .OrderBy(() => joinTipoInscripcion.Nombre).Asc
                                    .ThenBy(x => x.Id).Asc;
                            }
                            else
                            {
                                query = query
                                    .OrderBy(() => joinTipoInscripcion.Nombre).Desc
                                    .ThenBy(x => x.Id).Desc;
                            }

                        } break;

                    case Enums.InscripcionOrderBy.TipoAuto:
                        {
                            JoinTipoAuto(query);
                            if (consulta.OrderByAsc)
                            {
                                query = query
                                    .OrderBy(() => joinTipoAuto.Nombre).Asc
                                    .ThenBy(x => x.Id).Asc;
                            }
                            else
                            {
                                query = query
                                    .OrderBy(() => joinTipoAuto.Nombre).Desc
                                    .ThenBy(x => x.Id).Desc;
                            }
                        } break;
                    case Enums.InscripcionOrderBy.FechaInicio:
                        {
                            if (consulta.OrderByAsc)
                            {
                                query = query
                                    .OrderBy((x) => x.FechaInicio).Asc
                                    .ThenBy(x => x.Id).Asc;

                            }
                            else
                            {
                                query = query
                                    .OrderBy((x) => x.FechaInicio).Desc
                                    .ThenBy(x => x.Id).Desc;
                            }
                        } break;
                    case Enums.InscripcionOrderBy.FechaFin:
                        {
                            if (consulta.OrderByAsc)
                            {
                                query = query
                                    .OrderBy((x) => x.FechaFin).Asc
                                    .ThenBy(x => x.Id).Asc;
                            }
                            else
                            {
                                query = query
                                    .OrderBy((x) => x.FechaFin).Desc
                                    .ThenBy(x => x.Id).Desc;
                            }
                        } break;
                    case Enums.InscripcionOrderBy.UsuarioApellidoNombre:
                        {
                            JoinUsuario(query);
                            if (consulta.OrderByAsc)
                            {
                                query = query
                                    .OrderBy(() => joinUsuario.Apellido).Asc
                                    .ThenBy(() => joinUsuario.Nombre).Asc
                                    .ThenBy(x => x.Id).Asc;
                            }
                            else
                            {
                                query = query
                                    .OrderBy(() => joinUsuario.Apellido).Desc
                                    .ThenBy(() => joinUsuario.Nombre).Desc
                                    .ThenBy(x => x.Id).Desc;
                            }

                        } break;
                }


                var items = query.Take(tamaño).Skip(pagina * tamaño).List().ToList();

                resultado.Return = new Resultado_Paginador<Inscripcion>();
                resultado.Return.Count = count;
                resultado.Return.Data = items;
                resultado.Return.PaginaActual = consulta.Pagina;
                resultado.Return.TamañoPagina = consulta.TamañoPagina;
                resultado.Return.CantidadPaginas = cantidadPaginas;

                return resultado;
            }
            catch (Exception e)
            {
                resultado.SetError(e);
            }
            return resultado;

        }

        public Resultado<int> GetCantidad(_Model.Consultas.Consulta_Inscripcion consulta)
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

        #region calcular errores

        public Resultado<bool> GenerarErroresInscripcion()
        {
            var resultado = new Resultado<bool>();

            try
            {
                using (var s = SessionManager.Instance.SessionFactory.OpenSession())
                {
                    s.SetBatchSize(1000);

                    var usuarios = s.QueryOver<Inscripcion>()
                        .List()
                        .ToList();

                    using (var t = s.BeginTransaction())
                    {
                        usuarios.ForEach((entity) =>
                        {
                            try
                            {
                                //DateTime fechaAlta = entity.FechaAlta;
                                //if (fechaAlta.Day == 7 && fechaAlta.Month == 5 && fechaAlta.Year == 2019)
                                //{
                                List<string> errores = new List<string>();

                                //Usuario
                                //Es error cuando no tiene usuario o cuando su usuario tiene error
                                bool conUsuario = entity.Usuario != null;
                                bool conUsuarioConError = entity.Usuario != null && entity.Usuario.Error != null;
                                if (!conUsuario || conUsuarioConError)
                                {
                                    if (!conUsuario)
                                    {
                                        errores.Add("Sin usuario");
                                    }
                                    else
                                    {
                                        errores.Add("Usuario con error: " + entity.Usuario.Error);
                                    }
                                }

                                //Tipo auto
                                //Cuando no tiene tipo de auto
                                bool conTipoAuto = entity.TipoAuto != null;
                                if (!conTipoAuto)
                                {
                                    errores.Add("Sin tipo de auto");
                                }

                                //Identificador
                                //Cuando no tiene identificador
                                bool conIdentificador = entity.Identificador != null && entity.Identificador.Trim() != "";
                                if (!conIdentificador)
                                {
                                    errores.Add("Sin identificador");
                                }

                                if (errores.Count != 0)
                                {
                                    entity.Error = string.Join(" - ", errores);
                                }
                                else
                                {
                                    entity.Error = null;
                                }
                                s.Update(entity);
                                //}
                            }
                            catch (Exception ex)
                            {
                                var e = ex;
                            }
                        });

                        t.Commit();
                    }
                }

            }
            catch (Exception e)
            {
                resultado.SetError(e);
            }
            return resultado;
        }

        public Resultado<bool> GenerarErroresUsuario()
        {
            var resultado = new Resultado<bool>();

            try
            {
                using (var s = SessionManager.Instance.SessionFactory.OpenSession())
                {
                    s.SetBatchSize(1000);

                    var usuarios = s.QueryOver<Usuario>()
                        .List()
                        .ToList();

                    using (var t = s.BeginTransaction())
                    {
                        usuarios.ForEach((entity) =>
                        {
                            try
                            {

                                //DateTime fechaAlta = entity.FechaAlta;
                                //if (fechaAlta.Day == 7 && fechaAlta.Month == 5 && fechaAlta.Year == 2019)
                                //{
                                List<string> errores = new List<string>();

                                //Nombre
                                bool conNombre = entity.Nombre != null && entity.Nombre.Trim() != "";
                                bool conApellido = entity.Apellido != null && entity.Apellido.Trim() != "";
                                if (!conNombre && !conApellido)
                                {
                                    errores.Add("El nombre y/o apellido es requerido");
                                }


                                //Sexo
                                if (!entity.SexoMasculino.HasValue)
                                {
                                    errores.Add("El campo sexo es requerido");
                                }

                                //DNI
                                bool conDni = entity.Dni.HasValue;
                                bool dniValido = conDni && entity.Dni.Value > 0 && entity.Dni.Value < 200000000;
                                if (!dniValido)
                                {
                                    errores.Add("El campo N° de DNI es inválido");
                                }

                                if (errores.Count != 0)
                                {
                                    entity.Error = string.Join(" - ", errores);
                                }
                                else
                                {
                                    entity.Error = null;
                                }
                                s.Update(entity);

                            }
                            catch (Exception ex)
                            {
                                var e = ex;
                            }
                        });

                        t.Commit();
                    }
                }

            }
            catch (Exception e)
            {
                resultado.SetError(e);
            }
            return resultado;
        }

        //public Resultado<bool> GenerarErrorInhabilitacion()
        //{
        //    var resultado = new Resultado<bool>();

        //    try
        //    {
        //        using (var s = SessionManager.Instance.SessionFactory.OpenSession())
        //        {
        //            s.SetBatchSize(1000);

        //            var usuarios = s.QueryOver<Inhabilitacion>()
        //                .List()
        //                .ToList();

        //            using (var t = s.BeginTransaction())
        //            {
        //                usuarios.ForEach((entity) =>
        //                {
        //                    try
        //                    {
        //                        List<string> errores = new List<string>();

        //                        //Usuario
        //                        //Es error cuando no tiene usuario o cuando su usuario tiene error
        //                        bool conUsuario = entity.Usuario != null;
        //                        bool conUsuarioConError = entity.Usuario != null && entity.Usuario.Error != null;
        //                        if (!conUsuario || conUsuarioConError)
        //                        {
        //                            if (!conUsuario)
        //                            {
        //                                errores.Add("Sin usuario");
        //                            }
        //                            else
        //                            {
        //                                errores.Add("Usuario con error: " + entity.Usuario.Error);
        //                            }
        //                        }

        //                        //Tipo
        //                        //Cuando no tiene
        //                        //Cuando tiene pero es invalido
        //                        //Cuando tiene pero es permanente y no tiene fecha fin
        //                        bool conTipo= entity.TipoInhabilitacion!= null;
        //                        if (!conTipo)
        //                        {
        //                            errores.Add("Sin tipo inhabilitacion");
        //                        }
        //                        else
        //                        {
        //                            bool esValido = !entity.TipoInhabilitacion.Invalido;
        //                            if (!esValido)
        //                            {
        //                                errores.Add("Tipo inhabilitacion inválido");
        //                            }
        //                            else
        //                            {
        //                                //No es permanente y no tiene fecha fin
        //                                if (!entity.TipoInhabilitacion.Permanente && !entity.FechaFin.HasValue)
        //                                {
        //                                    errores.Add("Inhabilitacion no permanente y sin fecha de fin");
        //                                }
        //                            }
        //                        }



        //                        if (errores.Count != 0)
        //                        {
        //                            entity.Error = string.Join(" - ", errores);
        //                        }
        //                        else
        //                        {
        //                            entity.Error = null;
        //                        }
        //                        s.Update(entity);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        var e = ex;
        //                    }
        //                });

        //                t.Commit();
        //            }
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        resultado.SetError(e);
        //    }
        //    return resultado;
        //}-

        //public Resultado<bool> CorregirFechaInicio()
        //{
        //    var resultado = new Resultado<bool>();


        //    try
        //    {
        //        using (var s = SessionManager.Instance.SessionFactory.OpenSession())
        //        {
        //            s.SetBatchSize(1000);

        //            var inscripciones = s.QueryOver<Inhabilitacion>()
        //                .Where(x => x.FechaInicioString != null)
        //                .List()
        //                .ToList();

        //            using (var t = s.BeginTransaction())
        //            {
        //                inscripciones.ForEach((entity) =>
        //                {
        //                    try
        //                    {
        //                        var fechaString = entity.FechaInicioString.Trim();
        //                        var partes = fechaString.Split('/');

        //                        var año = int.Parse(partes[2]);
        //                        //if (año <= 19)
        //                        //{
        //                        //    año += 2000;
        //                        //}
        //                        //else
        //                        //{
        //                        //    año += 1900;
        //                        //}

        //                        //int mes = -1;
        //                        //var mesString = partes[1].ToLower();
        //                        //if (mesString.Contains("ene")) mes = 1;
        //                        //if (mesString.Contains("feb")) mes = 2;
        //                        //if (mesString.Contains("mar")) mes = 3;
        //                        //if (mesString.Contains("abr")) mes = 4;
        //                        //if (mesString.Contains("may")) mes = 5;
        //                        //if (mesString.Contains("jun")) mes = 6;
        //                        //if (mesString.Contains("jul")) mes = 7;
        //                        //if (mesString.Contains("ago")) mes = 8;
        //                        //if (mesString.Contains("sep")) mes = 9;
        //                        //if (mesString.Contains("oct")) mes = 10;
        //                        //if (mesString.Contains("nov")) mes = 11;
        //                        //if (mesString.Contains("dic")) mes = 12;
        //                        int mes = int.Parse(partes[1].Trim().ToLower());

        //                        int dia = int.Parse(partes[0]);
        //                        if (mes <= 12 && año > 1900)
        //                        {
        //                            var fecha = new DateTime(año, mes, dia);
        //                            entity.FechaInicio = fecha;
        //                            entity.FechaInicioString = null;
        //                            s.Update(entity);
        //                        }

        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        var e = ex;
        //                    }
        //                });

        //                t.Commit();
        //            }
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        resultado.SetError(e);
        //    }
        //    return resultado;
        //}

        #endregion
    }
}