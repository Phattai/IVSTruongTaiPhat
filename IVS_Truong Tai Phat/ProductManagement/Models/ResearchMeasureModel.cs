using DTO.Product;
using DTO.Product.Measure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductManagement.Models
{
    public class ResearchMeasureModel
    {
        public MeasureDTO measure { get; set; }

        public string code { get; set; }

        public string name { get; set; }

        public int page_count { get; set; }

        public List<MeasureDTO> listMeasure { get; set; }
    }
}