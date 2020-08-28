using AbstractRemontListImplement.Models;
using System.Collections.Generic;

namespace AbstractRemontListImplement
{
    public class DataListSingleton
    {
        private static DataListSingleton instance;
        public List<Client> Clients { get; set; }
        public List<Implementer> Implementers { get; set; }
        public List<Component> Components { get; set; }

        public List<Remont> Remonts { get; set; }

        public List<Ship> Ships { get; set; }

        public List<ShipComponents> ShipComponents { get; set; }
        public List<MessageInfo> MessageInfoes { get; set; }
        private DataListSingleton()
        {
            Components = new List<Component>();
            Clients = new List<Client>();
            Remonts = new List<Remont>();
            Ships = new List<Ship>();
            ShipComponents = new List<ShipComponents>();
            Implementers = new List<Implementer>();
            MessageInfoes = new List<MessageInfo>();
        }

        public static DataListSingleton GetInstance()
        {
            if (instance == null)
                instance = new DataListSingleton();
            return instance;
        }
    }
}