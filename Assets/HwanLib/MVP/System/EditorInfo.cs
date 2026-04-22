using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace HwanLib.MVP.System
{
    public static class EditorInfo
    {
        private static Assembly _uiAssembly;

        public static Assembly UIAssembly
        {
            get
            {
                if (_uiAssembly == null)
                {
                    Type targetType = AppDomain.CurrentDomain.GetAssemblies()
                        .Where(ass => ass.FullName.StartsWith("UI_Assembly"))
                        .SelectMany(ass => ass.GetTypes())
                        .Where(type => type.Name == "MVPAssemblyMarker")
                        .FirstOrDefault();

                    if (targetType == null)
                    {
                        EditorUtility.DisplayDialog("Error",
                            "타입을 찾기 위해 MVPAssemblyMarker.cs 파일이 필요합니다.", "OK");
                        return null;
                    }
            
                    _uiAssembly = Assembly.GetAssembly(targetType);
                }

                return _uiAssembly;
            }
        }
        
        public static IEnumerable<string> GetTypeNames(Type baseType, bool isUIAssembly)
        {
            Assembly assembly = isUIAssembly ? UIAssembly : Assembly.GetAssembly(typeof(EditorInfo));
            IEnumerable<string> choices = assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsInterface && !type.IsAbstract 
                               && baseType.IsAssignableFrom(type))
                .Select(type => type.Name);
            
            return choices;
        }

        public static Type GetType(Assembly assembly, string typeName)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.Name == typeName)
                    return type;
            }

            return null;
        }
    }

}
