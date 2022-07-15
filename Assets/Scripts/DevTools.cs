using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeveloperTools
{
    public class DevTools
    {
        public static GameObject FindGameObject(string name)
        {
            var obj = GameObject.Find(name);
            if (obj == null)
                throw new ObjectNotFoundException("GameObject \"" + name + "\" Not Found!");
            return obj;
        }

        ///Looks for a Transform in a Child
        ///Using "/" it can search everywhere with a path name
        public static Transform FindTransform(string name, Transform parent)
        {
            
            var obj = parent.Find(name);
            if (obj == null)
                throw new ObjectNotFoundException("Transform \"" + name + "\" Not Found!");
            return obj;
        }

        public static void LogException(Exception exception, string message)
        {
            Debug.LogException(exception);
            Debug.Log(message+'\n' + exception.Message + '\n' + exception.Source);
        }


    }












    [Serializable]
    public class ObjectNotFoundException : Exception 
    {
        public ObjectNotFoundException() { }

        public ObjectNotFoundException(string message)
            : base(message) { }

        public ObjectNotFoundException(string message, Exception inner)
            : base(message, inner) { }
    }
}
