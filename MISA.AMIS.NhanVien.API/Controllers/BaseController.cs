using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.NhanVien.BL.BaseBL;
using MISA.AMIS.NhanVien.Common;
using MISA.AMIS.NhanVien.Common.Entities;
using MISA.AMIS.NhanVien.Common.Resources;

namespace MISA.AMIS.NhanVien.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<T> : ControllerBase
    {
        #region Field
        private IBaseBL<T> _baseBL;
        #endregion

        #region Constructor
        public BaseController(IBaseBL<T> baseBL)
        {
            _baseBL = baseBL;
        }
        #endregion

        #region Method
        /// <summary>
        /// Lấy toàn bộ bản ghi
        /// </summary>
        /// <returns>Toàn bộ bản ghi của bảng</returns>
        [HttpGet]
        public IActionResult GetAllReCord()
        {
            try
            {
                var result = _baseBL.GetAllReCord();
                if (result != null)
                {
                    return StatusCode(StatusCodes.Status200OK, result);
                }
                return StatusCode(StatusCodes.Status200OK, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult(
                    AMISErrorCode.Exception,
                    Resource.DevMsg_Exception,
                    Resource.UserMsg_Exception,
                    Resource.More_Info,
                    HttpContext.TraceIdentifier
                    ));
            }
        }
        
        /// <summary>
        /// Insert hoặc update bản ghi
        /// </summary>
        /// <param name="record">Thông tin bản ghi</param>
        /// <param name="recordID">Id bản ghi (nếu có)</param>
        /// <returns></returns>
        [HttpPost("updateOrInsert")]
        public IActionResult UpdateOrInsert([FromBody] T record, [FromQuery] Guid recordID)
        {
            try
            {
                //Call method UpdateOrInsert
                var result = _baseBL.UpdateOrInsert(recordID, record);
                if (result.IsSuccess)
                {
                    return recordID != Guid.Empty ? StatusCode(StatusCodes.Status200OK, result.Data) : StatusCode(StatusCodes.Status201Created, result.Data);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                            result.Data);
                   
                }

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
        /// Xóa 1 bản ghi
        /// </summary>
        /// <param name="recordID">Id của bản ghi</param>
        /// <returns>id của bản ghi vừa xóa</returns>
        [HttpDelete("{recordID}")]
        public IActionResult DeleteRecord(Guid recordID)
        {
            try
            {
                var result = _baseBL.DeleteRecord(recordID);
                if (result > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, result);
                }
                return StatusCode(StatusCodes.Status200OK, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult(
                    AMISErrorCode.Exception,
                    Resource.DevMsg_Exception,
                    Resource.UserMsg_Exception,
                    Resource.More_Info,
                    HttpContext.TraceIdentifier
                    ));
            }
        }

        /// <summary>
        /// Lấy 1 bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID của bản ghi</param>
        /// <returns>Thông tin bản ghi</returns>
        [HttpGet("{recordID}")]
        public IActionResult GetRecordByID(Guid recordID)
        {
            try
            {
                var result = _baseBL.GetRecordByID(recordID);
                if (result != null)
                {
                    return StatusCode(StatusCodes.Status200OK, result);
                }
                return StatusCode(StatusCodes.Status200OK, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult(
                    AMISErrorCode.Exception,
                    Resource.DevMsg_Exception,
                    Resource.UserMsg_Exception,
                    Resource.More_Info,
                    HttpContext.TraceIdentifier
                    ));
            }
        }
        #endregion
    }
}
