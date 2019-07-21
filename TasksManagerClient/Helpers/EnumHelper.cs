using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksManagerClient.Helpers
{
    static class EnumHelper
    {
        public static string GetDescription(this Enum value)
        {
            var attr = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attr.Any())
                return (attr.First() as DescriptionAttribute).Description;
            return "Не найдено";
        }

        public static ICollection<ValueDescription> GetAllValuesDescriptions(Type t)
        {
            if (!t.IsEnum)
                throw new ArgumentException($"{nameof(t)} должен быть Enum");
            return Enum.GetValues(t).Cast<Enum>().Select((e) =>  new ValueDescription() { Value = e, Description = e.GetDescription() }).ToList();
        }
    }

    class ValueDescription
    {
        public Enum Value { get; set; }
        public string Description { get; set; }
    }
}
