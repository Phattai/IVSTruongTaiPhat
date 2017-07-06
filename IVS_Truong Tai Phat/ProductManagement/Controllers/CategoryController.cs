using BL.Product;
using Core.Common;
using DTO.Product;
using ProductManagement.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace ProductManagement.Controllers
{
    public class CategoryController : Controller
    {
        [HttpGet]
        public ActionResult Category(string page, ResearchCategoryModel model)
        {
            ModelState.Clear();
            CategoryBL bl = new CategoryBL();
            if (model == null)
            {
                model = new ResearchCategoryModel();
                model.Category = new CategoryDTO();
                model.lstCategory = new List<CategoryDTO>();
            }
            else
            {
                if (model.Category == null)
                {
                    model.Category = new CategoryDTO();
                }
                if (model.lstCategory == null)
                {
                    model.lstCategory = new List<CategoryDTO>();
                }
            }
            if (page == null)
            {
                page = "1";
            }
            model.page_count = bl.CountData(model.Category);
            List<CategoryDTO> list;
            model.Category.page = int.Parse(page);
            bl.SearchData(model.Category, out list);
            model.lstCategory = list;
            TempData["CountData"] = model.page_count + " row(s) has found.";
            return View(model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View("Add", LoadCategoryAddForm(new CategoryDTO()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCategory(CategoryDTO category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    category.created_by = 113;
                    CategoryBL categorybl = new CategoryBL();
                    if (category.code.Contains(" "))
                    {
                        TempData["Error"] = "Don't Input Space";
                        return View("Add", LoadCategoryAddForm(category));
                    }
                    int count = categorybl.CountData(new CategoryDTO() { code = category.code });
                    if (count == 0)
                    {
                        categorybl.InsertData(category);
                    }
                    else
                    {
                        TempData["Error"] = "The Code already is exister!";
                        return View("Add", LoadCategoryAddForm(category));
                    }
                    return RedirectToAction("Category");
                }
            }
            catch (DataException dex)
            {
                ModelState.AddModelError("", "Unable to perform action. Please contact us.");
                return RedirectToAction("SubmissionFailed", category);
            }

            return View("Add", LoadCategoryAddForm(category));
        }

        [HttpGet]
        public ActionResult Update(string id)
        {
            CategoryDTO dto = new CategoryDTO();
            if (id.IsNumber())
            {
                List<CategoryDTO> list;
                CategoryBL bl = new CategoryBL();
                dto.id = int.Parse(id);
                bl.SearchData(dto, out list);
                if (list.Count > 0)
                {
                    dto = list[0];
                }
                else
                {
                    TempData["Success"] = "0 row(s) has found.";
                    return RedirectToAction("Category");
                }
            }
            return View(LoadCategoryAddForm(dto));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCategory(CategoryDTO category)
        {
            CategoryBL categoryBL = new CategoryBL();
            try
            {
                if (ModelState.IsValid)
                {
                    category.created_by = 123;
                    category.updated_by = 123;
                    if (category.id != category.parent_id)
                    {
                        categoryBL.UpdateData(category);
                        return RedirectToAction("Category");
                    }
                    else
                    {
                        TempData["Error"] = "id and pararent_id is duplicated!";
                    }
                }
            }
            catch (DataException dex)
            {
                ModelState.AddModelError("", "Unable to perform action. Please contact us.");
                return RedirectToAction("SubmissionFailed", category);
            }
            category = new CategoryDTO() { id = category.id };
            return View("Update", LoadCategoryAddForm(category));
        }

        [HttpPost]
        public ActionResult DeleteCategory(string id)
        {
            try
            {
                CategoryBL categoryBL = new CategoryBL();
                categoryBL.DeleteData(id.ParseInt32());

                TempData["Success"] = "Delete Successful!";

                return RedirectToAction("Category");
                
            }
            catch (DataException dex)
            {
                ModelState.AddModelError("", "Unable to perform action. Please contact us.");
                return RedirectToAction("SubmissionFailed");
            }
        }

        private CategoryDTO LoadCategoryAddForm(CategoryDTO category)
        {
            if (category == null)
            {
                category = new CategoryDTO();
            }
            CategoryBL categoryBL = new CategoryBL();
            SelectList lstCategory = new SelectList(categoryBL.SelectDropdownData(), "id", "name");
            ViewBag.ListCategory = lstCategory;

            return category;
        }
    }
}