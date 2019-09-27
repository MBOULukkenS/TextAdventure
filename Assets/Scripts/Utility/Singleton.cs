using System;
using System.IO;
using UnityEngine;

namespace Utility
{
    public abstract class Singleton<T>
    {
        protected bool Initialized;
    
        private static T _instance;
        public static T Instance {
            get
            {
                CreateInstance();
                return _instance;
            }
            protected set => _instance = value;
        }
    
        protected virtual void Init()
        {
            Initialized = true;
        }

        public static bool CreateInstance()
        {
            bool instanceExists = _instance == null;
            if (instanceExists)
                _instance = Activator.CreateInstance<T>();

            return instanceExists;
        }
    }

    [DisallowMultipleComponent]
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private bool _initialized;
    
        protected static T _instance;
        public static T Instance {
            get
            {
                CreateInstance();
                return _instance;
            }
            protected set => _instance = value;
        }

        public virtual void Start()
        {
            Instance = GetComponent<T>();
            if (!_initialized)
                Init();
        }

        protected virtual void Init()
        {
            _initialized = true;
        }

        public static bool CreateInstance()
        {
            if (_instance != null) 
                return false;
            
            _instance = new GameObject(typeof(T).Name).AddComponent<T>();
            _instance.Init();

            return true;
        }
    }

    [DisallowMultipleComponent]
    public abstract class MonoTargettedSingleton<T, T2> : MonoSingleton<T> where T : MonoTargettedSingleton<T, T2>
    {
        public new static T Instance
        {
            get
            {
                CreateInstance();
                return _instance;
            }
            protected set => _instance = value;
        }
        
        public new static bool CreateInstance()
        {
            if (_instance != null) 
                return false;
            
            _instance = (GameObject.Find(typeof(T2).Name) ?? new GameObject(typeof(T2).Name)).AddComponent<T>();
            _instance.Init();

            return true;
        }
    }

    public abstract class ScriptableObjectConfigSingleton<T> : ScriptableObject 
        where T : ScriptableObjectConfigSingleton<T>
    {
        private static T _instance;
        public static T Instance {
            get
            {
                if (!CreateInstance())
                    throw new FileNotFoundException(typeof(T).Name);
                
                return _instance;
            }
            protected set => _instance = value;
        }

        public static bool CreateInstance()
        {
            if (_instance == null)
                _instance = Resources.Load<T>(typeof(T).Name);

            return _instance != null;
        }
    }

    public abstract class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObjectSingleton<T>
    {
        private static T _instance;
        public static T Instance {
            get
            {
                CreateInstance();
                return _instance;
            }
            protected set => _instance = value;
        }

        public static bool CreateInstance()
        {
            bool instanceExists = _instance != null;
            
            if (!instanceExists && !(_instance = Resources.FindObjectsOfTypeAll<T>()?[0]))
                _instance = ScriptableObject.CreateInstance<T>();

            return !instanceExists;
        }
    }
}