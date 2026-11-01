/* =========================================================

 * Kỹ thuật áp dụng: Design Pattern - Strategy Pattern (Mẫu Chiến lược)
 * Mô tả mục đích: Viết mới module tính chiết khấu bán hàng nông dược theo từng 
 *                 loại khách hàng (Khách lẻ, Đại lý cấp 1, Đại lý cấp 2) giúp 
 *                 dễ dàng mở rộng chính sách giá mà không sửa đổi code cũ.
 * ========================================================= */

using System;

namespace CuahangNongduoc
{
    // 1. Giao diện chiến lược tính chiết khấu
    public interface IChietKhauStrategy
    {
        long TinhTienSauChietKhau(long tongTienGoc);
    }

    // 2. Chiến lược cho Khách mua lẻ (Không chiết khấu)
    public class KhachLeStrategy : IChietKhauStrategy
    {
        public long TinhTienSauChietKhau(long tongTienGoc)
        {
            return tongTienGoc;
        }
    }

    // 3. Chiến lược cho Đại lý cấp 2 (Chiết khấu 5%)
    public class DaiLyCap2Strategy : IChietKhauStrategy
    {
        public long TinhTienSauChietKhau(long tongTienGoc)
        {
            return (long)(tongTienGoc * 0.95);
        }
    }

    // 4. Chiến lược cho Đại lý cấp 1 (Chiết khấu 10%)
    public class DaiLyCap1Strategy : IChietKhauStrategy
    {
        public long TinhTienSauChietKhau(long tongTienGoc)
        {
            return (long)(tongTienGoc * 0.90);
        }
    }

    // 5. Lớp Context điều phối tính tiền
    public class TinhTienContext
    {
        private IChietKhauStrategy m_Strategy;

        public TinhTienContext(IChietKhauStrategy strategy)
        {
            m_Strategy = strategy;
        }

        public void SetStrategy(IChietKhauStrategy strategy)
        {
            m_Strategy = strategy;
        }

        public long ThucHienTinhTien(long tongTienGoc)
        {
            return m_Strategy.TinhTienSauChietKhau(tongTienGoc);
        }
    }
}