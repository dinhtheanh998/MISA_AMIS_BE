using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.NhanVien.Common.Entities
{
    /// <summary>
    /// Lớp lọc dữ liệu
    /// </summary>
    public class Filter
    {
        /// <summary>
        /// Từ khóa
        /// </summary>
        /// 
        
        public string? keyword { get; set; } = null;

        /// <summary>
        /// Id vị trí
        /// </summary>
        public string? JobPositionName { get; set; } = null;

        /// <summary>
        /// Id đơn vị
        /// </summary>
        public Guid? departmentID { get; set; } = null;

        /// <summary>
        /// Số bản ghi
        /// </summary>
        public int? pageSize { get; set; } = 10;

        /// <summary>
        /// trang bắt đầu
        /// </summary>
        public int? pageNumber { get; set; } = 1;

        /// <summary>
        /// Kiểu sắp xếp
        /// </summary>
        public string? sort { get; set; } = null;
    }
}
