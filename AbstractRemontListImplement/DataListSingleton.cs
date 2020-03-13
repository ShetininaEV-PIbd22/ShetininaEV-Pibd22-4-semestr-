using AbstractRemontListImplement.Models;
using System.Collections.Generic;

namespace AbstractRemontListImplement
{
    public class DataListSingleton
    {
        private static DataListSingleton instance;
        public List<Component> Components { get; set; }
        public List<Remont> Remonts { get; set; }
        public List<Ship> Ships { get; set; }
        public List<ShipComponents> ShipComponents { get; set; }
        private DataListSingleton() 
        {
            Components = new List<Component>();
            Remonts = new List<Remont>();
            Ships = new List<Ship>();
            ShipComponents = new List<ShipComponents>();
        }

        public static DataListSingleton GetInstance()
        {
            if (instance == null)
                instance = new DataListSingleton();
            return instance;
        }
    }
}