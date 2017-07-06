using Core.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using DAL.Dao.Product;
using DTO.Product;
using Core.Common;

namespace BL.Product
{
    public class CategoryBL : IBL
    {
        public CommonData.ReturnCode SearchData(IDTO searchDto, out List<CategoryDTO> list)
        {
            CommonData.ReturnCode returnCode = CategoryDAO.SearchData(searchDto as CategoryDTO, out list);
            return returnCode;
        }

        public CommonData.ReturnCode DeleteData(int id)
        {
            CommonData.ReturnCode returnCode = CommonData.ReturnCode.Success;
            returnCode = CategoryDAO.DeleteData(id);
            return returnCode;
        }

        public CommonData.ReturnCode UpdateData(IDTO updateDto)
        {
            CommonData.ReturnCode returnCode = CategoryDAO.UpdateData(updateDto as CategoryDTO);
            return returnCode;
        }

        public CommonData.ReturnCode InsertData(IDTO insertDto)
        {
            CommonData.ReturnCode returnCode = CategoryDAO.InsertData(insertDto as CategoryDTO);
            return returnCode;
        }

        public int CountData(CategoryDTO updateDto)
        {
            return CategoryDAO.CountData(updateDto);
        }

        public List<CategoryDetailDTO> SelectDropdownData()
        {
            List<CategoryDetailDTO> list;
            CategoryDAO.SelectSimpleData(out list);
            return list;
        }

        public CommonData.ReturnCode SearchData(IDTO searchDto, out DataTable dtResult)
        {
            throw new NotImplementedException();
        }
    }
}
