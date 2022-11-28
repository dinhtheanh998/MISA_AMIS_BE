using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.NhanVien.Common
{
    /// <summary>
    /// Thuộc tính khóa chính
    /// </summary>
    public class PrimaryKeyAttribute : Attribute
    {
        public string ErrorMessage { get; set; }
        public PrimaryKeyAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }

    /// <summary>
    /// Thuộc tính không được null hoặc rỗng
    /// </summary>
    public class IsNotNullOrEmptyAttribute : Attribute
    {
        public string ErrorMessage { get; set; }
        public IsNotNullOrEmptyAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
    
    /// <summary>
    /// Thuộc tính phải là email
    /// </summary>
    public class IsEmailAttribute : Attribute
    {
        public string ErrorMessage { get; set; }
        public IsEmailAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }

    /// <summary>
    /// Thuộc tính ngày sinh
    /// </summary>
    public class BirhOfDateAttribute : Attribute
    {
        public string ErrorMessage { get; set;}
        public BirhOfDateAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }

    /// <summary>
    /// Thuộc tính không trùng 
    /// </summary>
    public class UniCodeAttribute : Attribute
    {
        public string ErrorMessage { get; set;}
        public UniCodeAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }

    /// <summary>
    /// Kết thúc bằng số
    /// </summary>
    public class EndWithNumberAttribute : Attribute
    {
        public string Format { get; set; }
        public string ErrorMessage { get; set; }
        public EndWithNumberAttribute(string format, string errorMessage)
        {
            Format = format;
            ErrorMessage = errorMessage;
        }
    }

    /// <summary>
    /// chỉ được là số, kết thúc là số, email,
    /// </summary>
    public class OnlyNumberAttribute : Attribute
    {
        public string Format { get; set; }
        public string ErrorMessage { get; set; }
        public OnlyNumberAttribute(string format, string errorMessage)
        {
            Format = format;
            ErrorMessage = errorMessage;
        }

    }

    /// <summary>
    /// Validate theo regex
    /// </summary>
    public class FormatRegexAttribute : Attribute
    {
        public string Format { get; set; }
        public string ErrorMessage { get; set; }
        public FormatRegexAttribute(string format, string errorMessage)
        {
            Format = format;
            ErrorMessage = errorMessage;
        }
    }

    /// <summary>
    /// Có thể null hoặc trống
    /// </summary>
    public class CanNullOrEmptyAttribute : Attribute
    {
    }

    public class AttributeChungAttribute : Attribute
    {
        public bool khongtrong { get; set; }

        public bool Ngaythang { get; set; }
    }
}
