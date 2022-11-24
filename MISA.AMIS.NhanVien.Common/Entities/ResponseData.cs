using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.NhanVien.Common
{
    public class ResponseData
    {
        /// <summary>
        /// Trạng thái của kết quả trả về
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Dữ liệu trả về
        /// </summary>
        public object? Data { get; set; }

        public ResponseData(bool isSuccess, object? data)
        {
            IsSuccess = isSuccess;
            Data = data;
        }

    }
}
