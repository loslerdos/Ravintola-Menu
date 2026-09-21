using System;

[Serializable]
public class CartItem
{
    public MenuItem menuItem;
    public int quantity;

    public CartItem(MenuItem menuItem)
    {
        this.menuItem = menuItem;
        quantity = 1;
    }
}