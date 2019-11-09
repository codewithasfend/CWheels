using System;
using System.Collections.Generic;
using System.Text;

namespace RealWorldApp.Models
{
    public class UserImageModel
    {
        public string imageUrl { get; set; }

        public string FullImagePath => $"http://cvehicles.azurewebsites.net/{imageUrl}";
    }
}
