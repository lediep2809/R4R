using System;
using System.Collections.Generic;

namespace R4R_API.Models
{
    public class getAllRoom
    {

        public Room room { get; set; } = null!;

        public List<string>? ImgRoom { get; set; }

        public string[] Utilities { get; set; } = null!;

        public int total { get; set; }
    }
}


