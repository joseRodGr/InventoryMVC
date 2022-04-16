using AutoMapper;
using InventoryMVC.Helpers;
using InventoryMVC.Interfaces;
using InventoryMVC.Models;
using InventoryMVC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(PaginationParams paginationParams)
        {
            var categoriesVM = await _unitOfWork.CategoryRepository.GetAllPagedAsync(paginationParams);
            return View(categoriesVM);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var newCategory = _mapper.Map<Category>(model);

            _unitOfWork.CategoryRepository.Add(newCategory);

            await _unitOfWork.SaveAllAsync();

            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var category = await _unitOfWork.CategoryRepository.GetByIdAsync((int)id);

            if (category == null) return NotFound("Could not find the category");

            var categoryVM = _mapper.Map<EditCategoryViewModel>(category);

            return View(categoryVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute]int id, EditCategoryViewModel editCategoryVM)
        {
            if (id != editCategoryVM.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);

                if (category == null) return NotFound();

                _mapper.Map(editCategoryVM, category);

                _unitOfWork.CategoryRepository.update(category);

                await _unitOfWork.SaveAllAsync();

                return RedirectToAction("Index");
            }

            return View(editCategoryVM);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);

            if (category == null) return NotFound();

            _unitOfWork.CategoryRepository.delete(category);

            if (await _unitOfWork.SaveAllAsync()) return Ok();

            return BadRequest();
        }


    }
}
