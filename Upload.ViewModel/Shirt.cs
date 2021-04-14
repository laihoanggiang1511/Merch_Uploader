using System;
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
      public string MainTag { get; set; }
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
      public bool Validate(out string errorMessage)
      {
         errorMessage = string.Empty;
         try
         {
            if (string.IsNullOrEmpty(ImagePath))
            {
               errorMessage = "No image selected!";
               return false;
            }
            //Brand name
            foreach (Language language in Languages)
            {
               if (!string.IsNullOrEmpty(language.BrandName) &&
                   (language.BrandName.Length > 50 ||
                   language.BrandName.Length < 3))
               {
                  errorMessage = "Brand must be 3-50 characters";
                  return false;
               }
               if (!string.IsNullOrEmpty(language.Title) &&
                  (language.Title.Length > 60 ||
                     language.Title.Length < 3))
               {
                  errorMessage = "Design Title must be 3 - 60 characters";
                  return false;
               }
               if (!string.IsNullOrEmpty(language.FeatureBullet1) &&
                     language.FeatureBullet1.Length > 256)
               {
                  errorMessage = "Feature Bullet must be 256 characters or fewer";
                  return false;
               }
               if (!string.IsNullOrEmpty(language.FeatureBullet2) &&
                   language.FeatureBullet2.Length > 256)
               {
                  errorMessage = "Feature Bullet must be 256 characters or fewer";
                  return false;
               }
               if (!string.IsNullOrEmpty(language.Description) &&
                     (language.Description.Length > 2000 ||
                     language.Description.Length < 75))
               {
                  errorMessage = "Description must be 75-2000 characters";
                  return false;
               }
            }
            //FitType
            return true;
         }
         catch
         {
            errorMessage = "Unknown Error";
            return false;
         }
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
