using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionPanel : MonoBehaviour
{
    [SerializeField] private ItemData[] _itemDatas; // массив данных наших предметов в UI
    [SerializeField] private ObjectPlacer _objectPlacer; // класс который обрабатывает, где будет располагаться объект (мебель) после создания
    [SerializeField] private GameObject _itemTemplate; // префаб нашего item-а в UI (шаблон предметов)
    [SerializeField] private Transform _container; // контейнер, в котором создаются наш предметы в UI

    private void Start()
    {
        for (int i = 0; i < _itemDatas.Length; i++)
            AddItem(_itemDatas[i]); // при старте программы добавляем все наши предметы в виртуальный магазин
    }

    private void AddItem(ItemData itemData)
    {
        Instantiate(_itemTemplate, _container).TryGetComponent(out ItemView itemView); // пытаемся получить у созданного объекта компонент ItemView
        itemView.Initialize(itemData); // инициализируем предмет и передаем ему свои данные
        itemView.ItemSelected += OnItemSelected;
        itemView.ItemDisabled += OnItemDisable;
    }

    private void OnItemSelected(ItemData itemData) // обработчик события: "когда item выбран"
    {
        _objectPlacer.SetInstalledObject(itemData); // установить объект на сцене
    }

    private void OnItemDisable(ItemView itemView) // отписываемся от событий при дективации предмета
    {
        itemView.ItemSelected -= OnItemSelected;
        itemView.ItemDisabled -= OnItemDisable;
    }
}
