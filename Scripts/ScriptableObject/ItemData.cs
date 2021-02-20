using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemData", menuName = "ItemData", order = 51)] // создаем меню для вызова нашего ScriptableObject
public class ItemData : ScriptableObject // данные об определенном item
{
    [SerializeField] private Sprite _preview; // отображение товара в магазине
    [SerializeField] private string _label; // описание товара
    [SerializeField] private GameObject _prefab; // моделька товара (сам префаб)

    public Sprite Preview => _preview; // делаем свойства для того, чтобы мы могли достать элементы из ScriptableObject
    public string Label => _label;
    public GameObject Prefab => _prefab;
}
