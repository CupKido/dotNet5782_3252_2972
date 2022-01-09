using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DalXml
{
    internal class XMLTools
    {
        static string dir = @"bin\";
        static XMLTools()
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }

        public static void CreateFile(string path, string Name)
        {
            if(!File.Exists(dir + path))
            {
                XElement temp = new XElement(Name);
                SaveListToXMLElement(temp, path);
            }
        }

        internal static void CreateConfig(string path, int parcelCount)
        {
            if (!File.Exists(dir + path))
            {
                Double AvailbleElec = 20;
                Double LightElec = 35;
                Double IntermediateElec = 50;
                Double HeavyElec = 80;
                Double ChargePerHours = 10;
                double[] a = { AvailbleElec, LightElec, IntermediateElec, HeavyElec, ChargePerHours };
                XElement Elec = new XElement("Elec",
                    new XElement("AvailbleElec", AvailbleElec),
                    new XElement("LightElec", LightElec),
                    new XElement("IntermediateElec", IntermediateElec),
                    new XElement("HeavyElec", HeavyElec),
                    new XElement("ChargePerHours", ChargePerHours)
                    );
                XElement runningNum = new XElement("ParcelsRunningNum", parcelCount);
                XElement config = new XElement("Config", Elec, runningNum);
                SaveListToXMLElement(config, path);
            }
        }

        #region SaveLoadWithXElement
        public static void SaveListToXMLElement(XElement rootElem, string filePath)
        {
            try
            {
                rootElem.Save(dir + filePath);
            }
            catch (Exception ex)
            {
               throw new DO.XMLFileLoadCreateException(filePath, $"Failed to Create XML file: {filePath}", ex);
            }
        }

        public static XElement LoadListFromXMLElement(string filePath)
        {
            try
            {
                if (File.Exists(dir + filePath))
                {
                    return XElement.Load(dir + filePath);
                }
                else
                {
                    XElement rootElem = new XElement(filePath);
                    rootElem.Save(dir + filePath);
                    return rootElem;
                }
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileLoadCreateException(filePath, $"fail to load xml file: {filePath}", ex);
            }
        }
        #endregion

        #region SaveLoadWithXMLSerializer
        public static void SaveListToXMLSerializer<T>(List<T> list, string filePath)
        {
            try
            {
                FileStream file = new FileStream(dir + filePath, FileMode.Create);
                XmlSerializer x = new XmlSerializer(list.GetType());
                x.Serialize(file, list);
                file.Close();
            }
            catch (Exception ex)
            {
               throw new DO.XMLFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
            }
        }
        public static List<T> LoadListFromXMLSerializer<T>(string filePath)
        {
            try
            {
                if (File.Exists(dir + filePath))
                {
                    List<T> list;
                    XmlSerializer x = new XmlSerializer(typeof(List<T>));
                    FileStream file = new FileStream(dir + filePath, FileMode.Open);
                    list = (List<T>)x.Deserialize(file);
                    file.Close();
                    return list;
                }
                else
                    return new List<T>();
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileLoadCreateException(filePath, $"fail to load xml file: {filePath}", ex);
            }
        }
        #endregion
    }
}