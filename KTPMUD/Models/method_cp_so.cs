using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace KTPMUD.Models
{
    internal class method_co_so
    {
        string connectionString = "Data Source=localhost;Initial Catalog=ktpmud;Integrated Security=True";

        // Phương thức kiểm tra xem tên cơ sở đã tồn tại chưa
        public bool CheckCoSoNameExists(string name)
        {
            bool exists = false;
            string query = "SELECT COUNT(*) FROM Co_so WHERE name = @name";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@name", System.Data.SqlDbType.NVarChar).Value = name;
                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                            exists = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi kiểm tra tên cơ sở: " + ex.Message);
                    throw;
                }
            }

            return exists;
        }

        // Phương thức thêm cơ sở vào cơ sở dữ liệu
        public void AddCoSo(Co_so coSo)
        {
            if (string.IsNullOrEmpty(coSo.name))
            {
                throw new ArgumentException("Tên cơ sở không thể rỗng.");
            }

            string query = "INSERT INTO Co_so (name, id_con_giong_vat_nuoi, id_animal, id_tinh_phoi, id_nguon_gen, id_loai_hinh, id_user_dung_dau, id_xa, id_huyen) " +
                           "VALUES (@name, @id_con_giong_vat_nuoi, @id_animal, @id_tinh_phoi, @id_nguon_gen, @id_loai_hinh, @id_user_dung_dau, @id_xa, @id_huyen)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", coSo.name);
                    cmd.Parameters.AddWithValue("@id_con_giong_vat_nuoi", coSo.id_con_giong_vat_nuoi);
                    cmd.Parameters.AddWithValue("@id_animal", coSo.id_animal);
                    cmd.Parameters.AddWithValue("@id_tinh_phoi", coSo.id_tinh_phoi);
                    cmd.Parameters.AddWithValue("@id_nguon_gen", coSo.id_nguon_gen);
                    cmd.Parameters.AddWithValue("@id_loai_hinh", coSo.id_loai_hinh);
                    cmd.Parameters.AddWithValue("@id_user_dung_dau", coSo.id_user_dung_dau);
                    cmd.Parameters.AddWithValue("@id_xa", coSo.id_xa);
                    cmd.Parameters.AddWithValue("@id_huyen", coSo.id_huyen);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Phương thức cập nhật cơ sở
        public void UpdateCoSo(Co_so coSo)
        {
            string query = "UPDATE Co_so SET name = @name, id_con_giong_vat_nuoi = @id_con_giong_vat_nuoi, id_animal = @id_animal, " +
                           "id_tinh_phoi = @id_tinh_phoi, id_nguon_gen = @id_nguon_gen, id_loai_hinh = @id_loai_hinh, " +
                           "id_user_dung_dau = @id_user_dung_dau, id_xa = @id_xa, id_huyen = @id_huyen " +
                           "WHERE id = @id";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", coSo.id);
                    cmd.Parameters.AddWithValue("@name", coSo.name);
                    cmd.Parameters.AddWithValue("@id_con_giong_vat_nuoi", coSo.id_con_giong_vat_nuoi);
                    cmd.Parameters.AddWithValue("@id_animal", coSo.id_animal);
                    cmd.Parameters.AddWithValue("@id_tinh_phoi", coSo.id_tinh_phoi);
                    cmd.Parameters.AddWithValue("@id_nguon_gen", coSo.id_nguon_gen);
                    cmd.Parameters.AddWithValue("@id_loai_hinh", coSo.id_loai_hinh);
                    cmd.Parameters.AddWithValue("@id_user_dung_dau", coSo.id_user_dung_dau);
                    cmd.Parameters.AddWithValue("@id_xa", coSo.id_xa);
                    cmd.Parameters.AddWithValue("@id_huyen", coSo.id_huyen);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Phương thức xóa cơ sở
        public void DeleteCoSo(int id)
        {
            string query = "DELETE FROM Co_so WHERE id = @id";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi khi xóa cơ sở: " + ex.Message);
                    throw;
                }
            }
        }

        // Phương thức lấy danh sách tất cả các cơ sở
        public List<Co_so> GetAllCoSo()
        {
            List<Co_so> coSos = new List<Co_so>();
            string query = "SELECT id, name, id_con_giong_vat_nuoi, id_animal, id_tinh_phoi, id_nguon_gen, id_loai_hinh, id_user_dung_dau, id_xa, id_huyen FROM Co_so";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Co_so coSo = new Co_so
                                {
                                    id = Convert.ToInt32(reader["id"]),
                                    name = reader["name"].ToString(),
                                    id_con_giong_vat_nuoi = Convert.ToInt32(reader["id_con_giong_vat_nuoi"]),
                                    id_animal = Convert.ToInt32(reader["id_animal"]),
                                    id_tinh_phoi = Convert.ToInt32(reader["id_tinh_phoi"]),
                                    id_nguon_gen = Convert.ToInt32(reader["id_nguon_gen"]),
                                    id_loai_hinh = Convert.ToInt32(reader["id_loai_hinh"]),
                                    id_user_dung_dau = Convert.ToInt32(reader["id_user_dung_dau"]),
                                    id_xa = Convert.ToInt32(reader["id_xa"]),
                                    id_huyen = Convert.ToInt32(reader["id_huyen"])
                                };
                                coSos.Add(coSo);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi khi lấy danh sách cơ sở: " + ex.Message);
                    throw;
                }
            }

            return coSos;
        }
    }
}
