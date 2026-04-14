using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HwanLib.MVP.System;
using UnityEditor;

namespace HwanLib.MVP.Editor
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
                        EditorUtility.DisplayDialog("Error",
                            "타입을 찾기 위해 MVPAssemblyMarker.cs 파일이 필요합니다.", "OK");
            
                    _uiAssembly = Assembly.GetAssembly(targetType);
                }

                return _uiAssembly;
            }
        }

        public static string[] FormNames
        {
            get
            {
                return Assembly.GetAssembly(typeof(EditorInfo)).GetTypes()
                    .Where(type => type.IsClass && !type.IsInterface && !type.IsAbstract
                                   && typeof(BaseForm).IsAssignableFrom(type))
                    .Select(type => type.Name).ToArray();
            }
        }
        
        public static IEnumerable<string> GetTypeNames(Type baseType)
        {
            IEnumerable<string> choices = UIAssembly.GetTypes()
                .Where(type => type.IsClass && !type.IsInterface && !type.IsAbstract 
                               && baseType.IsAssignableFrom(type))
                .Select(type => type.Name);
            
            return choices;
        }
    }

}
