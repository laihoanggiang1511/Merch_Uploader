using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upload.Definitions
{
    public enum ShirtStatus
    {
        OK,
        Fail,
        EmptyPath,
        ImageDimensionFail,
        BrandNameFail,
        TitleFail,
        FeatureBulletFail,
        DescriptionFail,
        PriceFail,
        ColorFail,
        NoShirtType,
    }
}
