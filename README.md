# ChatApp-RealTime

Ứng dụng chat real-time xây dựng bằng ASP.NET Core Razor Pages và SignalR, hỗ trợ đăng ký/đăng nhập và lưu trữ tin nhắn bằng SQLite.

## Tính năng
- Chat real-time với SignalR
- Đăng ký/đăng nhập bằng Cookie Authentication
- Mật khẩu được mã hóa với BCrypt
- Lưu trữ tin nhắn vào SQLite

## Yêu cầu
- .NET SDK 10.0 (target framework: net10.0)

## Cài đặt & chạy
Tại thư mục gốc repo:

```bash
dotnet restore
dotnet run --project ChatApp-RealTime/ChatApp-RealTime.csproj
```

Ứng dụng mặc định chạy tại:
- https://localhost:7271
- http://localhost:5189

> Nếu gặp lỗi HTTPS, hãy chạy `dotnet dev-certs https --trust` để tin cậy chứng chỉ phát triển.

## Hướng dẫn sử dụng
1. Truy cập trang chủ và chọn **Đăng Ký** để tạo tài khoản (mật khẩu tối thiểu 6 ký tự).
2. **Đăng Nhập** bằng tài khoản vừa tạo.
3. Vào trang **Chat**, chọn người dùng ở danh sách bên trái để bắt đầu trò chuyện.
4. Tin nhắn sẽ được lưu tự động; bạn có thể **Đăng xuất** ở góc trên bên trái.

## Cấu hình
- Chuỗi kết nối SQLite nằm trong `ChatApp-RealTime/appsettings.json`:
  - `DefaultConnection`: `Data Source=chatapp.db`
- CSDL được tự động tạo khi ứng dụng khởi động lần đầu (file `chatapp.db`).

## Build & Test
```bash
dotnet build ChatApp-RealTime.slnx
```

Hiện chưa có dự án test trong repo.
