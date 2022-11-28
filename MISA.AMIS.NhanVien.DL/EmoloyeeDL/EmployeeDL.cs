using Dapper;
using MISA.AMIS.NhanVien.Common;
using MISA.AMIS.NhanVien.Common.Entities;
using MISA.AMIS.NhanVien.Common.Resources;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.NhanVien.DL
{
    public class EmployeeDL : BaseDL<Employee>, IEmployeeDL
    {
        /// <summary>
        /// Lấy mã nhân viên mới
        /// </summary>
        /// <returns>Mã mới</returns>
        public string GetNewEmployeeCode()
        {
            string connectionString = DatabaseContext.ConnectionString;
            var mySqlConnection = new MySqlConnection(connectionString);

            // chuẩn bị proc
            string sqlCommand = Resource.Proc_New_Code;

            var parameters = new DynamicParameters();
            parameters.Add("type", "employee");
            //parameters.Add("@out_number", dbType: DbType.String, direction: ParameterDirection.Output);
            var newCode = mySqlConnection.QueryFirstOrDefault<String>(sqlCommand, parameters, commandType: System.Data.CommandType.StoredProcedure);
            //string newCode = parameters.Get<string>("@out_number");
            if (newCode != null)
            {
                return newCode.ToString();
            }
            return "";
        }


        /// <summary>
        /// Xóa nhiều nhân viên
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int DeleteBatchEmployee(ListEmployeeIDs listEmpID)
        {
            var listRemove = "";           
            
            string connectionString = DatabaseContext.ConnectionString;
            var mySqlConnection = new MySqlConnection(connectionString);
            if (mySqlConnection.State == ConnectionState.Closed)
            {
                mySqlConnection.Open();
            }
            var transaction = mySqlConnection.BeginTransaction();
            try
            {
                // chuẩn bị proc
                string sqlCommand = Resource.Proc_employee_DeleteBatch;
                //var param
                foreach (var empID in listEmpID.EmployeeIDs)
                {
                    if (listRemove == "")
                    {
                        listRemove += "'" + empID + "'";
                    }
                    else
                    {
                        listRemove += "," + "'" + empID + "'";
                    }
                }

                var parameters = new DynamicParameters();
                parameters.Add("listId", listRemove);
                parameters.Add("countRows", dbType: DbType.Int32, direction: ParameterDirection.Output);
                mySqlConnection.Execute(sqlCommand, parameters, transaction, commandType:CommandType.StoredProcedure);
                int countRows = parameters.Get<int>("countRows");
                if (countRows == listEmpID.EmployeeIDs.Count)
                {
                    transaction.Commit();
                    if (mySqlConnection.State == ConnectionState.Open)
                    {
                        mySqlConnection.Close();
                    }
                    return countRows;
                }
                else
                {
                    transaction.Rollback();
                    if (mySqlConnection.State == ConnectionState.Open)
                    {
                        mySqlConnection.Close();
                    }
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                transaction.Rollback();
                throw ex;

            }
        }

        /// <summary>
        /// Tìm kiếm và phân trang
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="positionID"></param>
        /// <param name="departmentID"></param>
        /// <param name="sort"></param>
        /// <param name="pageSize" type="number"></param>
        /// <param name="pageNumber" type="number"></param>
        /// <returns>Danh sách các bản ghi</returns>
        public object Filter(string? keyword, string? JobPositionName, Guid? departmentID, string? sort = null, int? pageSize = 10, int? pageNumber = 1)
        {
            //lấy chuỗi kết nối
            var connectionString = DatabaseContext.ConnectionString;

            //lấy tên Procedure 
            string sqlCommand = Resource.Proc_employee_filter;

            //Khai báo tham số cho procedure
            var parameters = new DynamicParameters();
            parameters.Add("@keyword", keyword);
            parameters.Add("@JobPositionName", JobPositionName);
            parameters.Add("@DepartmentID", departmentID);
            parameters.Add("@limit", pageSize);
            parameters.Add("@offset", (pageNumber - 1) * pageSize);
            parameters.Add("@sort", sort);
            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                var result = new PaggingData();
                var multipleResults = mySqlConnection.QueryMultiple(sqlCommand, parameters, commandType: CommandType.StoredProcedure);
                if (multipleResults != null)
                {
                    var TotalPage = 0;
                    var employees = multipleResults.Read<Employee>().ToList();
                    var totalRecord = multipleResults.Read<int>().Single();
                    if (totalRecord == 0)
                    {
                        TotalPage = 1;
                    }
                    else
                    {
                        TotalPage = (double)totalRecord % (double)pageSize == 0 ? (int)((double)totalRecord / (double)pageSize) : (int)Math.Ceiling((double)totalRecord / (double)pageSize);
                    }

                    result.TotalPage = TotalPage;
                    result.Data = employees;
                    result.TotalRecord = totalRecord;

                }
                return result;
            }
        }

    }
}
