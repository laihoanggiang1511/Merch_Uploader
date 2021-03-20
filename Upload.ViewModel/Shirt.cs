﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EzUpload.ViewModel
{
    public class Shirt : ICloneable
    {
        public string MainTags { get; set; }
        public string SupportingTags { get; set; }

        public string ImagePath { get; set; }
        public int ImageType { get; set; }

        public ObservableCollection<Language> Languages { get; set; }

        public ObservableCollection<ShirtType> ShirtTypes { get; set; }

        public Shirt()
        {
            this.ImageType = 0;
            Languages = new ObservableCollection<Language>()
            {
                new Language (0,"English"), //English
                new Language(1,"German"),
                new Language(2,"French"),
                new Language(3,"Italian"),
                new Language (4,"Spanish"),
            };
            ShirtTypes = new ObservableCollection<ShirtType>()
            {
                 new StandardTShirt(),
                 new PremiumTShirt(),
                 new VNeckTShirt(),
                 new TankTop(),
                 new LongSleeveTShirt(),
                 new Raglan(),
                 new SweetShirt(),
                 new PullOverHoodie(),
                 new ZipHoodie(),
                 new PopSocketsGrip(),
                 new IPhoneCase(),
                 new SamsungCase(),
            };
        }
        public object Clone()
        {
            try
            {
                Shirt s = new Shirt();
                PropertyInfo[] properties = this.GetType().GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.GetType() == typeof(ShirtType))
                    {
                        ShirtType sBase = property.GetValue(this) as ShirtType;
                        property.SetValue(s, sBase.Clone());
                    }
                    else
                    if (property.CanWrite /*&&
                            property.Name != "ShirtTypes"*/)
                    {
                        property.SetValue(s, property.GetValue(this));
                    }

                }
                return s;
            }
            catch
            {
                return null;
            }
        }
    }
}
