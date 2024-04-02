using System.Collections.Generic;

namespace EasySettle.Models;

public class PropertyViewModel
{
    public Property? Property { get; set; }
    public List<string> ImageUrls { get; set; } = new List<string>();
}

