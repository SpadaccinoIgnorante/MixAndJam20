using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrdersManager : MonoBehaviour
{
    public GameObject orderPrefab;
    public OrderPosition[] positions;
    public List<OrderSet> orderSets;
    public List<Tables> tables { get; private set; }

    private int lastOrder;

    private List<OrderPaper> activeOrders;
    private void Start()
    {
        activeOrders = new List<OrderPaper>();

        tables = new List<Tables>();
        for (int i = 0; i < positions.Length; i++)
        {
            Tables tableToAdd = new Tables();
            tables.Add(tableToAdd);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            NewOrder();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            RemoveOrder(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            RemoveOrder(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            RemoveOrder(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            RemoveOrder(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            RemoveOrder(5);
        }
    }

    public void RemoveOrder(int tableNumber)
    {
        for (int i = 0; i < activeOrders.Count; i++)
        {
            if (activeOrders[i].table == tableNumber)
            {
                Destroy(activeOrders[i].gameObject);
                activeOrders.RemoveAt(i);
                tables[tableNumber - 1].hasOrder = false;
                positions[tables[tableNumber - 1].tablePosition].hasOrder = false;
            }
        }
    }

    public void NewOrder()
    {
        int allTablesFull = 0;

        for (int i = 0; i < tables.Count; i++)
        {
            if (tables[i].hasOrder)
            {
                allTablesFull++;
            }
        }

        if (allTablesFull < tables.Count)
        {
            int currentTable = CallTable();
            int currentPos = 0;
            OrderPosition currentPosition = null;

            for (int i = 0; i < positions.Length; i++)
            {
                if (!positions[i].hasOrder)
                {
                    currentPosition = positions[i];
                    currentPos = i;
                    break;
                }
            }

            GameObject orderObject = Instantiate(orderPrefab, currentPosition.position, Quaternion.Euler(0, 90, -90));
            int orderIndex = Random.Range(0, orderSets.Count);
            OrderSet currentOrder = orderSets[orderIndex];

            tables[currentTable].currentOrder = currentOrder;
            tables[currentTable].hasOrder = true;
            tables[currentTable].tablePosition = currentPos;
            currentPosition.hasOrder = true;

            orderObject.GetComponent<OrderPaper>().Init(lastOrder + 1, currentTable += 1, currentOrder.meat, currentOrder.tomato, currentOrder.lettuce, currentOrder.potato, currentOrder.egg, currentOrder.cheddar); ;
            activeOrders.Add(orderObject.GetComponent<OrderPaper>());         

            //
            lastOrder++;
        }
    }

    public int CallTable()
    {
        int result;

        result = Random.Range(0, tables.Count);

        if (tables[result].hasOrder)
        {
            result = CallTable();
        }

        return result;
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

    [System.Serializable]
    public class OrderPosition
    {
        public bool hasOrder;
        public Vector3 position;
    }

    [System.Serializable]
    public class Tables
    {
        public bool hasOrder;
        public int tablePosition;
        public OrderSet currentOrder;
    }
}
