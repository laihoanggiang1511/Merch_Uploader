using System;
using System.Collections;
using System.Collections.Generic;

namespace Upload.DataAccess.DTO
{
    public class ShirtDTO
    {
        ////public static T MapData<T>(object obj)
        ////{
        ////    object result = Activator.CreateInstance(typeof(T));
        ////    if (typeof(T) == typeof(ShirtData))
        ////    {
        ////        var config = new MapperConfiguration(cfg => { cfg.CreateMap<Shirt, ShirtData>(); });
        ////        IMapper iMapper = config.CreateMapper();
        ////        result = iMapper.Map<Shirt, ShirtData>(obj as Shirt);
        ////    }
        ////    else if (typeof(T) == typeof(Shirt))
        ////    {
        ////        var config = new MapperConfiguration(cfg => { cfg.CreateMap<ShirtData, Shirt>(); });
        ////        IMapper iMapper = config.CreateMapper();
        ////        result = iMapper.Map<ShirtData, Shirt>(obj as ShirtData);
        ////    }
        ////    return (T)result;

        ////}

        //public static T MapLanguage<T>(object obj)
        //{
        //    object result = null;
        //    if (typeof(T) == typeof(LanguageData))
        //    {
        //        var config = new MapperConfiguration(cfg => { cfg.CreateMap<Language, LanguageData>(); });
        //        IMapper iMapper = config.CreateMapper();
        //        result = iMapper.Map<Language, LanguageData>(obj as Language);
        //    }
        //    else if (typeof(T) == typeof(Language))
        //    {
        //        var config = new MapperConfiguration(cfg => { cfg.CreateMap<LanguageData, Language>(); });
        //        IMapper iMapper = config.CreateMapper();
        //        result = iMapper.Map<LanguageData, Language>(obj as LanguageData);
        //    }
        //    return (T)result;
        //}
        //public static T MapShirtType<T>(object obj)
        //{
        //    object result = null;
        //    if (typeof(T) == typeof(ShirtTypeData))
        //    {
        //        var config = new MapperConfiguration(cfg => { cfg.CreateMap<ShirtType, ShirtTypeData>(); });
        //        IMapper iMapper = config.CreateMapper();
        //        result = iMapper.Map<ShirtType, ShirtTypeData>(obj as ShirtType);
        //    }
        //    else if (typeof(T) == typeof(ShirtType))
        //    {
        //        var config = new MapperConfiguration(cfg => { cfg.CreateMap<ShirtTypeData, ShirtType>(); });
        //        IMapper iMapper = config.CreateMapper();
        //        result = iMapper.Map<ShirtTypeData, ShirtType>(obj as ShirtTypeData);
        //    }
        //    return (T)result;
        //}
        //public static T MapColor<T>(object obj)
        //{
        //    object result = null;
        //    if (typeof(T) == typeof(ColorData))
        //    {
        //        var config = new MapperConfiguration(cfg => { cfg.CreateMap<Color, ColorData>(); });
        //        IMapper iMapper = config.CreateMapper();
        //        result = iMapper.Map<Color, ColorData>(obj as Color);
        //    }
        //    else if (typeof(T) == typeof(ShirtType))
        //    {
        //        var config = new MapperConfiguration(cfg => { cfg.CreateMap<ColorData, Color>(); });
        //        IMapper iMapper = config.CreateMapper();
        //        result = iMapper.Map<ColorData, Color>(obj as ColorData);
        //    }
        //    return (T)result;
        //}




        public static object MapData(object sourceObj, Type targetType)
        {
            //try
            //{
                if (sourceObj != null)
                {
                    if (targetType != null)
                    {
                        object newOb = Activator.CreateInstance(targetType);
                        if (newOb != null)
                        {
                            var props = targetType.GetProperties();

                            foreach (var item in props)
                            {
                                var objProp = sourceObj.GetType().GetProperty(item.Name);
                                if (objProp != null)
                                {
                                    if (IsCollectionType(item.PropertyType))
                                    {
                                        var destinationType = item.PropertyType.GetGenericArguments()[0];     
                                        IList<object> objects = objProp.GetValue(sourceObj) as IList<object>;
                                        if (objects != null && objects.Count > 0)
                                        {
                                            IList lstMapResult = item.GetValue(newOb) as IList;
                                            if (lstMapResult != null)
                                            {
                                                lstMapResult.Clear();

                                                for (int i = 0; i < objects.Count; i++)
                                                {
                                                    object ob = objects[i];
                                                    object mapResult = MapData(ob, destinationType);
                                                    lstMapResult.Add(mapResult);
                                                }
                                                item.SetValue(newOb, lstMapResult);
                                            }
                                        }
                                    }
                                    //else if (item.PropertyType.IsClass)
                                    //{
                                    //    object val = MapData(objProp.GetValue(sourceObj), item.PropertyType);
                                    //    item.SetValue(newOb, val);
                                    //}
                                    else
                                    {
                                        item.SetValue(newOb, objProp.GetValue(sourceObj));
                                    }
                                }
                            }
                        }

                        return newOb;
                    }
                }
            //}
            //catch (Exception ex)
            //{
            //}
            return null;
        }

        static bool IsCollectionType(Type type)
        {
            return (type.GetInterface(nameof(ICollection)) != null);
        }
        //public static T MapData<T>(object source)
        //{

        //    dynamic target = Activator.CreateInstance(typeof(T));
        //    Type targetType = target.GetType();
        //    Type sourceType = source.GetType();
        //    PropertyInfo[] props = sourceType.GetProperties();
        //    foreach (PropertyInfo prop in props)
        //    {
        //        if (prop.PropertyType == typeof(string) ||
        //           prop.PropertyType == typeof(int) ||
        //           prop.PropertyType == typeof(double) ||
        //           prop.PropertyType == typeof(bool))
        //        {

        //            prop.SetValue(target, prop.GetValue(source));
        //        }
        //        else
        //        if (prop.PropertyType == typeof(LanguageData[]))
        //        {
        //        }
        //        else
        //        {
        //        }
        //    }

        //    return target;
        //}
    }
}
