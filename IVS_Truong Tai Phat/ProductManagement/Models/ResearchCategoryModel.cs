using DTO.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace ProductManagement.Models
{
    public class ResearchCategoryModel
    {
        public CategoryDTO Category { get; set; }

        public int page_count { get; set; }

        public List<CategoryDTO>  lstCategory { get; set; }

        public int parentID { get; set; }

        public string code { get; set; }

        public string name { get; set; }
    }
}