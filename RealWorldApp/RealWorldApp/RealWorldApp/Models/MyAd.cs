using System;
using System.Collections.Generic;
using System.Text;

namespace RealWorldApp.Models
{
    public class MyAd
    {
        public int id { get; set; }
        public string title { get; set; }
        public double price { get; set; }
        public string model { get; set; }
        public string location { get; set; }
        public string company { get; set; }
        public DateTime datePosted { get; set; }
        public bool isFeatured { get; set; }
        public string imageUrl { get; set; }
        public string IsFeaturedAd => isFeatured ? "Featured" : "Free";
        public string AdPostedDate => datePosted.ToString("y");
        public string FullImageUrl => $"http://cvehicles.azurewebsites.net/{imageUrl}";
    }
}
