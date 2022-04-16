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
    public class SupplierController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SupplierController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(PaginationParams paginationParams)
        {
            var suppliersVM = await _unitOfWork.SupplierRepository.GetAllPagedAsync(paginationParams); ;
            return View(suppliersVM);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSupplierViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var newSupplier = _mapper.Map<Supplier>(model);

            _unitOfWork.SupplierRepository.Add(newSupplier);

            await _unitOfWork.SaveAllAsync();

            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var supplier = await _unitOfWork.SupplierRepository.GetByIdAsync((int)id);

            if (supplier == null) return NotFound("Could not find the supplier");

            var supplierVM = _mapper.Map<EditSupplierViewModel>(supplier);

            return View(supplierVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditSupplierViewModel editSupplierVM)
        {
            if (id != editSupplierVM.Id) return NotFound();

            var supplier = await _unitOfWork.SupplierRepository.GetByIdAsync(id);

            if (supplier == null) return NotFound("Could not find the supplier");

            _mapper.Map(editSupplierVM, supplier);

            _unitOfWork.SupplierRepository.update(supplier);

            await _unitOfWork.SaveAllAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var supplier = await _unitOfWork.SupplierRepository.GetByIdAsync(id);

            if (supplier == null) return NotFound();

            _unitOfWork.SupplierRepository.delete(supplier);

            if (await _unitOfWork.SaveAllAsync()) return Ok();

            return BadRequest();
        }

    }
}
