using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace leta.Application.ViewModels
{
    public static class Enums
    {
        public static DiaSemana ParseToEnumDiaSemana(this string value)
        {
            if (int.TryParse(value, out int result))
            {
                return ParseToEnum<DiaSemana>(value);
            }
            else
            {
                return GetValueFromDescription<DiaSemana>(value);
            }
        }
        public static string ToDescription(this DiaSemana value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
        private static T ParseToEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        private static T GetValueFromDescription<T>(string description) where T : Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }

            throw new ArgumentException("Not found.", nameof(description));
        }
    }

    public enum DiaSemana
    {
        [Description("Domingo")]
        [Display(Name = "Domingo")]
        Domingo = 0,
        [Description("Segunda-Feira")]
        [Display(Name = "Segunda-Feira")]
        Segunda = 1,
        [Description("Terça-Feira")]
        [Display(Name = "Terça-Feira")]
        Terca = 2,
        [Description("Quarta-Feira")]
        [Display(Name = "Quarta-Feira")]
        Quarta = 3,
        [Description("Quinta-Feira")]
        [Display(Name = "Quinta-Feira")]
        Quinta = 4,
        [Description("Sexta-Feira")]
        [Display(Name = "Sexta-Feira")]
        Sexta = 5,
        [Description("Sábado")]
        [Display(Name = "Sábado")]
        Sabado = 6
    }
}
