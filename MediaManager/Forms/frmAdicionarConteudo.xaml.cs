﻿using System;
using System.Windows;
using ConfigurableInputMessageBox;
using MediaManager.Helpers;
using MediaManager.Model;
using MediaManager.ViewModel;

namespace MediaManager.Forms
{
    /// <summary>
    /// Interaction logic for frmAdicionarSerie.xaml
    /// </summary>
    public partial class frmAdicionarConteudo : Window
    {
        public AdicionarConteudoViewModel AdicionarConteudoViewModel { get; set; }

        public bool IsEdicao { get; set; }

        public frmAdicionarConteudo(Enums.TipoConteudo tipoConteudo, bool bIsProcurarConteudo = false)
        {
            InitializeComponent();

            InputMessageBox inputMessageBox = new InputMessageBox(inputType.AdicionarConteudo);
            inputMessageBox.ShowDialog();

            if (inputMessageBox.DialogResult == true)
            {
                Video serie = new Serie();
                serie.nIdTipoConteudo = tipoConteudo;
                serie.sDsTitulo = inputMessageBox.InputViewModel.Properties.InputText;

                AdicionarConteudoViewModel = new AdicionarConteudoViewModel(serie, tipoConteudo);
                AdicionarConteudoViewModel.bProcurarConteudo = bIsProcurarConteudo;
                AdicionarConteudoViewModel.ActionClose = new Action<bool>((dialogResult) => { DialogResult = dialogResult; Close(); });
            }

            DataContext = AdicionarConteudoViewModel;
        }

        public frmAdicionarConteudo(Enums.TipoConteudo tipoConteudo, Video video, bool bIsProcurarConteudo = false)
        {
            InitializeComponent();

            AdicionarConteudoViewModel = new AdicionarConteudoViewModel(video, tipoConteudo);
            AdicionarConteudoViewModel.bProcurarConteudo = bIsProcurarConteudo;
            AdicionarConteudoViewModel.ActionClose = new Action<bool>((dialogResult) => { DialogResult = dialogResult; Close(); });

            DataContext = AdicionarConteudoViewModel;
        }

        private void btnPasta_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            dialog.SelectedPath = tbxPasta.Text;

            if ((bool)dialog.ShowDialog())
            {
                tbxPasta.Text = dialog.SelectedPath;
            }
        }

        private async void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            //if (AdicionarConteudoViewModel.oVideoSelecionado == null || AdicionarConteudoViewModel.oVideoSelecionado.sDsPasta == null)
            //{
            //    Helper.MostrarMensagem("Favor preencher todos os campos antes de salvar.", Enums.eTipoMensagem.Alerta);
            //    return;
            //}
            //else if (IsProcurarConteudo)
            //{
            //    DialogResult = true;
            //}
            //else
            //{
            //    switch (AdicionarConteudoViewModel.nIdTipoConteudo)
            //    {
            //        case Enums.TipoConteudo.Filme:
            //            //Filme filme = await Helper.API_GetFilmeInfoAsync(AdicionarConteudoViewModel.Video.Ids.slug);
            //            //filme.FolderPath = AdicionarConteudoViewModel.Video.FolderPath;

            //            //if (IsEdicao)
            //            //{
            //            //    filme.ID = AdicionarConteudoViewModel.Video.ID;
            //            //    try { await DatabaseHelper.UpdateFilmeAsync(filme); }
            //            //    catch (Exception ex)
            //            //    {
            //            //        Console.Write(ex.Message + " Detalhes: " + ex.InnerException);
            //            //        DialogResult = false;
            //            //    }
            //            //}
            //            //else
            //            //{
            //            //    try { await DatabaseHelper.AddFilmeAsync(filme); }
            //            //    catch (Exception ex)
            //            //    {
            //            //        Console.Write(ex.Message + " Detalhes: " + ex.InnerException);
            //            //        DialogResult = false;
            //            //    }
            //            //}
            //            break;

            //        case Enums.TipoConteudo.Série:
            //        case Enums.TipoConteudo.Anime:
            //            {
            //                Serie serie = null;
            //                DBHelper DBHelper = new DBHelper();

            //                if (AdicionarConteudoViewModel.oVideoSelecionado is Serie)
            //                {
            //                    serie = (Serie)AdicionarConteudoViewModel.oVideoSelecionado;
            //                }
            //                //else if (AdicionarConteudoViewModel.SelectedVideo is PosterGrid)
            //                //{
            //                //    serie = DBHelper.GetSeriePorID(AdicionarConteudoViewModel.SelectedVideo.IDBanco);
            //                //    serie.FolderPath = AdicionarConteudoViewModel.SelectedVideo.FolderPath;
            //                //}

            //                serie.lstSerieAlias = Helper.PopularCampoSerieAlias(serie);

            //                if (IsEdicao)
            //                {
            //                    try
            //                    {
            //                        await DBHelper.UpdateSerieAsync(serie);
            //                    }
            //                    catch (Exception ex)
            //                    {
            //                        Helper.TratarException(ex, "Ocorreu um erro ao atualizar a série " + serie.sDsTitulo);
            //                        DialogResult = false;
            //                    }
            //                }
            //                else
            //                {
            //                    try
            //                    {
            //                        await DBHelper.AddSerieAsync(serie);
            //                    }
            //                    catch (Exception ex)
            //                    {
            //                        Helper.TratarException(ex, "Ocorreu um erro ao incluir a série " + serie.sDsTitulo);
            //                        DialogResult = false;
            //                    }
            //                }
            //                break;
            //            }

            //        case Enums.TipoConteudo.AnimeFilmeSérie:
            //            {
            //                Serie anime = null;
            //                DBHelper DBHelper = new DBHelper();

            //                if (AdicionarConteudoViewModel.oVideoSelecionado is Serie)
            //                    anime = (Serie)AdicionarConteudoViewModel.oVideoSelecionado;
            //                //else if (AdicionarConteudoViewModel.SelectedVideo is PosterGrid)
            //                //{
            //                //    anime = DBHelper.GetSeriePorID(AdicionarConteudoViewModel.SelectedVideo.IDBanco);
            //                //    anime.FolderPath = AdicionarConteudoViewModel.SelectedVideo.FolderPath;
            //                //}

            //                if (IsEdicao)
            //                {
            //                    try { await DBHelper.UpdateSerieAsync(anime); }
            //                    catch (Exception ex)
            //                    {
            //                        Console.Write(ex.Message + " Detalhes: " + ex.InnerException);
            //                        DialogResult = false;
            //                    }
            //                }
            //                else
            //                {
            //                    try { await DBHelper.AddSerieAsync(anime); }
            //                    catch (Exception ex)
            //                    {
            //                        Console.Write(ex.Message + " Detalhes: " + ex.InnerException);
            //                        DialogResult = false;
            //                    }
            //                }
            //                break;
            //            }
            //        default:
            //            break;
            //    }
            //    DialogResult = true;
            //    Close();
            //}
        }

        public void ShowDialog(Window owner)
        {
            Owner = owner;
            ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (AdicionarConteudoViewModel.bFechar)
            {
                AdicionarConteudoViewModel.ActionClose(false);
            }
        }
    }
}