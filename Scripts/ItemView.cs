using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemView : MonoBehaviour // отображение item-а
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _label;
    [SerializeField] private Button _selectionButton;

    private ItemData _itemData;

    public event UnityAction<ItemData> ItemSelected; // событие по выбору Item (при нажатии на него)
    public event UnityAction<ItemView> ItemDisabled; // событие по отписке от событий, при удалении или отключении предмета

    private void OnEnable()
    {
        _selectionButton.onClick.AddListener(OnSelectionButtonClick);
    }

    private void OnDisable()
    {
        ItemDisabled?.Invoke(this);
        _selectionButton.onClick.RemoveListener(OnSelectionButtonClick);
    }

    private void OnSelectionButtonClick() // при нажатии на кнопку
    {
        ItemSelected?.Invoke(_itemData); // передает данные выбранного предмета
    }

    public void Initialize(ItemData itemData)
    {
        _itemData = itemData;
        _image.sprite = itemData.Preview;
        _label.text = itemData.Label;
    }
}
