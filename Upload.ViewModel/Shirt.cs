using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Upload.ViewModel
{
    public class Shirt : ICloneable
    {
        //public string FrontStdPath { get; set; }
        //public string BackStdPath { get; set; }

        //public string FrontHoodiePath { get; set; }
        //public string BackHoodiePath { get; set; }
        //public string PopSocketsGripPath { get; set; }

        //public string[] PNGPaths
        //{
        //    get
        //    {
        //        string[] paths = new string[]
        //        {
        //            this.FrontStdPath,
        //            this.BackStdPath,
        //            this.FrontHoodiePath,
        //            this.BackHoodiePath,
        //            this.PopSocketsGripPath,
        //        };
        //        return paths;
        //    }
        //}
        public string ImagePath { get; set; }
        public int ImageType { get; set; }  

        public ObservableCollection<Language> Languages { get; set; }

        private StandardTShirt StandardTShirt { get; set; }
        private PremiumTShirt PremiumTShirt { get; set; }
        private LongSleeveTShirt LongSleeveTShirt { get; set; }
        private PopSocketsGrip PopSocketsGrip { get; set; }
        private PullOverHoodie PullOverHoodie { get; set; }
        private Raglan Raglan { get; set; }
        private SweetShirt SweetShirt { get; set; }
        private TankTop TankTop { get; set; }
        private VNeckTShirt VNeckTShirt { get; set; }
        private ZipHoodie ZipHoodie { get; set; }
        private IPhoneCase IPhoneCase { get; set; }
        private SamsungCase SSCase { get; set; }

        public ObservableCollection<ShirtType> ShirtTypes
        {
            get
            {
                ObservableCollection<ShirtType> shirtTypes = new ObservableCollection<ShirtType>()
                {
                    this.StandardTShirt,
                    this.PremiumTShirt,
                    this.VNeckTShirt,
                    this.TankTop,
                    this.LongSleeveTShirt,
                    this.Raglan,
                    this.SweetShirt,
                    this.PullOverHoodie,
                    this.ZipHoodie,
                    this.PopSocketsGrip,
                    this.IPhoneCase,
                    this.SSCase,
                };
                return shirtTypes;
            }
            set
            {
                if (value.Count > 11)
                {
                    this.StandardTShirt = value[0].Clone() as StandardTShirt;
                    this.PremiumTShirt = value[1].Clone() as PremiumTShirt;
                    this.VNeckTShirt = value[2].Clone() as VNeckTShirt;
                    this.TankTop = value[3].Clone() as TankTop;
                    this.LongSleeveTShirt = value[4].Clone() as LongSleeveTShirt;
                    this.Raglan = value[5].Clone() as Raglan;
                    this.SweetShirt = value[6].Clone() as SweetShirt;
                    this.PullOverHoodie = value[7].Clone() as PullOverHoodie;
                    this.ZipHoodie = value[8].Clone() as ZipHoodie;
                    this.PopSocketsGrip = value[9].Clone() as PopSocketsGrip;
                    this.IPhoneCase = value[10].Clone() as IPhoneCase;
                    this.SSCase = value[11].Clone() as SamsungCase;
                }
            }
        }

        public Shirt()
        {
            //this.FrontStdPath = string.Empty;
            //this.BackStdPath = string.Empty;
            //this.FrontHoodiePath = string.Empty;
            //this.BackHoodiePath = string.Empty;
            //this.PopSocketsGripPath = string.Empty;
            this.ImageType = 0;
            Languages = new ObservableCollection<Language>()
            {
                new Language (0,"English"), //English
                new Language(1,"German"),
                new Language(2,"French"),
                new Language(3,"Italian"),
                new Language (4,"Spanish"),
            };

            this.LongSleeveTShirt = new LongSleeveTShirt();
            this.PopSocketsGrip = new PopSocketsGrip();
            this.PremiumTShirt = new PremiumTShirt();
            this.PullOverHoodie = new PullOverHoodie();
            this.Raglan = new Raglan();
            this.StandardTShirt = new StandardTShirt();
            this.SweetShirt = new SweetShirt();
            this.TankTop = new TankTop();
            this.VNeckTShirt = new VNeckTShirt();
            this.ZipHoodie = new ZipHoodie();
            this.IPhoneCase = new IPhoneCase();
            this.SSCase = new SamsungCase();
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
