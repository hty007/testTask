using System;
using System.Collections.Generic;
using System.Reflection;

namespace Algorithms
{
    public static class ReflectionHelper
    {
        /// <summary>
        /// Все типы этой сборки с определённым артибутом
        /// </summary>
        /// <returns>Возвращает все типы сборки с артибутом <c>baseType</c></returns>
        /// <param name="baseType">отрибут по которому ищутся типы</param>
        public static Type[] GetTypeThisAssembly(Type attr)
        {
            var result = new List<Type>();
            var modules = Assembly.GetCallingAssembly().DefinedTypes;
            foreach (var mod in modules)
            {
                if (mod.GetCustomAttribute(attr) != null)
                    result.Add(mod);
            }
            return result.ToArray();
        }

        /// <summary>
        /// Найти все методы с определённым атрибутом
        /// </summary>
        /// <returns>Типы методов с атрибутом <c>attr</c></returns>
        /// <param name="unit">Модеь или класс в котором ищутся методы</param>
        /// <param name="attr">Атрибут по которому ищутся методы</param>
        public static MethodInfo[] GetMetodIsAttribute(Type unit, Type attr)
        {
            var result = new List<MethodInfo>();
            foreach (var met in unit.GetMethods())
            {
                if (met.GetCustomAttribute(attr) != null)
                    result.Add(met);
            }
            return result.ToArray();
        }

        /// <summary>
        /// Найти все методы с определённым атрибутом
        /// </summary>
        /// <returns>Типы свойств с атрибутом <c>attr</c></returns>
        /// <param name="unit">Модеь или класс в котором ищутся методы</param>
        /// <param name="attr">Атрибут по которому ищутся методы</param>
        public static PropertyInfo[] GetPropertyIsAttributes(Type unit, Type attr)
        {
            var result = new List<PropertyInfo>();
            foreach (var pro in unit.GetProperties())
            {
                if (pro.GetCustomAttribute(attr) != null)
                    result.Add(pro);
            }
            return result.ToArray();
        }

        
    }
}
