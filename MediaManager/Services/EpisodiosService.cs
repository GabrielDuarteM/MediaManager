﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using MediaManager.Helpers;
using MediaManager.Model;

namespace MediaManager.Services
{
    public class EpisodiosService : IService<Episodio>
    {
        private IContext _context;

        public EpisodiosService(IContext context)
        {
            _context = context;
        }

        public bool Adicionar(params Episodio[] obj)
        {
            try
            {
                foreach (var episodio in obj)
                {
                    try
                    {
                        if (episodio.nCdEpisodio > 0)
                        {
                            _context.Episodio.Add(episodio);
                        }
                        else
                        {
                            Serie serie = App.Container.Resolve<SeriesService>().GetPorCodigoApi(episodio.nCdVideoAPI);
                            episodio.nCdVideo = serie.nCdVideo;
                            _context.Episodio.Add(episodio);
                        }
                    }
                    catch (Exception e) { Helper.TratarException(e, "Ocorreu um erro ao adicionar o episódio com o ID " + episodio.nCdEpisodioAPI + " ao banco.", true); return false; }
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Helper.TratarException(e, "Ocorreu um erro ao adicionar episódios.");
                return false;
            }
        }

        public bool Adicionar(params Serie[] obj)
        {
            try
            {
                foreach (var serie in obj)
                {
                    try
                    {
                        serie.lstEpisodios.ForEach(episodio =>
                        {
                            episodio.nCdVideo = serie.nCdVideo;
                            Adicionar(episodio);
                        });
                    }
                    catch (Exception e)
                    {
                        Helper.TratarException(e, "Ocorreu um erro ao salvar os episódios de " + serie.sDsTitulo);
                        return false;
                    }
                }

                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Helper.TratarException(e, "Ocorreu um erro ao adicionar episódios.");
                return false;
            }
        }

        public Episodio Get(int ID)
        {
            try
            {
                Episodio episodio = _context.Episodio
                        .Where(x => x.nCdEpisodio == ID)
                        .FirstOrDefault();
                return episodio;
            }
            catch (Exception e)
            {
                Helper.TratarException(e, "Ocorreu um erro ao retornar o episódio com o código " + ID);
                return null;
            }
        }

        public Episodio Get(int nCdVideo, int nNrTemporada, int nNrEpisodio)
        {
            try
            {
                Episodio episodio = _context.Episodio
                        .Where(x => x.nCdVideo == nCdVideo && x.nNrTemporada == nNrTemporada && x.nNrEpisodio == nNrEpisodio)
                        .FirstOrDefault();
                return episodio;
            }
            catch (Exception e)
            {
                Helper.TratarException(e, "Ocorreu um erro ao retornar o episódio " + nNrEpisodio + " da temporada " + nNrTemporada + " do vídeo de ID " + nCdVideo);
                return null;
            }
        }

        public Episodio Get(int nCdVideo, int nNrAbsoluto)
        {
            Episodio episodio = _context.Episodio
                .Where(x => x.nCdVideo == nCdVideo && x.nNrAbsoluto == nNrAbsoluto)
                .FirstOrDefault();
            return episodio;
        }

        public Episodio GetPorCodigoAPI(int nCdEpisodioAPI)
        {
            Episodio episodio = _context.Episodio
                .Where(x => x.nCdEpisodioAPI == nCdEpisodioAPI)
                .FirstOrDefault();
            return episodio;
        }

        public List<Episodio> GetLista()
        {
            List<Episodio> lstEpisodios = _context.Episodio.ToList();
            return lstEpisodios;
        }

        public List<Episodio> GetLista(Video serie)
        {
            List<Episodio> lstEpisodios = _context.Episodio.Where(x => x.nCdVideo == serie.nCdVideo).ToList();
            return lstEpisodios;
        }

        public bool Update(params Episodio[] obj)
        {
            Episodio original = null;

            try
            {
                foreach (var episodio in obj)
                {
                    try
                    {
                        original = _context.Episodio.Find(episodio.nCdEpisodio);

                        if (original != null)
                        {
                            original.nNrAbsoluto = episodio.nNrAbsoluto;
                            original.sLkArtwork = episodio.sLkArtwork;
                            original.sDsEpisodio = episodio.sDsEpisodio;
                            original.nNrEpisodio = episodio.nNrEpisodio;
                            original.tDtEstreia = episodio.tDtEstreia;
                            original.nCdTemporadaAPI = episodio.nCdTemporadaAPI;
                            original.nCdVideoAPI = episodio.nCdVideoAPI;
                            original.nCdEpisodioAPI = episodio.nCdEpisodioAPI;
                            original.sDsIdioma = episodio.sDsIdioma;
                            original.sNrUltimaAtualizacao = episodio.sNrUltimaAtualizacao;
                            original.sDsSinopse = episodio.sDsSinopse;
                            original.dNrAvaliacao = episodio.dNrAvaliacao;
                            original.nNrTemporada = episodio.nNrTemporada;
                        }
                        else return false;
                    }
                    catch (Exception e) { Helper.TratarException(e, "Ocorreu um erro ao atualizar o episódio de ID " + episodio.nCdEpisodio); return false; }
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                string frase = "Ocorreu um erro ao atualizar os episódios ";

                foreach (var item in obj)
                {
                    frase += "\"" + item.sDsFilepath + "\", ";
                }

                Helper.TratarException(e, frase.Remove(frase.Length - 3));
                return false;
            }
        }

        public bool UpdateEpisodioRenomeado(Episodio atualizado)
        {
            try
            {
                Episodio original;
                original = _context.Episodio.Find(atualizado.nCdEpisodio);
                if (original != null)
                {
                    original.sDsFilepathOriginal = atualizado.sDsFilepathOriginal;
                    original.sDsFilepath = atualizado.sDsFilepath;
                    original.bFlRenomeado = atualizado.bFlRenomeado;
                    original.nIdEstadoEpisodio = atualizado.nIdEstadoEpisodio;

                    foreach (var nNrEpisodio in atualizado.lstIntEpisodios)
                    {
                        if (nNrEpisodio == atualizado.nNrEpisodio)
                        {
                            continue;
                        }
                        var episodio = Get(atualizado.nCdVideo, atualizado.nNrTemporada, nNrEpisodio);
                        var episodioDB = _context.Episodio.Find(episodio.nCdEpisodio);
                        episodioDB.sDsFilepathOriginal = atualizado.sDsFilepathOriginal;
                        episodioDB.sDsFilepath = atualizado.sDsFilepath;
                        episodioDB.bFlRenomeado = atualizado.bFlRenomeado;
                        episodioDB.nIdEstadoEpisodio = atualizado.nIdEstadoEpisodio;
                    }

                    _context.SaveChanges();
                    return true;
                }
                else return false;
            }
            catch (Exception e) { Helper.TratarException(e, "Ocorreu um erro ao atualizar o episódio de ID " + atualizado.nCdEpisodioAPI + " no banco.", true); return false; }
        }

        public bool VerificarSeEpisodioJaFoiRenomeado(Episodio episodio)
        {
            try
            {
                var episodios = from episodiosDB in _context.Episodio
                                where episodiosDB.nCdEpisodio == episodio.nCdEpisodio && episodiosDB.bFlRenomeado
                                select episodiosDB;
                return episodios.Count() > 0 ? true : false;
            }
            catch (Exception e)
            {
                Helper.TratarException(e, "Ocorreu um erro ao verificar se o episódio \"" + episodio.sDsFilepath + "\" ja foi renomeado.");
                return false;
            }
        }

        public bool VerificaEpisodiosNoDiretorio(Video serie)
        {
            try
            {
                if (Directory.Exists(serie.sDsPasta))
                {
                    var arquivos = new DirectoryInfo(serie.sDsPasta).EnumerateFiles("*.*", SearchOption.AllDirectories);
                    string[] extensoesPermitidas = Properties.Settings.Default.ExtensoesRenomeioPermitidas.Split('|');

                    foreach (var item in arquivos)
                    {
                        if (extensoesPermitidas.Contains(item.Extension))
                        {
                            Episodio episodio = new Episodio();
                            episodio.sDsFilepath = item.FullName;
                            episodio.oSerie = (Serie)serie;

                            if (episodio.IdentificarEpisodio())
                            {
                                episodio.sDsFilepath = item.FullName;
                                episodio.bFlRenomeado = (episodio.sDsFilepath == Path.Combine(serie.sDsPasta, Helper.RenomearConformePreferencias(episodio)) + item.Extension);

                                Episodio episodeDB = Get(serie.nCdVideo, episodio.nNrTemporada, episodio.nNrEpisodio);
                                episodeDB = _context.Episodio.Find(episodeDB.nCdEpisodio);
                                episodeDB.sDsFilepath = episodio.sDsFilepath;
                                episodeDB.bFlRenomeado = episodio.bFlRenomeado;
                                episodeDB.nIdEstadoEpisodio = Enums.EstadoEpisodio.Baixado;

                                foreach (var nNrEpisodio in episodio.lstIntEpisodios)
                                {
                                    if (nNrEpisodio == episodeDB.nNrEpisodio)
                                    {
                                        continue;
                                    }
                                    var episodioAgregado = Get(episodeDB.nCdVideo, episodeDB.nNrTemporada, nNrEpisodio);
                                    var episodioAgregadoDB = _context.Episodio.Find(episodioAgregado.nCdEpisodio);

                                    episodioAgregadoDB.sDsFilepath = episodeDB.sDsFilepath;
                                    episodioAgregadoDB.bFlRenomeado = episodeDB.bFlRenomeado;
                                    episodioAgregadoDB.nIdEstadoEpisodio = episodeDB.nIdEstadoEpisodio;
                                }

                                _context.SaveChanges();
                            }
                        }
                    }
                }
            }
            catch (Exception e) { Helper.TratarException(e, "Ocorreu um erro ao verificar os episódios no diretório da série \"" + serie.sDsTitulo + "\".", true); return false; }
            return true;
        }

        public bool Remover(params Episodio[] obj)
        {
            foreach (var episodio in obj)
            {
                try
                {
                    _context.Episodio.Remove(episodio);
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                    Helper.TratarException(e, "Ocorreu um erro ao remover o episódio \"" + episodio.sDsFilepath + "\"");
                    return false;
                }
            }
            return true;
        }
    }
}