using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Upload.DataAccess.Helper;
using System.Xml;
using Upload.DataAccess.Model;

namespace Upload.DataAccess
{
    public class XMLDataAccess
    {
        public bool DeleteShirt()
        {
            throw new NotImplementedException();
        }

        public ShirtData ReadShirt(string xmlFilePath)
        {
            try
            {
                const string RootNode = "Shirt";
                var xDoc = XDocument.Load(xmlFilePath);
                ShirtData mShirt = new ShirtData();

                string frontStdPath = DataHelper.GetXElementValue(xDoc, RootNode + "/FrontStdPath");
                if (!string.IsNullOrEmpty(frontStdPath))
                    mShirt.ImagePath = Path.GetDirectoryName(xmlFilePath) + "\\" + frontStdPath;
                //string backStdPath = DataHelper.GetXElementValue(xDoc, RootNode + "/BackStdPath");
                //if (!string.IsNullOrEmpty(backStdPath))
                //    mShirt.BackStdPath = Path.GetDirectoryName(xmlFilePath) + "\\" + backStdPath;
                //string frontHoodiePath = DataHelper.GetXElementValue(xDoc, RootNode + "/FrontHoodiePath");
                //if (!string.IsNullOrEmpty(frontHoodiePath))
                //    mShirt.FrontHoodiePath = Path.GetDirectoryName(xmlFilePath) + "\\" + frontHoodiePath;
                //string backHoodiePath = DataHelper.GetXElementValue(xDoc, RootNode + "/BackHoodiePath");
                //if (!string.IsNullOrEmpty(backHoodiePath))
                //    mShirt.BackHoodiePath = Path.GetDirectoryName(xmlFilePath) + "\\" + backHoodiePath;
                //string popSocketsGripPath = DataHelper.GetXElementValue(xDoc, RootNode + "/PopsocketsGripPath");
                //if (!string.IsNullOrEmpty(popSocketsGripPath))
                //    mShirt.PopSocketsGripPath = Path.GetDirectoryName(xmlFilePath) + "\\" + popSocketsGripPath;


                mShirt.Languages[0].BrandName = DataHelper.GetXElementValue(xDoc, RootNode + "/BrandName");
                mShirt.Languages[0].Title = DataHelper.GetXElementValue(xDoc, RootNode + "/DesignTitle");
                mShirt.Languages[0].FeatureBullet1 = DataHelper.GetXElementValue(xDoc, RootNode + "/FeatureBullet1");
                mShirt.Languages[0].FeatureBullet2 = DataHelper.GetXElementValue(xDoc, RootNode + "/FeatureBullet2");
                mShirt.Languages[0].Description = DataHelper.GetXElementValue(xDoc, RootNode + "/Description");
                mShirt.Languages[1].BrandName = DataHelper.GetXElementValue(xDoc, RootNode + "/BrandNameGerman");
                mShirt.Languages[1].Title = DataHelper.GetXElementValue(xDoc, RootNode + "/DesignTitleGerman");
                mShirt.Languages[1].FeatureBullet1 = DataHelper.GetXElementValue(xDoc, RootNode + "/FeatureBullet1German");
                mShirt.Languages[1].FeatureBullet2 = DataHelper.GetXElementValue(xDoc, RootNode + "/FeatureBullet2German");
                mShirt.Languages[1].Description = DataHelper.GetXElementValue(xDoc, RootNode + "/DescriptionGerman");

                foreach (ShirtTypeData shirtType in mShirt.ShirtTypes)
                {
                    string XPath = RootNode + "/" + DataHelper.ConvertTypeToString(shirtType);
                    shirtType.IsActive = DataHelper.ConvertToBool(DataHelper.GetXElementValue(xDoc, XPath + "/IsActive"));
                    DataHelper.StringToColorArray(DataHelper.GetXElementValue(xDoc, XPath + "/Colors"), shirtType.Colors);
                    shirtType.FitTypes = DataHelper.StringToBoolArray(DataHelper.GetXElementValue(xDoc, XPath + "/FitTypes"));
                    shirtType.MarketPlaces = DataHelper.StringToBoolArray(DataHelper.GetXElementValue(xDoc, XPath + "/MarketPlaces"));
                    shirtType.Prices = DataHelper.StringToDoubleArray(DataHelper.GetXElementValue(xDoc, XPath + "/Prices"));
                }
                return mShirt;
            }
            catch { }
            return null;
        }

        public bool SaveShirt(ShirtData s)
        {
            throw new NotImplementedException();
        }

        public bool UpdateShirt(ShirtData s)
        {
            throw new NotImplementedException();
        }

    }

    //public class XMLDataAccess : IDataAccess
    //{
    //    const string RootNode = "Shirt";

    //    private XDocument xDoc = null;
    //    public string XmlFilePath { get; set; }

    //    public XMLDataAccess(string xmlFilePath)
    //    {
    //        try
    //        {
    //            if (!string.IsNullOrEmpty(xmlFilePath))
    //            {
    //                this.XmlFilePath = xmlFilePath;
    //                xDoc = XDocument.Load(xmlFilePath);
    //            }
    //        }
    //        catch
    //        {
    //            xDoc = new XDocument();
    //        }

    //    }
    //    public XMLDataAccess()
    //    {
    //        xDoc = new XDocument();
    //    }
    //    public Shirt GetShirt()
    //    {
    //        try
    //        {
    //            Shirt mShirt = new Shirt();

    //            string frontStdPath = DataHelper.GetXElementValue(xDoc, RootNode + "/FrontStdPath");
    //            if (!string.IsNullOrEmpty(frontStdPath))
    //                mShirt.FrontStdPath = Path.GetDirectoryName(XmlFilePath) + "\\" + frontStdPath;
    //            string backStdPath = DataHelper.GetXElementValue(xDoc, RootNode + "/BackStdPath");
    //            if (!string.IsNullOrEmpty(backStdPath))
    //                mShirt.BackStdPath = Path.GetDirectoryName(XmlFilePath) + "\\" + backStdPath;
    //            string frontHoodiePath = DataHelper.GetXElementValue(xDoc, RootNode + "/FrontHoodiePath");
    //            if (!string.IsNullOrEmpty(frontHoodiePath))
    //                mShirt.FrontHoodiePath = Path.GetDirectoryName(XmlFilePath) + "\\" + frontHoodiePath;
    //            string backHoodiePath = DataHelper.GetXElementValue(xDoc, RootNode + "/BackHoodiePath");
    //            if (!string.IsNullOrEmpty(backHoodiePath))
    //                mShirt.BackHoodiePath = Path.GetDirectoryName(XmlFilePath) + "\\" + backHoodiePath;
    //            string popSocketsGripPath = DataHelper.GetXElementValue(xDoc, RootNode + "/PopsocketsGripPath");
    //            if (!string.IsNullOrEmpty(popSocketsGripPath))
    //                mShirt.PopSocketsGripPath = Path.GetDirectoryName(XmlFilePath) + "\\" + popSocketsGripPath;

    //            mShirt.BrandName = DataHelper.GetXElementValue(xDoc, RootNode + "/BrandName");
    //            mShirt.DesignTitle = DataHelper.GetXElementValue(xDoc, RootNode + "/DesignTitle");
    //            mShirt.FeatureBullet1 = DataHelper.GetXElementValue(xDoc, RootNode + "/FeatureBullet1");
    //            mShirt.FeatureBullet2 = DataHelper.GetXElementValue(xDoc, RootNode + "/FeatureBullet2");
    //            mShirt.Description = DataHelper.GetXElementValue(xDoc, RootNode + "/Description");
    //            mShirt.BrandNameGerman = DataHelper.GetXElementValue(xDoc, RootNode + "/BrandNameGerman");
    //            mShirt.DesignTitleGerman = DataHelper.GetXElementValue(xDoc, RootNode + "/DesignTitleGerman");
    //            mShirt.FeatureBullet1German = DataHelper.GetXElementValue(xDoc, RootNode + "/FeatureBullet1German");
    //            mShirt.FeatureBullet2German = DataHelper.GetXElementValue(xDoc, RootNode + "/FeatureBullet2German");
    //            mShirt.DescriptionGerman = DataHelper.GetXElementValue(xDoc, RootNode + "/DescriptionGerman");

    //            foreach (ShirtBase shirtType in mShirt.ShirtTypes)
    //            {
    //                string XPath = RootNode + "/" + DataHelper.ConvertTypeToString(shirtType);
    //                shirtType.IsActive = DataHelper.ConvertToBool(DataHelper.GetXElementValue(xDoc, XPath + "/IsActive"));
    //                DataHelper.StringToColorArray(DataHelper.GetXElementValue(xDoc, XPath + "/Colors"), shirtType.Colors);
    //                shirtType.FitTypes = DataHelper.StringToBoolArray(DataHelper.GetXElementValue(xDoc, XPath + "/FitTypes"));
    //                shirtType.MarketPlaces = DataHelper.StringToBoolArray(DataHelper.GetXElementValue(xDoc, XPath + "/MarketPlaces"));
    //                shirtType.Prices = DataHelper.StringToDoubleArray(DataHelper.GetXElementValue(xDoc, XPath + "/Prices"));
    //            }
    //            return mShirt;
    //        }
    //        catch
    //        {
    //            return null;
    //        }
    //    }
    //    bool SaveDatabase(string _xmlFilePath)
    //    {
    //        if (File.Exists(_xmlFilePath))
    //        {
    //            if (xDoc != null)
    //            {
    //                xDoc.Save(_xmlFilePath);
    //                return true;
    //            }
    //        }
    //        else
    //        {
    //            File.Create(_xmlFilePath).Close();
    //            if (xDoc != null)
    //            {
    //                xDoc.Save(_xmlFilePath);
    //                return true;
    //            }
    //        }
    //        return false;
    //    }

    //    public bool ModifyShirt()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool DeleteShirt()
    //    {
    //        throw new NotImplementedException();
    //    }
    //    //Need To Complete
    //    public bool SaveShirt(Shirt s)
    //    {
    //        XElement xShirt = new XElement(RootNode);
    //        xShirt.Add(new XElement("FrontStdPath", Path.GetFileName(s.FrontStdPath)));
    //        xShirt.Add(new XElement("BackStdPath", Path.GetFileName(s.BackStdPath)));
    //        xShirt.Add(new XElement("FrontHoodiePath", Path.GetFileName(s.FrontHoodiePath)));
    //        xShirt.Add(new XElement("BackHoodiePath", Path.GetFileName(s.BackHoodiePath)));
    //        xShirt.Add(new XElement("PopSocketsGripPath", Path.GetFileName(s.PopSocketsGripPath)));

    //        xShirt.Add(new XElement("BrandName", s.BrandName));
    //        xShirt.Add(new XElement("DesignTitle", s.DesignTitle));
    //        xShirt.Add(new XElement("FeatureBullet1", s.FeatureBullet1));
    //        xShirt.Add(new XElement("FeatureBullet2", s.FeatureBullet2));
    //        xShirt.Add(new XElement("Description", s.Description));
    //        xShirt.Add(new XElement("BrandNameGerman", s.BrandNameGerman));
    //        xShirt.Add(new XElement("DesignTitleGerman", s.DesignTitleGerman));
    //        xShirt.Add(new XElement("FeatureBullet1German", s.FeatureBullet1German));
    //        xShirt.Add(new XElement("FeatureBullet2German", s.FeatureBullet2German));
    //        xShirt.Add(new XElement("DescriptionGerman", s.DescriptionGerman));

    //        foreach (var shirtItem in s.ShirtTypes)
    //        {

    //            XElement xShirtItem = new XElement(DataHelper.ConvertTypeToString(shirtItem));
    //            xShirtItem.Add(new XElement("FitTypes", DataHelper.ArrayToString(shirtItem.FitTypes)));
    //            bool[] colorArray = shirtItem.Colors == null ? null : shirtItem.Colors.Select(x => x.IsActive).ToArray();
    //            xShirtItem.Add(new XElement("Colors", DataHelper.ArrayToString(colorArray)));
    //            xShirtItem.Add(new XElement("IsActive", shirtItem.IsActive));
    //            xShirtItem.Add(new XElement("MarketPlaces", DataHelper.ArrayToString(shirtItem.MarketPlaces)));
    //            xShirtItem.Add(new XElement("Prices", DataHelper.ArrayToString(shirtItem.Prices)));
    //            xShirt.Add(xShirtItem);
    //        }
    //        xDoc = new XDocument();
    //        xDoc.Add(xShirt);
    //        if (string.IsNullOrEmpty(XmlFilePath))
    //        {
    //            if (!string.IsNullOrEmpty(s.FrontStdPath))
    //                XmlFilePath = GetXmlPathFromImage(s.FrontStdPath);
    //            else if (!string.IsNullOrEmpty(s.BackStdPath))
    //                XmlFilePath = GetXmlPathFromImage(s.BackStdPath);
    //            else if (!string.IsNullOrEmpty(s.FrontHoodiePath))
    //                XmlFilePath = GetXmlPathFromImage(s.FrontHoodiePath);
    //            else if (!string.IsNullOrEmpty(s.BackHoodiePath))
    //                XmlFilePath = GetXmlPathFromImage(s.BackHoodiePath);
    //            else if (!string.IsNullOrEmpty(s.PopSocketsGripPath))
    //                XmlFilePath = GetXmlPathFromImage(s.PopSocketsGripPath);
    //        }
    //        return (SaveDatabase(XmlFilePath));
    //    }

    //    private string GetXmlPathFromImage(string PNGPath)
    //    {
    //        string xmlFilePath = Path.GetDirectoryName(PNGPath) + "\\"
    //                            + Path.GetFileNameWithoutExtension(PNGPath) +
    //                            ".xml";
    //        return xmlFilePath;
    //    }
    //    public bool UpdateShirt(Shirt s)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
