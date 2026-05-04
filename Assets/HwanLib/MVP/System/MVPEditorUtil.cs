using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HwanLib.MVP.System.BaseMVP;

namespace HwanLib.MVP.System
{
    public static class MVPEditorUtil
    {
        private static Assembly[] _uiAssembly;

        private static Assembly[] UIAssembly
        {
            get
            {
                _uiAssembly = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(assembly => assembly.GetTypes().Any(type => type.IsSubclassOf(typeof(BasePresenter))
                                       && !type.IsAbstract && !type.IsInterface))
                    .ToArray();
                
                return _uiAssembly;
            }
        }

        public static IEnumerable<Type> GetTypesInUIAssembly(Type baseType)
        {
            return UIAssembly.SelectMany(assembly => GetAssignedTypesInAssembly(baseType, assembly));
        }
        
        public static IEnumerable<string> GetTypeNamesInUIAssembly(Type baseType)
        {
            return UIAssembly
                .SelectMany(assembly => GetAssignedTypesInAssembly(baseType, assembly))
                .Select(type => type.Name);
        }

        public static Type GetTypeInUIAssembly(string typeName)
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
        
        public static IEnumerable<Type> GetAssignedTypesInAssembly(Type baseType, Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(type => baseType.IsAssignableFrom(type)
                               && !type.IsAbstract && !type.IsInterface);
        }
        
        public static bool IsAnonymous(this MethodInfo method)
        {
            var invalidChars = new[] {'<', '>'};
            return method.Name.Any(invalidChars.Contains);
        }
        
        public static IEnumerable<string> Replace(this IEnumerable<string> enumerable, string prevValue, string newValue)
        {
            return enumerable.Select(value => value == prevValue ? newValue : value);
        }
        
    }
}