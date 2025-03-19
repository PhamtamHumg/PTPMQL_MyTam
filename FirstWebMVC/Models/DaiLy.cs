using System.ComponentModel.DataAnnotations;

namespace FirstWebMVC.Models
{
    public class DaiLy
    {
        [Key] // Đặt MaDaiLy làm khóa chính
        public required string MaDaiLy { get; set; }
        public string TenDaiLy { get; set; }
        public string DiaChi { get; set; }
        public string NguoiDaiDien { get; set; }
        public string DienThoai { get; set; }
        public string MaHTPP { get; set; }
    }
}
