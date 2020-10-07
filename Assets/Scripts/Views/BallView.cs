using PathCreation;
using PathCreation.Examples;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BallView : MonoBehaviour
{
    [SerializeField]
    private GameObject ballCollider = null;

    [SerializeField]
    private GameObject trackObj = null;

    [SerializeField]
    private List<GameObject> itemsObject = new List<GameObject>();

    [SerializeField]
    private float horizontalScale = 10;

    private Dictionary<Item, GameObject> items = new Dictionary<Item, GameObject>();

    private PathCreator pathCreator;

    private GameObject track;

    public event Action<Item> OnCollideItem;

    public event Action OnLeftPressed;

    public event Action OnRightPressed;

    private void Start()
    {
        ballCollider.GetComponent<BallCollider>().OnCollideItem += OnCollideItem;
    }

    private void OnDestroy()
    {
        ballCollider.GetComponent<BallCollider>().OnCollideItem -= OnCollideItem;
    }

    private void Update()
    {
        if (Input.GetKey("a"))
        {
            OnLeftPressed?.Invoke();
        }

        if (Input.GetKey("d"))
        {
            OnRightPressed?.Invoke();
        }
    }

    public void SetPositionX(float positionX)
    {
        if (pathCreator)
        {
            transform.position = pathCreator.path.GetPointAtTime(positionX);
            transform.rotation = pathCreator.path.GetRotation(positionX);
        }
    }

    public void SetPositionY(float positionY)
    {
        if (true)
        {
            ballCollider.transform.localPosition = new Vector3(-0.5f * ballCollider.transform.localScale.x, positionY * horizontalScale, 0);
        }
    }

    public void SetSize(float size)
    {
        ballCollider.transform.localScale = Vector3.one * size;
    }

    public void CreateItems(List<Item> items)
    {

        DestroyItems();
        foreach (Item item in items)
        {
            if ((int)item.itemType < itemsObject.Count)
            {
                GameObject obj = Instantiate(itemsObject[(int)item.itemType]);
                obj.transform.position = pathCreator.path.GetPointAtTime(item.positionX);
                obj.transform.rotation = pathCreator.path.GetRotation(item.positionX);
                obj.transform.position = obj.transform.TransformPoint(Vector3.up * (item.positionY / obj.transform.localScale.y)
                    + Vector3.right * -2 * obj.transform.localScale.x);
                obj.GetComponent<ItemView>().item = item;
                this.items.Add(item, obj);
            }
        }
    }

    public void DestroyItems()
    {
        foreach (KeyValuePair<Item, GameObject> entry in items)
        {
            Destroy(entry.Value, 0);
        }
        items.Clear();
    }

    public void DestroyItem(Item item)
    {
        Destroy(items[item], 0);
        items.Remove(item);
    }

    public void CreateTrack(int level)
    {
        if (!track)
        {
            track = Instantiate(trackObj);
            pathCreator = track.GetComponent<PathCreator>();
            track.GetComponent<RoadMeshCreator>().TriggerUpdate();
        }
        if (pathCreator)
        {
            transform.position = pathCreator.path.GetPointAtTime(0);
            transform.rotation = pathCreator.path.GetRotation(0);
            DestroyItems();
        }
    }
}