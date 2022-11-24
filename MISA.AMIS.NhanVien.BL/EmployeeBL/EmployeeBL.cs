using MISA.AMIS.NhanVien.BL.BaseBL;
using MISA.AMIS.NhanVien.Common;
using MISA.AMIS.NhanVien.Common.Entities;
using MISA.AMIS.NhanVien.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.NhanVien.BL
{
    public class EmployeeBL : BaseBL<Employee> ,IEmployeeBL
    {
        #region Field
        private IEmployeeDL _employeeDL;
        #endregion
        
        #region Constructor
        public EmployeeBL(IEmployeeDL employeeDL) : base(employeeDL)
        {
            _employeeDL = employeeDL;
        }

        
        #endregion

        /// <summary>
        /// Lấy thông tin 1 nhân viên theo id
        /// </summary>
        /// <param name="employeeID">ID của nhân viên muốn lấy</param>
        /// <returns>Thông tin của 1 nhân viên</returns>
        public string GetNewEmployeeCode()
        {
            return _employeeDL.GetNewEmployeeCode();
        }

        public int DeleteBatchEmployee(ListEmployeeIDs listEmpID)
        {
            return _employeeDL.DeleteBatchEmployee(listEmpID);
        }
        public object Filter(string? keyword, string? JobPositionName, Guid? departmentID, string? sort, int? pageSize = 10, int? pageNumber = 1)
        {
            return _employeeDL.Filter(keyword, JobPositionName, departmentID,sort,pageSize,pageNumber);
        }
        
    }
}
