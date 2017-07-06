using Core.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Product
{
    public class CategoryDTO :IDTO
    {
        [Key]
        public int? id { get; set; }

        public string parent_name { get; set; }

        [Required(ErrorMessage = "*Category Code is required")]
        [StringLength(16, ErrorMessage = "Category Code Maximum is 16")]
        public string code { get; set; }

        [StringLength(16, ErrorMessage = "Category Code Maximum is 16")]
        public string code_key { get; set; }

        [Required(ErrorMessage = "*Category Name is required")]
        [StringLength(64, ErrorMessage = "Category Code Maximum is 64")]
        public string name { get; set; }

        [StringLength(256, ErrorMessage = "Decription Maximum is 256")]
        public string decription { get; set; }

        [Required(ErrorMessage = "*Parent_ID is required")]
        public int? parent_id { get; set; }

        public List<CategoryDetailDTO> Category{ get; set; }
    }
}
