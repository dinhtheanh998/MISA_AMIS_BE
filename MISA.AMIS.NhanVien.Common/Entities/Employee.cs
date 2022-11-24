using MISA.AMIS.NhanVien.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MISA.AMIS.NhanVien.Common.Entities
{
    /// <summary>
    /// Nhân viên
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// ID Nhân Viên
        /// </summary>
        /// 
        [Key]
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Mã Nhân Viên
        /// </summary>
        /// 
        [UniCode("Mã nhân viên đã tồn tại trong hệ thống")]
        [IsNotNullOrEmpty("Mã nhân viên không được phép để trống")]
        [FormatRegex("(?=^.{0,20}$)^[a-zA-Z0-9]+[0-9]$", "Mã nhân viên phải kết thúc bằng số và nhỏ hơn 20 ký tự")]
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Tên Nhân Viên
        /// </summary>
        /// 
        [IsNotNullOrEmpty("Tên nhân viên không được phép để trống")]
        public string EmployeeName { get; set; }

        /// <summary>
        /// Ngày Sinh
        /// </summary>
        /// 
        [BirhOfDate("Ngày sinh phải nhỏ hơn ngày hiện tại , năm sinh lớn hơn 1900")]
        [CanNullOrEmpty]
        public DateTime? DateofBirth { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        /// 
        [DisplayName("Giới tính")]
        public Gender? Gender { get; set; }

        /// <summary>
        /// Số chứng minh nhân dân
        /// </summary>
        /// 
        [FormatRegex("^[0-9]*$","Số chứng minh nhân dân chỉ được là số")]        
        public string? IdentityNumber { get; set; }

        /// <summary>
        /// Ngày cấp
        /// </summary>
        public DateTime? IdentityDate { get; set; }

        /// <summary>
        /// Nơi cấp
        /// </summary>
        public string? IdentityPlace { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Ngày chỉnh sửa
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Mã phòng ban
        /// </summary>
        /// 
        [IsNotNullOrEmpty("Vui lòng chọn 1 đơn vị")]
        public Guid DepartmentID { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string? Adress { get; set; }

        /// <summary>
        /// Điện thoại di động
        /// </summary>
        /// 
        [FormatRegex("^[0-9]*$", "Số điện thoại chỉ được là số")]
        [CanNullOrEmpty]
        public string? TelephoneNumber { get; set; }

        /// <summary>
        /// Điện thoại cố định
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        /// 
        [IsEmail("Email không đúng định dạng")]
        public string? Email { get; set; }

        /// <summary>
        /// Số tài khoản 
        /// </summary>
        public string? BankNumber { get; set; }

        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        public string? BankName { get; set; }

        /// <summary>
        /// Chi nhánh
        /// </summary>
        public string? BankBranch { get; set; }

        /// <summary>
        /// Mã đơn vị
        /// </summary>
        public string? DepartmentCode { get; set; }

        /// <summary>
        /// Tên đơn vị
        /// </summary>
        public string? DepartmentName { get; set; }

        /// <summary>
        /// Tên vị trí
        /// </summary>
        public string? JobPositionName { get; set; }

        /// <summary>
        /// Ngày chỉnh sửa gần nhất
        /// </summary>
        public DateTime ModifiedDate  { get; set; }

        /// <summary>
        /// Người chỉnh sửa gần nhất
        /// </summary>
        public string? ModifiedBy { get; set; }
       
    }
}
