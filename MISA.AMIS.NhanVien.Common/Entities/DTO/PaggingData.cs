namespace MISA.AMIS.NhanVien.Common.Entities
{
    public class PaggingData
    {
        public List<Employee> Data { get; set; }
        public int TotalRecord { get; set; }
        public int TotalPage { get; set; }

    }
}
