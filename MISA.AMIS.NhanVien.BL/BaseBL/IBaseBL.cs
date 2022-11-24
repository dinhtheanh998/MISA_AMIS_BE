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
        public IEnumerable<T> GetAllReCord();

        public T GetRecordByID(Guid recordID);

        public int DeleteRecord(Guid recordID);

        //public ResponseData InsertRecord(T record);

        //public int UpdateRecord(Guid recordID, T record);

        public ResponseData UpdateOrInsert(Guid recordID, T record);

        #endregion
    }
}
