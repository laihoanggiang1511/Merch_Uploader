using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upload.Model
{
    public class Shirt
    {
        public string FrontStdPath { get; set; }
        public string BackStdPath { get; set; }

        public string FrontHoodiePath { get; set; }
        public string BackHoodiePath { get; set; }
        public string PopSocketsGripPath { get; set; }

        public string BrandName { get; set; }
        public string DesignTitle { get; set; }
        public string FeatureBullet1 { get; set; }
        public string FeatureBullet2 { get; set; }
        public string Description { get; set; }

        public string BrandNameGerman { get; set; }
        public string DesignTitleGerman { get; set; }
        public string FeatureBullet1German { get; set; }
        public string FeatureBullet2German { get; set; }
        public string DescriptionGerman { get; set; }

        public StandardTShirt StandardTShirt { get; set; }
        public PremiumTShirt PremiumTShirt { get; set; }
        public LongSleeveTShirt LongSleeveTShirt { get; set; }
        public PopSocketsGrip PopSocketsGrip { get; set; }
        public PullOverHoodie PullOverHoodie { get; set; }
        public Raglan Raglan { get; set; }
        public SweetShirt SweetShirt { get; set; }
        public TankTop TankTop { get; set; }
        public VNeckTShirt VNeckTShirt { get; set; }
        public ZipHoodie ZipHoodie { get; set; }

        public ShirtBase[] ShirtTypes
        {
            get
            {
                ShirtBase[] shirtTypes = new ShirtBase[]
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
                };
                return shirtTypes;
            }
        }

        public Shirt()
        {
            this.FrontStdPath = string.Empty;
            this.BackStdPath = string.Empty;
            this.FrontHoodiePath = string.Empty;
            this.BackHoodiePath = string.Empty;
            this.PopSocketsGripPath = string.Empty;

            this.BrandName = string.Empty;
            this.DesignTitle = string.Empty;
            this.Description = string.Empty;
            this.FeatureBullet1 = string.Empty;
            this.FeatureBullet2 = string.Empty;

            this.BrandNameGerman = string.Empty;
            this.DesignTitleGerman = string.Empty;
            this.DescriptionGerman = string.Empty;
            this.FeatureBullet1German = string.Empty;
            this.FeatureBullet2German = string.Empty;

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
        }
    }
}
