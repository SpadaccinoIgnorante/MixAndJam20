using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrdersManager : MonoBehaviour
{
    public GameObject orderPrefab;

    public List<OrderSet> orderSets;
    private int lastOrder;
    private Vector3 lastOrderPosition;

    private List<OrderPaper> activeOrders;
    private void Start()
    {
        activeOrders = new List<OrderPaper>();
        lastOrderPosition = Vector3.zero;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            GameObject orderObject = Instantiate(orderPrefab, lastOrderPosition, Quaternion.identity);
            int orderIndex = Random.Range(0, orderSets.Count);
            OrderSet currentOrder = orderSets[orderIndex];
            orderObject.GetComponent<OrderPaper>().Init(lastOrder, 0, currentOrder.meat, currentOrder.tomato, currentOrder.lettuce, currentOrder.potato, currentOrder.egg, currentOrder.cheddar);
            activeOrders.Add(orderObject.GetComponent<OrderPaper>());

            // Set new position
            Vector3 newOrderPosition = new Vector3(lastOrderPosition.x + 1, lastOrderPosition.y, lastOrderPosition.z);
            lastOrderPosition = newOrderPosition;

            //
            lastOrder++;
        }
    }

    [System.Serializable]
    public class OrderSet
    {
        public int meat;
        public int tomato;
        public int lettuce;
        public int potato;
        public int egg;
        public int cheddar;
    }
}
