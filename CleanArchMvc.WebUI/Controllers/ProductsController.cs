using AutoMapper;
using CleanArchMvc.Application.DTOs;
using CleanArchMvc.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CleanArchMvc.WebUI.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _environment;
        private readonly IMapper _mapper;

        public ProductsController(
            IProductService productService, 
            ICategoryService categoryService,
            IWebHostEnvironment environment,
            IMapper mapper)
        {
            _productService = productService;
            _categoryService = categoryService;
            _environment = environment;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetProducts();
            return View(products);
        }

        [HttpGet()]
        public async Task<IActionResult> Create()
        {
            ViewBag.CategoryId = new SelectList(await _categoryService.GetCategories(), "Id", "Name");

            return View();
        }

        [HttpPost()]
        public async Task<IActionResult> Create(ProductDTO productDto)
        {
            ModelState.Remove("Category");
            if (ModelState.IsValid) 
            {
                await _productService.Add(productDto);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoryId = new SelectList(await _categoryService.GetCategories(), "Id", "Name");
            return View(productDto);
        }

        [HttpGet()]
        public async Task<IActionResult> Edit(int id)
        {
            var productDto = await _productService.GetById(id);

            if (productDto == null)
                return NotFound();
            
            var categories = await _categoryService.GetCategories();

            ViewBag.CategoryId = new SelectList(categories, "Id", "Name", productDto.CategoryId);

            return View(productDto);
        }

        [HttpPost()]
        public async Task<IActionResult> Edit(ProductDTO productDto)
        {
            ModelState.Remove("Category");

            if (ModelState.IsValid)
            {
                await _productService.Update(productDto);
                return RedirectToAction(nameof(Index));
            }
            var categories = await _categoryService.GetCategories();

            ViewBag.CategoryId = new SelectList(categories, "Id", "Name", productDto.CategoryId);

            return View(productDto);
        }
        [Authorize(Roles ="Admin")]
        [HttpGet()]
        public async Task<IActionResult> Delete(int id)
        {
            var productDto = await _productService.GetById(id);

            if (productDto == null) 
                return NotFound();

            return View(productDto);
        }

        [HttpPost(), ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productService.Remove(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var productDto = await _productService.GetById(id);

            if (productDto == null)
                return NotFound();

            var wwwroot = _environment.WebRootPath;
            var image = Path.Combine(wwwroot, "images/" + productDto.Image);
            var exists = System.IO.File.Exists(image);
            ViewBag.ImageExist = exists;

            return View(productDto);
        }
    }
}