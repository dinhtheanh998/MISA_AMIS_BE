namespace MISA.AMIS.NhanVien.API.Entities
{
    public class Department
    {
        /// <summary>
        /// ID đơn vị
        /// </summary>
        public Guid DepartmentId { get; set; }
        /// <summary>
        /// Mã đơn vị
        /// </summary>
        public string DepartmentCode { get; set; }
        /// <summary>
        /// Tên đơn vị
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Người tạo
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Thời gian chỉnh sửa gần nhất
        /// </summary>
        public DateTime ModifiedDate { get; set; }
        /// <summary>
        /// Người chỉnh sửa gần nhất
        /// </summary>
        public string ModifieddBy { get; set; }
    }
}
