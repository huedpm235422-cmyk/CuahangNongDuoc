/* =========================================================
 
 * Kỹ thuật áp dụng: Refactoring - Extract Method (Tách phương thức)
 * Mô tả lỗi (Trước): Các hàm lấy và gán mã trong lớp ThamSo bị lặp lại code (Duplicate Code) 
 *                    khi khởi tạo DataService và thực thi câu lệnh SQL ở 10 chỗ khác nhau.
 * Giải pháp (Sau): Tách logic truy vấn chung thành 2 hàm LayGiaTriThamSo() và CapNhatGiaTriThamSo() 
 *                  để tái sử dụng và làm sạch mã nguồn.
 * ========================================================= */

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CuahangNongduoc
{
    public enum Controll
    {
        Normal,
        AddNew,
        Edit
    }

    public class ThamSo
    {
        // --- HÀM DÙNG CHUNG SAU KHI REFACTOR (EXTRACT METHOD) ---
        private static long LayGiaTriThamSo(string tenCot)
        {
            DataService ds = new DataService();
            object obj = ds.ExecuteScalar(new SqlCommand("SELECT " + tenCot + " FROM THAM_SO"));
            return Convert.ToInt64(obj);
        }

        private static void CapNhatGiaTriThamSo(string tenCot, long giaTri)
        {
            DataService ds = new DataService();
            ds.ExecuteNoneQuery(new SqlCommand("UPDATE THAM_SO SET " + tenCot + " = " + giaTri));
        }
        // --------------------------------------------------------

        public static void PreMonth(ref int thangtruoc, ref int namtruoc, int thang, int nam)
        {
            thangtruoc = thang - 1;
            namtruoc = nam;
            if (thangtruoc == 0)
            {
                thangtruoc = 12;
                namtruoc = nam - 1;
            }
        }

        public static bool LaSoNguyen(String so)
        {
            try
            {
                Convert.ToInt64(so);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static long LayMaPhieuNhap()
        {
            return LayGiaTriThamSo("PHIEU_NHAP");
        }

        public static void GanMaPhieuNhap(long id)
        {
            CapNhatGiaTriThamSo("PHIEU_NHAP", id);
        }

        public static long LayMaPhieuBan()
        {
            return LayGiaTriThamSo("PHIEU_BAN");
        }

        public static void GanMaPhieuBan(long id)
        {
            CapNhatGiaTriThamSo("PHIEU_BAN", id);
        }

        public static long LayMaPhieuThanhToan()
        {
            return LayGiaTriThamSo("PHIEU_THANH_TOAN");
        }

        public static void GanMaPhieuThanhToan(long id)
        {
            CapNhatGiaTriThamSo("PHIEU_THANH_TOAN", id);
        }

        public static long SanPham
        {
            get { return LayGiaTriThamSo("SAN_PHAM"); }
            set { CapNhatGiaTriThamSo("SAN_PHAM", value); }
        }

        public static CuahangNongduoc.BusinessObject.CuaHang LayCuaHang()
        {
            CuahangNongduoc.BusinessObject.CuaHang ch = new CuahangNongduoc.BusinessObject.CuaHang();
            DataService ds = new DataService();
            ds.Load(new SqlCommand("SELECT TEN_CUA_HANG, DIA_CHI, DIEN_THOAI FROM THAM_SO"));
            if (ds.Rows.Count > 0)
            {
                ch.TenCuaHang = ds.Rows[0]["TEN_CUA_HANG"].ToString();
                ch.DiaChi = ds.Rows[0]["DIA_CHI"].ToString();
                ch.DienThoai = ds.Rows[0]["DIEN_THOAI"].ToString();
            }
            return ch;
        }

        public static void GanCuaHang(String ten_cua_hang, String dia_chi, String dien_thoai)
        {
            DataService ds = new DataService();
            SqlCommand cmd = new SqlCommand("UPDATE THAM_SO SET TEN_CUA_HANG = @ten_cua_hang, DIA_CHI = @dia_chi, DIEN_THOAI = @dien_thoai ");
            cmd.Parameters.Add("@ten_cua_hang", SqlDbType.VarChar).Value = ten_cua_hang;
            cmd.Parameters.Add("@dia_chi", SqlDbType.VarChar).Value = dia_chi;
            cmd.Parameters.Add("@dien_thoai", SqlDbType.VarChar).Value = dien_thoai;

            ds.ExecuteNoneQuery(cmd);
        }

        public static long NhaCungCap
        {
            get { return LayGiaTriThamSo("NHA_CUNG_CAP"); }
            set { CapNhatGiaTriThamSo("NHA_CUNG_CAP", value); }
        }

        public static long KhachHang
        {
            get { return LayGiaTriThamSo("KHACH_HANG"); }
            set { CapNhatGiaTriThamSo("KHACH_HANG", value); }
        }

        public static long PhieuChi
        {
            get { return LayGiaTriThamSo("PHIEU_CHI"); }
            set { CapNhatGiaTriThamSo("PHIEU_CHI", value); }
        }
    }
}