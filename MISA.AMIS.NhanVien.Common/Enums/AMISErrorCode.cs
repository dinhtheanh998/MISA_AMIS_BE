using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.NhanVien.Common
{
    public enum AMISErrorCode
    {
        /// <summary>
        /// Lỗi do exception
        /// Author: DTANH(14/11/2022)
        /// </summary>
        Exception = 1,

        /// <summary>
        /// Lỗi do validate
        /// Author: DTANH(14/11/2022)
        /// </summary>
        Validate = 2,

        /// <summary>
        /// Lỗi do mã trống
        /// Author: DTANH(14/11/2022)
        /// </summary>
        EmptyCode = 3,

        /// <summary>
        /// Lỗi đơn vị trống
        /// Author: DTANH(14/11/2022)
        /// </summary>
        EmptyDepartment = 4,

        /// <summary>
        /// Lỗi trùng mã
        /// Author: DTANH(14/11/2022)
        /// </summary>
        DuplicateCode = 1062,

        /// <summary>
        /// Lỗi khóa ngoại
        /// Author: DTANH(14/11/2022)
        /// </summary>
        ForeignKeyConstraint = 1452,

        /// <summary>
        /// Lỗi không thể lấy mã
        /// </summary>
        GetCodeFail = 5
    }
}
