using Dapper;
using MISA.AMIS.NhanVien.Common;
using MySqlConnector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.NhanVien.DL
{
    public interface IBaseDL<T>
    {
        #region Method
        /// <summary>
        /// Lấy tất cả dữ liệu
        /// </summary>
        /// <returns>Danh sách tất cả bản ghi</returns>
        public IEnumerable<T> GetAllReCord();
        
        /// <summary>
        /// Lấy bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID bản ghi</param>
        /// <returns></returns>
        public T GetRecordByID(Guid recordID);

        /// <summary>
        /// XÓa bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID bản ghi</param>
        /// <returns></returns>
        public int DeleteRecord(Guid recordID);

        /// <summary>
        /// Thêm hoặc sửa bản ghi
        /// </summary>
        /// <param name="recordID">ID của bản ghi nếu là sửa </param>
        /// <param name="record">Thông tin của bản ghi</param>
        /// <returns>Id của bản ghi</returns>
        public object UpdateOrInsert(Guid recordID, T record);
        
        /// <summary>
        /// Kiểm tra trùng mã
        /// </summary>
        /// <param name="recordCode">Mã Bản ghi truyền vào</param>
        /// <returns></returns>
        public ResponseData CheckDuplicate(string recordCode);
        
        #endregion
    }
}
