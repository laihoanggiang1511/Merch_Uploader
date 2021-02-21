using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzUpload.ViewModel
{
    public enum ImageType
    {
        Standard_Front,
        Standard_Back,
        Hoodie_Front,
        Hoodie_Back,
        PopSockets,
        PhoneCase,
    }
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
