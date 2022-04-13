using AutoMapper;
using InventoryMVC.Helpers;
using InventoryMVC.Interfaces;
using InventoryMVC.Models;
using InventoryMVC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMVC.Controllers
{
    public class ProductSupplierController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductSupplierController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
          
        [HttpGet]
        public async Task<IActionResult> GetSuppliers(int? id)
        {
            if (id == null) return NotFound();

            var product = await _unitOfWork.ProductRepository.GetByIdAsync((int)id);

            if (product == null) return NotFound("Could not find the product");

            var suppliersVM = await _unitOfWork.ProductSupplierRepository.GetSuppliersByProductId((int)id);

            ViewBag.ProductId = product.Id;
            ViewBag.ProductName = product.Name;
            
            return View(suppliersVM);
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(int? id)
        {
            if (id == null) return NotFound();

            var supplier = await _unitOfWork.SupplierRepository.GetByIdAsync((int)id);

            if (supplier == null) return NotFound();

            var productsVM = await _unitOfWork.ProductSupplierRepository.GetProductsBySupplierId((int)id);

            ViewBag.SupplierId = supplier.Id;
            ViewBag.SupplierName = supplier.SupplierName;

            return View(productsVM);
        }

        [HttpGet]
        public async Task<IActionResult> AddSupplier(int? id)
        {
            if (id == null) return NotFound();

            var product = await _unitOfWork.ProductRepository.GetByIdAsync((int)id);
            if (product == null) return NotFound("Could not find the product");
            
            var suppliers = await _unitOfWork.SupplierRepository.GetAllAsync();
            var suppliersVM = _mapper.Map<IEnumerable<SupplierViewModel>>(suppliers);
            ViewBag.SupplierList = new SelectList(suppliersVM, "Id", "SupplierName");

            ViewBag.ProductName = product.Name;

            return View(new ProductSupplierViewModel { ProductId = product.Id});
        }

        [HttpPost]
        public async Task<IActionResult> AddSupplier(ProductSupplierViewModel model)
        {
            var response = await GetAddResponse(model);

            if (!response.IsSuccessful) return BadRequest(response.Message);

            return RedirectToAction("GetSuppliers", new { id = model.ProductId });
        }

        [HttpGet]
        public async Task<IActionResult> AddProduct(int? id)
        {
            if (id == null) return NotFound();

            var supplier = await _unitOfWork.SupplierRepository.GetByIdAsync((int)id);
            if (supplier == null) return NotFound("Could not find the supplier");

            var products = await _unitOfWork.ProductRepository.GetAllAsync();
            var productsVM = _mapper.Map<IEnumerable<ProductViewModel>>(products);

            ViewBag.ProductList = new SelectList(productsVM, "Id", "Name");
            ViewBag.SupplierName = supplier.SupplierName;

            return View(new ProductSupplierViewModel { SupplierId = supplier.Id });
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductSupplierViewModel model)
        {
            var response = await GetAddResponse(model);

            if (!response.IsSuccessful) return BadRequest(response.Message);

            return RedirectToAction("GetProducts", new { id = model.SupplierId});
        }

        private async Task<ServerResponse> GetAddResponse(ProductSupplierViewModel model)
        {
            var productSupplier = await _unitOfWork.ProductSupplierRepository.GetProductSupplier(model.ProductId, model.SupplierId);
            
            if (productSupplier != null) return new ServerResponse { IsSuccessful = false, Message = "Record already exists" };
       
            var newProductSupplier = new ProductSupplier
            {
                ProductId = model.ProductId,
                SupplierId = model.SupplierId
            };

            _unitOfWork.ProductSupplierRepository.Add(newProductSupplier);

            await _unitOfWork.SaveAllAsync();

            return new ServerResponse { IsSuccessful = true, Message = null };

        }







    }
}
