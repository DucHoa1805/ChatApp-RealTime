# ChatApp-RealTime

Ứng dụng chat real-time xây dựng bằng ASP.NET Core Razor Pages, SignalR và SQLite.

## Tính năng

- Chat real-time với SignalR.
- Đăng ký/đăng nhập bằng Cookie Authentication.
- Trạng thái online/offline và tìm kiếm người dùng.
- Tin nhắn được lưu vào SQLite.
- Mật khẩu được băm với BCrypt.

## Yêu cầu

- .NET SDK 10.0 (target framework: `net10.0`).

## Cài đặt & chạy

Tại thư mục gốc repo:

```bash
dotnet restore
dotnet run --project ChatApp-RealTime/ChatApp-RealTime.csproj
```

Sau khi chạy, xem URL được in ra trên console (thường là `https://localhost:****` hoặc `http://localhost:****`) và mở bằng trình duyệt.

> Nếu gặp lỗi HTTPS khi chạy lần đầu, hãy dùng:
> `dotnet dev-certs https --trust`

## Cấu hình

- Chuỗi kết nối nằm trong `ChatApp-RealTime/appsettings.json`:
  - `DefaultConnection`: `Data Source=chatapp.db`
- Cơ sở dữ liệu được tự động tạo khi ứng dụng khởi động lần đầu (file `chatapp.db`).

## Hướng dẫn sử dụng

1. Truy cập trang chủ và chọn **Đăng Ký** để tạo tài khoản.
2. **Đăng Nhập** bằng tài khoản vừa tạo.
3. Vào trang **Chat**, chọn người dùng ở danh sách bên trái để bắt đầu trò chuyện.
4. Nhập tin nhắn và bấm gửi (hoặc Enter).
5. Dùng nút nguồn ⏻ để đăng xuất.

## Build & Test

```bash
dotnet build ChatApp-RealTime.slnx
dotnet test ChatApp-RealTime.slnx
```

Hiện chưa có dự án test trong repo, nên `dotnet test` sẽ không chạy test nào.
