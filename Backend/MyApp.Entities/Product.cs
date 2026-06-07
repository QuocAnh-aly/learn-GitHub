using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int SoLuong { get; set; }
        public DateTime NgaySX { get; set; }
        public string? NoiSX { get; set; }
    }
}
