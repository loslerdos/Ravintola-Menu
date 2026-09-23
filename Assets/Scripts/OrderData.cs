using System;
using System.Collections.Generic;

[Serializable]
public class OrderLineData
{
    public MenuItem menuItem;
    public int quantity;
    public float lineTotal;
}

[Serializable]
public class OrderData
{
    public string orderId;
    public List<OrderLineData> items = new List<OrderLineData>();
    public float total;
}