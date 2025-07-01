using System;
using System.Reflection;
using Attributes;

namespace task07
{
    public static class ReflectionHelper
    {
        public static void PrintTypeInfo(Type type)
        {
            var displayName = type.GetCustomAttribute<DisplayNameAttribute>();
            Console.WriteLine($"Отображаемое имя класса: {(displayName != null ? displayName.DisplayName : "не указано имя")}");

            var version = type.GetCustomAttribute<VersionAttribute>();
            Console.WriteLine($"Версия класса: {(version != null ? $"{version.Major}.{version.Minor}" : "версия указана")}");

            // Вывод информации о методах
            PrintMembersInfo(type.GetMethods(BindingFlags.Public | BindingFlags.Instance), "Методы");

            // Вывод информации о свойствах
            PrintMembersInfo(type.GetProperties(), "Свойства");
        }

        private static void PrintMembersInfo(MemberInfo[] members, string title)
        {
            Console.WriteLine($"\n{title}:");

            foreach (var member in members)
            {
                var displayName = member.GetCustomAttribute<DisplayNameAttribute>();
                Console.WriteLine($"  {member.Name}: {(displayName != null ? displayName.DisplayName : "нет описания")}");
            }
        }
    }
}