﻿using System;
using System.Collections.Generic;
using System.Linq;
using MediaManager.Model;

namespace MediaManager.Services
{
    public class SerieAliasService : IRepositorio<SerieAlias>
    {
        private readonly IContext _context;

        public SerieAliasService(IContext context)
        {
            _context = context;
        }

        public bool Adicionar(params SerieAlias[] obj)
        {
            foreach (var alias in obj)
            {
                try
                {
                    _context.SerieAlias.Add(alias);
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                    new MediaManagerException(e).TratarException(
                        $"Ocorreu um erro ao adicionar o alias \"{alias.sDsAlias}\" ao banco.", true);
                    return false;
                }
            }
            return true;
        }

        public SerieAlias Get(int id)
        {
            try
            {
                SerieAlias oAlias = _context.SerieAlias.Where(x => x.nCdAlias == id).FirstOrDefault();
                return oAlias;
            }
            catch (Exception e)
            {
                new MediaManagerException(e).TratarException($"Ocorreu um erro ao retornar o alias com o id \"{id}\" ");
                return null;
            }
        }

        public List<SerieAlias> GetLista()
        {
            try
            {
                List<SerieAlias> lstAlias = _context.SerieAlias.ToList();
                return lstAlias != null ? lstAlias : new List<SerieAlias>();
            }
            catch (Exception e)
            {
                new MediaManagerException(e).TratarException("Ocorreu um erro ao retornar a lista de alias.");
                return new List<SerieAlias>();
            }
        }

        public bool Remover(params SerieAlias[] obj)
        {
            foreach (var alias in obj)
            {
                try
                {
                    _context.SerieAlias.Attach(alias);
                    _context.SerieAlias.Remove(alias);
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                    new MediaManagerException(e).TratarException(
                        $"Ocorreu um erro ao remover o alias \"{alias.sDsAlias}\" do banco.", true);
                    return false;
                }
            }
            return true;
        }

        public bool Update(params SerieAlias[] obj)
        {
            foreach (var oAlias in obj)
            {
                try
                {
                    var oAliasDB = _context.SerieAlias.Find(oAlias.nCdAlias);
                    _context.Entry(oAliasDB).CurrentValues.SetValues(oAlias);
                }
                catch (Exception e)
                {
                    new MediaManagerException(e).TratarException(
                        $"Ocorreu um erro ao atualizar o alias \"{oAlias.sDsAlias}\"");
                    return false;
                }
            }
            _context.SaveChanges();
            return true;
        }

        public bool Adicionar(params Video[] videos)
        {
            foreach (var video in videos)
            {
                try
                {
                    foreach (var item in video.sAliases.Split('|'))
                    {
                        SerieAlias alias = new SerieAlias(item);
                        alias.nNrEpisodio = 1;
                        alias.nNrTemporada = 1;
                        alias.nCdVideo = video.nCdVideo;
                        _context.SerieAlias.Add(alias);
                    }
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                    new MediaManagerException(e).TratarException(
                        $"Ocorreu um erro ao adicionar o alias padrão do video \"{video.sDsTitulo}\" ao banco.", true);
                    return false;
                }
            }
            return true;
        }

        public List<SerieAlias> GetLista(Video video)
        {
            try
            {
                List<SerieAlias> lstAlias = _context.SerieAlias.Where(x => x.nCdVideo == video.nCdVideo).ToList();
                return lstAlias != null ? lstAlias : new List<SerieAlias>();
            }
            catch (Exception e)
            {
                new MediaManagerException(e).TratarException(
                    $"Ocorreu um erro ao retornar a lista de alias de \"{video.sDsTitulo}\"");
                return new List<SerieAlias>();
            }
        }
    }
}