using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HwanLib.MVP.System.BaseMVP;
using UnityEngine;

namespace HwanLib.MVP.System
{
    public static class EditorInfo
    {
        private static Assembly[] _uiAssembly;

        private static Assembly[] UIAssembly
        {
            get
            {
                _uiAssembly = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(assembly => assembly.GetTypes().Any(type => type.IsSubclassOf(typeof(BasePresenter))
                                       && !type.IsAbstract
                                       && !type.IsInterface))
                    .ToArray();
                
                return _uiAssembly;
            }
        }

        public static IEnumerable<Type> GetUIAssemblyTypes(Type baseType)
        {
            return UIAssembly.SelectMany(assembly => assembly.GetTypes())
                .Where(baseType.IsAssignableFrom);
        }
        
        public static IEnumerable<string> GetUIAssemblyTypeNames(Type baseType)
        {
            List<string> names = new List<string>();
                
            foreach (Assembly assembly in UIAssembly)
            {
                names.AddRange(assembly.GetTypes()
                    .Where(type => type.IsClass && !type.IsInterface && !type.IsAbstract 
                                   && baseType.IsAssignableFrom(type))
                    .Select(type => type.Name));
            }
                
            return names;
        }

        public static Type GetUIAssemblyType(string typeName)
        {
            foreach (Assembly assembly in UIAssembly)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.Name == typeName)
                        return type;
                }
            }

            return null;
        }
    }
}