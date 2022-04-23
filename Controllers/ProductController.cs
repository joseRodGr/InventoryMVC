using AutoMapper;
using InventoryMVC.Helpers;
using InventoryMVC.Interfaces;
using InventoryMVC.Models;
using InventoryMVC.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMVC.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(PaginationParams paginationParams) 
        {
            var productsVM = await _unitOfWork.ProductRepository.GetAllPagedAsync(paginationParams);
            return View(productsVM);
        }

        
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(await GetCategories(), "Id", "CategoryName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductViewModel model)
        {
            if (!ModelState.IsValid) 
            {
                ViewBag.Categories = new SelectList(await GetCategories(), "Id", "CategoryName");
                return View(model);
            }

            var newProduct = _mapper.Map<Product>(model);

            _unitOfWork.ProductRepository.Add(newProduct);

            await _unitOfWork.SaveAllAsync();

            return RedirectToAction("Index");

            
        }

        [HttpGet]
        [Authorize(Policy = Constants.Policies.RequiredAdmin)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _unitOfWork.ProductRepository.GetByIdAsync((int)id);

            if (product == null) return NotFound("Could not find the product");

            var editProductVM = _mapper.Map<EditProductViewModel>(product);

            ViewBag.Categories = new SelectList(await GetCategories(), "Id", "CategoryName");

            return View(editProductVM);
        }

        [HttpPost]
        [Authorize(Policy = Constants.Policies.RequiredAdmin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute]int id, EditProductViewModel editProductVM)
        {
            if (editProductVM.Id != id) return NotFound();

            if (ModelState.IsValid)
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);

                if (product == null) return NotFound("Could not find the product");

                _mapper.Map(editProductVM, product);

                _unitOfWork.ProductRepository.update(product);

                await _unitOfWork.SaveAllAsync();

                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(await GetCategories(), "Id", "CategoryName");
            return View(editProductVM);
            
        }

        [HttpPost]
        [Authorize(Policy = Constants.Policies.RequiredAdmin)]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null) return NotFound();

            _unitOfWork.ProductRepository.delete(product);

            if (await _unitOfWork.SaveAllAsync()) return Ok();

            return BadRequest();
        }
      

        private async Task<IEnumerable<Category>> GetCategories()
        {
            return await _unitOfWork.CategoryRepository.GetAllAsync();
        }

        
    }
}
