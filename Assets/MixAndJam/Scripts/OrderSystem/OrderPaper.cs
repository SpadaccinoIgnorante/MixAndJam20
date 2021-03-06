﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class OrderPaper : BehaviourBase
{
    [Header("Meat")]
    public GameObject meatObject;
    public TMP_Text meatQuantityText;
    [Header("Tomato")]
    public GameObject tomatoObject;
    public TMP_Text tomatoQuantityText;
    [Header("Lettuce")]
    public GameObject lettuceObject;
    public TMP_Text lettuceQuantityText;
    [Header("Potato")]
    public GameObject potatoObject;
    public TMP_Text potatoQuantityText;
    [Header("Egg")]
    public GameObject eggObject;
    public TMP_Text eggQuantityText;
    [Header("Cheddar")]
    public GameObject cheddarObject;
    public TMP_Text cheddarQuantityText;
    [Header("Generic")]
    public TMP_Text orderText;
    public TMP_Text tableText;

    public float orderDeadline;
    public float currentTime;
    public int table { get; private set; }
    public int OrderNumber { get; private set; }
    public Action OnEnd;

    public void Init(OrdersManager.Tables table, OrdersManager.OrderSet order)
    {
        Init(order.orderNumber,table.tableNumber,order.meat,order.tomato,order.lettuce,order.potato,order.egg,order.cheddar);
    }

    public void Init(int orderNumber, int tableNumber, int meatQuantity = 0, int tomatoQuantity = 0, int lettuceQuantity = 0, int potatoQuantity = 0, int eggQuantity = 0, int cheddarQuantity = 0)
    {
        orderText.text = $"Order #{orderNumber}";
        tableText.text = $"Table n.{tableNumber}";

        OrderNumber = orderNumber;
        table = tableNumber;

        if (meatQuantity > 0)
        {
            meatQuantityText.text = $"X {meatQuantity}";
        }
        else
        {
            meatObject.SetActive(false);
        }

        if (tomatoQuantity > 0)
        {
            tomatoQuantityText.text = $"X {tomatoQuantity}";
        }
        else
        {
            tomatoObject.SetActive(false);
        }

        if (lettuceQuantity > 0)
        {
            lettuceQuantityText.text = $"X {lettuceQuantity}";
        }
        else
        {
            lettuceObject.SetActive(false);
        }

        if (potatoQuantity > 0)
        {
            potatoQuantityText.text = $"X {potatoQuantity}";
        }
        else
        {
            potatoObject.SetActive(false);
        }

        if (eggQuantity > 0)
        {
            eggQuantityText.text = $"X {eggQuantity}";
        }
        else
        {
            eggObject.SetActive(false);
        }

        if (cheddarQuantity > 0)
        {
            cheddarQuantityText.text = $"X {cheddarQuantity}";
        }
        else
        {
            cheddarObject.SetActive(false);
        }

        currentTime = GetOriginalTimer();
        StartCoroutine(TimerRoutine());
    }

    private float GetOriginalTimer()
    {
        var orderPaper = FindObjectsOfType<OrderPaper>().ToList().Find(paper => paper.OrderNumber == OrderNumber && paper.currentTime > currentTime);

        if (orderPaper == null)
            return 0;
        else
            return orderPaper.currentTime;
    }

    public IEnumerator TimerRoutine()
    {
        yield return new WaitForSeconds(orderDeadline);
    }

    protected override void CustomFixedUpdate() { }

    protected override void CustomUpdate()
    {
        if (currentTime < orderDeadline)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            OnEnd?.Invoke();

            Destroy(gameObject);
        }
    }
}
