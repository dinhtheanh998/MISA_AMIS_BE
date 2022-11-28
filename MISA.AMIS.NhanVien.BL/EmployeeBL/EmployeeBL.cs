using MISA.AMIS.NhanVien.BL.BaseBL;
using MISA.AMIS.NhanVien.Common;
using MISA.AMIS.NhanVien.Common.Entities;
using MISA.AMIS.NhanVien.Common.Resources;
using MISA.AMIS.NhanVien.DL;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.Style;

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

        /// <summary>
        /// Xóa nhiều nhân viên
        /// </summary>
        /// <param name="listEmpID">mảng các id cần xóa</param>
        /// <returns>Số lượng bản ghi đã xóa</returns>
        public int DeleteBatchEmployee(ListEmployeeIDs listEmpID)
        {
            return _employeeDL.DeleteBatchEmployee(listEmpID);
        }

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
        public object Filter(string? keyword, string? JobPositionName, Guid? departmentID, string? sort, int? pageSize = 10, int? pageNumber = 1)
        {
            return _employeeDL.Filter(keyword, JobPositionName, departmentID,sort,pageSize,pageNumber);
        }

        /// <summary>
        /// Xuất khẩu dữ liệu
        /// </summary>
        /// <returns></returns>
        public ResponseData ExportExcel()
        {
            var result = _employeeDL.GetAllReCord();
            if (result == null || result.Count() == 0)
            {
                return new ResponseData(false, new ErrorResult(
                               AMISErrorCode.Validate,
                               Resource.DevMsg_Validate,
                               Resource.UserMsg_Exception,
                               moreInfo: Resource.More_Info));
            }
                
            
                // Xử lý khi result  là null             
            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                string listColName = "ABCDEFGHI";
                var workSheet = package.Workbook.Worksheets.Add(Resource.Excel_List_Employee);

                #region Style cho các ô header
                workSheet.Cells.Style.Font.SetFromFont("Times New Roman", 11);
                workSheet.Cells["A1:I1"].Merge = true;
                workSheet.Cells["A2:I2"].Merge = true;
                workSheet.Cells["A1"].Value = Resource.Excel_List_Employee;
                workSheet.Cells["A1"].Style.Font.SetFromFont("Arial", 16, true);
                workSheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["A1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells["A3:I3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells["A3:I3"].Style.Fill.BackgroundColor.SetColor(OfficeOpenXml.Drawing.eThemeSchemeColor.Accent3);
                workSheet.Cells["A3:I3"].Style.Font.SetFromFont("Arial", 10, true);
                workSheet.Cells["A3:I3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells["A3:I3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                #endregion

                #region Đặt chiều rộng các cột 
                workSheet.Column(1).Width = 5;
                workSheet.Column(2).Width = 15;
                workSheet.Column(3).Width = 26;
                workSheet.Column(4).Width = 12;
                workSheet.Column(5).Width = 15;
                workSheet.Column(6).Width = 26;
                workSheet.Column(7).Width = 26;
                workSheet.Column(8).Width = 16;
                workSheet.Column(9).Width = 26;
                #endregion

                #region Header các cột
                var properties = typeof(Employee).GetProperties();
                int i = 1;
                //foreach (var prop in properties)
                //{
                //    var propName = prop.Name;
                //    var displayName = propName;
                //    var displayAttr = prop.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
                //    if (displayAttr != null)
                //    {
                //        displayName = displayAttr.Name;
                //    }
                //    workSheet.Cells[string.Format("{0}3", listColName[i - 1])].Value = displayName;
                //    i++;
                //}
                workSheet.Cells["A3"].Value = "STT";
                workSheet.Cells["B3"].Value = "Mã nhân viên";
                workSheet.Cells["C3"].Value = "Tên Nhân viên";
                workSheet.Cells["D3"].Value = "Giới tính";
                workSheet.Cells["E3"].Value = "Ngày sinh";
                workSheet.Cells["F3"].Value = "Chức Danh";
                workSheet.Cells["G3"].Value = "Tên đơn vị";
                workSheet.Cells["H3"].Value = "Số tài khoản";
                workSheet.Cells["I3"].Value = "Tên ngân hàng";
                workSheet.Cells["A3:I3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                #endregion

                #region render bản ghi
                int rowStart = 3;
                foreach (var text in listColName)
                {
                    workSheet.Cells[$"{text}{rowStart}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    workSheet.Cells[$"{text}{rowStart}"].Style.WrapText = true;
                }
                foreach (var val in result.Select((value, i) => new { i, value }))
                {
                    for (int col = 1; col <= 9; col++)
                    {
                        workSheet.Cells[val.i + 1 + rowStart, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[val.i + 1 + rowStart, col].Style.WrapText = true;
                        if (col == 5)
                        {
                            workSheet.Cells[val.i + 1 + rowStart, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }
                    }

                    workSheet.Cells[val.i + 1 + rowStart, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    workSheet.Cells[val.i + 1 + rowStart, 1].Value = val.i + 1;
                    workSheet.Cells[val.i + 1 + rowStart, 2].Value = val.value.EmployeeCode.ToString();
                    workSheet.Cells[val.i + 1 + rowStart, 3].Value = val.value.EmployeeName.ToString();
                    workSheet.Cells[val.i + 1 + rowStart, 4].Value = val.value.Gender == Gender.Male ? Resource.Display_Excel_Male : val.value.Gender == Gender.Female ? Resource.Display_Excel_FeMale : Resource.Display_excel_Other;
                    workSheet.Cells[val.i + 1 + rowStart, 5].Value = val.value.DateofBirth?.Year == null ? "" : val.value.DateofBirth?.ToString("dd/MM/yyyy");
                    workSheet.Cells[val.i + 1 + rowStart, 6].Value = val.value.JobPositionName == null ? "" : val.value.JobPositionName.ToString();
                    workSheet.Cells[val.i + 1 + rowStart, 7].Value = val.value.DepartmentName.ToString() == null ? "" : val.value.DepartmentName.ToString();
                    workSheet.Cells[val.i + 1 + rowStart, 8].Value = val.value.BankNumber == null ? "" : Int64.Parse(val.value.BankNumber);
                    workSheet.Cells[val.i + 1 + rowStart, 9].Value = val.value.BankName == null ? "" : val.value.BankName.ToString();

                }
                package.SaveAs(stream);
                #endregion
            }
            stream.Position = 0;
            return new ResponseData(true, stream);
        }
    }
}
