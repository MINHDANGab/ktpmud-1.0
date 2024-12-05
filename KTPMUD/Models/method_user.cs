using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;

namespace KTPMUD.Models
{
    internal class method_user
    {
        string connectionString = "Data Source=localhost;Initial Catalog=ktpmud;Integrated Security=True";
        // Phương thức kiểm tra xem tên người dùng đã tồn tại chưa
        public bool CheckUserNameExists(string userName)
        {
            bool exists = false;
            string query = "SELECT COUNT(*) FROM [user] WHERE user_name = @userName";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open(); // Mở kết nối

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@userName", System.Data.SqlDbType.NVarChar).Value = userName;
                        int count = (int)cmd.ExecuteScalar(); // Trả về số lượng người dùng khớp

                        if (count > 0)
                        {
                            exists = true; // Nếu có người dùng trùng tên, trả về true
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Ghi log chi tiết lỗi (có thể ghi vào file log, hoặc hiển thị trong UI)
                    Console.WriteLine("Lỗi kiểm tra tên người dùng: " + ex.Message);
                    throw new Exception("Lỗi kết nối cơ sở dữ liệu khi kiểm tra tên người dùng.", ex);
                }
            }

            return exists;
        }

        // Phương thức thêm người dùng vào cơ sở dữ liệu
        public void Add_User(User user)
        {
            if (string.IsNullOrEmpty(user.user_name) || string.IsNullOrEmpty(user.password))
            {
                throw new ArgumentException("Tên người dùng và mật khẩu không thể rỗng.");
            }

            string query = "INSERT INTO [user] (user_name, password, email, sdt, id_xa, status, id_quy_mo, id_huyen, id_co_so) " +
                           "VALUES (@user_name, @password, @email, @sdt, @id_xa, @status, @id_quy_mo, @id_huyen, @id_co_so)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    AddNullableParameter(cmd, "@user_name", user.user_name);
                    AddNullableParameter(cmd, "@password", user.password);
                    AddNullableParameter(cmd, "@email", user.email);
                    AddNullableParameter(cmd, "@sdt", user.sdt);
                    cmd.Parameters.AddWithValue("@status", user.status);
                    AddNullableParameter(cmd, "@id_quy_mo", user.id_quy_mo);
                    AddNullableParameter(cmd, "@id_huyen", user.id_huyen);
                    AddNullableParameter(cmd, "@id_co_so", user.id_co_so);
                    AddNullableParameter(cmd, "@id_xa", user.id_xa);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void AddNullableParameter(SqlCommand cmd, string parameterName, object value)
        {
            if (value == null || value.ToString() == "")
            {
                cmd.Parameters.AddWithValue(parameterName, DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue(parameterName, value);
            }
        }


        public bool CheckPassword(string userName, string password)
        {
            bool isValid = false;
            string query = "SELECT password FROM [user] WHERE user_name = @userName";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open(); // Mở kết nối

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@userName", System.Data.SqlDbType.NVarChar).Value = userName;

                        string storedPassword = (string)cmd.ExecuteScalar();

                        if (storedPassword != null && storedPassword == password)
                        {
                            isValid = true;
                        }
                        else
                        {
                            // Thêm thông báo nếu mật khẩu không khớp
                            Console.WriteLine("Mật khẩu không chính xác.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi kiểm tra mật khẩu: " + ex.Message);
                    throw new Exception("Lỗi khi kiểm tra mật khẩu.", ex);
                }
            }

            return isValid;
        }

        public User GetUserDataAllMethods(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("Tên người dùng không thể rỗng.");
            }

            User result = null;
            string query = "SELECT  id, user_name, email, sdt, status, id_xa, id_huyen, id_quy_mo, id_co_so FROM [user] WHERE user_name = @userName";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@userName", System.Data.SqlDbType.NVarChar).Value = userName;

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                result = new User
                                {
                                    id= reader["id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["id_co_so"]),
                                    user_name = reader["user_name"].ToString(),
                                    email = reader["email"] == DBNull.Value ? null : reader["email"].ToString(),
                                    sdt = reader["sdt"] == DBNull.Value ? null : reader["sdt"].ToString(),
                                    status = reader["status"] == DBNull.Value ? false : Convert.ToBoolean(reader["status"]),
                                    id_xa = reader["id_xa"] == DBNull.Value ? 0 : Convert.ToInt32(reader["id_xa"]),
                                    id_huyen = reader["id_huyen"] == DBNull.Value ? 0 : Convert.ToInt32(reader["id_huyen"]),
                                    id_quy_mo = reader["id_quy_mo"] == DBNull.Value ? 0 : Convert.ToInt32(reader["id_quy_mo"]),
                                    id_co_so = reader["id_co_so"] == DBNull.Value ? 0 : Convert.ToInt32(reader["id_co_so"])
                                };

                                // Ghi log kết quả đã đọc
                                Console.WriteLine($"User: {result.user_name}, ID Quy Mô: {result.id_quy_mo}");
                            }
                            else
                            {
                                Console.WriteLine("Không tìm thấy người dùng.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi khi lấy dữ liệu người dùng: " + ex.Message);
                    throw;
                }
            }


            return result;
        }


        public object GetUserDataEachMethod(string userName, string method)
        {
            object result = null; // Kết quả trả về có thể là bất kỳ kiểu dữ liệu nào
            string query = "";

            // Xây dựng câu lệnh SQL dựa trên phương thức (method)
            switch (method.ToLower())
            {
                case "username":
                    query = "SELECT user_name FROM [user] WHERE user_name = @userName";
                    break;
                case "email":
                    query = "SELECT email FROM [user] WHERE user_name = @userName"; // Tìm theo email
                    break;
                case "status":
                    query = "SELECT status FROM [user] WHERE user_name = @userName"; // Tìm theo status
                    break;
                case "id_xa":
                    query = "SELECT id_xa FROM [user] WHERE user_name = @userName"; // Tìm theo id_xa
                    break;
                case "id_quy_mo":
                    query = "SELECT id_quy_mo FROM [user] WHERE user_name = @userName"; // Tìm theo id_quy_mo
                    break;
                case "id_huyen":
                    query = "SELECT id_huyen FROM [user] WHERE user_name = @userName"; // Tìm theo id_huyen
                    break;
                case "id_co_so":
                    query = "SELECT id_co_so FROM [user] WHERE user_name = @userName"; // Tìm theo id_co_so
                    break;
                default:
                    throw new ArgumentException("Phương thức không hợp lệ: " + method); // Ném lỗi nếu method không hợp lệ
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open(); // Mở kết nối

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@userName", System.Data.SqlDbType.NVarChar).Value = userName;

                        // Truyền dữ liệu ra và kiểm tra nếu không có kết quả
                        result = cmd.ExecuteScalar();  // ExecuteScalar() sẽ trả về giá trị đầu tiên của trường

                        if (result == DBNull.Value)
                        {
                            result = null; // Nếu kết quả là DBNull, trả về null
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi nếu có
                    Console.WriteLine("Lỗi truy xuất dữ liệu người dùng: " + ex.Message);
                    throw; // Ném lại lỗi để có thể xử lý ở nơi khác
                }
            }

            return result; // Trả về giá trị trường cần lấy (null nếu không tìm thấy)
        }

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();  // List to hold users

            string query = "SELECT id, user_name, id_co_so, id_xa, id_huyen, id_quy_mo FROM [user]";  // SQL query

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open(); // Open the connection to the database

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Create a new User object and map data from SQL
                                User user = new User
                                {
                                    id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : 0,
                                    user_name = reader["user_name"].ToString(),
                                    id_co_so = reader["id_co_so"] != DBNull.Value ? Convert.ToInt32(reader["id_co_so"]) : 0,
                                    id_xa = reader["id_xa"] != DBNull.Value ? Convert.ToInt32(reader["id_xa"]) : 0,
                                    id_huyen = reader["id_huyen"] != DBNull.Value ? Convert.ToInt32(reader["id_huyen"]) : 0,
                                    id_quy_mo = reader["id_quy_mo"] != DBNull.Value ? Convert.ToInt32(reader["id_xa"]) : 0
                                };

                                // Add the user to the list
                                users.Add(user);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions and display error messages
                    MessageBox.Show($"Lỗi truy vấn dữ liệu người dùng: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return users;  // Return the list of users
        }
        // Phương thức xóa người dùng khỏi cơ sở dữ liệu
        public void DeleteUser(string userName)
        {
            string query = "DELETE FROM [user] WHERE user_name = @userName";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open(); // Mở kết nối

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@userName", System.Data.SqlDbType.NVarChar).Value = userName;

                        int rowsAffected = cmd.ExecuteNonQuery(); // Thực thi câu lệnh xóa

                        if (rowsAffected == 0)
                        {
                            throw new Exception("Không tìm thấy người dùng để xóa.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi nếu có
                    Console.WriteLine("Lỗi khi xóa người dùng: " + ex.Message);
                    throw;
                }
            }
        }
        public void UpdateUser(User user)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE [user] SET password = @password, email = @email, sdt = @sdt, status = @status, " +
                               "id_xa = @id_xa, id_huyen = @id_huyen, id_quy_mo = @id_quy_mo, id_co_so = @id_co_so " +
                               "WHERE user_name = @user_name";

                SqlCommand cmd = new SqlCommand(query, conn);

                // Thêm tham số vào câu lệnh SQL
                cmd.Parameters.AddWithValue("@user_name", user.user_name);

                // Kiểm tra nếu giá trị password không null hoặc rỗng
                cmd.Parameters.AddWithValue("@password", string.IsNullOrEmpty(user.password) ? DBNull.Value : (object)user.password);

                // Kiểm tra và thêm các tham số khác vào câu lệnh
                cmd.Parameters.AddWithValue("@email", string.IsNullOrEmpty(user.email) ? DBNull.Value : (object)user.email);
                cmd.Parameters.AddWithValue("@sdt", string.IsNullOrEmpty(user.sdt) ? DBNull.Value : (object)user.sdt);
                cmd.Parameters.AddWithValue("@status", user.status);
                cmd.Parameters.AddWithValue("@id_xa", user.id_xa == 0 ? DBNull.Value : (object)user.id_xa);
                cmd.Parameters.AddWithValue("@id_huyen", user.id_huyen == 0 ? DBNull.Value : (object)user.id_huyen);
                cmd.Parameters.AddWithValue("@id_quy_mo", user.id_quy_mo == 0 ? DBNull.Value : (object)user.id_quy_mo);
                cmd.Parameters.AddWithValue("@id_co_so", user.id_co_so == 0 ? DBNull.Value : (object)user.id_co_so);

                cmd.ExecuteNonQuery();  // Thực thi câu lệnh SQL
            }
        }

    }
}

