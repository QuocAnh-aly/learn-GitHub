using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.DTO
{
    public class CreateProductDto
    {
        public string? Name { get; set; }
        public int SoLuong { get; set; }
        public DateTime NgaySX { get; set; }
        public string? NoiSX { get; set; }
    }
}
