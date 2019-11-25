using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace AutoImageWaterMarker.Extensions
{
    public class EnumMarkupExtension : MarkupExtension
    {
        private Type _enumType;

        public EnumMarkupExtension(Type enumType)
        {
            EnumType = enumType ?? throw new ArgumentNullException("Enumeration Type can't be null!");
        }

        public Type EnumType
        {
            get => _enumType;
            private set
            {
                if (_enumType == value)
                    return;

                var enumType = Nullable.GetUnderlyingType(value) ?? value;

                if (enumType.IsEnum == false)
                    throw new ArgumentException("Type must be an Enum.");

                _enumType = value;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var enumValues = Enum.GetValues(EnumType);

            return (
                from object enumValue in enumValues
                select new EnumMember
                {
                    Value = enumValue,
                    Description = GetDescription(enumValue)
                }).ToArray();
        }

        private string GetDescription(object enumValue)
        {
            return EnumType.GetField(enumValue.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() is DescriptionAttribute descriptionAttribute ? descriptionAttribute.Description : enumValue.ToString();
        }
    }
}
