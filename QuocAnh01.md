Quocanh here
📍 LEVEL 3 — DTO + Service

  3a. Tạo DTO (trong MyApp.DTO, xoá Class1.cs, tạo ProductDto.cs):
  namespace MyApp.DTO;

  public class ProductDto
  {
      public int Id { get; set; }
      public string? Name { get; set; }
      public int SoLuong { get; set; }
      public DateTime NgaySX { get; set; }
      public string? NoiSX { get; set; }
  }
  Và CreateProductDto giống vậy nhưng bỏ Id (DB tự sinh).

  3b. Repository — bổ sung CRUD. Hiện IProductRepository mới có GetAllAsync. Thêm vào interface và class 4 hàm
  còn lại. Đây là việc bạn tự code, đây là "hợp đồng" cần đạt:
  Task<Product?> GetByIdAsync(int id);
  Task<Product>  AddAsync(Product p);
  Task<bool>     UpdateAsync(Product p);
  Task<bool>     DeleteAsync(int id);

  ▎ Gợi ý hàm EF Core: FindAsync(id), _db.Products.Add(p), _db.Products.Remove(p), và nhớ await
  ▎ _db.SaveChangesAsync() sau mỗi lần Add/Update/Delete (không gọi cái này thì DB không lưu).

  3c. Service (trong MyApp.Services, xoá Class1.cs, tạo IProductService.cs + ProductService.cs):
  // IProductService.cs
  public interface IProductService
  {
      Task<List<ProductDto>> GetAllAsync();
      Task<ProductDto?>      GetByIdAsync(int id);
      Task<ProductDto>       CreateAsync(CreateProductDto dto);
      Task<bool>            UpdateAsync(int id, ProductDto dto);
      Task<bool>            DeleteAsync(int id);
  }
  ProductService nhận IProductRepository qua constructor, gọi repo và chuyển Entity ⇄ DTO. Nhớ chèn 1 luật
  nghiệp vụ: ví dụ CreateAsync mà dto.SoLuong < 0 thì ném ArgumentException.

  ---
  📍 LEVEL 4 — Controller

  Trong MyApp.APIService/Controllers/, tạo ProductController.cs, nhận IProductService qua constructor, viết 5
  endpoint (GET all, GET by id, POST, PUT, DELETE) — trả mã HTTP đúng (Ok, NotFound, CreatedAtAction,
  NoContent). Đây là phần bạn tự ráp từ kiến thức Level 4 trong file BAITAP_Product.md.

  ---
  📍 LEVEL 5 — Đăng ký DI + Connection String

  Trong MyApp.APIService/Program.cs, trước dòng var app = builder.Build();, thêm:
  using Microsoft.EntityFrameworkCore;
  using MyApp.Repository;
  using MyApp.Services;

  builder.Services.AddDbContext<AppDbContext>(opt =>
      opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

  builder.Services.AddScoped<IProductRepository, ProductRepository>();
  builder.Services.AddScoped<IProductService, ProductService>();
  Và thêm vào appsettings.json (cạnh "AllowedHosts"):
  "ConnectionStrings": {
    "Default":
  "Server=(localdb)\\MSSQLLocalDB;Database=MyAppDb;Trusted_Connection=True;TrustServerCertificate=True"
  }

  ▎ ⚠️  Chuỗi kết nối tuỳ máy bạn. Bạn đang ở thư mục docker — bạn định chạy SQL Server bằng Docker, LocalDB,
  ▎ hay SQL Server cài sẵn? Cho tôi biết để tôi đưa đúng chuỗi.

  Task<Product>  AddAsync(Product p);
  Task<bool>     UpdateAsync(Product p);
  Task<bool>     DeleteAsync(int id);

  ▎ Gợi ý hàm EF Core: FindAsync(id), _db.Products.Add(p), _db.Products.Remove(p), và nhớ await
  ▎ _db.SaveChangesAsync() sau mỗi lần Add/Update/Delete (không gọi cái này thì DB không lưu).

  3c. Service (trong MyApp.Services, xoá Class1.cs, tạo IProductService.cs + ProductService.cs):
  // IProductService.cs
  public interface IProductService
  {
      Task<List<ProductDto>> GetAllAsync();
      Task<ProductDto?>      GetByIdAsync(int id);
      Task<ProductDto>       CreateAsync(CreateProductDto dto);
      Task<bool>            UpdateAsync(int id, ProductDto dto);
      Task<bool>            DeleteAsync(int id);
  }
  ProductService nhận IProductRepository qua constructor, gọi repo và chuyển Entity ⇄ DTO. Nhớ chèn 1 luật
  nghiệp vụ: ví dụ CreateAsync mà dto.SoLuong < 0 thì ném ArgumentException.

  ---
  📍 LEVEL 4 — Controller

  Trong MyApp.APIService/Controllers/, tạo ProductController.cs, nhận IProductService qua constructor, viết 5
  endpoint (GET all, GET by id, POST, PUT, DELETE) — trả mã HTTP đúng (Ok, NotFound, CreatedAtAction,
  NoContent). Đây là phần bạn tự ráp từ kiến thức Level 4 trong file BAITAP_Product.md.

  ---
  📍 LEVEL 5 — Đăng ký DI + Connection String

  Trong MyApp.APIService/Program.cs, trước dòng var app = builder.Build();, thêm:
  using Microsoft.EntityFrameworkCore;
  using MyApp.Repository;
  using MyApp.Services;

  builder.Services.AddDbContext<AppDbContext>(opt =>
      opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

  builder.Services.AddScoped<IProductRepository, ProductRepository>();
  builder.Services.AddScoped<IProductService, ProductService>();
  Và thêm vào appsettings.json (cạnh "AllowedHosts"):
  "ConnectionStrings": {
    "Default":
  "Server=(localdb)\\MSSQLLocalDB;Database=MyAppDb;Trusted_Connection=True;TrustServerCertificate=True"
  }

  ▎ ⚠️  Chuỗi kết nối tuỳ máy bạn. Bạn đang ở thư mục docker — bạn định chạy SQL Server bằng Docker, LocalDB,
  ▎ hay SQL Server cài sẵn? Cho tôi biết để tôi đưa đúng chuỗi.

  ---
  📍 LEVEL 6 — Tạo DB & chạy thử

  Sau khi build sạch, tạo bảng bằng migration rồi test bằng file .http. Tôi sẽ hướng dẫn lệnh cụ thể khi bạn
  tới đây.

  ---
  👉 Bắt đầu từ Level 3. Làm xong (hoặc kẹt) thì báo, tôi build + review. Và trả lời giúp tôi câu hỏi về SQL
  Server ở trên để chuẩn bị connection string cho đúng. 💪