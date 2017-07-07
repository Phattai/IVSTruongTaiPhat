using BL.Product;
using Core.Common;
using DTO.Measure;
using DTO.Product.Measure;
using ProductManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductManagement.Controllers
{
    public class MeasureController : Controller
    {
        [HttpGet]
        public ActionResult Measure(string page, ResearchMeasureModel model)
        {
            ModelState.Clear();
            MeasureBL bl = new MeasureBL();
            if (model == null)
            {
                model = new ResearchMeasureModel();
                model.measure = new MeasureDTO();
                model.listMeasure = new List<MeasureDTO>();
            }
            else
            {
                if (model.measure == null)
                {
                    model.measure = new MeasureDTO();
                }
                if (model.listMeasure == null)
                {
                    model.listMeasure = new List<MeasureDTO>();
                }
            }
            if (page == null)
            {
                page = "1";
            }
            model.page_count = bl.CountData(model.measure);
            List<MeasureDTO> list;
            model.measure.page = int.Parse(page);
            bl.SearchData(model.measure, out list);
            model.listMeasure = list;

            TempData["Success"] = model.page_count + " row(s) has found.";

            return View(model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View("Add");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMeasure(MeasureDTO mesure)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    mesure.created_by = 113;
                    MeasureBL categorybl = new MeasureBL();
                    if (mesure.code.Contains(" "))
                    {
                        TempData["Error"] = "Don't input Space in Measure Code";
                        return View("Add");
                    }
                    int count = categorybl.CountData(new MeasureDTO() { code = mesure.code });
                    if (count == 0)
                    {
                        categorybl.InsertData(mesure);
                    }
                    else
                    {
                        TempData["Error"] = "The Code already is exister!";
                        return View("Add");
                    }
                    return RedirectToAction("Measure");
                }
            }
            catch (DataException dex)
            {
                ModelState.AddModelError("", "Unable to perform action. Please contact us.");
                return RedirectToAction("SubmissionFailed", mesure);
            }

            return View("Add");
        }

        [HttpGet]
        public ActionResult Update(string id)
        {
            MeasureDTO dto = new MeasureDTO();
            if (id.IsNumber())
            {
                List<MeasureDTO> list;
                MeasureBL bl = new MeasureBL();
                dto.id = int.Parse(id);
                bl.SearchData(dto, out list);
                if (list.Count > 0)
                {
                    dto = list[0];
                }
                else
                {
                    TempData["Success"] = "0 row(s) has found.";
                    return RedirectToAction("Measure");
                }
            }
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateMeasure(MeasureDTO measure)
        {
            MeasureBL mesureBL = new MeasureBL();
            try
            {
                if (ModelState.IsValid)
                {
                    measure.created_by = 123;
                    measure.updated_by = 123;
                    mesureBL.UpdateData(measure);
                    return RedirectToAction("Measure");
                }
            }
            catch (DataException dex)
            {
                ModelState.AddModelError("", "Unable to perform action. Please contact us.");
                return RedirectToAction("SubmissionFailed", measure);
            }
            measure = new MeasureDTO() { id = measure.id };
            return View("Update");
        }

        [HttpPost]
        public ActionResult DeleteMeasure(string id)
        {
            try
            {
                MeasureBL categoryBL = new MeasureBL();
                categoryBL.DeleteData(id.ParseInt32());

                return RedirectToAction("Measure");
            }
            catch (DataException dex)
            {
                ModelState.AddModelError("", "Unable to perform action. Please contact us.");
                return RedirectToAction("SubmissionFailed");
            }
        }
    }
}