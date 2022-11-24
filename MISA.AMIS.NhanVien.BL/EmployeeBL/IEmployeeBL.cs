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
        public int DeleteBatchEmployee(ListEmployeeIDs listEmpID);
        public object Filter(string? keyword, string? JobPositionName, Guid? departmentID, string? sort, int? pageSize = 10, int? pageNumber = 1);
        
        
    }
}
