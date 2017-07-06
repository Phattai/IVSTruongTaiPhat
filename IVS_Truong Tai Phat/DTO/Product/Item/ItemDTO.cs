using Core.Interface;
using DTO.Product;
using DTO.Product;
using DTO.Product.Measure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Item
{
    public class ItemDTO : IDTO
    {
        [Key]
        public int? id { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public int? category_id { get; set; }

        public string category_name { get; set; }

        [Required(ErrorMessage = "Item Code is required")]
        [StringLength(30, ErrorMessage = "Item Code Maximum is 30")]
        public string code { get; set; }

        [StringLength(30, ErrorMessage = "Item Code Maximum is 30")]
        public string code_key { get; set; }

        [Required(ErrorMessage = "Item Name is required")]
        public string name { get; set; }

        [StringLength(256, ErrorMessage = "Specification Maximum is 256")]
        public string specification { get; set; }

        [StringLength(64, ErrorMessage = "Description Maximum is 64")]
        public string description { get; set; }

        public bool dangerous { get; set; }

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Discontinued Date is required")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? discontinued_datetime { get; set; }

        [Required(ErrorMessage = "Inventory Measure is required")]
        public int? inventory_measure_id { get; set; }

        public int? inventory_expired { get; set; }

        public double? inventory_standard_cost { get; set; }

        public double? inventory_list_price { get; set; }

        public double? manufacture_day { get; set; }

        public bool manufacture_make { get; set; }

        public bool manufacture_tool { get; set; }

        public bool manufacture_finished_goods { get; set; }

        [StringLength(16, ErrorMessage = "Item Code Maximum is 16")]
        public string manufacture_size { get; set; }

        [Required(ErrorMessage = "Manufacture Size Measure is required")]
        public int? manufacture_size_measure_id { get; set; }

        public string manufacture_weight { get; set; }

        [Required(ErrorMessage = "Manufacture Weight Measure is required")]
        public int? manufacture_weight_measure_id { get; set; }

        public string manufacture_style { get; set; }

        public string manufacture_class { get; set; }

        public string manufacture_color { get; set; }

        public List<MeasureDTO> Measure { get; set; }
    }

}
