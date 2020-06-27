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
        private readonly string RemontFileName = "Remont.xml";
        private readonly string ShipFileName = "Ship.xml";
        private readonly string ShipComponentFileName = "ShipComponent.xml";
        private readonly string SkladFileName = "Sklad.xml";
        private readonly string SkladComponentFileName = "SkladComponent.xml";

        public List<Component> Components { get; set; }
        public List<Remont> Remonts { get; set; }
        public List<Ship> Ships { get; set; }
        public List<ShipComponent> ShipComponents { get; set; }
        public List<Sklad> Sklads { get; set; }
        public List<SkladComponent> SkladComponents { get; set; }

        private FileDataListSingleton()
        {
            Components = LoadComponents();
            Remonts = LoadRemonts();
            Ships = LoadShips();
            ShipComponents = LoadShipComponents();
            Sklads = LoadSklads();
            SkladComponents = LoadSkladComponents();
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
            SaveRemonts();
            SaveShips();
            SaveShipComponents();
            SaveSklads();
            SaveSkladComponents();
        }
        private List<Sklad> LoadSklads()
        {
            var list = new List<Sklad>();

            if (File.Exists(SkladFileName))
            {
                XDocument xDocument = XDocument.Load(SkladFileName);
                var xElements = xDocument.Root.Elements("Sklad").ToList();
                foreach (var elem in xElements)
                {
                    list.Add(new Sklad
                    {
                        Id = Convert.ToInt32(elem.Attribute("Id").Value),
                        SkladName = elem.Element("SkladName").Value
                    });
                }
            }
            return list;
        }

        private List<SkladComponent> LoadSkladComponents()
        {
            var list = new List<SkladComponent>();
            if (File.Exists(SkladComponentFileName))
            {
                XDocument xDocument = XDocument.Load(SkladComponentFileName);
                var xElements = xDocument.Root.Elements("SkladComponent").ToList();
                foreach (var elem in xElements)
                {
                    list.Add(new SkladComponent
                    {
                        Id = Convert.ToInt32(elem.Attribute("Id").Value),
                        SkladId = Convert.ToInt32(elem.Element("SkladId").Value),
                        ComponentId = Convert.ToInt32(elem.Element("ComponentId").Value),
                        Count = Convert.ToInt32(elem.Element("Count").Value)
                    });
                }
            }
            return list;
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
        private void SaveRemonts()
        {
            if (Remonts != null)
            {
                var xElement = new XElement("Remonts");
                foreach (var order in Remonts)
                {
                    xElement.Add(new XElement("Remont",
                    new XAttribute("Id", order.Id),
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
        private void SaveSklads()
        {
            if (ShipComponents != null)
            {
                var xElement = new XElement("Sklads");

                foreach (var warehouse in Sklads)
                {
                    xElement.Add(new XElement("Sklad",
                    new XAttribute("Id", warehouse.Id),
                    new XElement("SkladName", warehouse.SkladName)));
                }

                XDocument xDocument = new XDocument(xElement);
                xDocument.Save(SkladFileName);
            }
        }

        private void SaveSkladComponents()
        {
            if (SkladComponents != null)
            {
                var xElement = new XElement("SkladComponents");
                foreach (var warehouseComponent in SkladComponents)
                {
                    xElement.Add(new XElement("SkladComponent",
                    new XAttribute("Id", warehouseComponent.Id),
                    new XElement("SkladId", warehouseComponent.SkladId),
                    new XElement("ComponentId", warehouseComponent.ComponentId),
                    new XElement("Count", warehouseComponent.Count)));
                }

                XDocument xDocument = new XDocument(xElement);
                xDocument.Save(SkladComponentFileName);
            }
        }
    }
}
