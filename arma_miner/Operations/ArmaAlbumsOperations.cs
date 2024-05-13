using arma_miner.Data;
using arma_miner.Models;
using Microsoft.Data.SqlClient;
using SharpCompress.Readers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace arma_miner.Operations
{
    public class ArmaAlbumsOperations : IArmaAlbumsOperations
    {
        private List<MBArtistSimpleDataItem> allArtists = new List<MBArtistSimpleDataItem>();
        private DataTable tbl_a_albumsMapper = new DataTable();
        private DataTable tbl_b_albumsMapper = new DataTable();
        private DataTable tbl_c_albumsMapper = new DataTable();
        private DataTable tbl_d_albumsMapper = new DataTable();
        private DataTable tbl_e_albumsMapper = new DataTable();
        private DataTable tbl_f_albumsMapper = new DataTable();
        private DataTable tbl_g_albumsMapper = new DataTable();
        private DataTable tbl_h_albumsMapper = new DataTable();
        private DataTable tbl_i_albumsMapper = new DataTable();
        private DataTable tbl_j_albumsMapper = new DataTable();
        private DataTable tbl_k_albumsMapper = new DataTable();
        private DataTable tbl_l_albumsMapper = new DataTable();
        private DataTable tbl_m_albumsMapper = new DataTable();
        private DataTable tbl_n_albumsMapper = new DataTable();
        private DataTable tbl_num_albumsMapper = new DataTable();
        private DataTable tbl_o_albumsMapper = new DataTable();
        private DataTable tbl_p_albumsMapper = new DataTable();
        private DataTable tbl_q_albumsMapper = new DataTable();
        private DataTable tbl_r_albumsMapper = new DataTable();
        private DataTable tbl_s_albumsMapper = new DataTable();
        private DataTable tbl_symb_albumsMapper = new DataTable();
        private DataTable tbl_t_albumsMapper = new DataTable();
        private DataTable tbl_u_albumsMapper = new DataTable();
        private DataTable tbl_v_albumsMapper = new DataTable();
        private DataTable tbl_w_albumsMapper = new DataTable();
        private DataTable tbl_x_albumsMapper = new DataTable();
        private DataTable tbl_y_albumsMapper = new DataTable();
        private DataTable tbl_z_albumsMapper = new DataTable();

        private DataTable tbl_a_songsMapper = new DataTable();
        private DataTable tbl_b_songsMapper = new DataTable();
        private DataTable tbl_c_songsMapper = new DataTable();
        private DataTable tbl_d_songsMapper = new DataTable();
        private DataTable tbl_e_songsMapper = new DataTable();
        private DataTable tbl_f_songsMapper = new DataTable();
        private DataTable tbl_g_songsMapper = new DataTable();
        private DataTable tbl_h_songsMapper = new DataTable();
        private DataTable tbl_i_songsMapper = new DataTable();
        private DataTable tbl_j_songsMapper = new DataTable();
        private DataTable tbl_k_songsMapper = new DataTable();
        private DataTable tbl_l_songsMapper = new DataTable();
        private DataTable tbl_m_songsMapper = new DataTable();
        private DataTable tbl_n_songsMapper = new DataTable();
        private DataTable tbl_num_songsMapper = new DataTable();
        private DataTable tbl_o_songsMapper = new DataTable();
        private DataTable tbl_p_songsMapper = new DataTable();
        private DataTable tbl_q_songsMapper = new DataTable();
        private DataTable tbl_r_songsMapper = new DataTable();
        private DataTable tbl_s_songsMapper = new DataTable();
        private DataTable tbl_symb_songsMapper = new DataTable();
        private DataTable tbl_t_songsMapper = new DataTable();
        private DataTable tbl_u_songsMapper = new DataTable();
        private DataTable tbl_v_songsMapper = new DataTable();
        private DataTable tbl_w_songsMapper = new DataTable();
        private DataTable tbl_x_songsMapper = new DataTable();
        private DataTable tbl_y_songsMapper = new DataTable();
        private DataTable tbl_z_songsMapper = new DataTable();

        private readonly bool IsLinux = false;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IDapperHelper _dapper;
        public ArmaAlbumsOperations(
            IHostEnvironment hostEnvironment,
            IDapperHelper dapper
        )
        {
            _hostEnvironment = hostEnvironment;
            _dapper = dapper;

            IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

            createTablesForFirstTimeParsing();
        }

        private void createTablesForFirstTimeParsing()
        {
            tbl_a_albumsMapper.Columns.Add("MBAlbumId", typeof(string));
            tbl_a_albumsMapper.Columns.Add("ArtistId", typeof(string));
            tbl_a_albumsMapper.Columns.Add("MBArtistId", typeof(string));
            tbl_a_albumsMapper.Columns.Add("Title", typeof(string));
            tbl_a_albumsMapper.Columns.Add("TitleFlat", typeof(string));
            tbl_a_albumsMapper.Columns.Add("Status", typeof(string));
            tbl_a_albumsMapper.Columns.Add("PrimaryType", typeof(string));
            tbl_a_albumsMapper.Columns.Add("FirstReleaseDate", typeof(string));
            tbl_a_albumsMapper.Columns.Add("AddedDateTime", typeof(DateTime));

            tbl_b_albumsMapper.Columns.Add("MBAlbumId", typeof(string));
            tbl_b_albumsMapper.Columns.Add("ArtistId", typeof(string));
            tbl_b_albumsMapper.Columns.Add("MBArtistId", typeof(string));
            tbl_b_albumsMapper.Columns.Add("Title", typeof(string));
            tbl_b_albumsMapper.Columns.Add("TitleFlat", typeof(string));
            tbl_b_albumsMapper.Columns.Add("Status", typeof(string));
            tbl_b_albumsMapper.Columns.Add("PrimaryType", typeof(string));
            tbl_b_albumsMapper.Columns.Add("FirstReleaseDate", typeof(string));
            tbl_b_albumsMapper.Columns.Add("AddedDateTime", typeof(DateTime));

            tbl_c_albumsMapper.Columns.Add("MBAlbumId", typeof(string));
            tbl_c_albumsMapper.Columns.Add("ArtistId", typeof(string));
            tbl_c_albumsMapper.Columns.Add("MBArtistId", typeof(string));
            tbl_c_albumsMapper.Columns.Add("Title", typeof(string));
            tbl_c_albumsMapper.Columns.Add("TitleFlat", typeof(string));
            tbl_c_albumsMapper.Columns.Add("Status", typeof(string));
            tbl_c_albumsMapper.Columns.Add("PrimaryType", typeof(string));
            tbl_c_albumsMapper.Columns.Add("FirstReleaseDate", typeof(string));
            tbl_c_albumsMapper.Columns.Add("AddedDateTime", typeof(DateTime));

            tbl_d_albumsMapper.Columns.Add("MBAlbumId", typeof(string));
            tbl_d_albumsMapper.Columns.Add("ArtistId", typeof(string));
            tbl_d_albumsMapper.Columns.Add("MBArtistId", typeof(string));
            tbl_d_albumsMapper.Columns.Add("Title", typeof(string));
            tbl_d_albumsMapper.Columns.Add("TitleFlat", typeof(string));
            tbl_d_albumsMapper.Columns.Add("Status", typeof(string));
            tbl_d_albumsMapper.Columns.Add("PrimaryType", typeof(string));
            tbl_d_albumsMapper.Columns.Add("FirstReleaseDate", typeof(string));
            tbl_d_albumsMapper.Columns.Add("AddedDateTime", typeof(DateTime));

            tbl_e_albumsMapper.Columns.Add("MBAlbumId", typeof(string));
            tbl_e_albumsMapper.Columns.Add("ArtistId", typeof(string));
            tbl_e_albumsMapper.Columns.Add("MBArtistId", typeof(string));
            tbl_e_albumsMapper.Columns.Add("Title", typeof(string));
            tbl_e_albumsMapper.Columns.Add("TitleFlat", typeof(string));
            tbl_e_albumsMapper.Columns.Add("Status", typeof(string));
            tbl_e_albumsMapper.Columns.Add("PrimaryType", typeof(string));
            tbl_e_albumsMapper.Columns.Add("FirstReleaseDate", typeof(string));
            tbl_e_albumsMapper.Columns.Add("AddedDateTime", typeof(DateTime));

            tbl_f_albumsMapper.Columns.Add("MBAlbumId", typeof(string));
            tbl_f_albumsMapper.Columns.Add("ArtistId", typeof(string));
            tbl_f_albumsMapper.Columns.Add("MBArtistId", typeof(string));
            tbl_f_albumsMapper.Columns.Add("Title", typeof(string));
            tbl_f_albumsMapper.Columns.Add("TitleFlat", typeof(string));
            tbl_f_albumsMapper.Columns.Add("Status", typeof(string));
            tbl_f_albumsMapper.Columns.Add("PrimaryType", typeof(string));
            tbl_f_albumsMapper.Columns.Add("FirstReleaseDate", typeof(string));
            tbl_f_albumsMapper.Columns.Add("AddedDateTime", typeof(DateTime));

            tbl_g_albumsMapper.Columns.Add("MBAlbumId", typeof(string));
            tbl_g_albumsMapper.Columns.Add("ArtistId", typeof(string));
            tbl_g_albumsMapper.Columns.Add("MBArtistId", typeof(string));
            tbl_g_albumsMapper.Columns.Add("Title", typeof(string));
            tbl_g_albumsMapper.Columns.Add("TitleFlat", typeof(string));
            tbl_g_albumsMapper.Columns.Add("Status", typeof(string));
            tbl_g_albumsMapper.Columns.Add("PrimaryType", typeof(string));
            tbl_g_albumsMapper.Columns.Add("FirstReleaseDate", typeof(string));
            tbl_g_albumsMapper.Columns.Add("AddedDateTime", typeof(DateTime));

            tbl_h_albumsMapper.Columns.Add("MBAlbumId", typeof(string));
            tbl_h_albumsMapper.Columns.Add("ArtistId", typeof(string));
            tbl_h_albumsMapper.Columns.Add("MBArtistId", typeof(string));
            tbl_h_albumsMapper.Columns.Add("Title", typeof(string));
            tbl_h_albumsMapper.Columns.Add("TitleFlat", typeof(string));
            tbl_h_albumsMapper.Columns.Add("Status", typeof(string));
            tbl_h_albumsMapper.Columns.Add("PrimaryType", typeof(string));
            tbl_h_albumsMapper.Columns.Add("FirstReleaseDate", typeof(string));
            tbl_h_albumsMapper.Columns.Add("AddedDateTime", typeof(DateTime));

            tbl_i_albumsMapper.Columns.Add("MBAlbumId", typeof(string));
            tbl_i_albumsMapper.Columns.Add("ArtistId", typeof(string));
            tbl_i_albumsMapper.Columns.Add("MBArtistId", typeof(string));
            tbl_i_albumsMapper.Columns.Add("Title", typeof(string));
            tbl_i_albumsMapper.Columns.Add("TitleFlat", typeof(string));
            tbl_i_albumsMapper.Columns.Add("Status", typeof(string));
            tbl_i_albumsMapper.Columns.Add("PrimaryType", typeof(string));
            tbl_i_albumsMapper.Columns.Add("FirstReleaseDate", typeof(string));
            tbl_i_albumsMapper.Columns.Add("AddedDateTime", typeof(DateTime));

            tbl_j_albumsMapper.Columns.Add("MBAlbumId", typeof(string));
            tbl_j_albumsMapper.Columns.Add("ArtistId", typeof(string));
            tbl_j_albumsMapper.Columns.Add("MBArtistId", typeof(string));
            tbl_j_albumsMapper.Columns.Add("Title", typeof(string));
            tbl_j_albumsMapper.Columns.Add("TitleFlat", typeof(string));
            tbl_j_albumsMapper.Columns.Add("Status", typeof(string));
            tbl_j_albumsMapper.Columns.Add("PrimaryType", typeof(string));
            tbl_j_albumsMapper.Columns.Add("FirstReleaseDate", typeof(string));
            tbl_j_albumsMapper.Columns.Add("AddedDateTime", typeof(DateTime));

            tbl_k_albumsMapper.Columns.Add("MBAlbumId", typeof(string));
            tbl_k_albumsMapper.Columns.Add("ArtistId", typeof(string));
            tbl_k_albumsMapper.Columns.Add("MBArtistId", typeof(string));
            tbl_k_albumsMapper.Columns.Add("Title", typeof(string));
            tbl_k_albumsMapper.Columns.Add("TitleFlat", typeof(string));
            tbl_k_albumsMapper.Columns.Add("Status", typeof(string));
            tbl_k_albumsMapper.Columns.Add("PrimaryType", typeof(string));
            tbl_k_albumsMapper.Columns.Add("FirstReleaseDate", typeof(string));
            tbl_k_albumsMapper.Columns.Add("AddedDateTime", typeof(DateTime));

            tbl_l_albumsMapper.Columns.Add("MBAlbumId", typeof(string));
            tbl_l_albumsMapper.Columns.Add("ArtistId", typeof(string));
            tbl_l_albumsMapper.Columns.Add("MBArtistId", typeof(string));
            tbl_l_albumsMapper.Columns.Add("Title", typeof(string));
            tbl_l_albumsMapper.Columns.Add("TitleFlat", typeof(string));
            tbl_l_albumsMapper.Columns.Add("Status", typeof(string));
            tbl_l_albumsMapper.Columns.Add("PrimaryType", typeof(string));
            tbl_l_albumsMapper.Columns.Add("FirstReleaseDate", typeof(string));
            tbl_l_albumsMapper.Columns.Add("AddedDateTime", typeof(DateTime));

            tbl_m_albumsMapper.Columns.Add("MBAlbumId", typeof(string));
            tbl_m_albumsMapper.Columns.Add("ArtistId", typeof(string));
            tbl_m_albumsMapper.Columns.Add("MBArtistId", typeof(string));
            tbl_m_albumsMapper.Columns.Add("Title", typeof(string));
            tbl_m_albumsMapper.Columns.Add("TitleFlat", typeof(string));
            tbl_m_albumsMapper.Columns.Add("Status", typeof(string));
            tbl_m_albumsMapper.Columns.Add("PrimaryType", typeof(string));
            tbl_m_albumsMapper.Columns.Add("FirstReleaseDate", typeof(string));
            tbl_m_albumsMapper.Columns.Add("AddedDateTime", typeof(DateTime));

            tbl_n_albumsMapper.Columns.Add("MBAlbumId", typeof(string));
            tbl_n_albumsMapper.Columns.Add("ArtistId", typeof(string));
            tbl_n_albumsMapper.Columns.Add("MBArtistId", typeof(string));
            tbl_n_albumsMapper.Columns.Add("Title", typeof(string));
            tbl_n_albumsMapper.Columns.Add("TitleFlat", typeof(string));
            tbl_n_albumsMapper.Columns.Add("Status", typeof(string));
            tbl_n_albumsMapper.Columns.Add("PrimaryType", typeof(string));
            tbl_n_albumsMapper.Columns.Add("FirstReleaseDate", typeof(string));
            tbl_n_albumsMapper.Columns.Add("AddedDateTime", typeof(DateTime));

            tbl_num_albumsMapper.Columns.Add("MBAlbumId", typeof(string));
            tbl_num_albumsMapper.Columns.Add("ArtistId", typeof(string));
            tbl_num_albumsMapper.Columns.Add("MBArtistId", typeof(string));
            tbl_num_albumsMapper.Columns.Add("Title", typeof(string));
            tbl_num_albumsMapper.Columns.Add("TitleFlat", typeof(string));
            tbl_num_albumsMapper.Columns.Add("Status", typeof(string));
            tbl_num_albumsMapper.Columns.Add("PrimaryType", typeof(string));
            tbl_num_albumsMapper.Columns.Add("FirstReleaseDate", typeof(string));
            tbl_num_albumsMapper.Columns.Add("AddedDateTime", typeof(DateTime));

            tbl_o_albumsMapper.Columns.Add("MBAlbumId", typeof(string));
            tbl_o_albumsMapper.Columns.Add("ArtistId", typeof(string));
            tbl_o_albumsMapper.Columns.Add("MBArtistId", typeof(string));
            tbl_o_albumsMapper.Columns.Add("Title", typeof(string));
            tbl_o_albumsMapper.Columns.Add("TitleFlat", typeof(string));
            tbl_o_albumsMapper.Columns.Add("Status", typeof(string));
            tbl_o_albumsMapper.Columns.Add("PrimaryType", typeof(string));
            tbl_o_albumsMapper.Columns.Add("FirstReleaseDate", typeof(string));
            tbl_o_albumsMapper.Columns.Add("AddedDateTime", typeof(DateTime));

            tbl_p_albumsMapper.Columns.Add("MBAlbumId", typeof(string));
            tbl_p_albumsMapper.Columns.Add("ArtistId", typeof(string));
            tbl_p_albumsMapper.Columns.Add("MBArtistId", typeof(string));
            tbl_p_albumsMapper.Columns.Add("Title", typeof(string));
            tbl_p_albumsMapper.Columns.Add("TitleFlat", typeof(string));
            tbl_p_albumsMapper.Columns.Add("Status", typeof(string));
            tbl_p_albumsMapper.Columns.Add("PrimaryType", typeof(string));
            tbl_p_albumsMapper.Columns.Add("FirstReleaseDate", typeof(string));
            tbl_p_albumsMapper.Columns.Add("AddedDateTime", typeof(DateTime));

            tbl_q_albumsMapper.Columns.Add("MBAlbumId", typeof(string));
            tbl_q_albumsMapper.Columns.Add("ArtistId", typeof(string));
            tbl_q_albumsMapper.Columns.Add("MBArtistId", typeof(string));
            tbl_q_albumsMapper.Columns.Add("Title", typeof(string));
            tbl_q_albumsMapper.Columns.Add("TitleFlat", typeof(string));
            tbl_q_albumsMapper.Columns.Add("Status", typeof(string));
            tbl_q_albumsMapper.Columns.Add("PrimaryType", typeof(string));
            tbl_q_albumsMapper.Columns.Add("FirstReleaseDate", typeof(string));
            tbl_q_albumsMapper.Columns.Add("AddedDateTime", typeof(DateTime));

            tbl_r_albumsMapper.Columns.Add("MBAlbumId", typeof(string));
            tbl_r_albumsMapper.Columns.Add("ArtistId", typeof(string));
            tbl_r_albumsMapper.Columns.Add("MBArtistId", typeof(string));
            tbl_r_albumsMapper.Columns.Add("Title", typeof(string));
            tbl_r_albumsMapper.Columns.Add("TitleFlat", typeof(string));
            tbl_r_albumsMapper.Columns.Add("Status", typeof(string));
            tbl_r_albumsMapper.Columns.Add("PrimaryType", typeof(string));
            tbl_r_albumsMapper.Columns.Add("FirstReleaseDate", typeof(string));
            tbl_r_albumsMapper.Columns.Add("AddedDateTime", typeof(DateTime));

            tbl_s_albumsMapper.Columns.Add("MBAlbumId", typeof(string));
            tbl_s_albumsMapper.Columns.Add("ArtistId", typeof(string));
            tbl_s_albumsMapper.Columns.Add("MBArtistId", typeof(string));
            tbl_s_albumsMapper.Columns.Add("Title", typeof(string));
            tbl_s_albumsMapper.Columns.Add("TitleFlat", typeof(string));
            tbl_s_albumsMapper.Columns.Add("Status", typeof(string));
            tbl_s_albumsMapper.Columns.Add("PrimaryType", typeof(string));
            tbl_s_albumsMapper.Columns.Add("FirstReleaseDate", typeof(string));
            tbl_s_albumsMapper.Columns.Add("AddedDateTime", typeof(DateTime));

            tbl_symb_albumsMapper.Columns.Add("MBAlbumId", typeof(string));
            tbl_symb_albumsMapper.Columns.Add("ArtistId", typeof(string));
            tbl_symb_albumsMapper.Columns.Add("MBArtistId", typeof(string));
            tbl_symb_albumsMapper.Columns.Add("Title", typeof(string));
            tbl_symb_albumsMapper.Columns.Add("TitleFlat", typeof(string));
            tbl_symb_albumsMapper.Columns.Add("Status", typeof(string));
            tbl_symb_albumsMapper.Columns.Add("PrimaryType", typeof(string));
            tbl_symb_albumsMapper.Columns.Add("FirstReleaseDate", typeof(string));
            tbl_symb_albumsMapper.Columns.Add("AddedDateTime", typeof(DateTime));

            tbl_t_albumsMapper.Columns.Add("MBAlbumId", typeof(string));
            tbl_t_albumsMapper.Columns.Add("ArtistId", typeof(string));
            tbl_t_albumsMapper.Columns.Add("MBArtistId", typeof(string));
            tbl_t_albumsMapper.Columns.Add("Title", typeof(string));
            tbl_t_albumsMapper.Columns.Add("TitleFlat", typeof(string));
            tbl_t_albumsMapper.Columns.Add("Status", typeof(string));
            tbl_t_albumsMapper.Columns.Add("PrimaryType", typeof(string));
            tbl_t_albumsMapper.Columns.Add("FirstReleaseDate", typeof(string));
            tbl_t_albumsMapper.Columns.Add("AddedDateTime", typeof(DateTime));

            tbl_u_albumsMapper.Columns.Add("MBAlbumId", typeof(string));
            tbl_u_albumsMapper.Columns.Add("ArtistId", typeof(string));
            tbl_u_albumsMapper.Columns.Add("MBArtistId", typeof(string));
            tbl_u_albumsMapper.Columns.Add("Title", typeof(string));
            tbl_u_albumsMapper.Columns.Add("TitleFlat", typeof(string));
            tbl_u_albumsMapper.Columns.Add("Status", typeof(string));
            tbl_u_albumsMapper.Columns.Add("PrimaryType", typeof(string));
            tbl_u_albumsMapper.Columns.Add("FirstReleaseDate", typeof(string));
            tbl_u_albumsMapper.Columns.Add("AddedDateTime", typeof(DateTime));

            tbl_v_albumsMapper.Columns.Add("MBAlbumId", typeof(string));
            tbl_v_albumsMapper.Columns.Add("ArtistId", typeof(string));
            tbl_v_albumsMapper.Columns.Add("MBArtistId", typeof(string));
            tbl_v_albumsMapper.Columns.Add("Title", typeof(string));
            tbl_v_albumsMapper.Columns.Add("TitleFlat", typeof(string));
            tbl_v_albumsMapper.Columns.Add("Status", typeof(string));
            tbl_v_albumsMapper.Columns.Add("PrimaryType", typeof(string));
            tbl_v_albumsMapper.Columns.Add("FirstReleaseDate", typeof(string));
            tbl_v_albumsMapper.Columns.Add("AddedDateTime", typeof(DateTime));

            tbl_w_albumsMapper.Columns.Add("MBAlbumId", typeof(string));
            tbl_w_albumsMapper.Columns.Add("ArtistId", typeof(string));
            tbl_w_albumsMapper.Columns.Add("MBArtistId", typeof(string));
            tbl_w_albumsMapper.Columns.Add("Title", typeof(string));
            tbl_w_albumsMapper.Columns.Add("TitleFlat", typeof(string));
            tbl_w_albumsMapper.Columns.Add("Status", typeof(string));
            tbl_w_albumsMapper.Columns.Add("PrimaryType", typeof(string));
            tbl_w_albumsMapper.Columns.Add("FirstReleaseDate", typeof(string));
            tbl_w_albumsMapper.Columns.Add("AddedDateTime", typeof(DateTime));

            tbl_x_albumsMapper.Columns.Add("MBAlbumId", typeof(string));
            tbl_x_albumsMapper.Columns.Add("ArtistId", typeof(string));
            tbl_x_albumsMapper.Columns.Add("MBArtistId", typeof(string));
            tbl_x_albumsMapper.Columns.Add("Title", typeof(string));
            tbl_x_albumsMapper.Columns.Add("TitleFlat", typeof(string));
            tbl_x_albumsMapper.Columns.Add("Status", typeof(string));
            tbl_x_albumsMapper.Columns.Add("PrimaryType", typeof(string));
            tbl_x_albumsMapper.Columns.Add("FirstReleaseDate", typeof(string));
            tbl_x_albumsMapper.Columns.Add("AddedDateTime", typeof(DateTime));

            tbl_y_albumsMapper.Columns.Add("MBAlbumId", typeof(string));
            tbl_y_albumsMapper.Columns.Add("ArtistId", typeof(string));
            tbl_y_albumsMapper.Columns.Add("MBArtistId", typeof(string));
            tbl_y_albumsMapper.Columns.Add("Title", typeof(string));
            tbl_y_albumsMapper.Columns.Add("TitleFlat", typeof(string));
            tbl_y_albumsMapper.Columns.Add("Status", typeof(string));
            tbl_y_albumsMapper.Columns.Add("PrimaryType", typeof(string));
            tbl_y_albumsMapper.Columns.Add("FirstReleaseDate", typeof(string));
            tbl_y_albumsMapper.Columns.Add("AddedDateTime", typeof(DateTime));

            tbl_z_albumsMapper.Columns.Add("MBAlbumId", typeof(string));
            tbl_z_albumsMapper.Columns.Add("ArtistId", typeof(string));
            tbl_z_albumsMapper.Columns.Add("MBArtistId", typeof(string));
            tbl_z_albumsMapper.Columns.Add("Title", typeof(string));
            tbl_z_albumsMapper.Columns.Add("TitleFlat", typeof(string));
            tbl_z_albumsMapper.Columns.Add("Status", typeof(string));
            tbl_z_albumsMapper.Columns.Add("PrimaryType", typeof(string));
            tbl_z_albumsMapper.Columns.Add("FirstReleaseDate", typeof(string));
            tbl_z_albumsMapper.Columns.Add("AddedDateTime", typeof(DateTime));

            // ================ songs ==============================

            tbl_a_songsMapper.Columns.Add("MBSongId", typeof(string));
            tbl_a_songsMapper.Columns.Add("AlbumId", typeof(string));
            tbl_a_songsMapper.Columns.Add("CDNumber", typeof(string));
            tbl_a_songsMapper.Columns.Add("SongNumber", typeof(string));
            tbl_a_songsMapper.Columns.Add("SongTitle", typeof(string));
            tbl_a_songsMapper.Columns.Add("SongTitleFlat", typeof(string));
            tbl_a_songsMapper.Columns.Add("AddedDateTime", typeof(DateTime));
            tbl_a_songsMapper.Columns.Add("MBAlbumId", typeof(string));

            tbl_b_songsMapper.Columns.Add("MBSongId", typeof(string));
            tbl_b_songsMapper.Columns.Add("AlbumId", typeof(string));
            tbl_b_songsMapper.Columns.Add("CDNumber", typeof(string));
            tbl_b_songsMapper.Columns.Add("SongNumber", typeof(string));
            tbl_b_songsMapper.Columns.Add("SongTitle", typeof(string));
            tbl_b_songsMapper.Columns.Add("SongTitleFlat", typeof(string));
            tbl_b_songsMapper.Columns.Add("AddedDateTime", typeof(DateTime));
            tbl_b_songsMapper.Columns.Add("MBAlbumId", typeof(string));

            tbl_c_songsMapper.Columns.Add("MBSongId", typeof(string));
            tbl_c_songsMapper.Columns.Add("AlbumId", typeof(string));
            tbl_c_songsMapper.Columns.Add("CDNumber", typeof(string));
            tbl_c_songsMapper.Columns.Add("SongNumber", typeof(string));
            tbl_c_songsMapper.Columns.Add("SongTitle", typeof(string));
            tbl_c_songsMapper.Columns.Add("SongTitleFlat", typeof(string));
            tbl_c_songsMapper.Columns.Add("AddedDateTime", typeof(DateTime));
            tbl_c_songsMapper.Columns.Add("MBAlbumId", typeof(string));

            tbl_d_songsMapper.Columns.Add("MBSongId", typeof(string));
            tbl_d_songsMapper.Columns.Add("AlbumId", typeof(string));
            tbl_d_songsMapper.Columns.Add("CDNumber", typeof(string));
            tbl_d_songsMapper.Columns.Add("SongNumber", typeof(string));
            tbl_d_songsMapper.Columns.Add("SongTitle", typeof(string));
            tbl_d_songsMapper.Columns.Add("SongTitleFlat", typeof(string));
            tbl_d_songsMapper.Columns.Add("AddedDateTime", typeof(DateTime));
            tbl_d_songsMapper.Columns.Add("MBAlbumId", typeof(string));

            tbl_e_songsMapper.Columns.Add("MBSongId", typeof(string));
            tbl_e_songsMapper.Columns.Add("AlbumId", typeof(string));
            tbl_e_songsMapper.Columns.Add("CDNumber", typeof(string));
            tbl_e_songsMapper.Columns.Add("SongNumber", typeof(string));
            tbl_e_songsMapper.Columns.Add("SongTitle", typeof(string));
            tbl_e_songsMapper.Columns.Add("SongTitleFlat", typeof(string));
            tbl_e_songsMapper.Columns.Add("AddedDateTime", typeof(DateTime));
            tbl_e_songsMapper.Columns.Add("MBAlbumId", typeof(string));

            tbl_f_songsMapper.Columns.Add("MBSongId", typeof(string));
            tbl_f_songsMapper.Columns.Add("AlbumId", typeof(string));
            tbl_f_songsMapper.Columns.Add("CDNumber", typeof(string));
            tbl_f_songsMapper.Columns.Add("SongNumber", typeof(string));
            tbl_f_songsMapper.Columns.Add("SongTitle", typeof(string));
            tbl_f_songsMapper.Columns.Add("SongTitleFlat", typeof(string));
            tbl_f_songsMapper.Columns.Add("AddedDateTime", typeof(DateTime));
            tbl_f_songsMapper.Columns.Add("MBAlbumId", typeof(string));

            tbl_g_songsMapper.Columns.Add("MBSongId", typeof(string));
            tbl_g_songsMapper.Columns.Add("AlbumId", typeof(string));
            tbl_g_songsMapper.Columns.Add("CDNumber", typeof(string));
            tbl_g_songsMapper.Columns.Add("SongNumber", typeof(string));
            tbl_g_songsMapper.Columns.Add("SongTitle", typeof(string));
            tbl_g_songsMapper.Columns.Add("SongTitleFlat", typeof(string));
            tbl_g_songsMapper.Columns.Add("AddedDateTime", typeof(DateTime));
            tbl_g_songsMapper.Columns.Add("MBAlbumId", typeof(string));

            tbl_h_songsMapper.Columns.Add("MBSongId", typeof(string));
            tbl_h_songsMapper.Columns.Add("AlbumId", typeof(string));
            tbl_h_songsMapper.Columns.Add("CDNumber", typeof(string));
            tbl_h_songsMapper.Columns.Add("SongNumber", typeof(string));
            tbl_h_songsMapper.Columns.Add("SongTitle", typeof(string));
            tbl_h_songsMapper.Columns.Add("SongTitleFlat", typeof(string));
            tbl_h_songsMapper.Columns.Add("AddedDateTime", typeof(DateTime));
            tbl_h_songsMapper.Columns.Add("MBAlbumId", typeof(string));

            tbl_i_songsMapper.Columns.Add("MBSongId", typeof(string));
            tbl_i_songsMapper.Columns.Add("AlbumId", typeof(string));
            tbl_i_songsMapper.Columns.Add("CDNumber", typeof(string));
            tbl_i_songsMapper.Columns.Add("SongNumber", typeof(string));
            tbl_i_songsMapper.Columns.Add("SongTitle", typeof(string));
            tbl_i_songsMapper.Columns.Add("SongTitleFlat", typeof(string));
            tbl_i_songsMapper.Columns.Add("AddedDateTime", typeof(DateTime));
            tbl_i_songsMapper.Columns.Add("MBAlbumId", typeof(string));

            tbl_j_songsMapper.Columns.Add("MBSongId", typeof(string));
            tbl_j_songsMapper.Columns.Add("AlbumId", typeof(string));
            tbl_j_songsMapper.Columns.Add("CDNumber", typeof(string));
            tbl_j_songsMapper.Columns.Add("SongNumber", typeof(string));
            tbl_j_songsMapper.Columns.Add("SongTitle", typeof(string));
            tbl_j_songsMapper.Columns.Add("SongTitleFlat", typeof(string));
            tbl_j_songsMapper.Columns.Add("AddedDateTime", typeof(DateTime));
            tbl_j_songsMapper.Columns.Add("MBAlbumId", typeof(string));

            tbl_k_songsMapper.Columns.Add("MBSongId", typeof(string));
            tbl_k_songsMapper.Columns.Add("AlbumId", typeof(string));
            tbl_k_songsMapper.Columns.Add("CDNumber", typeof(string));
            tbl_k_songsMapper.Columns.Add("SongNumber", typeof(string));
            tbl_k_songsMapper.Columns.Add("SongTitle", typeof(string));
            tbl_k_songsMapper.Columns.Add("SongTitleFlat", typeof(string));
            tbl_k_songsMapper.Columns.Add("AddedDateTime", typeof(DateTime));
            tbl_k_songsMapper.Columns.Add("MBAlbumId", typeof(string));

            tbl_l_songsMapper.Columns.Add("MBSongId", typeof(string));
            tbl_l_songsMapper.Columns.Add("AlbumId", typeof(string));
            tbl_l_songsMapper.Columns.Add("CDNumber", typeof(string));
            tbl_l_songsMapper.Columns.Add("SongNumber", typeof(string));
            tbl_l_songsMapper.Columns.Add("SongTitle", typeof(string));
            tbl_l_songsMapper.Columns.Add("SongTitleFlat", typeof(string));
            tbl_l_songsMapper.Columns.Add("AddedDateTime", typeof(DateTime));
            tbl_l_songsMapper.Columns.Add("MBAlbumId", typeof(string));

            tbl_m_songsMapper.Columns.Add("MBSongId", typeof(string));
            tbl_m_songsMapper.Columns.Add("AlbumId", typeof(string));
            tbl_m_songsMapper.Columns.Add("CDNumber", typeof(string));
            tbl_m_songsMapper.Columns.Add("SongNumber", typeof(string));
            tbl_m_songsMapper.Columns.Add("SongTitle", typeof(string));
            tbl_m_songsMapper.Columns.Add("SongTitleFlat", typeof(string));
            tbl_m_songsMapper.Columns.Add("AddedDateTime", typeof(DateTime));
            tbl_m_songsMapper.Columns.Add("MBAlbumId", typeof(string));

            tbl_n_songsMapper.Columns.Add("MBSongId", typeof(string));
            tbl_n_songsMapper.Columns.Add("AlbumId", typeof(string));
            tbl_n_songsMapper.Columns.Add("CDNumber", typeof(string));
            tbl_n_songsMapper.Columns.Add("SongNumber", typeof(string));
            tbl_n_songsMapper.Columns.Add("SongTitle", typeof(string));
            tbl_n_songsMapper.Columns.Add("SongTitleFlat", typeof(string));
            tbl_n_songsMapper.Columns.Add("AddedDateTime", typeof(DateTime));
            tbl_n_songsMapper.Columns.Add("MBAlbumId", typeof(string));

            tbl_num_songsMapper.Columns.Add("MBSongId", typeof(string));
            tbl_num_songsMapper.Columns.Add("AlbumId", typeof(string));
            tbl_num_songsMapper.Columns.Add("CDNumber", typeof(string));
            tbl_num_songsMapper.Columns.Add("SongNumber", typeof(string));
            tbl_num_songsMapper.Columns.Add("SongTitle", typeof(string));
            tbl_num_songsMapper.Columns.Add("SongTitleFlat", typeof(string));
            tbl_num_songsMapper.Columns.Add("AddedDateTime", typeof(DateTime));
            tbl_num_songsMapper.Columns.Add("MBAlbumId", typeof(string));

            tbl_o_songsMapper.Columns.Add("MBSongId", typeof(string));
            tbl_o_songsMapper.Columns.Add("AlbumId", typeof(string));
            tbl_o_songsMapper.Columns.Add("CDNumber", typeof(string));
            tbl_o_songsMapper.Columns.Add("SongNumber", typeof(string));
            tbl_o_songsMapper.Columns.Add("SongTitle", typeof(string));
            tbl_o_songsMapper.Columns.Add("SongTitleFlat", typeof(string));
            tbl_o_songsMapper.Columns.Add("AddedDateTime", typeof(DateTime));
            tbl_o_songsMapper.Columns.Add("MBAlbumId", typeof(string));

            tbl_p_songsMapper.Columns.Add("MBSongId", typeof(string));
            tbl_p_songsMapper.Columns.Add("AlbumId", typeof(string));
            tbl_p_songsMapper.Columns.Add("CDNumber", typeof(string));
            tbl_p_songsMapper.Columns.Add("SongNumber", typeof(string));
            tbl_p_songsMapper.Columns.Add("SongTitle", typeof(string));
            tbl_p_songsMapper.Columns.Add("SongTitleFlat", typeof(string));
            tbl_p_songsMapper.Columns.Add("AddedDateTime", typeof(DateTime));
            tbl_p_songsMapper.Columns.Add("MBAlbumId", typeof(string));

            tbl_q_songsMapper.Columns.Add("MBSongId", typeof(string));
            tbl_q_songsMapper.Columns.Add("AlbumId", typeof(string));
            tbl_q_songsMapper.Columns.Add("CDNumber", typeof(string));
            tbl_q_songsMapper.Columns.Add("SongNumber", typeof(string));
            tbl_q_songsMapper.Columns.Add("SongTitle", typeof(string));
            tbl_q_songsMapper.Columns.Add("SongTitleFlat", typeof(string));
            tbl_q_songsMapper.Columns.Add("AddedDateTime", typeof(DateTime));
            tbl_q_songsMapper.Columns.Add("MBAlbumId", typeof(string));

            tbl_r_songsMapper.Columns.Add("MBSongId", typeof(string));
            tbl_r_songsMapper.Columns.Add("AlbumId", typeof(string));
            tbl_r_songsMapper.Columns.Add("CDNumber", typeof(string));
            tbl_r_songsMapper.Columns.Add("SongNumber", typeof(string));
            tbl_r_songsMapper.Columns.Add("SongTitle", typeof(string));
            tbl_r_songsMapper.Columns.Add("SongTitleFlat", typeof(string));
            tbl_r_songsMapper.Columns.Add("AddedDateTime", typeof(DateTime));
            tbl_r_songsMapper.Columns.Add("MBAlbumId", typeof(string));

            tbl_s_songsMapper.Columns.Add("MBSongId", typeof(string));
            tbl_s_songsMapper.Columns.Add("AlbumId", typeof(string));
            tbl_s_songsMapper.Columns.Add("CDNumber", typeof(string));
            tbl_s_songsMapper.Columns.Add("SongNumber", typeof(string));
            tbl_s_songsMapper.Columns.Add("SongTitle", typeof(string));
            tbl_s_songsMapper.Columns.Add("SongTitleFlat", typeof(string));
            tbl_s_songsMapper.Columns.Add("AddedDateTime", typeof(DateTime));
            tbl_s_songsMapper.Columns.Add("MBAlbumId", typeof(string));

            tbl_symb_songsMapper.Columns.Add("MBSongId", typeof(string));
            tbl_symb_songsMapper.Columns.Add("AlbumId", typeof(string));
            tbl_symb_songsMapper.Columns.Add("CDNumber", typeof(string));
            tbl_symb_songsMapper.Columns.Add("SongNumber", typeof(string));
            tbl_symb_songsMapper.Columns.Add("SongTitle", typeof(string));
            tbl_symb_songsMapper.Columns.Add("SongTitleFlat", typeof(string));
            tbl_symb_songsMapper.Columns.Add("AddedDateTime", typeof(DateTime));
            tbl_symb_songsMapper.Columns.Add("MBAlbumId", typeof(string));

            tbl_t_songsMapper.Columns.Add("MBSongId", typeof(string));
            tbl_t_songsMapper.Columns.Add("AlbumId", typeof(string));
            tbl_t_songsMapper.Columns.Add("CDNumber", typeof(string));
            tbl_t_songsMapper.Columns.Add("SongNumber", typeof(string));
            tbl_t_songsMapper.Columns.Add("SongTitle", typeof(string));
            tbl_t_songsMapper.Columns.Add("SongTitleFlat", typeof(string));
            tbl_t_songsMapper.Columns.Add("AddedDateTime", typeof(DateTime));
            tbl_t_songsMapper.Columns.Add("MBAlbumId", typeof(string));

            tbl_u_songsMapper.Columns.Add("MBSongId", typeof(string));
            tbl_u_songsMapper.Columns.Add("AlbumId", typeof(string));
            tbl_u_songsMapper.Columns.Add("CDNumber", typeof(string));
            tbl_u_songsMapper.Columns.Add("SongNumber", typeof(string));
            tbl_u_songsMapper.Columns.Add("SongTitle", typeof(string));
            tbl_u_songsMapper.Columns.Add("SongTitleFlat", typeof(string));
            tbl_u_songsMapper.Columns.Add("AddedDateTime", typeof(DateTime));
            tbl_u_songsMapper.Columns.Add("MBAlbumId", typeof(string));

            tbl_v_songsMapper.Columns.Add("MBSongId", typeof(string));
            tbl_v_songsMapper.Columns.Add("AlbumId", typeof(string));
            tbl_v_songsMapper.Columns.Add("CDNumber", typeof(string));
            tbl_v_songsMapper.Columns.Add("SongNumber", typeof(string));
            tbl_v_songsMapper.Columns.Add("SongTitle", typeof(string));
            tbl_v_songsMapper.Columns.Add("SongTitleFlat", typeof(string));
            tbl_v_songsMapper.Columns.Add("AddedDateTime", typeof(DateTime));
            tbl_v_songsMapper.Columns.Add("MBAlbumId", typeof(string));

            tbl_w_songsMapper.Columns.Add("MBSongId", typeof(string));
            tbl_w_songsMapper.Columns.Add("AlbumId", typeof(string));
            tbl_w_songsMapper.Columns.Add("CDNumber", typeof(string));
            tbl_w_songsMapper.Columns.Add("SongNumber", typeof(string));
            tbl_w_songsMapper.Columns.Add("SongTitle", typeof(string));
            tbl_w_songsMapper.Columns.Add("SongTitleFlat", typeof(string));
            tbl_w_songsMapper.Columns.Add("AddedDateTime", typeof(DateTime));
            tbl_w_songsMapper.Columns.Add("MBAlbumId", typeof(string));

            tbl_x_songsMapper.Columns.Add("MBSongId", typeof(string));
            tbl_x_songsMapper.Columns.Add("AlbumId", typeof(string));
            tbl_x_songsMapper.Columns.Add("CDNumber", typeof(string));
            tbl_x_songsMapper.Columns.Add("SongNumber", typeof(string));
            tbl_x_songsMapper.Columns.Add("SongTitle", typeof(string));
            tbl_x_songsMapper.Columns.Add("SongTitleFlat", typeof(string));
            tbl_x_songsMapper.Columns.Add("AddedDateTime", typeof(DateTime));
            tbl_x_songsMapper.Columns.Add("MBAlbumId", typeof(string));

            tbl_y_songsMapper.Columns.Add("MBSongId", typeof(string));
            tbl_y_songsMapper.Columns.Add("AlbumId", typeof(string));
            tbl_y_songsMapper.Columns.Add("CDNumber", typeof(string));
            tbl_y_songsMapper.Columns.Add("SongNumber", typeof(string));
            tbl_y_songsMapper.Columns.Add("SongTitle", typeof(string));
            tbl_y_songsMapper.Columns.Add("SongTitleFlat", typeof(string));
            tbl_y_songsMapper.Columns.Add("AddedDateTime", typeof(DateTime));
            tbl_y_songsMapper.Columns.Add("MBAlbumId", typeof(string));

            tbl_z_songsMapper.Columns.Add("MBSongId", typeof(string));
            tbl_z_songsMapper.Columns.Add("AlbumId", typeof(string));
            tbl_z_songsMapper.Columns.Add("CDNumber", typeof(string));
            tbl_z_songsMapper.Columns.Add("SongNumber", typeof(string));
            tbl_z_songsMapper.Columns.Add("SongTitle", typeof(string));
            tbl_z_songsMapper.Columns.Add("SongTitleFlat", typeof(string));
            tbl_z_songsMapper.Columns.Add("AddedDateTime", typeof(DateTime));
            tbl_z_songsMapper.Columns.Add("MBAlbumId", typeof(string));
        }

        public bool ProcessAlbumsFile(string Url, string tempFilesDir, string queueKey, bool fistTimeProcess)
        {
            //_dapper.ExecuteNonQuery("radioconn", "Operations_Sync_AlbumTaskMarkAsStarted", new
            //{
            //    version_number = queueKey
            //});

            bool completedWithErrors = false;
            string artistFile = $"{tempFilesDir}release.tar.xz";

            //using (WebClient webClient = new WebClient())
            //{
            //    webClient.Headers.Add("Accept: text/html, application/xhtml+xml, */*");
            //    webClient.Headers.Add("User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
            //    webClient.DownloadFile(new Uri(Url), artistFile);
            //}

            //if (File.Exists(artistFile))
            //{
            //    using (var fileStream = File.OpenRead(artistFile))
            //    using (IReader reader = ReaderFactory.Open(fileStream))
            //    {
            //        while (reader.MoveToNextEntry())
            //        {
            //            if (reader.Entry.Key.EndsWith("release"))
            //            {
            //                reader.WriteEntryToDirectory(tempFilesDir, new SharpCompress.Common.ExtractionOptions()
            //                {
            //                    ExtractFullPath = true,
            //                    Overwrite = true
            //                });
            //            }
            //        }
            //    }
            //}

            string albumFileFull = (IsLinux ? $"{tempFilesDir}mbdump/release" : $"{tempFilesDir}mbdump\\release");
            //string albumFileFull = "C:\\Users\\19039\\Downloads\\release";

            if (fistTimeProcess)
            {
                completedWithErrors = FirstTimeProcess(albumFileFull, queueKey);
                //completedWithErrors = FirstTimeBulkProcess(albumFileFull, queueKey);
            }
            else
            {
                completedWithErrors = AppendedProcess(albumFileFull, queueKey);
            }

            _dapper.ExecuteNonQuery("radioconn", "Operations_Sync_AlbumTaskMarkAsCompleted", new
            {
                version_number = queueKey
            });

            return completedWithErrors;
        }

        private bool FirstTimeProcess(string albumFileFull, string queueKey)
        {
            bool completedWithErrors = false;
            bool albumExists = false;
            int newAlbumsCount = 0;

            using (System.IO.Stream fs = new FileStream(albumFileFull, FileMode.Open, FileAccess.Read))
            using (StreamReader streamReader = new StreamReader(fs, System.Text.Encoding.UTF8))
            {
                while (!streamReader.EndOfStream)
                {
                    string result = streamReader.ReadLine();

                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        try
                        {
                            MBAlbumParseDataItem albumItem = Newtonsoft.Json.JsonConvert.DeserializeObject<MBAlbumParseDataItem>(result);

                            if (albumItem != null && albumItem.artistcredit != null && albumItem.artistcredit.Count > 0 && albumItem.media != null && albumItem.media.Count > 0)
                            {
                                albumExists = _dapper.GetFirstOrDefault<bool>("radioconn", "Operations_CheckIfMBAlbumIdExists", new
                                {
                                    mb_albumid = albumItem.id,
                                    mb_artistid = albumItem.artistcredit[0].artist.id
                                });

                                if (!albumExists)
                                {
                                    SaveMBAlbum(albumItem);
                                    newAlbumsCount++;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            completedWithErrors = true;

                            _dapper.ExecuteNonQuery("radioconn", "Operations_Sync_LogError", new
                            {
                                queue_key = queueKey,
                                error_parent = "AlbumsOperation",
                                error_message = ex.Message.ToString(),
                                json_source = (result ?? "")
                            });
                        }
                    }
                }
            }

            return completedWithErrors;
        }

        private bool FirstTimeBulkProcess(string albumFileFull, string queueKey)
        {
            bool completedWithErrors = false;
            bool albumExists = false;
            int newAlbumsCount = 0;

            allArtists = _dapper.GetList<MBArtistSimpleDataItem>("radioconn", "Operations_MBGetAllExistingArtists");

            using (System.IO.Stream fs = new FileStream(albumFileFull, FileMode.Open, FileAccess.Read))
            using (StreamReader streamReader = new StreamReader(fs, System.Text.Encoding.UTF8))
            {
                while (!streamReader.EndOfStream)
                {
                    string result = streamReader.ReadLine();

                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        try
                        {
                            MBAlbumParseDataItem albumItem = Newtonsoft.Json.JsonConvert.DeserializeObject<MBAlbumParseDataItem>(result);

                            if (albumItem != null && albumItem.artistcredit != null && albumItem.artistcredit.Count > 0 && albumItem.media != null && albumItem.media.Count > 0)
                            {
                                SaveMBAlbumBulk(albumItem);
                                newAlbumsCount++;
                            }
                        }
                        catch (Exception ex)
                        {
                            completedWithErrors = true;

                            _dapper.ExecuteNonQuery("radioconn", "Operations_Sync_LogError", new
                            {
                                queue_key = queueKey,
                                error_parent = "AlbumsOperation",
                                error_message = ex.Message.ToString(),
                                json_source = (result ?? "")
                            });
                        }
                    }
                }
            }

            AddAlbumsSongsToDb(true);

            return completedWithErrors;
        }

        private bool AppendedProcess(string albumFileFull, string queueKey)
        {
            bool completedWithErrors = false;
            bool albumExists = false;
            int newAlbumsCount = 0;

            using (FileStream fss = new FileStream(albumFileFull, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader fs = new StreamReader(fss, System.Text.Encoding.UTF8))
            {
                // Start reading from the end of the file
                fs.BaseStream.Seek(0, SeekOrigin.End);

                // Read the file stream backward
                long position = fs.BaseStream.Position;
                byte[] buffer = new byte[1024];
                StringBuilder sb = new StringBuilder();

                while (!albumExists && position > 0)
                {
                    fs.BaseStream.Seek(-Math.Min(position, buffer.Length), SeekOrigin.Current);

                    int bytesRead = fs.BaseStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                        break;

                    for (int i = bytesRead - 1; i >= 0; i--)
                    {
                        if (buffer[i] == '\n')
                        {
                            // Process the line
                            string line = sb.ToString();

                            try
                            {
                                MBAlbumParseDataItem albumItem = Newtonsoft.Json.JsonConvert.DeserializeObject<MBAlbumParseDataItem>(line);

                                if (albumItem != null && albumItem.artistcredit != null && albumItem.artistcredit.Count > 0 && albumItem.media != null && albumItem.media.Count > 0)
                                {
                                    albumExists = _dapper.GetFirstOrDefault<bool>("radioconn", "Operations_CheckIfMBAlbumIdExists", new
                                    {
                                        mb_albumid = albumItem.id,
                                        mb_artistid = albumItem.artistcredit[0].artist.id
                                    });

                                    if (!albumExists)
                                    {
                                        SaveMBAlbum(albumItem);
                                        newAlbumsCount++;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                completedWithErrors = true;

                                _dapper.ExecuteNonQuery("radioconn", "Operations_Sync_LogError", new
                                {
                                    queue_key = queueKey,
                                    error_parent = "AlbumsOperation",
                                    error_message = ex.Message.ToString(),
                                    json_source = (line ?? "")
                                });
                            }

                            sb.Clear();
                        }
                        else
                        {
                            sb.Insert(0, (char)buffer[i]);
                        }
                    }

                    position -= bytesRead;
                    fs.BaseStream.Seek(-bytesRead, SeekOrigin.Current);
                }

                // Process the first line if any
                if (position <= 0 && sb.Length > 0)
                {
                    string line = sb.ToString();

                    try
                    {
                        MBAlbumParseDataItem albumItem = Newtonsoft.Json.JsonConvert.DeserializeObject<MBAlbumParseDataItem>(line);

                        if (albumItem != null && albumItem.artistcredit != null && albumItem.artistcredit.Count > 0 && albumItem.media != null && albumItem.media.Count > 0)
                        {
                            albumExists = _dapper.GetFirstOrDefault<bool>("radioconn", "Operations_CheckIfMBAlbumIdExists", new
                            {
                                mb_albumid = albumItem.id,
                                mb_artistid = albumItem.artistcredit[0].artist.id
                            });

                            if (!albumExists)
                            {
                                SaveMBAlbum(albumItem);
                                newAlbumsCount++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        completedWithErrors = true;

                        _dapper.ExecuteNonQuery("radioconn", "Operations_Sync_LogError", new
                        {
                            queue_key = queueKey,
                            error_parent = "AlbumsOperation",
                            error_message = ex.Message.ToString(),
                            json_source = (line ?? "")
                        });
                    }
                }
            }

            int finalCount = newAlbumsCount;

            return completedWithErrors;
        }

        private void SaveMBAlbum(MBAlbumParseDataItem albumItem)
        {
            if (albumItem != null && albumItem.artistcredit != null && albumItem.artistcredit.Count > 0 && albumItem.media != null && albumItem.media.Count > 0)
            {
                MBNewAlbumInsertDataItem newId = _dapper.GetFirstOrDefault<MBNewAlbumInsertDataItem>("radioconn", "Operations_MBInsertAlbumForArtist", new
                {
                    mb_artistid = albumItem.artistcredit[0].artist.id,
                    mb_albumid = albumItem.id,
                    album_title = albumItem.title,
                    status = albumItem.status,
                    primary_type = (albumItem.releasegroup != null ? (albumItem.releasegroup.primarytype ?? "") : ""),
                    first_release_date = (albumItem.releasegroup != null ? (albumItem.releasegroup.firstreleasedate ?? "") : "")
                });

                if (newId != null && newId.new_id.HasValue && !string.IsNullOrWhiteSpace(newId.db_source))
                {
                    int cdNumber = 0;
                    foreach (var cd in albumItem.media)
                    {
                        cdNumber++;

                        if (cd != null && cd.tracks != null && cd.tracks.Count > 0)
                        {
                            int songPosition = 0;

                            foreach (var track in cd.tracks)
                            {
                                songPosition++;

                                if (track != null)
                                {
                                    _dapper.ExecuteNonQuery("radioconn", "Operations_MBInsertAlbumSong", new
                                    {
                                        db_source = newId.db_source,
                                        mb_songid = track.id,
                                        album_id = newId.new_id.Value,
                                        cd_number = cdNumber,
                                        song_number = (track.position.HasValue ? track.position.Value : songPosition),
                                        song_title = track.title
                                    });
                                }
                            }
                        }
                    }
                }
            }
        }

        private void SaveMBAlbumBulk(MBAlbumParseDataItem albumItem)
        {
            if (albumItem != null && albumItem.artistcredit != null && albumItem.artistcredit.Count > 0 && albumItem.media != null && albumItem.media.Count > 0)
            {
                MBArtistSimpleDataItem currentArtist = allArtists.Where(ar => ar.MBId.ToLower() == albumItem.artistcredit[0].artist.id.ToLower()).FirstOrDefault();

                if (currentArtist != null)
                {
                    var tblAlbums = GetAlbumReference(currentArtist.DBSource);

                    if (tblAlbums != null)
                    {
                        DataRow albumRow = tblAlbums.NewRow();
                        albumRow["MBAlbumId"] = albumItem.id;
                        albumRow["ArtistId"] = currentArtist.Id.ToString();
                        albumRow["MBArtistId"] = albumItem.artistcredit[0].artist.id;
                        albumRow["Title"] = albumItem.title;
                        albumRow["TitleFlat"] = Latinize(albumItem.title);
                        albumRow["Status"] = albumItem.status;
                        albumRow["PrimaryType"] = (albumItem.releasegroup != null ? (albumItem.releasegroup.primarytype ?? "") : "");
                        albumRow["FirstReleaseDate"] = (albumItem.releasegroup != null ? (albumItem.releasegroup.firstreleasedate ?? "") : "");
                        albumRow["AddedDateTime"] = DateTime.Now;

                        tblAlbums.Rows.Add(albumRow);

                        //MBNewAlbumInsertDataItem newId = _dapper.GetFirstOrDefault<MBNewAlbumInsertDataItem>("radioconn", "Operations_MBInsertAlbumForArtist", new
                        //{
                        //    mb_artistid = albumItem.artistcredit[0].artist.id,
                        //    mb_albumid = albumItem.id,
                        //    album_title = albumItem.title,
                        //    status = albumItem.status,
                        //    primary_type = (albumItem.releasegroup != null ? (albumItem.releasegroup.primarytype ?? "") : ""),
                        //    first_release_date = (albumItem.releasegroup != null ? (albumItem.releasegroup.firstreleasedate ?? "") : "")
                        //});

                        var tblSongs = GetSongsReference(currentArtist.DBSource);

                        if (tblSongs != null)
                        {
                            int cdNumber = 0;
                            foreach (var cd in albumItem.media)
                            {
                                cdNumber++;

                                if (cd != null && cd.tracks != null && cd.tracks.Count > 0)
                                {
                                    int songPosition = 0;

                                    foreach (var track in cd.tracks)
                                    {
                                        songPosition++;

                                        if (track != null)
                                        {
                                            DataRow songRow = tblSongs.NewRow();
                                            songRow["MBSongId"] = track.id;
                                            songRow["AlbumId"] = DBNull.Value;
                                            songRow["CDNumber"] = cdNumber.ToString();
                                            songRow["SongNumber"] = (track.position.HasValue ? track.position.Value : songPosition);
                                            songRow["SongTitle"] = track.title;
                                            songRow["SongTitleFlat"] = Latinize(track.title);
                                            songRow["AddedDateTime"] = DateTime.Now;
                                            songRow["MBAlbumId"] = albumItem.id;

                                            tblSongs.Rows.Add(songRow);



                                            //_dapper.ExecuteNonQuery("radioconn", "Operations_MBInsertAlbumSong", new
                                            //{
                                            //    db_source = newId.db_source,
                                            //    mb_songid = track.id,
                                            //    album_id = newId.new_id.Value,
                                            //    cd_number = cdNumber,
                                            //    song_number = (track.position.HasValue ? track.position.Value : songPosition),
                                            //    song_title = track.title
                                            //});
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            AddAlbumsSongsToDb();
        }

        private void AddAlbumsSongsToDb(bool process = false)
        {
            SendAlbumData(GetAlbumReference("a"), "a", process);
            SendAlbumData(GetAlbumReference("b"), "b", process);
            SendAlbumData(GetAlbumReference("c"), "c", process);
            SendAlbumData(GetAlbumReference("d"), "d", process);
            SendAlbumData(GetAlbumReference("e"), "e", process);
            SendAlbumData(GetAlbumReference("f"), "f", process);
            SendAlbumData(GetAlbumReference("g"), "g", process);
            SendAlbumData(GetAlbumReference("h"), "h", process);
            SendAlbumData(GetAlbumReference("i"), "i", process);
            SendAlbumData(GetAlbumReference("j"), "j", process);
            SendAlbumData(GetAlbumReference("k"), "k", process);
            SendAlbumData(GetAlbumReference("l"), "l", process);
            SendAlbumData(GetAlbumReference("m"), "m", process);
            SendAlbumData(GetAlbumReference("n"), "n", process);
            SendAlbumData(GetAlbumReference("num"), "num", process);
            SendAlbumData(GetAlbumReference("o"), "o", process);
            SendAlbumData(GetAlbumReference("p"), "p", process);
            SendAlbumData(GetAlbumReference("q"), "q", process);
            SendAlbumData(GetAlbumReference("r"), "r", process);
            SendAlbumData(GetAlbumReference("s"), "s", process);
            SendAlbumData(GetAlbumReference("symb"), "symb", process);
            SendAlbumData(GetAlbumReference("t"), "t", process);
            SendAlbumData(GetAlbumReference("u"), "u", process);
            SendAlbumData(GetAlbumReference("v"), "v", process);
            SendAlbumData(GetAlbumReference("w"), "w", process);
            SendAlbumData(GetAlbumReference("x"), "x", process);
            SendAlbumData(GetAlbumReference("y"), "y", process);
            SendAlbumData(GetAlbumReference("z"), "z", process);

            SendSongData(GetSongsReference("a"), "a", process);
            SendSongData(GetSongsReference("b"), "b", process);
            SendSongData(GetSongsReference("c"), "c", process);
            SendSongData(GetSongsReference("d"), "d", process);
            SendSongData(GetSongsReference("e"), "e", process);
            SendSongData(GetSongsReference("f"), "f", process);
            SendSongData(GetSongsReference("g"), "g", process);
            SendSongData(GetSongsReference("h"), "h", process);
            SendSongData(GetSongsReference("i"), "i", process);
            SendSongData(GetSongsReference("j"), "j", process);
            SendSongData(GetSongsReference("k"), "k", process);
            SendSongData(GetSongsReference("l"), "l", process);
            SendSongData(GetSongsReference("m"), "m", process);
            SendSongData(GetSongsReference("n"), "n", process);
            SendSongData(GetSongsReference("num"), "num", process);
            SendSongData(GetSongsReference("o"), "o", process);
            SendSongData(GetSongsReference("p"), "p", process);
            SendSongData(GetSongsReference("q"), "q", process);
            SendSongData(GetSongsReference("r"), "r", process);
            SendSongData(GetSongsReference("s"), "s", process);
            SendSongData(GetSongsReference("symb"), "symb", process);
            SendSongData(GetSongsReference("t"), "t", process);
            SendSongData(GetSongsReference("u"), "u", process);
            SendSongData(GetSongsReference("v"), "v", process);
            SendSongData(GetSongsReference("w"), "w", process);
            SendSongData(GetSongsReference("x"), "x", process);
            SendSongData(GetSongsReference("y"), "y", process);
            SendSongData(GetSongsReference("z"), "z", process);
        }

        private void SendAlbumData(DataTable table, string dbSource, bool process)
        {
            if ((process && table.Rows.Count > 0) || (!process && table.Rows.Count >= 1500))
            {
                using (var con = _dapper.GetConnection($"songs_{dbSource}"))
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
                {
                    try
                    {
                        bulkCopy.DestinationTableName = "AlbumMapper";
                        bulkCopy.BatchSize = 1500;
                        bulkCopy.BulkCopyTimeout = 0;

                        bulkCopy.ColumnMappings.Add("MBAlbumId", "MBAlbumId");
                        bulkCopy.ColumnMappings.Add("ArtistId", "ArtistId");
                        bulkCopy.ColumnMappings.Add("MBArtistId", "MBArtistId");
                        bulkCopy.ColumnMappings.Add("Title", "Title");
                        bulkCopy.ColumnMappings.Add("TitleFlat", "TitleFlat");
                        bulkCopy.ColumnMappings.Add("Status", "Status");
                        bulkCopy.ColumnMappings.Add("PrimaryType", "PrimaryType");
                        bulkCopy.ColumnMappings.Add("FirstReleaseDate", "FirstReleaseDate");
                        bulkCopy.ColumnMappings.Add("AddedDateTime", "AddedDateTime");

                        bulkCopy.WriteToServer(table);
                        table.Clear();
                    }
                    catch (Exception ex)
                    {
                        SendAlbumViaSingleLineSend(table, dbSource);

                        table.Clear();
                        throw;
                    }
                }
            }
        }

        private void SendAlbumViaSingleLineSend(DataTable table, string dbSource)
        {
            DataTable quickTable = new DataTable();
            quickTable.Columns.Add("MBAlbumId", typeof(string));
            quickTable.Columns.Add("ArtistId", typeof(string));
            quickTable.Columns.Add("MBArtistId", typeof(string));
            quickTable.Columns.Add("Title", typeof(string));
            quickTable.Columns.Add("TitleFlat", typeof(string));
            quickTable.Columns.Add("Status", typeof(string));
            quickTable.Columns.Add("PrimaryType", typeof(string));
            quickTable.Columns.Add("FirstReleaseDate", typeof(string));
            quickTable.Columns.Add("AddedDateTime", typeof(DateTime));

            using (var con = _dapper.GetConnection($"songs_{dbSource}"))
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
            {
                bulkCopy.DestinationTableName = "AlbumMapper";
                bulkCopy.BatchSize = 1500;
                bulkCopy.BulkCopyTimeout = 0;

                bulkCopy.ColumnMappings.Add("MBAlbumId", "MBAlbumId");
                bulkCopy.ColumnMappings.Add("ArtistId", "ArtistId");
                bulkCopy.ColumnMappings.Add("MBArtistId", "MBArtistId");
                bulkCopy.ColumnMappings.Add("Title", "Title");
                bulkCopy.ColumnMappings.Add("TitleFlat", "TitleFlat");
                bulkCopy.ColumnMappings.Add("Status", "Status");
                bulkCopy.ColumnMappings.Add("PrimaryType", "PrimaryType");
                bulkCopy.ColumnMappings.Add("FirstReleaseDate", "FirstReleaseDate");
                bulkCopy.ColumnMappings.Add("AddedDateTime", "AddedDateTime");

                foreach (var row in table.Rows)
                {
                    try
                    {
                        quickTable.Rows.Add(row);

                        bulkCopy.WriteToServer(quickTable);
                        quickTable.Clear();
                    }
                    catch (Exception ex)
                    {
                        quickTable.Clear();
                    }
                }
            }
        }

        private void SendSongData(DataTable table, string dbSource, bool process)
        {
            if ((process && table.Rows.Count > 0) || (!process && table.Rows.Count >= 1500))
            {
                using (var con = _dapper.GetConnection($"songs_{dbSource}"))
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
                {
                    bulkCopy.DestinationTableName = "SongMapper";
                    bulkCopy.BatchSize = 1500;
                    bulkCopy.BulkCopyTimeout = 0;

                    bulkCopy.ColumnMappings.Add("MBSongId", "MBSongId");
                    bulkCopy.ColumnMappings.Add("AlbumId", "AlbumId");
                    bulkCopy.ColumnMappings.Add("CDNumber", "CDNumber");
                    bulkCopy.ColumnMappings.Add("SongNumber", "SongNumber");
                    bulkCopy.ColumnMappings.Add("SongTitle", "SongTitle");
                    bulkCopy.ColumnMappings.Add("SongTitleFlat", "SongTitleFlat");
                    bulkCopy.ColumnMappings.Add("AddedDateTime", "AddedDateTime");
                    bulkCopy.ColumnMappings.Add("MBAlbumId", "MBAlbumId");

                    bulkCopy.WriteToServer(table);
                    table.Clear();
                }
            }
        }

        private DataTable GetAlbumReference(string dbSource)
        {
            if (dbSource == "a")
            {
                return tbl_a_albumsMapper;
            }
            else if (dbSource == "b")
            {
                return tbl_b_albumsMapper;
            }
            else if (dbSource == "c")
            {
                return tbl_c_albumsMapper;
            }
            else if (dbSource == "d")
            {
                return tbl_d_albumsMapper;
            }
            else if (dbSource == "e")
            {
                return tbl_e_albumsMapper;
            }
            else if (dbSource == "f")
            {
                return tbl_f_albumsMapper;
            }
            else if (dbSource == "g")
            {
                return tbl_g_albumsMapper;
            }
            else if (dbSource == "h")
            {
                return tbl_h_albumsMapper;
            }
            else if (dbSource == "i")
            {
                return tbl_i_albumsMapper;
            }
            else if (dbSource == "j")
            {
                return tbl_j_albumsMapper;
            }
            else if (dbSource == "k")
            {
                return tbl_k_albumsMapper;
            }
            else if (dbSource == "l")
            {
                return tbl_l_albumsMapper;
            }
            else if (dbSource == "m")
            {
                return tbl_m_albumsMapper;
            }
            else if (dbSource == "n")
            {
                return tbl_n_albumsMapper;
            }
            else if (dbSource == "num")
            {
                return tbl_num_albumsMapper;
            }
            else if (dbSource == "o")
            {
                return tbl_o_albumsMapper;
            }
            else if (dbSource == "p")
            {
                return tbl_p_albumsMapper;
            }
            else if (dbSource == "q")
            {
                return tbl_q_albumsMapper;
            }
            else if (dbSource == "r")
            {
                return tbl_r_albumsMapper;
            }
            else if (dbSource == "s")
            {
                return tbl_s_albumsMapper;
            }
            else if (dbSource == "symb")
            {
                return tbl_symb_albumsMapper;
            }
            else if (dbSource == "t")
            {
                return tbl_t_albumsMapper;
            }
            else if (dbSource == "u")
            {
                return tbl_u_albumsMapper;
            }
            else if (dbSource == "v")
            {
                return tbl_v_albumsMapper;
            }
            else if (dbSource == "w")
            {
                return tbl_w_albumsMapper;
            }
            else if (dbSource == "x")
            {
                return tbl_x_albumsMapper;
            }
            else if (dbSource == "y")
            {
                return tbl_y_albumsMapper;
            }
            else if (dbSource == "z")
            {
                return tbl_z_albumsMapper;
            }

            return null;
        }

        private DataTable GetSongsReference(string dbSource)
        {
            if (dbSource == "a")
            {
                return tbl_a_songsMapper;
            }
            else if (dbSource == "b")
            {
                return tbl_b_songsMapper;
            }
            else if (dbSource == "c")
            {
                return tbl_c_songsMapper;
            }
            else if (dbSource == "d")
            {
                return tbl_d_songsMapper;
            }
            else if (dbSource == "e")
            {
                return tbl_e_songsMapper;
            }
            else if (dbSource == "f")
            {
                return tbl_f_songsMapper;
            }
            else if (dbSource == "g")
            {
                return tbl_g_songsMapper;
            }
            else if (dbSource == "h")
            {
                return tbl_h_songsMapper;
            }
            else if (dbSource == "i")
            {
                return tbl_i_songsMapper;
            }
            else if (dbSource == "j")
            {
                return tbl_j_songsMapper;
            }
            else if (dbSource == "k")
            {
                return tbl_k_songsMapper;
            }
            else if (dbSource == "l")
            {
                return tbl_l_songsMapper;
            }
            else if (dbSource == "m")
            {
                return tbl_m_songsMapper;
            }
            else if (dbSource == "n")
            {
                return tbl_n_songsMapper;
            }
            else if (dbSource == "num")
            {
                return tbl_num_songsMapper;
            }
            else if (dbSource == "o")
            {
                return tbl_o_songsMapper;
            }
            else if (dbSource == "p")
            {
                return tbl_p_songsMapper;
            }
            else if (dbSource == "q")
            {
                return tbl_q_songsMapper;
            }
            else if (dbSource == "r")
            {
                return tbl_r_songsMapper;
            }
            else if (dbSource == "s")
            {
                return tbl_s_songsMapper;
            }
            else if (dbSource == "symb")
            {
                return tbl_symb_songsMapper;
            }
            else if (dbSource == "t")
            {
                return tbl_t_songsMapper;
            }
            else if (dbSource == "u")
            {
                return tbl_u_songsMapper;
            }
            else if (dbSource == "v")
            {
                return tbl_v_songsMapper;
            }
            else if (dbSource == "w")
            {
                return tbl_w_songsMapper;
            }
            else if (dbSource == "x")
            {
                return tbl_x_songsMapper;
            }
            else if (dbSource == "y")
            {
                return tbl_y_songsMapper;
            }
            else if (dbSource == "z")
            {
                return tbl_z_songsMapper;
            }

            return null;
        }

        private string Latinize(string Input)
        {
            Encoding latinizeEncoding = Encoding.GetEncoding("ISO-8859-8");
            var strBytes = latinizeEncoding.GetBytes(Input);

            return latinizeEncoding.GetString(strBytes);
        }
    }
}
