using System;
using System.Collections.Generic;

namespace R4R_API.Models
{
    public class getAllRoom
    {

        public Room? room { get; set; }

        public List<string>? ImgRoom { get; set; }

        public string[]? Utilities { get; set; }

        public int total { get; set; }
    }
}


