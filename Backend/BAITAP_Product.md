# 🎯 Bài tập làm quen: Xây dựng tính năng "Quản lý Sản phẩm" (Product CRUD)

> Mục tiêu: Tự tay đi xuyên qua **cả 4 tầng** của kiến trúc microservices
> (Entities → Repository → Services → Controller) để hiểu **luồng dữ liệu** chảy như thế nào.
>
> Quy tắc: **Tự code**, phần dưới chỉ cho yêu cầu + gợi ý + tiêu chí chấm.
> Bí chỗ nào hãy hỏi, đừng copy lời giải hoàn chỉnh — sẽ không học được gì.

Chủ đề: API quản lý sản phẩm với 5 chức năng:
`Lấy tất cả` · `Lấy theo Id` · `Thêm` · `Sửa` · `Xoá`

---

## 🧱 LEVEL 0 — Chuẩn bị nền (sửa lỗi sẵn có)

- [ ] **0.1** Build thử toàn bộ solution để xem trạng thái:
  ```powershell
  dotnet build Backend/MyApp.slnx
  ```
- [ ] **0.2** Mở `MyApp.Entities/Product.cs`: đổi `internal class Product` → `public class Product`.
  *Vì sao?* `internal` = chỉ dùng được trong cùng project. Các tầng khác cần thấy nó ⇒ phải `public`.
- [ ] **0.3** Mở `MyApp.Repository/AppDbContext.cs`:
  - Đổi `internal class` → `public class`.
  - Thêm dòng `using MyApp.Entities;` ở đầu file (để nhận diện `Product`).

✅ *Đạt khi:* `dotnet build` chạy không còn lỗi đỏ.

---

## 🧩 LEVEL 1 — Entities & DTO (hình dạng dữ liệu)

- [ ] **1.1** Trong `Product.cs`, đảm bảo các thuộc tính chuỗi không cảnh báo null.
  *Gợi ý:* dùng `string Name { get; set; } = string.Empty;` hoặc `string? Name`.
- [ ] **1.2** Tạo file `MyApp.DTO/ProductDto.cs` với class **public** `ProductDto` gồm:
  `Id, Name, SoLuong, NgaySX, NoiSX` (giống Entity).
- [ ] **1.3** Tạo thêm `CreateProductDto` (giống nhưng **không có `Id`** — vì khi tạo mới DB tự sinh Id).

💡 *Câu hỏi tự trả lời:* Tại sao cần DTO riêng thay vì dùng thẳng Entity cho API?
(Gợi ý: tách "hình dạng DB" khỏi "hình dạng client thấy", ẩn field nhạy cảm, tránh lộ cấu trúc bảng.)

✅ *Đạt khi:* 2 file DTO build được, đều là `public`.

---

## 🗄️ LEVEL 2 — Repository (nói chuyện với Database)

- [ ] **2.1** Mở `IProductRepository.cs` — bổ sung đủ "hợp đồng" CRUD:
  ```csharp
  Task<List<Product>> GetAllAsync();
  Task<Product?> GetByIdAsync(int id);
  Task<Product>  AddAsync(Product product);
  Task<bool>     UpdateAsync(Product product);
  Task<bool>     DeleteAsync(int id);
  ```
- [ ] **2.2** Tạo `ProductRepository.cs` **implement** `IProductRepository`.
  *Gợi ý:* nhận `AppDbContext` qua constructor, dùng `_context.Products` + các hàm EF Core
  (`ToListAsync`, `FindAsync`, `Add`, `Remove`, `SaveChangesAsync`).

💡 *Khái niệm:* `interface` = bản hợp đồng (cái gì làm được), `class` = cách làm cụ thể.
Tách ra để sau này thay cách lưu (SQL → Mongo) mà tầng trên không phải sửa.

✅ *Đạt khi:* `ProductRepository` build được, không còn method nào báo "chưa implement".

---

## ⚙️ LEVEL 3 — Services (logic nghiệp vụ)

- [ ] **3.1** Trong `MyApp.Services`, xoá `Class1.cs`, tạo `IProductService.cs` + `ProductService.cs`.
- [ ] **3.2** `ProductService` nhận `IProductRepository` qua constructor.
- [ ] **3.3** Viết các hàm gọi xuống Repository, và **chuyển đổi Entity ⇄ DTO** ở đây.
  Ví dụ `GetAllAsync` trả `List<ProductDto>` (không trả thẳng Entity ra ngoài).
- [ ] **3.4** Thêm **1 luật nghiệp vụ** để thấy vai trò tầng Service:
  *vd:* không cho thêm sản phẩm có `SoLuong < 0` → ném lỗi hoặc trả về thất bại.

💡 *Phân biệt:* Repository chỉ "lấy/lưu dữ liệu". Service mới là nơi đặt **quy tắc** (validate, tính toán, ghép nhiều nguồn).

✅ *Đạt khi:* Service build được và có ít nhất 1 chỗ kiểm tra điều kiện nghiệp vụ.

---

## 🌐 LEVEL 4 — Controller (cửa ra vào API)

Làm trong project **`MyApp.APIService`**.

- [ ] **4.1** Tạo `Controllers/ProductController.cs`, đánh dấu `[ApiController]` và `[Route("[controller]")]`.
- [ ] **4.2** Nhận `IProductService` qua constructor.
- [ ] **4.3** Viết 5 endpoint:
  | Method | URL | Việc |
  |--------|-----|------|
  | GET    | `/product`       | lấy tất cả |
  | GET    | `/product/{id}`  | lấy theo id (không có → trả `404 NotFound`) |
  | POST   | `/product`       | tạo mới (nhận `CreateProductDto`) → trả `201 Created` |
  | PUT    | `/product/{id}`  | cập nhật |
  | DELETE | `/product/{id}`  | xoá |

💡 *Khái niệm:* Controller **không** chứa logic, **không** đụng DB. Nó chỉ:
nhận request → gọi Service → trả mã HTTP phù hợp (`Ok`, `NotFound`, `BadRequest`, `CreatedAtAction`).

✅ *Đạt khi:* đủ 5 endpoint, mỗi cái trả mã HTTP hợp lý.

---

## 🔌 LEVEL 5 — Ráp nối (Dependency Injection + Database)

Mở `MyApp.APIService/Program.cs`:

- [ ] **5.1** Thêm tham chiếu project: `APIService` cần tham chiếu `MyApp.Services`, `MyApp.Repository`, `MyApp.DTO`, `MyApp.Entities`.
  ```powershell
  dotnet add Backend/MyApp.APIService reference Backend/MyApp.Services Backend/MyApp.Repository Backend/MyApp.DTO Backend/MyApp.Entities
  ```
- [ ] **5.2** Đăng ký DbContext + connection string:
  ```csharp
  builder.Services.AddDbContext<AppDbContext>(opt =>
      opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
  ```
  và thêm vào `appsettings.json`:
  ```json
  "ConnectionStrings": { "Default": "Server=...;Database=MyAppDb;Trusted_Connection=True;TrustServerCertificate=True" }
  ```
- [ ] **5.3** Đăng ký DI cho service & repo:
  ```csharp
  builder.Services.AddScoped<IProductRepository, ProductRepository>();
  builder.Services.AddScoped<IProductService, ProductService>();
  ```

💡 *Khái niệm — DI (Dependency Injection):* bạn "khai báo" ai cần gì, .NET tự "tiêm" vào constructor.
Nhờ vậy Controller xin `IProductService`, Service xin `IProductRepository` mà không phải tự `new`.

✅ *Đạt khi:* `dotnet run --project Backend/MyApp.APIService` lên được, không lỗi DI lúc khởi động.

---

## 🧪 LEVEL 6 — Tạo Database & chạy thử

- [ ] **6.1** Tạo migration đầu tiên (chạy ở thư mục `MyApp.APIService`):
  ```powershell
  dotnet ef migrations add InitProduct --project Backend/MyApp.Repository --startup-project Backend/MyApp.APIService
  dotnet ef database update --project Backend/MyApp.Repository --startup-project Backend/MyApp.APIService
  ```
  *(Cần SQL Server đang chạy. Nếu chưa có, hỏi tôi cách dùng Docker.)*
- [ ] **6.2** Mở file `MyApp.APIService.http`, viết thử các request POST/GET/PUT/DELETE tới `/product`.
- [ ] **6.3** Kiểm tra: POST tạo 1 sản phẩm → GET thấy nó → PUT sửa → GET thấy đổi → DELETE → GET không còn.

✅ *Đạt khi:* đủ vòng đời CRUD chạy thật, dữ liệu lưu vào DB.

---

## 🚀 BONUS (làm khi đã vững)

- [ ] **B1** Cấu hình **Ocelot** trong `MyApp.ApiGateway` để gọi `/product` qua Gateway thay vì gọi thẳng service.
- [ ] **B2** Viết **Unit Test** cho `ProductService` trong `MyApp.UnitTest` (giả lập repository bằng Moq).
- [ ] **B3** Khi tạo/sửa/xoá sản phẩm, gọi sang **AuditLog** để ghi lại "ai vừa thao tác gì".

---

## 📌 Tự kiểm tra hiểu bài (trả lời được là nắm vững)

1. Một request `GET /product` đi qua những file/tầng nào, theo thứ tự?
2. Vì sao Controller không gọi thẳng Repository?
3. Khác nhau giữa `Product` (Entity) và `ProductDto`?
4. DI giúp gì? Nếu bỏ DI thì Controller phải tự làm gì?
5. `interface IProductRepository` để làm gì khi đã có `ProductRepository`?

---

**Cách làm:** làm tuần tự từ Level 0. Xong mỗi Level, build thử rồi báo tôi —
tôi sẽ review code của bạn, chỉ chỗ sai và giải thích, **chứ không làm hộ**. Bắt đầu từ Level 0 nhé! 💪



Level 2 xong! Qua Level 3.

  ---
  Level 3: Service (logic nghiệp vụ)

  Tầng Service nằm giữa Controller và Repository. Controller gọi Service, Service gọi Repository.

  Tôi xem cấu trúc project Services trước:

  Searched for 1 pattern, read 2 files (ctrl+o to expand)

● Interface đã có sẵn. ProductService mới chỉ có constructor, chưa có method nào. Bạn cần thêm field _repo và viết 5 method.

  ---
  ✍️  Bạn sửa file ProductService.cs

  Xóa hết, gõ lại toàn bộ:

  using MyApp.DTO;
  using MyApp.Entities;
  using MyApp.Repository;

  namespace MyApp.Services
  {
      public class ProductService : IProductService
      {
          private readonly IProductRepository _repo;

          public ProductService(IProductRepository repo)
          {
              _repo = repo;
          }

          public async Task<List<ProductDto>> GetAllAsync()
          {
              var list = await _repo.GetAllAsync();
              return list.Select(p => new ProductDto
              {
                  Id = p.Id,
                  Name = p.Name,
                  SoLuong = p.SoLuong,
                  NgaySX = p.NgaySX,
                  NoiSX = p.NoiSX
              }).ToList();
          }

          public async Task<ProductDto?> GetByIdAsync(int id)
          {
              var p = await _repo.GetByIdAsync(id);
              if (p == null) return null;
              return new ProductDto
              {
                  Id = p.Id,
                  Name = p.Name,
                  SoLuong = p.SoLuong,
                  NgaySX = p.NgaySX,
                  NoiSX = p.NoiSX
              };
          }

          public async Task<ProductDto> CreateAsync(CreateProductDto dto)
          {
              var p = new Product
              {
                  Name = dto.Name,
                  SoLuong = dto.SoLuong,
                  NgaySX = dto.NgaySX,
                  NoiSX = dto.NoiSX
              };
              var created = await _repo.AddAsync(p);
              return new ProductDto
              {
                  Id = created.Id,
                  Name = created.Name,
                  SoLuong = created.SoLuong,
                  NgaySX = created.NgaySX,
                  NoiSX = created.NoiSX
              };
          }

          public async Task<bool> UpdateAsync(int id, ProductDto dto)
          {
              var p = new Product
              {
                  Id = id,
                  Name = dto.Name,
                  SoLuong = dto.SoLuong,
                  NgaySX = dto.NgaySX,
                  NoiSX = dto.NoiSX
              };
              return await _repo.UpdateAsync(p);
          }

          public async Task<bool> DeleteAsync(int id)
          {
              return await _repo.DeleteAsync(id);
                  NoiSX = created.NoiSX
              };
          }

          public async Task<bool> UpdateAsync(int id, ProductDto dto)
          {
              var p = new Product
              {
                  Id = id,
                  Name = dto.Name,
                  SoLuong = dto.SoLuong,
                  NgaySX = dto.NgaySX,
                  NoiSX = dto.NoiSX
              };
              return await _repo.UpdateAsync(p);
          }

          public async Task<bool> DeleteAsync(int id)
          {
              return await _repo.DeleteAsync(id);
          }
      }
  }

          public async Task<bool> DeleteAsync(int id)
          {
              return await _repo.DeleteAsync(id);
          }
      }
  }

  ---
  Điểm quan trọng cần hiểu — tại sao Service "chuyển đổi" dữ liệu?

  Repository trả về:  Product (Entity — hình dạng database)
  Service trả về:     ProductDto (hình dạng client thấy)

  Service là nơi map từ Entity sang DTO. Sau này nếu database thay đổi cấu trúc, bạn chỉ sửa tầng này — Controller và frontend không bị ảnh
  hưởng.

  ---
  Gõ xong, build lại:

  dotnet build E:\docker\learn-GitHub\Backend\MyApp.slnx

  Build succeeded → báo tôi, qua Level 4 (Controller — tầng cuối cùng!).