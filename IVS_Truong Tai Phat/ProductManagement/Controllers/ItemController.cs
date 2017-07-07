using BL.Product;
using Core.Common;
using DTO.Product;
using DTO.Item;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Ivs.Models;

namespace Ivs.Controllers
{
    public class ItemController : Controller
    {
        CategoryBL categoryBL = new CategoryBL();

        [HttpGet]
        public ActionResult Item(string page, ResearchItemModel model)
        {
            ModelState.Clear();
            if (model == null)
            {
                model = new ResearchItemModel();
                model.Item = new ItemDTO();
                model.Items = new List<ItemDTO>();
            }
            else
            {
                if (model.Item == null)
                {
                    model.Item = new ItemDTO();
                }
                if (model.Items == null)
                {
                    model.Items = new List<ItemDTO>();
                }
            }
            if (page == null)
            {
                page = "1";
            }
            ItemBL bl = new ItemBL();
            model.page_count = bl.CountData(model.Item);
            List<ItemDTO> list;
            model.Item.page = int.Parse(page);
            bl.SearchData(model.Item, out list);
            model.Items = list;
            SelectList listCategory = new SelectList(categoryBL.SelectDropdownData(), "id", "name");
            ViewBag.ListCategory = listCategory;
            TempData["Success"] = model.page_count + " row(s) has found.";
            return View(model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View("Add", LoadItemAddForm(new ItemDTO()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddItem(ItemDTO item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    item.created_by = 123;
                    ItemBL itemBL = new ItemBL();
                    int count = itemBL.CountData(new ItemDTO() { code = item.code });
                    if (count == 0)
                    {
                        itemBL.InsertData(item);
                    }
                    else
                    {
                        TempData["Error"] = "The Code already is exister!";
                        return View("Add", LoadItemAddForm(item));
                    }

                    return RedirectToAction("Item");
                }
            }
            catch (DataException dex)
            {
                ModelState.AddModelError("", "Lỗi không xác định");
                return RedirectToAction("SubmissionFailed", item);
            }

            return View("Add", LoadItemAddForm(item));
        }

        [HttpGet]
        public ActionResult Update(string id)
        {
            ItemDTO dto = new ItemDTO();
            if (id.IsNumber())
            {
                List<ItemDTO> list;
                ItemBL bl = new ItemBL();
                dto.id = int.Parse(id);
                bl.SearchData(dto, out list);
                if (list.Count > 0)
                {
                    dto = list[0];
                }
                else
                {
                    TempData["Success"] = "0 row(s) has found.";
                    return RedirectToAction("Item");
                }
            }
            return View(LoadItemAddForm(dto));
        }

        [HttpPost]
        public ActionResult UpdateItem(ItemDTO item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    item.created_by = 123;
                    item.updated_by = 123;
                    ItemBL itemBL = new ItemBL();
                    itemBL.UpdateData(item);

                    return RedirectToAction("Item");
                }
            }
            catch (DataException dex)
            {
                ModelState.AddModelError("", "Lỗi không xác định");
                return RedirectToAction("SubmissionFailed", item);
            }

            return View("Update", LoadItemAddForm(item));
        }

        [HttpPost]
        public ActionResult DeleteItem(string id)
        {
            try
            {
                ItemBL itemBL = new ItemBL();
                itemBL.DeleteData(id.ParseInt32());

                return RedirectToAction("Item");
            }
            catch (DataException dex)
            {
                ModelState.AddModelError("", "Lỗi không xác định");
                return RedirectToAction("SubmissionFailed");
            }
        }

        private ItemDTO LoadItemAddForm(ItemDTO item)
        {
            if (item == null)
            {
                item = new ItemDTO();
            }
            SelectList listCategory = new SelectList(categoryBL.SelectDropdownData(), "id", "name");
            ViewBag.ListCategory = listCategory;


            MeasureBL measureBL = new MeasureBL();
            SelectList listMeasure = new SelectList(measureBL.SelectDropdownData(), "id", "name");
            ViewBag.listMeasure = listMeasure;


            return item;
        }
    }
}