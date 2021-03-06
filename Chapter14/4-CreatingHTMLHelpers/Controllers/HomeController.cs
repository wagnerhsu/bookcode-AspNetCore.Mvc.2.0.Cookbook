﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private IProductRepository _productRepository;

        public HomeController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            var productsDtoFromRepo = _productRepository.GetProducts();
            var model =
            Mapper.Map<IEnumerable<ProductViewModel>>
            (productsDtoFromRepo);
            return View(model);
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProduct(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Id = _productRepository.GetProducts()
                .Max(p => p.Id) + 1;
                var productDto = Mapper.Map<ProductDto>(model);
                _productRepository.AddProduct(productDto);

                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
