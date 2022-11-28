using MISA.AMIS.NhanVien.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.NhanVien.BL.BaseBL
{
    public interface IBaseBL<T>
    {
        #region Method
        /// <summary>
        /// Lấy tất cả dữ liệu
        /// </summary>
        /// <returns>Danh sách tất cả bản ghi</returns>
        public IEnumerable<T> GetAllReCord();

        /// <summary>
        /// Lấy 1 bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID của bản ghi</param>
        /// <returns>1 bản ghi</returns>
        public T GetRecordByID(Guid recordID);

        /// <summary>
        /// Xóa 1 bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID bản ghi cần xóa</param>
        /// <returns>ID của bản ghi khi xóa thành công</returns>
        public int DeleteRecord(Guid recordID);

        /// <summary>
        /// Thêm hoặc sửa bản ghi
        /// </summary>
        /// <param name="recordID">ID của bản ghi nếu là sửa </param>
        /// <param name="record">Thông tin của bản ghi</param>
        /// <returns>Id của bản ghi</returns>
        public ResponseData UpdateOrInsert(Guid recordID, T record);
        #endregion
    }
}
