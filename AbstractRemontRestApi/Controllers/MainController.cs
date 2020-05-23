using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AbstractRemontBusinessLogic.BindingModels;
using AbstractRemontBusinessLogic.BusinessLogics;
using AbstractRemontBusinessLogic.Interfaces;
using AbstractRemontBusinessLogic.ViewModels;
using AbstractRemontRestApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AbstractRemontRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly IRemontLogic _order;
        private readonly IShipLogic _product;
        private readonly MainLogic _main;
        public MainController(IRemontLogic order, IShipLogic product, MainLogic main)
        {
            _order = order;
            _product = product;
            _main = main;
        }
        [HttpGet]
        public List<ShipModel> GetShipList() => _product.Read(null)?.Select(rec => Convert(rec)).ToList();
        [HttpGet]
        public ShipModel GetShip(int productId) => Convert(_product.Read(new ShipBindingModel { Id = productId })?[0]);
        [HttpGet]
        public List<RemontViewModel> GetOrders(int clientId) => _order.Read(new RemontBindingModel { ClientId = clientId });
        [HttpPost]
        public void CreateOrder(CreateRemontBindingModel model) => _main.CreateRemont(model);
        private ShipModel Convert(ShipViewModel model)
        {
            if (model == null) return null;
            return new ShipModel
            {
                Id = model.Id,
                ShipName = model.ShipName,
                Price = model.Price
            };
        }
    }
}