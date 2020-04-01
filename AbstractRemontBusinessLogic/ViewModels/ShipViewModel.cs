﻿using System.Collections.Generic;
using System.ComponentModel;

namespace AbstractRemontBusinessLogic.ViewModels
{
    /// <summary>
    /// Корабль, изготавливаемый в мастерской
    /// </summary>
    public class ShipViewModel
    {
        public int Id { get; set; }

        [DisplayName("Название корабля")]
        public string ShipName { get; set; }

        [DisplayName("Цена")]
        public decimal Price { get; set; }

        public Dictionary<int, (string, int)> ShipComponents { get; set; }
    }
}
