using Emgu.CV.UI;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using MySqlX.XDevAPI.Relational;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Security.Cryptography;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace AnomalyDetector.utils
{
    public class database : IDisposable
    {
        private MySqlConnection connection;

        public database(string host, string port, string database, string username, string password)
        {
            connection = new MySqlConnection($"Server={host};Port={port};Database={database};Uid={username};Pwd={password}");
            connection.Open();
        }

        public MySqlConnection session()
        {
            return connection;
        }

        public void Dispose()
        {
            connection.Close();
        }

        public bool Insert(string result, string detail, ref byte[] imageBytes)
        {
            MySqlCommand query = new MySqlCommand("INSERT INTO `inspection_history` (result, detail, image_source) VALUES(@result, @detail , @image)", connection);
            query.Parameters.AddWithValue("@result", result);
            query.Parameters.AddWithValue("@detail", detail);
            query.Parameters.AddWithValue("@image", imageBytes);

            if (query.ExecuteNonQuery() != 1)
                MessageBox.Show("Failed to delete data.");
            return true;
        }

        public int Select(ref DataGridView table, int page = 0, int page_size = 100, bool onlyNG = false)
        {
            try
            {
                string querystr = "";
                if (onlyNG)
                    querystr = "WHERE `result`='NG'";

                MySqlCommand query = new MySqlCommand(
                    $"SELECT `id`,`date`,`result`,`detail` FROM `inspection_history` {querystr} ORDER BY `date` DESC LIMIT @page, @page_size;", connection);
                query.Parameters.AddWithValue("@page", page * page_size);
                query.Parameters.AddWithValue("@page_size", page_size);

                MySqlDataAdapter da = new MySqlDataAdapter(query);

                DataTable dt = new DataTable();
                da.Fill(dt);

                table.DataSource = dt;                               

                return dt.Rows.Count;
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"ERROR: {ex.Message}");
            }
            return 0;
        }

        public bool ShowImage(ref PanAndZoomPictureBox imagebox, string id)
        {
            try
            {
                MySqlCommand query = new MySqlCommand("SELECT `image_source` FROM `inspection_history` WHERE `id`=@ID LIMIT 1;", connection);
                query.Parameters.AddWithValue("@ID", id);

                MySqlDataReader reader = query.ExecuteReader();

                byte[] bImage = null;
                while (reader.Read())
                {
                    bImage = (byte[])reader[0];
                }

                if (bImage != null)
                    imagebox.Image = new Bitmap(new MemoryStream(bImage));

                reader.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR: {ex.Message}");
                return false;
            }
            return true;
        }
    }
}
