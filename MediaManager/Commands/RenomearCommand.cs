﻿using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using MediaManager.Helpers;
using MediaManager.ViewModel;

namespace MediaManager.Commands
{
    public class RenomearCommands
    {
        public class CommandRenomear : ICommand
        {
            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public bool CanExecute(object parameter)
            {
                return parameter is RenomearViewModel
                    && (parameter as RenomearViewModel).lstEpisodios != null
                    && (parameter as RenomearViewModel).lstEpisodios.Count > 0;
            }

            public void Execute(object parameter)
            {
                try
                {
                    RenomearViewModel renomearVM = (parameter as RenomearViewModel);

                    foreach (var item in renomearVM.lstEpisodios)
                    {
                        try
                        {
                            DBHelper DBHelper = new DBHelper();

                            if (item.bFlSelecionado)
                            {
                                Helper.LogMessage("O arquivo \"" + item.sDsFilepathOriginal + "\" será copiado para \"" + item.sDsFilepath + "\"");
                                Helper.LogMessage("Método de processamento: " + ((Enums.MetodoDeProcessamento)Properties.Settings.Default.pref_MetodoDeProcessamento).ToString());

                                // Adiciona o FilenameRenamed para quando houver pasta no nome (Ex. "Season 04\\Arrow - 4x05 - Haunted.mkv")
                                if (!Directory.Exists(Path.GetDirectoryName(item.sDsFilepath)))
                                {
                                    Directory.CreateDirectory(Path.GetDirectoryName(item.sDsFilepath));
                                    Helper.LogMessage("Diretório \"" + Path.GetDirectoryName(item.sDsFilepath) + "\" criado.");
                                }

                                if (File.Exists(item.sDsFilepath) && !renomearVM.bFlSilencioso)
                                {
                                    if (MessageBox.Show("O arquivo " + item.sDsFilepath + " já existe. Você deseja sobrescrevê-lo pelo arquivo \"" + item.sDsFilepathOriginal + "\"?", Properties.Settings.Default.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                                    {
                                        File.Delete(item.sDsFilepathOriginal);
                                        if (Helper.RealizarPosProcessamento(item))
                                        {
                                            item.bFlRenomeado = true;
                                            item.nIdEstadoEpisodio = Enums.EstadoEpisodio.Baixado;
                                            DBHelper.UpdateEpisodioRenomeado(item);
                                        }
                                        else
                                        {
                                            Helper.TratarException(new Exception("Código: " + Marshal.GetLastWin32Error() + "\r\nArquivo: " + item.sDsFilepathOriginal), "Ocorreu um erro ao criar o HardLink.", true);
                                        }
                                    }
                                }
                                else
                                {
                                    if (File.Exists(item.sDsFilepath))
                                        File.Delete(item.sDsFilepath);
                                    if (Helper.RealizarPosProcessamento(item))
                                    {
                                        item.bFlRenomeado = true;
                                        item.nIdEstadoEpisodio = Enums.EstadoEpisodio.Baixado;
                                        DBHelper.UpdateEpisodioRenomeado(item);
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Helper.TratarException(e, "Ocorreu um erro ao renomear o episódio \"" + item.sDsFilepathOriginal + "\"");
                        }
                    }
                    if (renomearVM.ActionFechar != null)
                    {
                        renomearVM.ActionFechar();
                    }
                }
                catch (Exception e)
                {
                    Helper.TratarException(e, "Ocorreu um erro ao renomear o episódio");
                }
            }
        }

        public class CommandSelecionar : ICommand
        {
            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public bool CanExecute(object parameter)
            {
                return parameter is RenomearViewModel;
            }

            public void Execute(object parameter)
            {
                var renomearVM = parameter as RenomearViewModel;
                int episodiosSelecionadosCount = renomearVM.lstEpisodios.Where(x => x.bFlSelecionado).Count();
                if (episodiosSelecionadosCount == renomearVM.lstEpisodios.Count && renomearVM.lstEpisodios.Count > 0)
                {
                    renomearVM.bFlSelecionarTodos = true;
                }
                else if (episodiosSelecionadosCount == 0)
                {
                    renomearVM.bFlSelecionarTodos = false;
                }
                else if (episodiosSelecionadosCount > 0)
                {
                    renomearVM.bFlSelecionarTodos = null;
                }
            }
        }

        public class CommandSelecionarTodos : ICommand
        {
            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public bool CanExecute(object parameter)
            {
                return parameter is RenomearViewModel;
            }

            public void Execute(object parameter)
            {
                var renomearVM = parameter as RenomearViewModel;
                if (renomearVM.bFlSelecionarTodos == true)
                {
                    renomearVM.lstEpisodios.ToList().ForEach(x => x.bFlSelecionado = true);
                }
                else
                {
                    renomearVM.bFlSelecionarTodos = false;
                    renomearVM.lstEpisodios.ToList().ForEach(x => x.bFlSelecionado = false);
                }
            }
        }
    }
}