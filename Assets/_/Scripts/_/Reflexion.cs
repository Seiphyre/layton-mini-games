using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils
{
    public static class Reflexion
    {
        public static object GetProperty(object obj, string propertyName)
        {
            if (propertyName == null)
                return obj;

            var propertyNameSegments = propertyName.Split('.');
            var objType = obj.GetType();
            var property = objType.GetProperty(propertyNameSegments[0]);

            if (propertyNameSegments.Length == 1)
            {
                object propertyValue = property.GetValue(obj);

                return propertyValue;
            }
            else
            {
                var nestedObject = property.GetValue(obj);

                return GetProperty(nestedObject, string.Join(".", propertyNameSegments.Skip(1)));
            }
        }

        public static void SetProperty(object obj, string pathToValue, object value)
        {
            var pathSegments = pathToValue.Split('.');
            var objType = obj.GetType();
            var property = objType.GetProperty(pathSegments[0]);

            if (pathSegments.Length == 1)
            {
                property.SetValue(obj, value);
            }
            else
            {
                var nestedObject = property.GetValue(obj);
                SetProperty(nestedObject, string.Join(".", pathSegments.Skip(1)), value);
            }
        }
    }
}
