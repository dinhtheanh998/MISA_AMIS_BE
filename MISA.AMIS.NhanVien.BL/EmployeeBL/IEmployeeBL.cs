using MISA.AMIS.NhanVien.BL.BaseBL;
using MISA.AMIS.NhanVien.Common;
using MISA.AMIS.NhanVien.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.NhanVien.BL
{
    public interface IEmployeeBL : IBaseBL<Employee>
    {
        
        /// <summary>
        /// lấy mã nhân viên mới
        /// </summary>
        /// <returns>mã nhân viên mới</returns>
        public String GetNewEmployeeCode();
        /// <summary>
        /// Xóa nhiều nhân viên
        /// </summary>
        /// <param name="listEmpID">mảng các id cần xóa</param>
        /// <returns>Số lượng bản ghi đã xóa</returns>
        public int DeleteBatchEmployee(ListEmployeeIDs listEmpID);
        /// <summary>
        /// Tìm kiếm phân trang
        /// </summary>
        /// <param name="keyword">từ khóa</param>
        /// <param name="JobPositionName"></param>
        /// <param name="departmentID"></param>
        /// <param name="sort"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns>mảng nhân viên</returns>
        public object Filter(string? keyword, string? JobPositionName, Guid? departmentID, string? sort, int? pageSize = 10, int? pageNumber = 1);

        /// <summary>
        /// Xuất khẩu ra file excel
        /// </summary>
        /// <returns></returns>
        public ResponseData ExportExcel();
        
    }
}
