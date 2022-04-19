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
    public class StockController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StockController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var productsStockVM = await _unitOfWork.StockRepository.GetProductsStockAsync();
            return View(productsStockVM);
        }

        public async Task<IActionResult> RegisterStock(int? id)
        {
            if (id == null) return NotFound();

            var product = await _unitOfWork.ProductRepository.GetByIdAsync((int)id);

            if (product == null) return NotFound("Could not find the product");

            var createInventoryVM = new CreateInventoryViewModel
            {
                ProductId = product.Id
            };

            return await ShowRegistrationView(product, createInventoryVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterStock([FromRoute]int id, CreateInventoryViewModel createInventoryVM)
        {
            if (id != createInventoryVM.ProductId) return NotFound();

            var product = await _unitOfWork.ProductRepository.GetByIdAsync(createInventoryVM.ProductId);

            if (product == null) return NotFound("Could not find the product");

            if (!ModelState.IsValid) return await ShowRegistrationView(product, createInventoryVM);

            var newInventoryMovement = _mapper.Map<InventoryMovement>(createInventoryVM);

            var validationResult = await GetCreateValidationResult(newInventoryMovement);

            if (!validationResult.IsSuccessful) return BadRequest(validationResult.Message);

            _unitOfWork.StockRepository.Add(newInventoryMovement);

            await _unitOfWork.SaveAllAsync();

            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var movement = await _unitOfWork.StockRepository.GetByIdAsync((int)id);

            if (movement == null) return NotFound("Could not find the inventory movement");

            var editInventoryVM = _mapper.Map<EditInventoryViewModel>(movement);

            editInventoryVM.Ammount = (editInventoryVM.Ammount < 0 ? editInventoryVM.Ammount * -1 : editInventoryVM.Ammount);

            return ShowEditView(movement, editInventoryVM);

        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute]int id, EditInventoryViewModel editInventoryVM)
        {
            if (id != editInventoryVM.Id) return NotFound();

            var movement = await _unitOfWork.StockRepository.GetByIdAsync(id);
            if (movement == null) return NotFound("Could not find the inventory movement");

            if (!ModelState.IsValid) return ShowEditView(movement, editInventoryVM);

            var validationResult = await GetEditValidationResult(movement, editInventoryVM);

            if(!validationResult.IsSuccessful) return BadRequest(validationResult.Message);

            _mapper.Map(editInventoryVM, movement);
            _unitOfWork.StockRepository.update(movement);

            await _unitOfWork.SaveAllAsync();
            return RedirectToAction("Movements", new { id = movement.ProductId });

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var movement = await _unitOfWork.StockRepository.GetByIdAsync(id);

            if (movement == null) return NotFound();

            _unitOfWork.StockRepository.delete(movement);

            if (await _unitOfWork.SaveAllAsync()) return Ok();

            return BadRequest();
        }

        public async Task<IActionResult> Movements(int? id)
        {
            if (id == null) return NotFound();

            var product = await _unitOfWork.ProductRepository.GetByIdAsync((int)id);

            if (product == null) return NotFound("Could not find the product");

            var stockMovements = await _unitOfWork.StockRepository.GetStockMovements((int)id);
            ViewBag.ProductName = product.Name;

            return View(stockMovements);
        }

        #region Helpers

        private async Task<IActionResult> ShowRegistrationView(Product product, CreateInventoryViewModel createInventoryVM)
        {
            ViewBag.ProductName = product.Name;
            ViewBag.CurrentStock = await _unitOfWork.StockRepository.GetCurrentStockById(product.Id);
            return View(createInventoryVM);

        }

        private IActionResult ShowEditView(InventoryMovement movement, EditInventoryViewModel editInventoryVM)
        {
            ViewBag.ProductId = movement.ProductId;
            ViewBag.ProductName = movement.Product.Name;
            ViewBag.RegisterDate = movement.Fecha;
            return View(editInventoryVM);
        }

        private async Task<ServerResponse> GetCreateValidationResult(InventoryMovement newInventoryMovement)
        {
            newInventoryMovement.Fecha = DateTime.Now;

            if (newInventoryMovement.Type == "Output")
            {
                var currentStock = await _unitOfWork.StockRepository.GetCurrentStockById(newInventoryMovement.ProductId);
                if (newInventoryMovement.Ammount > currentStock)
                    return new ServerResponse
                    {
                        IsSuccessful = false,
                        Message = "There is not enough stock to complete the output"
                    };

                newInventoryMovement.Ammount *= -1;
            }

            return new ServerResponse
            {
                IsSuccessful = true,
                Message = null
            };
        }

        private async Task<ServerResponse> GetEditValidationResult(InventoryMovement movement, EditInventoryViewModel editInventoryVM)
        {
            editInventoryVM.Ammount = (editInventoryVM.Type == "Input" ? editInventoryVM.Ammount : editInventoryVM.Ammount * -1);

            var currentStock = await _unitOfWork.StockRepository.GetCurrentStockById(movement.ProductId);

            var change = movement.Ammount - editInventoryVM.Ammount;

            if (currentStock - change < 0) return new ServerResponse { IsSuccessful = false, 
                Message = "There is not enough stock to complete the operation"};

            return new ServerResponse
            {
                IsSuccessful = true,
                Message = null
            };
        }

        #endregion



    }
}
