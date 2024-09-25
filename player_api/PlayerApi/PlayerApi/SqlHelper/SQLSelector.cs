using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PlayerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayerApi
{
    public static class SQLSelector
    {
        private static readonly string conString = @"Server=LAPTOP-LMHEG0J6\SQLEXPRESS;Database=audioplayerdb;Trusted_Connection=True;";
        
        private static readonly string searchAudios_sqlquery = @"SELECT DISTINCT a.Id, a.AmountAuditions, CASE
                WHEN a.Title LIKE @Query + '%' THEN 1
                WHEN p.Name LIKE @Query + '%' THEN 2
                WHEN a.Title LIKE '%' + @Query + '%' THEN 3
                ELSE 4
				END as MyCase
            FROM Audio as a
            INNER JOIN AudioPerformer as ap on a.Id = ap.AudioId
            INNER JOIN Performer as p on p.Id = ap.PerformerId
            WHERE a.Title LIKE '%' + @Query + '%' OR p.Name LIKE '%' + @Query + '%'
            ORDER BY MyCase, a.AmountAuditions desc
			OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;";

        private static readonly string searchAudiosCount_sqlquery = @"SELECT COUNT(DISTINCT a.Id)
            FROM Audio as a
            INNER JOIN AudioPerformer as ap on a.Id = ap.AudioId
            INNER JOIN Performer as p on p.Id = ap.PerformerId
            WHERE a.Title LIKE '%' + @Query + '%' OR p.Name LIKE '%' + @Query + '%';";

        private static readonly string audiosByPlaylist_sqlquery = @"SELECT a.Id 
            FROM Audio as a
            INNER JOIN AudioPlaylist as ap ON ap.AudioId = a.Id
            INNER JOIN Playlist as p ON ap.PlaylistId = p.Id
            WHERE p.Id = @playlistId
            ORDER BY SequenceNumber desc
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;";

        private static readonly string audiosByPlaylistCount_sqlquery = @"SELECT COUNT(DISTINCT a.Id) 
            FROM Audio as a
            INNER JOIN AudioPlaylist as ap ON ap.AudioId = a.Id
            INNER JOIN Playlist as p ON ap.PlaylistId = p.Id
            WHERE p.Id = @playlistId;";

        public static List<Audio> SearchAudios(string query = "", int skip = 0, int take = 12)
        {
            var audios = new List<Audio>();
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                var command = new SqlCommand(searchAudios_sqlquery, connection);

                var queryParam = new SqlParameter("@Query", query);
                command.Parameters.Add(queryParam);
                var skipParam = new SqlParameter("@Skip", skip * take);
                command.Parameters.Add(skipParam);
                var takeParam = new SqlParameter("@Take", take);
                command.Parameters.Add(takeParam);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    using (var db = new AudioPlayerContext())
                    {
                        while (reader.Read())
                        {
                            var id = reader.GetInt32(0);
                            var aud = db.Audio.Include(a => a.Performer).FirstOrDefault(a => a.Id == id);
                            audios.Add(aud);
                        }
                    }
                }
            }
            return audios;
        }

        public static int SearchAudiosCount(string query = "")
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                var command = new SqlCommand(searchAudiosCount_sqlquery, connection);

                var queryParam = new SqlParameter("@Query", query);
                command.Parameters.Add(queryParam);

                return (int)command.ExecuteScalar();
            }
        }


        public static List<Audio> AudiosByPlaylist(int playlistId, int skip = 0, int take = 12)
        {
            var audios = new List<Audio>();
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                var command = new SqlCommand(audiosByPlaylist_sqlquery, connection);

                var playlistParam = new SqlParameter("@playlistId", playlistId);
                command.Parameters.Add(playlistParam);
                var skipParam = new SqlParameter("@Skip", skip * take);
                command.Parameters.Add(skipParam);
                var takeParam = new SqlParameter("@Take", take);
                command.Parameters.Add(takeParam);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    using (var db = new AudioPlayerContext())
                    {
                        while (reader.Read())
                        {
                            var id = reader.GetInt32(0);
                            var aud = db.Audio.Include(a => a.Performer).FirstOrDefault(a => a.Id == id);
                            audios.Add(aud);
                        }
                    }
                }
            }
            return audios;
        }

        public static int AudiosByPlaylistCount(int playlistId)
        {
            var audios = new List<Audio>();
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                var command = new SqlCommand(audiosByPlaylistCount_sqlquery, connection);

                var playlistParam = new SqlParameter("@playlistId", playlistId);
                command.Parameters.Add(playlistParam);

                return (int)command.ExecuteScalar();
            }
        }
    }
}
