using UnityEngine;
//设计模式--单例泛型
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;//声明一个类型为T的静态变量，T可以被任何类型代替例如GameManager

    public static T Instance//声明一个类型的属性来包装instancce,使得可以让外部访问到他
    {
        //get => instance;
        get { return instance; }
    }

    protected virtual void Awake()//声明一个只能被 继承类访问的虚方法
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = (T)this;
    }

    //public static bool IsInitialized
    //{//声明一个返回值为bool类型的属性返回的是该实例是否初始化

    //    get { return instance != null; }
    //}

    protected virtual void OnDestroy()//声明一个只能被 继承类访问的虚方法
    {
        if (instance == this)
            instance = null;
    }
}