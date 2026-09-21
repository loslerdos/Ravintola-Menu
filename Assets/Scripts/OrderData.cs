using System;
using System.Collections.Generic;

[Serializable]
public class OrderData
{
    public string orderId;
    public List<MenuItem> items = new List<MenuItem>();
    public float total;
}