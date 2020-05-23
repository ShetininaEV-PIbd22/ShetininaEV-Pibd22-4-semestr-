using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AbstractRemontBusinessLogic.Enums;
using AbstractRemontFileImplement.Models;

namespace AbstractRemontFileImplement
{
    public class FileDataListSingleton
    {
        private static FileDataListSingleton instance;
        private readonly string ComponentFileName = "Component.xml";
        private readonly string ClientFileName = "Client.xml";
        private readonly string RemontFileName = "Remont.xml";
        private readonly string ShipFileName = "Ship.xml";
        private readonly string ShipComponentFileName = "ShipComponent.xml";
        public List<Component> Components { get; set; }
        public List<Client> Clients { get; set; }
        public List<Remont> Remonts { get; set; }
        public List<Ship> Ships { get; set; }
        public List<ShipComponent> ShipComponents { get; set; }
        private FileDataListSingleton()
        {
            Components = LoadComponents();
            Clients= LoadClients();
            Remonts = LoadRemonts();
            Ships = LoadShips();
            ShipComponents = LoadShipComponents();
        }
        public static FileDataListSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new FileDataListSingleton();
            }
            return instance;
        }
        ~FileDataListSingleton()
        {
            SaveComponents();
            SaveClients();
            SaveRemonts();
            SaveShips();
            SaveShipComponents();
        }
        private List<Component> LoadComponents()
        {
            var list = new List<Component>();
            if (File.Exists(ComponentFileName))
            {
                XDocument xDocument = XDocument.Load(ComponentFileName);
                var xElements = xDocument.Root.Elements("Component").ToList();
                foreach (var elem in xElements)
                {
                    list.Add(new Component
                    {
                        Id = Convert.ToInt32(elem.Attribute("Id").Value),
                        ComponentName = elem.Element("ComponentName").Value
                    });
                }
            }
            return list;
        }
        private List<Client> LoadClients()
        {
            var list = new List<Client>();
            if (File.Exists(ClientFileName))
            {
                XDocument xDocument = XDocument.Load(ClientFileName);
                var xElements = xDocument.Root.Elements("Client").ToList();

                foreach (var elem in xElements)
                {
                    list.Add(new Client
                    {
                        Id = Convert.ToInt32(elem.Attribute("Id").Value),
                        FIO = elem.Element("FIO").Value,
                        Login = elem.Element("Email").Value,
                        Password = elem.Element("Password").Value
                    });
                }
            }
            return list;
        }
        private List<Remont> LoadRemonts()
        {
            var list = new List<Remont>();
                if (File.Exists(RemontFileName))
                {
                    XDocument xDocument = XDocument.Load(RemontFileName);
                    Console.WriteLine("xDocument " + xDocument.ToString());
                    var xElements = xDocument.Root.Elements("Remont").ToList();
                    Console.WriteLine("xElements"+xElements.Count);
                    foreach (var ind in xElements)
                    {
                        Console.WriteLine("YES");
                        Console.WriteLine(ind.Value);
                    }
                    foreach (var elem in xElements)
                    {
                        list.Add(new Remont
                        {
                            Id = Convert.ToInt32(elem.Attribute("Id").Value),
                            ClientId= Convert.ToInt32(elem.Attribute("ClientId").Value),
                            ShipId = Convert.ToInt32(elem.Element("ShipId").Value),
                            Count = Convert.ToInt32(elem.Element("Count").Value),
                            Sum = Convert.ToDecimal(elem.Element("Sum").Value),
                            Status = (RemontStatus)Enum.Parse(typeof(RemontStatus),elem.Element("Status").Value),
                            DateCreate = Convert.ToDateTime(elem.Element("DateCreate").Value),
                            DateImplement =
                       string.IsNullOrEmpty(elem.Element("DateImplement").Value) ? (DateTime?)null :
                       Convert.ToDateTime(elem.Element("DateImplement").Value),
                        });
                    }
                }
                return list;
            }
        private List<Ship> LoadShips()
        {
            var list = new List<Ship>();
            if (File.Exists(ShipFileName))
            {
                XDocument xDocument = XDocument.Load(ShipFileName);
                var xElements = xDocument.Root.Elements("Ship").ToList();
                foreach (var elem in xElements)
                {
                    list.Add(new Ship
                    {
                        Id = Convert.ToInt32(elem.Attribute("Id").Value),
                        ShipName = elem.Element("ShipName").Value,
                        Price = Convert.ToDecimal(elem.Element("Price").Value)
                    });
                }
            }
            return list;
        }
        private List<ShipComponent> LoadShipComponents()
        {
            var list = new List<ShipComponent>();
            if (File.Exists(ShipComponentFileName))
            {
                XDocument xDocument = XDocument.Load(ShipComponentFileName);
                var xElements = xDocument.Root.Elements("ShipComponent").ToList();
                foreach (var elem in xElements)
                {
                    list.Add(new ShipComponent
                    {
                        Id = Convert.ToInt32(elem.Attribute("Id").Value),
                        ShipId = Convert.ToInt32(elem.Element("ShipId").Value),
                        ComponentId = Convert.ToInt32(elem.Element("ComponentId").Value),
                        Count = Convert.ToInt32(elem.Element("Count").Value)
                    });
                }
            }
            return list;
        }
        private void SaveComponents()
        {
            if (Components != null)
            {
                var xElement = new XElement("Components");
                foreach (var component in Components)
                {
                    xElement.Add(new XElement("Component",
                    new XAttribute("Id", component.Id),
                    new XElement("ComponentName", component.ComponentName)));
                }
                XDocument xDocument = new XDocument(xElement);
                xDocument.Save(ComponentFileName);
            }
        }
        private void SaveClients()
        {
            if (Clients != null)
            {
                var xElement = new XElement("Clients");
                foreach (var client in Clients)
                {
                    xElement.Add(new XElement("Client",
                    new XAttribute("Id", client.Id),
                    new XElement("FIO", client.FIO),
                    new XElement("Email", client.Login),
                    new XElement("Password", client.Password)));
                }
                XDocument xDocument = new XDocument(xElement);
                xDocument.Save(ClientFileName);
            }
        }
        private void SaveRemonts()
        {
            if (Remonts != null)
            {
                var xElement = new XElement("Remonts");
                foreach (var order in Remonts)
                {
                    xElement.Add(new XElement("Remont",
                    new XAttribute("Id", order.Id),
                    new XAttribute("ClientId", order.ClientId),
                    new XElement("ShipId", order.ShipId),
                    new XElement("Count", order.Count),
                    new XElement("Sum", order.Sum),
                    new XElement("Status", order.Status),
                    new XElement("DateCreate", order.DateCreate),
                    new XElement("DateImplement", order.DateImplement)));
                }
                XDocument xDocument = new XDocument(xElement);
                xDocument.Save(RemontFileName);
            }
        }
        private void SaveShips()
        {
            if (Ships != null)
            {
                var xElement = new XElement("Ships");
                foreach (var product in Ships)
                {
                    xElement.Add(new XElement("Ship",
                    new XAttribute("Id", product.Id),
                    new XElement("ShipName", product.ShipName),
                    new XElement("Price", product.Price)));
                }
                XDocument xDocument = new XDocument(xElement);
                xDocument.Save(ShipFileName);
            }
        }
        private void SaveShipComponents()
        {
            if (ShipComponents != null)
            {
                var xElement = new XElement("ShipComponents");
                foreach (var productComponent in ShipComponents)
                {
                    xElement.Add(new XElement("ShipComponent",
                    new XAttribute("Id", productComponent.Id),
                    new XElement("ShipId", productComponent.ShipId),
                    new XElement("ComponentId", productComponent.ComponentId),
                    new XElement("Count", productComponent.Count)));
                }
                XDocument xDocument = new XDocument(xElement);
                xDocument.Save(ShipComponentFileName);
            }
        }
    }
}
