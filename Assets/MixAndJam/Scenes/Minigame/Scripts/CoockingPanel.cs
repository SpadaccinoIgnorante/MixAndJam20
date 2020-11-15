using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoockingPanel : BehaviourBase
{
    public Transform orderContent;
    public GameObject orderPaperPrefab;

    private OrdersManager ordersManager;

    public void Open()
    {
        ordersManager = FindObjectOfType<OrdersManager>();

        if(!MJGameManager.Instance.IsPaused)
            MJGameManager.Instance.PauseUnpause();

        foreach (var table in ordersManager.tables)
        {
            if (table == null) continue;

            if (table.hasOrder)
            {
                InstantiateOrder(table.currentOrder,table);
            }
        }
    }

    public void Close()
    {
        for (int i = 0; i < orderContent.childCount; i++)
        {
            Destroy(orderContent.GetChild(i).gameObject);
        }
    }

    private void InstantiateOrder(OrdersManager.OrderSet order, OrdersManager.Tables table)
    {
        var go = Instantiate(orderPaperPrefab,orderContent);

        go.GetComponent<OrderPaper>().Init(table,order);

    }

    protected override void CustomFixedUpdate() { }

    protected override void CustomUpdate() { }
}

[System.Serializable]
public class IngredientDictionary : Dictionary<int, Ingredient> { }


[System.Serializable]
public class Ingredient
{
    public Sprite sprite;
    public IngredientButton button;

}

[System.Serializable]
public class IngredientButton
{
    public Sprite sprite;
    public ButtonUI button;
    public bool hasClicked;
}
