using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace IttezanPos.Models
{
public    class PopUpPassParameter
    {
        public Stream Myvalue { get; set; }
        public MediaFile mediaFile { get; set; }
        public Color productcolor { get; set; }
    }
}
