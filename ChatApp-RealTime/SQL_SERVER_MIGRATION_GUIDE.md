# ChatApp-RealTime Migration Guide: SQLite → SQL Server

Hướng dẫn chi tiết để chuyển từ SQLite sang SQL Server cho ứng dụng chat real-time.

## 📋 Tóm tắt các thay đổi

| Thành phần | Thay đổi |
|-----------|---------|
| **Package** | `Microsoft.EntityFrameworkCore.Sqlite` → `Microsoft.EntityFrameworkCore.SqlServer` |
| **Connection String** | SQLite (`.db` file) → SQL Server (connection string) |
| **Code** | `UseSqlite()` → `UseSqlServer()` |
| **Migration** | `EnsureCreated()` → `Migrate()` |

## 🔧 Bước 1: Tạo Migration đầu tiên

```bash
cd ChatApp-RealTime
dotnet ef migrations add InitialCreate
```

Điều này sẽ tạo folder `Migrations/` chứa các file migration.

## 🗄️ Bước 2: Cấu hình SQL Server Connection String

Chọn **một** trong các tùy chọn dưới đây:

### ✅ Option 1: SQL Server Local (Windows Authentication)
```json
"DefaultConnection": "Server=localhost;Database=ChatAppDb;Integrated Security=true;TrustServerCertificate=true;"
```

### ✅ Option 2: SQL Server with Username/Password
```json
"DefaultConnection": "Server=localhost;Database=ChatAppDb;User Id=sa;Password=YourPassword123!;TrustServerCertificate=true;"
```

### ✅ Option 3: SQL Server Express (LocalDB)
```json
"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ChatAppDb;Integrated Security=true;TrustServerCertificate=true;"
```

### ✅ Option 4: Azure SQL Database
```json
"DefaultConnection": "Server=your-server.database.windows.net,1433;Initial Catalog=ChatAppDb;Persist Security Info=False;User ID=your-username;Password=your-password;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
```

## 📝 Bước 3: Cập nhật appsettings.json

Mở file `ChatApp-RealTime/appsettings.json` và thay thế `DefaultConnection`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ChatAppDb;User Id=sa;Password=YourPassword123!;TrustServerCertificate=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

## 🚀 Bước 4: Chạy ứng dụng

```bash
dotnet restore
dotnet run --project ChatApp-RealTime/ChatApp-RealTime.csproj
```

**Database sẽ được tạo tự động!** 🎉

## ✨ Những lợi ích của SQL Server

| Tính năng | SQLite | SQL Server |
|----------|--------|-----------|
| **Concurrency** | ❌ Kém | ✅ Xuất sắc |
| **Multi-user** | ❌ Giới hạn | ✅ Unlimited |
| **Real-time** | ⚠️ Chậm | ✅ Nhanh |
| **Scalability** | ❌ Không | ✅ Có |
| **Deploy** | ⚠️ File-based | ✅ Server-based |

## ⚠️ Khắc phục sự cố

### Lỗi: "Cannot open database"
- Kiểm tra SQL Server đang chạy: `sqlcmd -S localhost -U sa -P YourPassword123!`

### Lỗi: "Login failed for user"
- Kiểm tra username/password trong connection string

### Lỗi: "Database already exists"
- Xóa database hoặc đặt tên database khác trong connection string

### Lỗi: "No migrations have been applied"
- Chạy: `dotnet ef database update`

## 📚 Tài liệu tham khảo

- [Microsoft EF Core SQL Server](https://learn.microsoft.com/en-us/ef/core/providers/sql-server/)
- [Connection Strings Reference](https://www.connectionstrings.com/sql-server/)
- [EF Core Migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/)

## 🎯 Tiếp theo

Sau khi deploy thành công, bạn có thể:
- ✅ Hỗ trợ nhiều người dùng đồng thời
- ✅ Cải thiện hiệu suất real-time
- ✅ Thêm tính năng nâng cao (backup, restore, etc.)
- ✅ Triển khai lên production

---

**Chúc mừng! ChatApp-RealTime của bạn giờ đã sẵn sàng cho production!** 🚀
