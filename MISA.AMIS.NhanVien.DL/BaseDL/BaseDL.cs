using Dapper;
using MISA.AMIS.NhanVien.Common;
using MISA.AMIS.NhanVien.Common.Entities;
using MISA.AMIS.NhanVien.Common.Resources;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.NhanVien.DL
{
    public class BaseDL<T> : IBaseDL<T>
    {
        /// <summary>
        /// lấy tất cả bản ghi
        /// </summary>
        /// <returns>Danh sách các bản ghi</returns>
        public IEnumerable<T> GetAllReCord()
        {
            //Khai báo kiểu dữ liệu trả về
            IEnumerable<T> result;

            //tạo liên kết với database
            var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString);

            // chuẩn bị proc
            string sqlCommand = String.Format("Proc_{0}_GetAll", typeof(T).Name);

            //thực hiện gọi vào DB
            result = mySqlConnection.Query<T>(sqlCommand, commandType: System.Data.CommandType.StoredProcedure);

            //Xử lý kết quả trả về
            if (result != null)
            {
                return result;
            }
            return result;
        }

        /// <summary>
        /// Lấy 1 bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID của bản ghi</param>
        /// <returns>1 bản ghi</returns>
        public T GetRecordByID(Guid recordID)
        {
            //Khai báo kiểu dữ liệu trả về
            T result;

            //tạo liên kết với database
            var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString);

            // chuẩn bị proc
            string sqlCommand = String.Format(Resource.Proc_GetByID, typeof(T).Name);
            var parameters = new DynamicParameters();
            parameters.Add($"@{typeof(T).Name}ID", recordID);
            //thực hiện gọi vào DB
            result = mySqlConnection.QueryFirstOrDefault<T>(sqlCommand, parameters, commandType: System.Data.CommandType.StoredProcedure);

            //Xử lý kết quả trả về
            if (result != null)
            {
                return result;
            }
            return result;
        }

        /// <summary>
        /// Xóa 1 bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID bản ghi cần xóa</param>
        /// <returns>ID của bản ghi khi xóa thành công</returns>
        public int DeleteRecord(Guid recordID)
        {
            var connectionString = DatabaseContext.ConnectionString;
            string sqlCommand = String.Format(Resource.Proc_Delete, typeof(T).Name);
            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                if (mySqlConnection.State != ConnectionState.Open)
                {
                    mySqlConnection.Open();
                }
                var transaction = mySqlConnection.BeginTransaction();
                var parameters = new DynamicParameters();
                parameters.Add($"@{typeof(T).Name}ID", recordID);
                var result = mySqlConnection.Execute(sqlCommand, parameters, transaction, commandType: System.Data.CommandType.StoredProcedure);
                if (result > 0)
                {
                    transaction.Commit();
                    if (mySqlConnection.State == ConnectionState.Open)
                    {
                        mySqlConnection.Close();
                    }
                    return result;
                }
                else
                {
                    transaction.Rollback();
                    if (mySqlConnection.State == ConnectionState.Open)
                    {
                        mySqlConnection.Close();
                    }
                    return result;
                }
            }

        }

        ///// <summary>
        ///// Thêm 1 bản ghi
        ///// </summary>
        ///// <param name="record">Thông tin của bản ghi</param>
        ///// <returns>bản ghi vừa thêm nếu thành công</returns>
        ///// Created : DTANH(11/11/2022)
        //public Guid InsertRecord(T record)
        //{
        //    //lấy chuỗi kết nối
        //    var connectionString = DatabaseContext.ConnectionString;
            
        //    //lấy tên Procedure 
        //    string sqlCommand = String.Format(Resource.Proc_Insert, typeof(T).Name);
            
        //    //Khai báo tham số cho procedure
        //    var parameters = new DynamicParameters();
            
        //    //lấy ra mảng các thuộc tính của Generic
        //    var properties = record.GetType().GetProperties();
            
        //    // Khai báo giá trị của thuộc tính
        //    object propValue;

        //    //Tạo Guid cho ID
        //    var newRecordID = Guid.NewGuid();
            
        //    //Lặp qua mảng các thuộc  tính
        //    foreach (var prop in properties)
        //    {
        //        // Kiểm tra xem có phải ID không
        //        var primaryKeyAttribute = KeyAttribute.GetCustomAttribute(prop, typeof(KeyAttribute));
                
        //        if (primaryKeyAttribute != null)
        //        {
        //            propValue = newRecordID;
        //        }else
        //        {                    
        //            propValue = prop.GetValue(record);
        //        }
        //        parameters.Add($"@{prop.Name}", propValue);
        //    }
        //    using(var mySqlConnection = new MySqlConnection(connectionString))
        //    {
        //        {
        //            mySqlConnection.Open();
        //            var transaction = mySqlConnection.BeginTransaction();
        //            var result = mySqlConnection.Execute(sqlCommand, parameters, transaction, commandType: System.Data.CommandType.StoredProcedure);
        //            if (result > 0)
        //            {
        //                transaction.Commit();
        //                mySqlConnection.Close();
        //                return newRecordID;
        //            }
        //            else
        //            {
        //                transaction.Rollback();
        //                mySqlConnection.Close();
        //                return Guid.Empty;
        //            }
        //        }
        //    }           
        //}

        ///// <summary>
        ///// Sửa 1 bản ghi theo ID
        ///// </summary>
        ///// <param name="recordID">ID của bản ghi cần sửa</param>
        ///// <param name="record">Thông tin của bản ghi</param>
        ///// <returns>Thông tin bản ghi sau khi chỉnh sửa thành công</returns>
        //public int UpdateRecord(Guid recordID, T record)
        //{
        //    //lấy chuỗi kết nối
        //    var connectionString = DatabaseContext.ConnectionString;

        //    //lấy tên Procedure 
        //    string sqlCommand = String.Format(Resource.Proc_Update, typeof(T).Name);

        //    //Khai báo tham số cho procedure
        //    var parameters = new DynamicParameters();

        //    //lấy ra mảng các thuộc tính của Generic
        //    var properties = record.GetType().GetProperties();

        //    object propValue;
            
        //    foreach (var prop in properties)
        //    {
        //        // Kiểm tra xem có phải ID không
        //        var primaryKeyAttribute = KeyAttribute.GetCustomAttribute(prop, typeof(KeyAttribute));

        //        if (primaryKeyAttribute != null)
        //        {
        //            propValue = recordID;
        //        }
        //        else
        //        {
        //            propValue = prop.GetValue(record);
        //        }
        //        parameters.Add($"@{prop.Name}", propValue);
        //    }

        //    using(var mySqlConnection = new MySqlConnection(connectionString))
        //    {
        //        mySqlConnection.Open();
        //        var transaction = mySqlConnection.BeginTransaction();
        //        var result = mySqlConnection.Execute(sqlCommand, parameters, transaction, commandType: System.Data.CommandType.StoredProcedure);
        //        if(result > 0)
        //        {
        //            transaction.Commit();
        //            mySqlConnection.Close();
        //            return result;
        //        }else
        //        {
        //            transaction.Rollback();
        //            mySqlConnection.Close();
        //            return result;
        //        }
        //    }
        //}

        /// <summary>
        /// Thêm hoặc sửa bản ghi
        /// </summary>
        /// <param name="recordID">ID của bản ghi nếu là sửa </param>
        /// <param name="record">Thông tin của bản ghi</param>
        /// <returns>Id của bản ghi</returns>
        public object UpdateOrInsert(Guid recordID, T record)
        {
            var newRecordID = Guid.Empty;
            string sqlCommand = "";
            
            //lấy chuỗi kết nối
            var connectionString = DatabaseContext.ConnectionString;
            
            //Khai báo tham số cho procedure
            var parameters = new DynamicParameters();
            //lấy ra mảng các thuộc tính của Generic
            var properties = record.GetType().GetProperties();
            if (recordID == Guid.Empty)
            {
                //lấy tên Procedure 
                sqlCommand = String.Format(Resource.Proc_Insert, typeof(T).Name);
                newRecordID = Guid.NewGuid();
            }else
            {
                //lấy tên Procedure 
                sqlCommand = String.Format(Resource.Proc_Update, typeof(T).Name);   
                newRecordID = recordID;
            }
            
            object propValue;

            foreach (var prop in properties)
            {
                // Kiểm tra xem có phải ID không
                var primaryKeyAttribute = KeyAttribute.GetCustomAttribute(prop, typeof(KeyAttribute));

                if (primaryKeyAttribute != null)
                {
                    propValue = newRecordID;
                }
                else
                {
                    if(prop.GetValue(record) == null)
                    {
                        propValue = prop.GetValue(record);
                    }else
                    {
                        propValue = prop.GetValue(record).ToString().Trim() == "" ? null : prop.GetValue(record);                        
                    }
                }
                parameters.Add($"@{prop.Name}", propValue);
            }

            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                if (mySqlConnection.State != ConnectionState.Open)
                {
                    mySqlConnection.Open();
                }                
                var transaction = mySqlConnection.BeginTransaction();
                var result = mySqlConnection.Execute(sqlCommand, parameters, transaction, commandType: System.Data.CommandType.StoredProcedure);
                if (result > 0)
                {
                    transaction.Commit();
                    if (mySqlConnection.State == ConnectionState.Open)
                    {
                        mySqlConnection.Close();
                    }
                    return newRecordID;
                }
                else
                {
                    transaction.Rollback();
                    if (mySqlConnection.State == ConnectionState.Open)
                    {
                        mySqlConnection.Close();
                    }
                    return result;
                }
            }
        }
        
        /// <summary>
        /// Tìm kiếm bản ghi theo mã
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <returns></returns>
        public ResponseData CheckDuplicate(string employeeCode)
        {
            //lấy chuỗi kết nối
            var connectionString = DatabaseContext.ConnectionString;

            //lấy tên Procedure 
            string sqlCommand = String.Format("Proc_{0}_FindByCode", typeof(T).Name);

            var parameters = new DynamicParameters();
            parameters.Add("recordCode", employeeCode);
            
            parameters.Add("empID", dbType: DbType.String, direction: ParameterDirection.InputOutput);
            parameters.Add("empCode", dbType: DbType.String, direction: ParameterDirection.InputOutput);
            
            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                var multipleResults = mySqlConnection.Query(sqlCommand, parameters, commandType: CommandType.StoredProcedure);
                Guid? recordID = parameters.Get<Guid?>("empID");
                string? recordCode = parameters.Get<string?>("empCode");
                return new ResponseData(true, new
                {
                    recordID = recordID,
                    recordCode = recordCode,
                });

            }
            //return new ResponseData(true, null);
        }
    }
}
