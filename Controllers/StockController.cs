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
    public class StockController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StockController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string searchString)
        {
            var productsStockVM = await _unitOfWork.StockRepository.GetProductsStockAsync(searchString);
            ViewBag.SearchString = searchString;
            return View(productsStockVM);
        }

        public async Task<IActionResult> RegisterStock(int? id)
        {
            if (id == null) return NotFound();

            var product = await _unitOfWork.ProductRepository.GetByIdAsync((int)id);

            if (product == null) return NotFound();

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

            if (product == null) return NotFound();

            if (!ModelState.IsValid) return await ShowRegistrationView(product, createInventoryVM);

            var newInventoryMovement = _mapper.Map<InventoryMovement>(createInventoryVM);

            var validationResult = await GetCreateValidationResult(newInventoryMovement);

            if (!validationResult.IsSuccessful) return BadRequest(validationResult.Message);

            _unitOfWork.StockRepository.Add(newInventoryMovement);

            await _unitOfWork.SaveAllAsync();

            return RedirectToAction("Index");

        }

        [Authorize(Policy = Constants.Policies.RequiredAdmin)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var movement = await _unitOfWork.StockRepository.GetByIdAsync((int)id);

            if (movement == null) return NotFound();

            var editInventoryVM = _mapper.Map<EditInventoryViewModel>(movement);

            editInventoryVM.Ammount = (editInventoryVM.Ammount < 0 ? editInventoryVM.Ammount * -1 : editInventoryVM.Ammount);

            return ShowEditView(movement, editInventoryVM);

        }

        [HttpPost]
        [Authorize(Policy = Constants.Policies.RequiredAdmin)]
        public async Task<IActionResult> Edit([FromRoute]int id, EditInventoryViewModel editInventoryVM)
        {
            if (id != editInventoryVM.Id) return NotFound();

            var movement = await _unitOfWork.StockRepository.GetByIdAsync(id);

            if (movement == null) return NotFound();

            if (!ModelState.IsValid) return ShowEditView(movement, editInventoryVM);

            var validationResult = await GetEditValidationResult(movement, editInventoryVM);

            if(!validationResult.IsSuccessful) return BadRequest(validationResult.Message);

            _mapper.Map(editInventoryVM, movement);
            _unitOfWork.StockRepository.update(movement);

            await _unitOfWork.SaveAllAsync();
            return RedirectToAction("Movements", new { id = movement.ProductId });

        }

        [HttpPost]
        [Authorize(Policy = Constants.Policies.RequiredAdmin)]
        public async Task<IActionResult> Delete(int id)
        {
            var movement = await _unitOfWork.StockRepository.GetByIdAsync(id);

            if (movement == null) return NotFound();

            var currentStock = await _unitOfWork.StockRepository.GetCurrentStockById(movement.ProductId);

            if (currentStock - movement.Ammount < 0) return BadRequest("There is not enough stock to complete the operation");

            _unitOfWork.StockRepository.delete(movement);

            if (await _unitOfWork.SaveAllAsync()) return Ok();

            return BadRequest();
        }

        public async Task<IActionResult> Movements(int? id, string typeString)
        {
            if (id == null) return NotFound();

            var product = await _unitOfWork.ProductRepository.GetByIdAsync((int)id);

            if (product == null) return NotFound();

            var stockMovements = await _unitOfWork.StockRepository.GetStockMovements((int)id, typeString);

            var typeItems = new SelectList(new[] {"All", "Input", "Output"}, typeString ?? "All");

            ViewBag.TypeItems = typeItems;
            
            ViewBag.ProductName = product.Name;

            ViewBag.ProductId = product.Id;

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
