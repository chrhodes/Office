using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using SupportTools_Excel.AzureDevOpsExplorer.Presentation.ModelWrappers;

namespace SupportTools_Excel.Presentation.Converters
{
    public class SelectedItemsConverter3: MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return ((WorkItemQueryWrapper)value).Name;
                //return new List<object>((IEnumerable<string>)value);

            return null;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            var valueType = value.GetType();

            //var returnValue = ((WorkItemQueryWrapper)value).Name;
            var returnValue = (WorkItemQueryWrapper)value;

            return returnValue;

        }
    }
}
