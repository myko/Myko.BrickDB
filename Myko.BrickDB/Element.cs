using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Myko.BrickDB
{
    public class Element
    {
        public string ElementId { get; set; }
        public Design? Design { get; set; }
        public Color? Color { get; set; }
        public string Description { get; set; }

        public Element(string elementId, string description)
        {
            ElementId = elementId;
            Description = description;
        }
    }

    public class Design
    {
        public string DesignId { get; set; }
        public string Description { get; set; }

        public ICollection<Element> Elements { get; set; } = new List<Element>();

        public Design(string designId, string description)
        {
            DesignId = designId;
            Description = description;
        }
    }

    public class Color
    {
        public string ColorId { get; set; }
        public string Description { get; set; }

        public ICollection<Element> Elements { get; set; } = new List<Element>();

        public Color(string colorId, string description)
        {
            ColorId = colorId;
            Description = description;
        }
    }

    //public class Vendor
    //{
    //    public int VendorId { get; set; }
    //    public string Name { get; set; }
    //}

    //public class VendorPrice
    //{
    //    public int VendorId { get; set; }
    //    public string ElementId { get; set; }
    //    public decimal Price { get; set; }
    //}
}
