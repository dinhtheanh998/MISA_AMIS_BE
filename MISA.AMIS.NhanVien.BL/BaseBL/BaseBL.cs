using MISA.AMIS.NhanVien.Common;
using MISA.AMIS.NhanVien.Common.Resources;
using MISA.AMIS.NhanVien.DL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static OfficeOpenXml.ExcelErrorValue;
using System.Xml.Linq;

namespace MISA.AMIS.NhanVien.BL.BaseBL
{
    public class BaseBL<T> : IBaseBL<T>
    {
        #region Field
        private IBaseDL<T> _baseDL;
        #endregion

        #region Constructor
        public BaseBL(IBaseDL<T> baseDL)
        {
            _baseDL = baseDL;
        }
        #endregion

        /// <summary>
        /// Xóa 1 bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID bản ghi cần xóa</param>
        /// <returns>ID của bản ghi khi xóa thành công</returns>
        /// CreatedBy: DTANH(16/11/2022)
        public int DeleteRecord(Guid recordID)
        {
            return _baseDL.DeleteRecord(recordID);
        }

        /// <summary>
        /// lấy tất cả bản ghi
        /// </summary>
        /// <returns>danh sách tất cả bản ghi</returns>
        /// CreatedBy:DTANH(16/112022)
        public IEnumerable<T> GetAllReCord()
        {
            return _baseDL.GetAllReCord();
        }

        /// <summary>
        /// Lấy thông tin 1 nhân viên theo id
        /// </summary>
        /// <param name="recordID">id của nhân viên</param>
        /// <returns>Thông tin của nhân viên</returns>
        /// CreatedBy: DTANH(16/11/2022)
        public T GetRecordByID(Guid recordID)
        {
            return _baseDL.GetRecordByID(recordID);
        }

        /// <summary>
        /// Thêm hoặc sửa bản ghi
        /// </summary>
        /// <param name="recordID">Id của bản ghi ( null nếu là thêm bản ghi mới)</param>
        /// <param name="record">Thông tin của bản ghi</param>
        /// <returns>thông tin lỗi nếu dữ liệu không phù hợp ngược lại là id của bản ghi </returns>
        public ResponseData UpdateOrInsert(Guid recordID, T record)
        {

            var isValid = ValidateData(recordID, record);
            //return new ResponseData(true, null);
            if (!isValid.IsSuccess)
            {
                return new ResponseData(isValid.IsSuccess, isValid.Data);
            }
            else
            {
                return new ResponseData(true, _baseDL.UpdateOrInsert(recordID, record));
            }
        }

        /// <summary>
        /// Validate Data
        /// </summary>
        /// <param name="record">Thông tin bản ghi</param>
        /// <returns></returns>
        public ResponseData ValidateData(Guid? recordID, T record)
        {
            List<object> Errors = new List<object>();
            //lấy ra tất cả attribute có attribute là "IsNotNullOrEmptyAttribute"

            //var properties = record.GetType().GetProperties();
            var properties = record.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var propName = prop.Name;
                // lấy giá trị của property truyền lên
                var propValue = prop.GetValue(record);

                var isUniqueCode = Attribute.IsDefined(prop, typeof(UniCodeAttribute));

                
                // Kiểm tra xem property có attribute là endWithNumber không
                var isEndWithNumber = Attribute.IsDefined(prop, typeof(EndWithNumberAttribute));

                // Kiểm tra xem property có attribute là isNotNullOrEmpty không
                var isNotNullOrEmpty = Attribute.IsDefined(prop, typeof(IsNotNullOrEmptyAttribute));

                // Kiểm tra xem property có attribute là isEmail không
                var isEmail = Attribute.IsDefined(prop, typeof(IsEmailAttribute));

                // Kiểm tra xem property có attribute là BirhOfDate không
                var birhOfDate = Attribute.IsDefined(prop, typeof(BirhOfDateAttribute));

                // Kiểm tra xem property có attribute là OnlyNumber không
                var isRegexFormat = Attribute.IsDefined(prop, typeof(FormatRegexAttribute));

                var canNullOrEmpty = Attribute.IsDefined(prop, typeof(CanNullOrEmptyAttribute));

                if (isUniqueCode == true)
                {
                    //lấy ra bản ghi trước khi chỉnh sửa 
                    var oldRecord = _baseDL.GetRecordByID((Guid)recordID);
                    bool compareCode = false;
                    // Lấy ra mã trước lúc chỉnh sửa
                    if (oldRecord != null)
                    {
                    var oldRecordCode = oldRecord.GetType().GetProperty(propName).GetValue(oldRecord);
                    compareCode = CompareCode(oldRecordCode.ToString(), propValue.ToString(), 2);

                    }

                    // so sánh 2 mã  ( true nếu mã cũ lớn hơn mã mới)
                    if (compareCode) {
                        Errors.Add(new {
                            name = propName,
                            value = Resource.UserMsg_Code_LessThan
                        });
                        //return new ResponseData(false, new ErrorResult(
                        //        AMISErrorCode.Validate,
                        //        Resource.DevMsg_Validate,
                        //        Resource.UserMsg_Code_LessThan,
                        //        moreInfo: Resource.More_Info));
                    }
                    var data = _baseDL.CheckDuplicate(propValue.ToString()).Data;
                    var codeData = data.GetType().GetProperty("recordCode").GetValue(data)?.ToString();
                    var attribute = prop.GetCustomAttributes(typeof(UniCodeAttribute), true).FirstOrDefault();
                    var errorMessage = (attribute as UniCodeAttribute).ErrorMessage;
                    // nếu có ID =>  là sửa
                    if (recordID != Guid.Empty)
                    {
                        if (data.GetType().GetProperty("recordID").GetValue(data) != null)
                        {
                            
                            // Lấy ra id của bản ghi trong db
                            var idData = data.GetType().GetProperty("recordID").GetValue(data).ToString();
                            if (idData != recordID.ToString())
                            {
                                Errors.Add(errorMessage);
                                //return new ResponseData(false, new ErrorResult(
                                //AMISErrorCode.Validate,
                                //Resource.DevMsg_Validate,
                                //errorMessage,
                                //moreInfo: Resource.More_Info
                            //));
                            }
                        }
                    } else
                    {
                        if (data != null)
                        {
                            
                            if (codeData == propValue.ToString())
                            {
                                Errors.Add(new
                                {
                                    name = propName,
                                    value = errorMessage
                                });
                                //return new ResponseData(false, new ErrorResult(
                                //AMISErrorCode.Validate,
                                //Resource.DevMsg_Validate,
                                //errorMessage,
                                //moreInfo: Resource.More_Info
                                //));
                            }
                        }
                    }

                    // Kiểm tra nếu không phải sửa thì có trùng với mã trong database không

                    // Nếu là sửa thì mã được phép trùng nhưng ID phải giống nhau. ID khác nhau đưa ra cảnh báo
                }
                
                if (isNotNullOrEmpty == true)
                {
                    var attribute = prop.GetCustomAttributes(typeof(IsNotNullOrEmptyAttribute), true).FirstOrDefault();
                    var errorMessage = (attribute as IsNotNullOrEmptyAttribute).ErrorMessage;
                    if (propValue == null || propValue.ToString().Trim() == "" || propValue.ToString() == Guid.Empty.ToString())
                    {
                        Errors.Add(new
                        {
                            name = propName,
                            value = errorMessage
                        })
                        //return new ResponseData(false, new ErrorResult(
                        //AMISErrorCode.Validate,
                        //        Resource.DevMsg_Validate,
                        //        errorMessage,
                        //        moreInfo: Resource.More_Info

                        //));
                    }
                }

                if (isEmail == true)
                {
                    var attribute = prop.GetCustomAttributes(typeof(IsEmailAttribute), true).FirstOrDefault();
                    var errorMessage = (attribute as IsEmailAttribute).ErrorMessage;
                    bool checkEmail = IsValidEmail(propValue?.ToString());
                    if (propValue != null && !checkEmail)
                    {
                        //return new ResponseData(false, new ErrorResult(
                        //AMISErrorCode.Validate,
                        //        Resource.DevMsg_Validate,
                        //        errorMessage,
                        //        moreInfo: Resource.More_Info
                        //));
                        Errors.Add(new
                        {
                            name = propName,
                            value = errorMessage
                        })
                    }
                }

                if (birhOfDate == true)
                {
                    var attribute = prop.GetCustomAttributes(typeof(BirhOfDateAttribute), true).FirstOrDefault();
                    var errorMessage = (attribute as BirhOfDateAttribute).ErrorMessage;
                    if (propValue != null && !IsValidDateOfBirth(propValue.ToString()) && propValue.ToString() != "")
                    {
                        //return new ResponseData(false, new ErrorResult(
                        //AMISErrorCode.Validate,
                        //        Resource.DevMsg_Validate,
                        //        errorMessage,
                        //        moreInfo: Resource.More_Info
                        //));
                        Errors.Add(new
                        {
                            name = propName,
                            value = errorMessage
                        });
                    }
                }
                
                if(isRegexFormat == true)
                {
                    //lấy ra attribute
                    var attribute = prop.GetCustomAttributes(typeof(FormatRegexAttribute), true).FirstOrDefault();
                    
                    // lấy ra regex 
                    var regex = new Regex((attribute as FormatRegexAttribute).Format);
                    
                    var errorMessage = (attribute as FormatRegexAttribute).ErrorMessage;
                    
                    if(propValue != null && !regex.IsMatch(propValue.ToString()) && !canNullOrEmpty)
                    {
                        //return new ResponseData(false, new ErrorResult(
                        //        AMISErrorCode.Validate,
                        //        Resource.DevMsg_Validate,
                        //        errorMessage,
                        //        moreInfo: Resource.More_Info
                        //));
                        Errors.Add(new
                        {
                            name = propName,
                            value = errorMessage
                        });
                    }
                }
            }
            if (Errors.Count > 0)
            {
                return new ResponseData(false, new ErrorResult(
                        AMISErrorCode.Validate,
                        Resource.DevMsg_Validate,
                        Errors.ToString(),
                        moreInfo: Resource.More_Info
                ));
            }
            return new ResponseData(true, null);
        }

        /// <summary>
        /// validate Email
        /// </summary>
        /// <param name="email">giá trị email truyền vào</param>
        /// <returns>true nếu là email, false nếu không phải email</returns>
        private static bool IsValidEmail(string email)
        {
            if (email == null) return false;
            if (email.Trim().EndsWith("."))
            {
                return false;
            }
            try
            {
                MailAddress mail = new MailAddress(email);

                return true;
            }
            catch (FormatException)
            {
                return false;
                throw;
            }
        }

        /// <summary>
        /// validate ngày sinh
        /// </summary>
        /// <param name="date">Ngày sinh</param>
        /// <returns>true: nếu nhỏ hơn ngày hiện tại và tuổi bé hơn 200 ngược lại false </returns>
        public static bool IsValidDateOfBirth(string date)
        {
            try
            {
                DateTime dt = DateTime.Parse(date);
                if (dt > DateTime.Now)
                {
                    return false;
                }
                else if (dt.Year < 1900)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// So sánh 2 mã
        /// </summary>
        /// <param name="oldCode"></param>
        /// <param name="newCode"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool CompareCode(string oldCode, string newCode, int index)
        {
            int oldCodeSub = int.Parse(oldCode.Substring(index));
            int newCodeSub = int.Parse(newCode.Substring(index));
            return oldCodeSub > newCodeSub;
        }
    }
}
