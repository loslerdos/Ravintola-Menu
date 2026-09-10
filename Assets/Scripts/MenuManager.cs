using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public List<MenuCategory> categories = new List<MenuCategory>
    {
        new MenuCategory { categoryName = "Alkuruoat" },
        new MenuCategory { categoryName = "Pääruoat" },
        new MenuCategory { categoryName = "Jälkiruoat" },
        new MenuCategory { categoryName = "Juomat" }
    };
}
