using MISA.AMIS.NhanVien.Common;
using MISA.AMIS.NhanVien.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.NhanVien.DL
{
    public interface IEmployeeDL : IBaseDL<Employee>
    {
        /// <summary>
        /// Lấy mã nhân viên mới
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        public String GetNewEmployeeCode();

        /// <summary>
        /// Xóa nhiều bản ghi
        /// </summary>
        /// <returns>Số lượng bản ghi đc xóa</returns>
        public int DeleteBatchEmployee(ListEmployeeIDs listEmpID);

        /// <summary>
        /// Lọc nhân viên
        /// </summary>
        /// <param name="keyword">Từ khóa</param>
        /// <param name="positionID">id vị trí</param>
        /// <param name="departmentID">id đơn vị</param>
        /// <param name="sort">kiểu sắp xếp</param>
        /// <param name="pageSize">Số lượng bản ghi</param>
        /// <param name="pageNumber">Trang bắt đầu</param>
        /// <returns>Danh sách bản ghi</returns>
        public object Filter(string? keyword, string? JobPositionName, Guid? departmentID, string? sort, int? pageSize = 10, int? pageNumber = 1);

       
    }
    
}
