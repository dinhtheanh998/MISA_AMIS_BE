using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.NhanVien.Common
{
    public class ErrorResult
    {
        #region Contructor
        public ErrorResult()
        {

        }

        public ErrorResult(AMISErrorCode errorCode, string? devMsg, string? userMsg, string? moreInfo)
        {
            ErrorCode = errorCode;
            DevMsg = devMsg;
            UserMsg = userMsg;
            MoreInfo = moreInfo;
        }
        
        public ErrorResult(AMISErrorCode errorCode, string? devMsg, string? userMsg, string? moreInfo, string? traceId )
        {
            ErrorCode = errorCode;
            DevMsg = devMsg;
            UserMsg = userMsg;
            MoreInfo = moreInfo;
            TraceId = traceId;
        }
        #endregion

        #region property

        /// <summary>
        /// Mã lỗi
        /// Author: DTANH(14/11/2022)
        /// </summary>
        public AMISErrorCode ErrorCode { get; set; }

        /// <summary>
        /// Phải hồi cho dev
        /// Author: DTANH(14/11/2022)
        /// </summary>
        public string? DevMsg { get; set; }

        /// <summary>
        /// Phải hồi cho user
        /// Author: DTANH(14/11/2022)
        /// </summary>
        public string? UserMsg { get; set; }

        /// <summary>
        /// Thông tin thêm
        /// Author: DTANH(14/11/2022)
        /// </summary>
        public string? MoreInfo { get; set; }

        /// <summary>
        /// ID của lỗi
        /// Author: DTANH(14/11/2022)
        /// </summary>
        public string? TraceId { get; set; }
        #endregion

       
    }
}
