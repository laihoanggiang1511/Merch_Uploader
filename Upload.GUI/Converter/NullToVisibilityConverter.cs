﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using EzUpload.ViewModel;

namespace EzUpload.GUI.Converter
{
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return System.Windows.Visibility.Collapsed;
            else
                return System.Windows.Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return System.Windows.Visibility.Collapsed;
            else
                return System.Windows.Visibility.Visible;
        }
    }

    public class InvertableBoolToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {

                int nParam = int.Parse(parameter.ToString());
                if (nParam == 0)
                {
                    if ((bool)value == true)
                        return System.Windows.Visibility.Visible;
                    else
                        return System.Windows.Visibility.Collapsed;
                }
                else if (nParam == 1)
                {
                    if ((bool)value == true)
                        return System.Windows.Visibility.Collapsed;
                    else
                        return System.Windows.Visibility.Visible;
                }
            }
            catch
            {
            }
            return System.Windows.Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class ShirtTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                ShirtType selectedShirtType = value as ShirtType;
                if (selectedShirtType != null)
                {
                    int option = int.Parse(parameter.ToString());
                    if (selectedShirtType.TypeName == "StandardTShirt" ||
                        selectedShirtType.TypeName == "PremiumTShirt")
                    {
                        if (option == 0) // Light/Dark
                        {
                            return Visibility.Visible;
                        }
                    }
                    else if (selectedShirtType.TypeName != "PopSocketsGrip")
                    {
                        if (option == 1) // Select All
                        {
                            return Visibility.Visible;
                        }
                    }
                }
            }
            catch { }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
