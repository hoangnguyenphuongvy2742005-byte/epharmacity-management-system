using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NhaThuoc
{
    internal class DatabaseHelper
    {
        public class DatabaseConnection
        {
            
            public string connection = "Data Source=Thao-PC\\SQLEXPRESS;Initial Catalog=NhaThuocDB1;User ID=sa;Password=123;Integrated Security=True";
            public bool TestConnection()
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connection))
                    {
                        conn.Open();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi kết nối database:\n{ex.Message}\n\n" +
                                  "Vui lòng kiểm tra:\n" +
                                  "1. SQL Server đang chạy\n" +
                                  "2. Database 'NhaThuocDB' đã được tạo\n" +
                                  "3. Chạy script CreateDatabaseOnly.sql", 
                                  "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            public DataTable GetData(string query, params SqlParameter[] parameters)
            {
                using (SqlConnection conn = new SqlConnection(connection))
                {
                conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                cmd.Parameters.AddRange(parameters);
                        
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
                }
            }
            public int ExecuteNonQuery(string query, params SqlParameter[] parameters)
            {
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                            cmd.Parameters.AddRange(parameters);
                        
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            public object ExecuteScalar(string query, params SqlParameter[] parameters)
            {
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                            cmd.Parameters.AddRange(parameters);
                        
                        return cmd.ExecuteScalar();
                    }
                }
            }
            public bool IsPhoneNumberExists(string phoneNumber)
            {
                string query = "SELECT COUNT(*) FROM Thongtinkhachhang WHERE Sodienthoai = @phone";
                SqlParameter param = new SqlParameter("@phone", phoneNumber);
                int count = Convert.ToInt32(ExecuteScalar(query, param));
                return count > 0;
            }
            public DataTable GetCustomerByPhone(string phoneNumber)
            {
                string query = @"SELECT * FROM Thongtinkhachhang 
                               WHERE Sodienthoai = @phone";
                SqlParameter param = new SqlParameter("@phone", phoneNumber);
                return GetData(query, param);
            }
            public DataTable GetOrderHistory(string searchCriteria = "", string searchValue = "", DateTime? fromDate = null, DateTime? toDate = null)
            {
                string storedProcedure = "sp_GetOrderHistory";
                
                SqlParameter[] parameters = {
                    new SqlParameter("@SearchCriteria", searchCriteria ?? ""),
                    new SqlParameter("@SearchValue", searchValue ?? ""),
                    new SqlParameter("@FromDate", fromDate.HasValue ? (object)fromDate.Value : DBNull.Value),
                    new SqlParameter("@ToDate", toDate.HasValue ? (object)toDate.Value : DBNull.Value)
                };

                return GetDataFromStoredProcedure(storedProcedure, parameters);
            }
            public DataTable GetOrderDetails(int orderId)
            {
                string storedProcedure = "sp_GetOrderDetails";
                SqlParameter param = new SqlParameter("@OrderId", orderId);
                return GetDataFromStoredProcedure(storedProcedure, param);
            }
            public DataTable GetAvailableProducts()
            {
                string storedProcedure = "sp_GetAvailableProducts";
                return GetDataFromStoredProcedure(storedProcedure);
            }
            public int GetProductStock(int productId)
            {
                DataTable result = GetDataFromStoredProcedure("sp_CheckProductStock", 
                    new SqlParameter("@ProductId", productId));
                
                if (result.Rows.Count > 0)
                {
                    if (result.Columns.Contains("Kho") && result.Rows[0]["Kho"] != DBNull.Value)
                        return Convert.ToInt32(result.Rows[0]["Kho"]);
                    if (result.Columns.Contains("SoLuongTon") && result.Rows[0]["SoLuongTon"] != DBNull.Value)
                        return Convert.ToInt32(result.Rows[0]["SoLuongTon"]);
                    return 0;
                }
                return 0;
            }
            public int CreateOrder(string customerPhone, string employeeId)
            {
                SqlParameter orderIdParam = new SqlParameter("@OrderId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                
                SqlParameter[] parameters = {
                    new SqlParameter("@CustomerPhone", customerPhone),
                    new SqlParameter("@EmployeeId", employeeId),
                    orderIdParam
                };
                
                ExecuteStoredProcedure("sp_CreateOrder", parameters);
                
                return Convert.ToInt32(orderIdParam.Value);
            }
            public void AddOrderDetail(int orderId, int productId, int quantity, decimal price)
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@OrderId", orderId),
                    new SqlParameter("@ProductId", productId),
                    new SqlParameter("@Quantity", quantity),
                    new SqlParameter("@Price", price)
                };
                
                ExecuteStoredProcedure("sp_AddOrderDetail", parameters);
            }
            public void UpdateProductStock(int productId, int newQuantity)
            {
                string query = "UPDATE Sanphamthuoc SET Soluong = @quantity WHERE ID = @productId";
                SqlParameter[] parameters = {
                    new SqlParameter("@quantity", newQuantity),
                    new SqlParameter("@productId", productId)
                };
                
                ExecuteNonQuery(query, parameters);
            }
            public bool IsEmployeeExists(string employeeId)
            {
                string query = "SELECT COUNT(*) FROM Taikhoan WHERE Manguoidung = @employeeId";
                SqlParameter param = new SqlParameter("@employeeId", employeeId);
                int count = Convert.ToInt32(ExecuteScalar(query, param));
                return count > 0;
            }
            public bool ValidateDatabaseIntegrity()
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connection))
                    {
                        conn.Open();
                    }
                    EnsureDefaultEmployeeExists();
                    string[] requiredTables = { "Taikhoan", "Thongtinkhachhang", "Donhang", "Sanphamthuoc" };
                    foreach (string table in requiredTables)
                    {
                        string checkQuery = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{table}'";
                        int count = Convert.ToInt32(ExecuteScalar(checkQuery));
                        if (count == 0)
                        {
                            throw new Exception($"Bảng {table} không tồn tại trong database.");
                        }
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Database validation failed: {ex.Message}");
                    return false;
                }
            }
            public DataTable GetDataFromStoredProcedure(string storedProcedureName, params SqlParameter[] parameters)
            {
                DataTable dataTable = new DataTable();
                
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        
                        conn.Open();
                        
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
                
                return dataTable;
            }
            private void ExecuteStoredProcedure(string storedProcedureName, params SqlParameter[] parameters)
            {
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            public void EnsureDefaultEmployeeExists()
            {
                try
                {
                    ExecuteStoredProcedure("sp_EnsureDefaultEmployee");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error ensuring default employee: {ex.Message}");
                }
            }
            public void CreateCustomer(string phoneNumber, string fullName, string employeeId)
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@PhoneNumber", phoneNumber),
                    new SqlParameter("@FullName", fullName),
                    new SqlParameter("@EmployeeId", employeeId)
                };
                
                ExecuteStoredProcedure("sp_CreateCustomer", parameters);
            }
            public DataTable CheckCustomerPhone(string phoneNumber)
            {
                return GetDataFromStoredProcedure("sp_CheckCustomerPhone", 
                    new SqlParameter("@PhoneNumber", phoneNumber));
            }
            public DataTable GetOrderStatistics(DateTime? fromDate = null, DateTime? toDate = null)
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@FromDate", fromDate.HasValue ? (object)fromDate.Value : DBNull.Value),
                    new SqlParameter("@ToDate", toDate.HasValue ? (object)toDate.Value : DBNull.Value)
                };
                
                return GetDataFromStoredProcedure("sp_GetOrderStatistics", parameters);
            }
            public DataTable GetAllCustomers()
            {
                string query = @"
                    SELECT Hovaten, Sodienthoai, Diemtichluy
                    FROM Thongtinkhachhang 
                    ORDER BY Sodienthoai DESC";
                return GetData(query);
            }
            public DataTable SearchCustomers(string searchTerm)
            {
                string query = @"
                    SELECT Hovaten, Sodienthoai, Diemtichluy
                    FROM Thongtinkhachhang 
                    WHERE Sodienthoai LIKE @SearchTerm 
                       OR Hovaten LIKE @SearchTerm
                    ORDER BY Sodienthoai DESC";
                
                SqlParameter[] parameters = {
                    new SqlParameter("@SearchTerm", "%" + searchTerm + "%")
                };
                
                return GetData(query, parameters);
            }
            public bool UpdateCustomer(string phoneNumber, string fullName, int points)
            {
                try
                {
                    string updateQuery = @"
                        UPDATE Thongtinkhachhang 
                        SET Hovaten = @FullName, 
                            Diemtichluy = @Points
                        WHERE Sodienthoai = @PhoneNumber";
                    
                    SqlParameter[] updateParams = {
                        new SqlParameter("@PhoneNumber", phoneNumber),
                        new SqlParameter("@FullName", fullName),
                        new SqlParameter("@Points", points)
                    };
                    
                    int rowsAffected = ExecuteNonQuery(updateQuery, updateParams);
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi cập nhật khách hàng: {ex.Message}", 
                                  "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            private void ExecuteQuery(string query, params SqlParameter[] parameters)
            {
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            public int CalculatePointsFromOrder(decimal orderValue)
            {
                return (int)Math.Floor(orderValue / 1000);
            }
            public bool AddPointsToCustomer(string phoneNumber, int points)
            {
                try
                {
                    string query = @"
                        UPDATE Thongtinkhachhang 
                        SET Diemtichluy = Diemtichluy + @Points
                        WHERE Sodienthoai = @PhoneNumber";
                    
                    SqlParameter[] parameters = {
                        new SqlParameter("@PhoneNumber", phoneNumber),
                        new SqlParameter("@Points", points)
                    };
                    
                    ExecuteQuery(query, parameters);
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi cộng điểm tích lũy: {ex.Message}", 
                                  "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool AddPointsFromOrder(string phoneNumber, decimal orderValue)
            {
                int points = CalculatePointsFromOrder(orderValue);
                if (points > 0)
                {
                    return AddPointsToCustomer(phoneNumber, points);
                }
                return true; 
            }
            public int GetCustomerPoints(string phoneNumber)
            {
                try
                {
                    string query = "SELECT Diemtichluy FROM Thongtinkhachhang WHERE Sodienthoai = @PhoneNumber";
                    SqlParameter param = new SqlParameter("@PhoneNumber", phoneNumber);
                    object result = ExecuteScalar(query, param);
                    return result != null ? Convert.ToInt32(result) : 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lấy điểm tích lũy: {ex.Message}", 
                                  "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                }
            }
            public bool RedeemCustomerPoints(string phoneNumber, int points)
            {
                try
                {
                    // Đảm bảo không trừ quá số điểm hiện có
                    string query = @"
                        UPDATE Thongtinkhachhang 
                        SET Diemtichluy = Diemtichluy - @Points
                        WHERE Sodienthoai = @PhoneNumber AND Diemtichluy >= @Points";

                    SqlParameter[] parameters = {
                        new SqlParameter("@PhoneNumber", phoneNumber),
                        new SqlParameter("@Points", points)
                    };

                    int affected = ExecuteNonQuery(query, parameters);
                    if (affected == 0)
                    {
                        MessageBox.Show("Không đủ điểm để áp dụng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi trừ điểm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool CheckStockAvailability(List<OrderItem> orderItems, out List<string> insufficientStockItems)
            {
                insufficientStockItems = new List<string>();
                bool hasInsufficientStock = false;

                foreach (var item in orderItems)
                {
                    int currentStock = GetProductStock(item.ProductId);
                    if (currentStock < item.Quantity)
                    {
                        insufficientStockItems.Add($"{item.ProductName}: Cần {item.Quantity}, Có {currentStock}");
                        hasInsufficientStock = true;
                    }
                }
                return !hasInsufficientStock;
            }

            // Transaction methods
            private SqlTransaction currentTransaction;
            private SqlConnection transactionConnection;

            public void BeginTransaction()
            {
                transactionConnection = new SqlConnection(connection);
                transactionConnection.Open();
                currentTransaction = transactionConnection.BeginTransaction();
            }

            public void CommitTransaction()
            {
                if (currentTransaction != null)
                {
                    currentTransaction.Commit();
                    currentTransaction.Dispose();
                    transactionConnection.Close();
                    transactionConnection.Dispose();
                    currentTransaction = null;
                    transactionConnection = null;
                }
            }

            public void RollbackTransaction()
            {
                if (currentTransaction != null)
                {
                    currentTransaction.Rollback();
                    currentTransaction.Dispose();
                    transactionConnection.Close();
                    transactionConnection.Dispose();
                    currentTransaction = null;
                    transactionConnection = null;
                }
            }

            public int ExecuteNonQueryWithTransaction(string query, params SqlParameter[] parameters)
            {
                if (currentTransaction == null)
                {
                    throw new InvalidOperationException("Transaction chưa được bắt đầu. Gọi BeginTransaction() trước.");
                }

                using (SqlCommand cmd = new SqlCommand(query, transactionConnection, currentTransaction))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    
                    return cmd.ExecuteNonQuery();
                }
            }

            private static string connectionString;

            // Fix for CS1520: Method must have a return type
            // Change 'static DatabaseHelper()' to a static constructor for the DatabaseConnection class

            static DatabaseConnection()
            {
                connectionString = "Data Source=Thao-PC\\SQLEXPRESS;Initial Catalog=NhaThuocDB1;User ID=sa;Password=123;Integrated Security=True";
            }


            private static bool IsConnectionConfigured()
            {
                return !string.IsNullOrEmpty(connectionString);
            }

            public static DataTable GetAllThuoc()
            {
                DataTable dt = new DataTable();
                if (!IsConnectionConfigured()) return dt;
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = @"
                        SELECT 
                            sp.ID,
                            lt.TenLoai as 'Kiểu hàng',
                            nt.TenNhom as 'Nhóm hàng',
                            sp.Mahanghoa as 'Mã hàng',
                            sp.Tenhang as 'Tên hàng',
                            sp.Soluong as 'Số lượng',
                            sp.Thanhphan as 'Hoạt chất',
                            sp.Nhasanxuat as 'Nhà sản xuất',
                            sp.Gianhap as 'Giá nhập',
                            sp.Kho as 'Tồn kho',
                            sp.Ngayhethan as 'Ngày hết hạn',
                            sp.Donggoi as 'Đóng gói',
                            sp.Giaban as 'Giá bán'
                        FROM Sanphamthuoc sp
                        INNER JOIN LoaiThuoc lt ON sp.MaLoai = lt.MaLoai
                        INNER JOIN NhomThuoc nt ON lt.MaNhom = nt.MaNhom
                        ORDER BY sp.ID";

                        SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return dt;
            }

            public static DataTable SearchThuoc(string keyword)
            {
                DataTable dt = new DataTable();
                if (!IsConnectionConfigured()) return dt;
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = @"
                        SELECT 
                            sp.ID,
                            lt.TenLoai as 'Kiểu hàng',
                            nt.TenNhom as 'Nhóm hàng',
                            sp.Mahanghoa as 'Mã hàng',
                            sp.Tenhang as 'Tên hàng',
                            sp.Soluong as 'Số lượng',
                            sp.Thanhphan as 'Hoạt chất',
                            sp.Nhasanxuat as 'Nhà sản xuất',
                            sp.Gianhap as 'Giá nhập',
                            sp.Kho as 'Tồn kho',
                            sp.Ngayhethan as 'Ngày hết hạn',
                            sp.Donggoi as 'Đóng gói',
                            sp.Giaban as 'Giá bán'
                        FROM Sanphamthuoc sp
                        INNER JOIN LoaiThuoc lt ON sp.MaLoai = lt.MaLoai
                        INNER JOIN NhomThuoc nt ON lt.MaNhom = nt.MaNhom
                        WHERE sp.Tenhang LIKE @keyword 
                           OR sp.Mahanghoa LIKE @keyword 
                           OR nt.TenNhom LIKE @keyword
                        ORDER BY sp.ID";

                        SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                        adapter.SelectCommand.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return dt;
            }

            public static bool AddThuoc(Dictionary<string, object> thuocData)
            {
                if (!IsConnectionConfigured()) return false;
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand("sp_AddThuoc", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            // Thêm các tham số đầu vào
                            cmd.Parameters.AddWithValue("@MaLoai", Convert.ToInt32(thuocData["MaLoai"]));
                            cmd.Parameters.AddWithValue("@Mahanghoa", thuocData["Mahanghoa"] ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Mavach", thuocData["Mavach"] ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Tenhang", Convert.ToString(thuocData["Tenhang"]));
                            cmd.Parameters.AddWithValue("@Soluong", Convert.ToInt32(thuocData["Soluong"]));
                            cmd.Parameters.AddWithValue("@Mathuoc", thuocData["Mathuoc"] ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Thanhphan", thuocData["Thanhphan"] ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Nhasanxuat", thuocData["Nhasanxuat"] ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Donggoi", thuocData["Donggoi"] ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Gianhap", Convert.ToDecimal(thuocData["Gianhap"]));
                            cmd.Parameters.AddWithValue("@Kho", Convert.ToInt32(thuocData["Kho"]));
                            cmd.Parameters.AddWithValue("@Mota", thuocData["Mota"] ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Lohang", thuocData["Lohang"] ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Ngayhethan", thuocData["Ngayhethan"] ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Giaban", Convert.ToDecimal(thuocData["Giaban"]));

                            // Thêm các tham số output
                            SqlParameter resultParam = new SqlParameter("@Result", SqlDbType.Int);
                            resultParam.Direction = ParameterDirection.Output;
                            cmd.Parameters.Add(resultParam);

                            SqlParameter messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 500);
                            messageParam.Direction = ParameterDirection.Output;
                            cmd.Parameters.Add(messageParam);

                            cmd.ExecuteNonQuery();

                            int result = Convert.ToInt32(resultParam.Value);
                            string message = messageParam.Value?.ToString() ?? "";

                            if (result == 1)
                            {
                                MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return true;
                            }
                            else
                            {
                                MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm thuốc: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            public static bool UpdateThuoc(int id, Dictionary<string, object> thuocData)
            {
                if (!IsConnectionConfigured()) return false;
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand("sp_UpdateThuoc", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            // Thêm các tham số đầu vào
                            cmd.Parameters.AddWithValue("@ID", id);
                            cmd.Parameters.AddWithValue("@MaLoai", Convert.ToInt32(thuocData["MaLoai"]));
                            cmd.Parameters.AddWithValue("@Mahanghoa", thuocData["Mahanghoa"] ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Mavach", thuocData["Mavach"] ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Tenhang", Convert.ToString(thuocData["Tenhang"]));
                            cmd.Parameters.AddWithValue("@Soluong", Convert.ToInt32(thuocData["Soluong"]));
                            cmd.Parameters.AddWithValue("@Mathuoc", thuocData["Mathuoc"] ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Thanhphan", thuocData["Thanhphan"] ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Nhasanxuat", thuocData["Nhasanxuat"] ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Donggoi", thuocData["Donggoi"] ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Gianhap", Convert.ToDecimal(thuocData["Gianhap"]));
                            cmd.Parameters.AddWithValue("@Kho", Convert.ToInt32(thuocData["Kho"]));
                            cmd.Parameters.AddWithValue("@Mota", thuocData["Mota"] ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Lohang", thuocData["Lohang"] ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Ngayhethan", thuocData["Ngayhethan"] ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Giaban", Convert.ToDecimal(thuocData["Giaban"]));

                            // Thêm các tham số output
                            SqlParameter resultParam = new SqlParameter("@Result", SqlDbType.Int);
                            resultParam.Direction = ParameterDirection.Output;
                            cmd.Parameters.Add(resultParam);

                            SqlParameter messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 500);
                            messageParam.Direction = ParameterDirection.Output;
                            cmd.Parameters.Add(messageParam);

                            cmd.ExecuteNonQuery();

                            int result = Convert.ToInt32(resultParam.Value);
                            string message = messageParam.Value?.ToString() ?? "";

                            if (result == 1)
                            {
                                MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return true;
                            }
                            else
                            {
                                MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật thuốc: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            public static bool DeleteThuoc(int id)
            {
                if (!IsConnectionConfigured()) return false;
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand("sp_DeleteThuoc", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            // Thêm tham số đầu vào
                            cmd.Parameters.AddWithValue("@ID", id);

                            // Thêm các tham số output
                            SqlParameter resultParam = new SqlParameter("@Result", SqlDbType.Int);
                            resultParam.Direction = ParameterDirection.Output;
                            cmd.Parameters.Add(resultParam);

                            SqlParameter messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 500);
                            messageParam.Direction = ParameterDirection.Output;
                            cmd.Parameters.Add(messageParam);

                            cmd.ExecuteNonQuery();

                            int result = Convert.ToInt32(resultParam.Value);
                            string message = messageParam.Value?.ToString() ?? "";

                            if (result == 1)
                            {
                                MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return true;
                            }
                            else
                            {
                                MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa thuốc: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            public static DataRow GetThuocById(int id)
            {
                if (!IsConnectionConfigured()) return null;
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_GetThuocById", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ID", id);

                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lấy thông tin thuốc: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }

            public static DataTable GetLoaiThuoc()
            {
                DataTable dt = new DataTable();
                if (!IsConnectionConfigured()) return dt;
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = "SELECT MaLoai, TenLoai, MaNhom FROM LoaiThuoc ORDER BY TenLoai";
                        SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải loại thuốc: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return dt;
            }

            public static DataTable GetNhomThuoc()
            {
                DataTable dt = new DataTable();
                if (!IsConnectionConfigured()) return dt;
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = "SELECT MaNhom, TenNhom FROM NhomThuoc ORDER BY TenNhom";
                        SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải nhóm thuốc: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return dt;
            }

            public static DataTable GetThuocTonKhoThap()
            {
                DataTable dt = new DataTable();
                if (!IsConnectionConfigured()) return dt;
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = @"
                        SELECT 
                            sp.ID,
                            sp.Tenhang,
                            sp.Kho,
                            nt.TenNhom
                        FROM Sanphamthuoc sp
                        INNER JOIN LoaiThuoc lt ON sp.MaLoai = lt.MaLoai
                        INNER JOIN NhomThuoc nt ON lt.MaNhom = nt.MaNhom
                        WHERE sp.Kho < 10
                        ORDER BY sp.Kho";

                        SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải thuốc tồn kho thấp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return dt;
            }

            public static DataTable GetThuocSapHetHan()
            {
                DataTable dt = new DataTable();
                if (!IsConnectionConfigured()) return dt;
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = @"
                        SELECT 
                            sp.ID,
                            sp.Tenhang,
                            sp.Ngayhethan,
                            DATEDIFF(day, GETDATE(), sp.Ngayhethan) as 'Số ngày còn lại',
                            nt.TenNhom
                        FROM Sanphamthuoc sp
                        INNER JOIN LoaiThuoc lt ON sp.MaLoai = lt.MaLoai
                        INNER JOIN NhomThuoc nt ON lt.MaNhom = nt.MaNhom
                        WHERE sp.Ngayhethan IS NOT NULL 
                          AND DATEDIFF(day, GETDATE(), sp.Ngayhethan) < 30
                          AND DATEDIFF(day, GETDATE(), sp.Ngayhethan) >= 0
                        ORDER BY sp.Ngayhethan";

                        SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải thuốc sắp hết hạn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return dt;
            }

            public static DataTable GetThuocDaHetHan()
            {
                DataTable dt = new DataTable();
                if (!IsConnectionConfigured()) return dt;
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = @"
                        SELECT 
                            sp.ID,
                            sp.Tenhang,
                            sp.Ngayhethan,
                            DATEDIFF(day, sp.Ngayhethan, GETDATE()) as 'Số ngày đã hết hạn',
                            nt.TenNhom
                        FROM Sanphamthuoc sp
                        INNER JOIN LoaiThuoc lt ON sp.MaLoai = lt.MaLoai
                        INNER JOIN NhomThuoc nt ON lt.MaNhom = nt.MaNhom
                        WHERE sp.Ngayhethan IS NOT NULL 
                          AND sp.Ngayhethan < GETDATE()
                        ORDER BY sp.Ngayhethan";

                        SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải thuốc đã hết hạn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return dt;
            }

            public static Dictionary<string, int> GetThongKeNhanh()
            {
                Dictionary<string, int> thongKe = new Dictionary<string, int>
            {
                {"TongSoThuoc", 0},
                {"TongGiaTriKho", 0},
                {"ThuocSapHetHan", 0},
                {"ThuocTonKhoThap", 0}
            };
                if (!IsConnectionConfigured()) return thongKe;
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        string query1 = "SELECT COUNT(*) FROM Sanphamthuoc";
                        using (SqlCommand cmd = new SqlCommand(query1, conn))
                        {
                            thongKe["TongSoThuoc"] = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        string query2 = "SELECT ISNULL(SUM(Gianhap * Kho), 0) FROM Sanphamthuoc";
                        using (SqlCommand cmd = new SqlCommand(query2, conn))
                        {
                            thongKe["TongGiaTriKho"] = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        string query3 = @"
                        SELECT COUNT(*) FROM Sanphamthuoc 
                        WHERE Ngayhethan IS NOT NULL 
                          AND DATEDIFF(day, GETDATE(), Ngayhethan) < 30";
                        using (SqlCommand cmd = new SqlCommand(query3, conn))
                        {
                            thongKe["ThuocSapHetHan"] = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        string query4 = "SELECT COUNT(*) FROM Sanphamthuoc WHERE Kho < 10";
                        using (SqlCommand cmd = new SqlCommand(query4, conn))
                        {
                            thongKe["ThuocTonKhoThap"] = Convert.ToInt32(cmd.ExecuteScalar());
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lấy thống kê: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return thongKe;
            }

            public static DataTable GetBaoCaoTonKho()
            {
                DataTable dt = new DataTable();
                if (!IsConnectionConfigured()) return dt;
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = @"
                        SELECT 
                            sp.ID,
                            sp.Tenhang as 'Tên hàng',
                            nt.TenNhom as 'Nhóm hàng',
                            sp.Kho as 'Tồn kho',
                            sp.Gianhap as 'Giá nhập',
                            (sp.Kho * sp.Gianhap) as 'Giá trị tồn kho',
                            sp.Ngayhethan as 'Ngày hết hạn'
                        FROM Sanphamthuoc sp
                        INNER JOIN LoaiThuoc lt ON sp.MaLoai = lt.MaLoai
                        INNER JOIN NhomThuoc nt ON lt.MaNhom = nt.MaNhom
                        ORDER BY sp.Kho";

                        SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tạo báo cáo tồn kho: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return dt;
            }

            public static DataTable GetBaoCaoSapHetHan()
            {
                DataTable dt = new DataTable();
                if (!IsConnectionConfigured()) return dt;
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = @"
                        SELECT 
                            sp.ID,
                            sp.Tenhang as 'Tên hàng',
                            nt.TenNhom as 'Nhóm hàng',
                            sp.Kho as 'Tồn kho',
                            sp.Ngayhethan as 'Ngày hết hạn',
                            DATEDIFF(day, GETDATE(), sp.Ngayhethan) as 'Số ngày còn lại'
                        FROM Sanphamthuoc sp
                        INNER JOIN LoaiThuoc lt ON sp.MaLoai = lt.MaLoai
                        INNER JOIN NhomThuoc nt ON lt.MaNhom = nt.MaNhom
                        WHERE sp.Ngayhethan IS NOT NULL 
                          AND DATEDIFF(day, GETDATE(), sp.Ngayhethan) < 30
                        ORDER BY sp.Ngayhethan";

                        SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tạo báo cáo sắp hết hạn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return dt;
            }

            public static DataTable GetBaoCaoTheoNhom()
            {
                DataTable dt = new DataTable();
                if (!IsConnectionConfigured()) return dt;
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = @"
                        SELECT 
                            nt.TenNhom as 'Nhóm thuốc',
                            COUNT(sp.ID) as 'Số lượng thuốc',
                            SUM(sp.Kho) as 'Tổng tồn kho',
                            AVG(sp.Gianhap) as 'Giá nhập trung bình',
                            SUM(sp.Kho * sp.Gianhap) as 'Tổng giá trị'
                        FROM NhomThuoc nt
                        LEFT JOIN LoaiThuoc lt ON nt.MaNhom = lt.MaNhom
                        LEFT JOIN Sanphamthuoc sp ON lt.MaLoai = sp.MaLoai
                        GROUP BY nt.TenNhom
                        ORDER BY nt.TenNhom";

                        SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tạo báo cáo theo nhóm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return dt;
            }

            public static DataTable GetBaoCaoTongHop()
            {
                DataTable dt = new DataTable();
                if (!IsConnectionConfigured()) return dt;
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = @"
                        SELECT 
                            N'Tổng số thuốc' as N'Chỉ số',
                            COUNT(*) as N'Giá trị'
                        FROM Sanphamthuoc
                        UNION ALL
                        SELECT 
                            N'Tổng giá trị kho (VNĐ)' as N'Chỉ số',
                            SUM(Gianhap * Kho) as N'Giá trị'
                        FROM Sanphamthuoc
                        UNION ALL
                        SELECT 
                            N'Thuốc tồn kho thấp' as N'Chỉ số',
                            COUNT(*) as N'Giá trị'
                        FROM Sanphamthuoc WHERE Kho < 10
                        UNION ALL
                        SELECT 
                            N'Thuốc sắp hết hạn' as N'Chỉ số',
                            COUNT(*) as N'Giá trị'
                        FROM Sanphamthuoc 
                        WHERE Ngayhethan IS NOT NULL 
                          AND DATEDIFF(day, GETDATE(), Ngayhethan) < 30
                        UNION ALL
                        SELECT 
                            N'Thuốc đã hết hạn' as N'Chỉ số',
                            COUNT(*) as N'Giá trị'
                        FROM Sanphamthuoc 
                        WHERE Ngayhethan IS NOT NULL 
                          AND Ngayhethan < GETDATE()";

                        SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tạo báo cáo tổng hợp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return dt;
            }
        }
    }
}
