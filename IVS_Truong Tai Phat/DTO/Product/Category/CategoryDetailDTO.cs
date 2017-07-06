using Core.Interface;
using System.ComponentModel.DataAnnotations;

namespace DTO.Product
{
    public class CategoryDetailDTO : IDTO
    {
        [Key]
        public int id { get; set; }

        public string code { get; set; }

        public string name { get; set; }
    }
}
