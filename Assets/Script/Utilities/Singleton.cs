using UnityEngine;
//���ģʽ--��������
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;//����һ������ΪT�ľ�̬������T���Ա��κ����ʹ�������GameManager

    public static T Instance//����һ�����͵���������װinstancce,ʹ�ÿ������ⲿ���ʵ���
    {
        //get => instance;
        get { return instance; }
    }

    protected virtual void Awake()//����һ��ֻ�ܱ� �̳�����ʵ��鷽��
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = (T)this;
    }

    //public static bool IsInitialized
    //{//����һ������ֵΪbool���͵����Է��ص��Ǹ�ʵ���Ƿ��ʼ��

    //    get { return instance != null; }
    //}

    protected virtual void OnDestroy()//����һ��ֻ�ܱ� �̳�����ʵ��鷽��
    {
        if (instance == this)
            instance = null;
    }
}