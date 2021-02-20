using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation; // подключаем базовую библиотеку AR
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))] // данный компонент необходим для проверки рейкаста и размещения нашего объекта на полу
public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] private Transform _objectPlace; // само место для нашего объекта на сцене (которое будет отображаться при попытке установить объект)
    [SerializeField] private Camera _camera; // камера нужна для того, чтобы кидать рейкаст на проверку земли
    [SerializeField] private GameObject _container; // контейнер для размещения объектов на сцене

    private ARRaycastManager _aRRaycastManager;
    private GameObject _installedObject; // объект который установлен
    private List<ARRaycastHit> _hits = new List<ARRaycastHit>(); // массив для наших попаданий (рейкаста)

    private void Start()
    {
        _aRRaycastManager = GetComponent<ARRaycastManager>();
    }

    private void Update()
    {
        UpdatePlacementPose();

        if (Input.touchCount == 2)
        {
            SetObject();
        }
    }

    private void UpdatePlacementPose() // обновить позицию нашего _objectPlace
    {
        Vector2 screenCenter = _camera.ViewportToScreenPoint(new Vector2(0.5f, 0.5f)); // находим центр экрана в центральной точке области видимости камеры (new Vector2(0.5f, 0.5f) - середина области видимости)

        var ray = _camera.ScreenPointToRay(screenCenter); // пускаем луч из камеры в точку с координатами Vector2 screenCenter

        if (Physics.Raycast(ray, out RaycastHit raycastHit)) // проверяем рейкаст по физике игровых объектов по коллайдерам
        {
            SetObjectPosition(raycastHit.point); // передаем точку, в которую пришел наш рейкаст
        }
        else if (_aRRaycastManager.Raycast(screenCenter, _hits, TrackableType.PlaneWithinPolygon)) // Делаем рейкаст по горизонтальной плоскости земли реального мира, отслеживаемый тип будет плоскость внутри многоугольника (который составит _aRRaycastManager)
        {
            SetObjectPosition(_hits[0].pose.position); // передаем точку первого найденного рейкаста (объект следует за центром камеры)
        }
    }

    private void SetObjectPosition(Vector3 position) // задание позиции для размещения объекта
    {
        _objectPlace.position = position; // задаем позицию для размещения объекта

        Vector3 cameraForward = _camera.transform.forward; // запоминаем позицию нормализованного вектора поворота камеры вперед в локальных координатах
        Vector3 cameraRotation = new Vector3(cameraForward.x, 0, cameraForward.z); // запоминаем позицию осей x и z при повороте нашей камеры вперед
        _objectPlace.rotation = Quaternion.Euler(cameraRotation); // при смещении камеры по осям x и z, объект (мебель) будет вращаться относительно неё по тем же нормализованным координатам
    }

    private void SetObject() // установить (закрепить) объект на сцене
    {
        _installedObject.GetComponent<Collider>().enabled = true; // после того, как объект установлен на сцене включаем коллайдер, чтобы через него проходил рейкаст
        _installedObject.transform.parent = _container.transform; // переносим размещаемый объект в другой контейнер для установленных объектов
        _installedObject = null; // обнуляем наш _installedObject (стираем ссылку на него)
    }

    public void SetInstalledObject(ItemData itemData)
    {
        if (_installedObject != null) // если наш текущий размещаемый объект не пустой
            Destroy(_installedObject); // то мы уничтожаем, то что там сейчас есть

        _installedObject = Instantiate(itemData.Prefab, _objectPlace); // создаем префаб нашего объекта в точке _objectPlace
        _installedObject.GetComponent<Collider>().enabled = false; // отключаем коллайдер, чтобы мы не могли сами в него делать рейкаст
    }
}
