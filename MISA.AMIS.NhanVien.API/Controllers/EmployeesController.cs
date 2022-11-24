using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.NhanVien.BL;
using MISA.AMIS.NhanVien.Common;
using MISA.AMIS.NhanVien.Common.Entities;
using MISA.AMIS.NhanVien.Common.Resources;
using MISA.AMIS.NhanVien.DL;
using MySqlConnector;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Transactions;

namespace MISA.AMIS.NhanVien.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : BaseController<Employee>
    {
        #region Field
        private IEmployeeBL _employeeBL;
        #endregion

        #region Constructor
        public EmployeesController(IEmployeeBL employeeBL) : base(employeeBL)
        {
            _employeeBL = employeeBL;
        }
        #endregion

        #region Method

        /// <summary>
        /// Lọc dữ liệu
        /// </summary>
        /// <param name="keyword">Từ khóa tìm kiếm theo tên</param>
        /// <param name="positionID">ID vị trí tìm kiếm</param>
        /// <param name="departmentID">ID phòng ban</param>
        /// <param name="pageSize">Số lượng bản ghi</param>
        /// <param name="pageNumber">trang bắt đầu</param>
        /// <param name="sort"></param>
        /// <returns>Danh sách nhân viên và tổng bản ghi</returns>
        [HttpGet("filter")]
        public IActionResult FilterEmployee([FromQuery] string? keyword,
            [FromQuery] string? JobPositionName,
            [FromQuery] Guid? departmentID,
            [FromQuery] string? sort,
            [FromQuery] int? pageSize = 10,
            [FromQuery] int? pageNumber = 1
            )
        {
            try
            {
                var result = _employeeBL.Filter(keyword, JobPositionName, departmentID, sort, pageSize, pageNumber);

                if (result != null)
                {
                    return StatusCode(StatusCodes.Status200OK, result);
                }
                return StatusCode(StatusCodes.Status400BadRequest, "ERROR");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult(
                        AMISErrorCode.Exception,
                        Resource.DevMsg_Exception,
                        Resource.UserMsg_Exception,
                        moreInfo: Resource.More_Info,
                        traceId: HttpContext.TraceIdentifier
                        )
                );
                throw;
            }
        }

        [HttpPost("post/filter")]
        public IActionResult FilterEmployee2([FromBody] Filter filter)
        {
            try
            {
                var result = _employeeBL.Filter(filter.keyword, filter.JobPositionName, filter.departmentID, filter.sort, filter.pageSize, filter.pageNumber);

                if (result != null)
                {
                    return StatusCode(StatusCodes.Status200OK, result);
                }
                return StatusCode(StatusCodes.Status400BadRequest, "ERROR");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult(
                        AMISErrorCode.Exception,
                        Resource.DevMsg_Exception,
                        Resource.UserMsg_Exception,
                        moreInfo: Resource.More_Info,
                        traceId: HttpContext.TraceIdentifier
                        )
                );
                throw;
            }
        }

        /// <summary>
        /// Sửa dữ liệu bản ghi
        /// </summary>
        /// <param name="employeeID">ID của Employee</param>
        /// <param name="employee"> Các thuộc tính của employee sau khi chỉnh sửa </param>
        /// <returns>employee đã chỉnh sửa</returns>
        //[HttpPut("{employeeID}")]
        //public IActionResult UpdateEmployee([FromRoute] Guid employeeID, [FromBody] Employee employee)
        //{
        //    try
        //    {
        //        var numberAffected = _employeeBL.UpdateRecord(employeeID, employee);

        //        if (numberAffected > 0)
        //        {
        //            return StatusCode(StatusCodes.Status200OK, employee);
        //        }
        //        return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult(
        //            AMISErrorCode.Validate,
        //            Resource.DevMsg_Update_Fail,
        //            Resource.UserMsg_Update_Fail,
        //            Resource.More_Info,
        //            HttpContext.TraceIdentifier
        //            ));
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //        return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult(
        //                AMISErrorCode.Exception,
        //                Resource.DevMsg_Exception,
        //                Resource.UserMsg_Exception,
        //                moreInfo: Resource.More_Info,
        //                traceId: HttpContext.TraceIdentifier
        //                )
        //        );
        //        throw;
        //    }
        //}

        /// <summary>
        /// lấy mã nhân viên mới
        /// </summary>
        /// <returns>mã nhân viên mới</returns>
        [HttpGet("getNewCode")]
        public IActionResult GetNewEmployeeCode()
        {
            try
            {
                string newCode = _employeeBL.GetNewEmployeeCode();
                if (newCode != null)
                {
                    return StatusCode(StatusCodes.Status200OK, newCode);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = 1,
                    DevMsg = "Cant get new Code emp",
                    UserMsg = "Không thể tạo mã nhân viên",
                    MoreInfo = "",
                    TraceID = HttpContext.TraceIdentifier,
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult(
                        AMISErrorCode.Exception,
                        Resource.DevMsg_Exception,
                        Resource.UserMsg_Exception,
                        moreInfo: Resource.More_Info,
                        traceId: HttpContext.TraceIdentifier
                        )
                );
                throw;
            }
        }

        /// <summary>
        /// Xóa hoàng loạt
        /// </summary>
        /// <param name="listEmpID"></param>
        /// <returns></returns>
        [HttpPost("DeleteBatch")]
        public IActionResult DeleteIN([FromBody] ListEmployeeIDs listEmpID)
        {
            try
            {
                var numberAffected = _employeeBL.DeleteBatchEmployee(listEmpID);
                if (numberAffected > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, numberAffected);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, 0);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult(
                        AMISErrorCode.Exception,
                        Resource.DevMsg_Exception,
                        Resource.UserMsg_Exception,
                        moreInfo: Resource.More_Info,
                        traceId: HttpContext.TraceIdentifier
                        )
                );
                throw;
            }
        }


        /// <summary>
        /// Xuất file excel
        /// </summary>
        /// <returns></returns>
        [HttpPost("ExportExcel")]
        public IActionResult ExportExcel()
        {
            var result = _employeeBL.GetAllReCord();

            var stream = new MemoryStream();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                string listColName = "ABCDEFGHI";
                var workSheet = package.Workbook.Worksheets.Add("DANH SÁCH NHÂN VIÊN");

                #region Style cho các ô header
                workSheet.Cells.Style.Font.SetFromFont("Times New Roman", 11);
                workSheet.Cells["A1:I1"].Merge = true;
                workSheet.Cells["A2:I2"].Merge = true;
                workSheet.Cells["A1"].Value = "Danh sách nhân viên";
                workSheet.Cells["A1"].Style.Font.SetFromFont("Arial", 16,true);
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

                int rowStart = 3;
                foreach(var text in listColName)
                {
                    workSheet.Cells[$"{text}{rowStart}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    workSheet.Cells[$"{text}{rowStart}"].Style.WrapText = true;
                }
                foreach (var val in result.Select((value, i) => new { i, value }))
                {
                    for(int col = 1; col <= 9; col++)
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
                    workSheet.Cells[val.i + 1 + rowStart, 4].Value = val.value.Gender == Gender.Male ? "Nam" : val.value.Gender == Gender.Female ? "Nữ" :"";                    
                    workSheet.Cells[val.i + 1 + rowStart, 5].Value = val.value.DateofBirth?.Year.ToString() == "0001" ? "" : val.value.DateofBirth?.ToString("dd/MM/yyyy");                    
                    workSheet.Cells[val.i + 1 + rowStart, 6].Value = val.value.JobPositionName == null ? "" : val.value.JobPositionName.ToString();                   
                    workSheet.Cells[val.i + 1 + rowStart, 7].Value = val.value.DepartmentName.ToString() == null ? "" : val.value.DepartmentName.ToString();                    
                    workSheet.Cells[val.i + 1 + rowStart, 8].Value = val.value.BankNumber == null ? "" : val.value.BankNumber.ToString();                    
                    workSheet.Cells[val.i + 1 + rowStart, 9].Value = val.value.BankName == null ? "" : val.value.BankName.ToString();

                }
                package.SaveAs(stream);                
            }
            stream.Position = 0;
            string excelName = $"Employee-{DateTime.Now.ToString("ddMMyyyyHHmmssfff")}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            
        }

        #endregion

    }
}
