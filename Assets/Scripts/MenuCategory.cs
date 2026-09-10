using System;
using System.Collections.Generic;

[Serializable]
public class MenuCategory
{
    public string categoryName;
    public List<MenuItem> menuItems = new List<MenuItem>();
}
