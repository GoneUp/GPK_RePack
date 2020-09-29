using System;
using System.Collections.Generic;
using GPK_RePack.Core;
using GPK_RePack.Core.Model;
using GPK_RePack.Core.Model.Interfaces;
using GPK_RePack.Core.Model.Prop;
using Nostrum;

namespace GPK_RePack_WPF
{
    public class PropertyViewModel : TSPropertyChanged
    {
        public string Name { get; set; }
        public string PropertyType { get; set; } //todo: use enum
        public int Size { get; set; }
        public int ArrayIndex { get; set; } //todo: is this even needed?
        public string InnerType { get; set; }
        public object Value { get; set; }
        public List<string> InnerTypes { get; set; }
        public bool EditAsEnum { get; set; }

        public IProperty GetIProperty(GpkPackage selectedPackage)
        {
            var baseProp = new GpkBaseProperty(Name, PropertyType, 0, ArrayIndex);
            IProperty iProp;

            //Check & Add name to our namelist
            selectedPackage.AddString(baseProp.name);

            var cellValue = Value.ToString();//.Cells["value"].Value.ToString();


            switch (baseProp.type)
            {
                case "StructProperty":
                    iProp = new GpkStructProperty(baseProp)
                    {

                        innerType = InnerType, //.Cells["iType"].Value.ToString();
                        value = (cellValue).ToBytes()
                    };
                    break;

                case "ArrayProperty":
                    //if (cellValue == "[##TOO_LONG##]")
                    //{
                    //    //use row embeeded property instead
                    //    tmpArray.value = ((GpkArrayProperty)row.Tag).value;
                    //}
                    //else
                    var tmpArray = new GpkArrayProperty(baseProp)
                    {
                        value = (cellValue).ToBytes()
                    };
                    tmpArray.size = tmpArray.value.Length;
                    tmpArray.RecalculateSize();
                    iProp = tmpArray;
                    break;

                case "ByteProperty":
                    var tmpByte = new GpkByteProperty(baseProp);

                    if (cellValue.Length > 2)
                    {
                        if (selectedPackage.x64)
                        {
                            tmpByte.enumType = InnerType; //.Cells["iType"].Value.ToString();
                            selectedPackage.AddString(tmpByte.enumType); //just in case 
                        }
                        selectedPackage.AddString(cellValue); //just in case 

                        tmpByte.nameValue = cellValue;
                    }
                    else
                    {
                        tmpByte.byteValue = Convert.ToByte(cellValue);
                    }
                    iProp = tmpByte;
                    break;

                case "NameProperty":
                    var tmpName = new GpkNameProperty(baseProp);
                    selectedPackage.AddString(cellValue); //just in case 
                    tmpName.value = cellValue;
                    iProp = tmpName;
                    break;
                case "ObjectProperty":
                    var tmpObj = new GpkObjectProperty(baseProp);
                    selectedPackage.GetObjectByUID(cellValue); //throws ex if uid is not present
                    tmpObj.objectName = cellValue;
                    iProp = tmpObj;
                    break;

                case "BoolProperty":
                    var tmpBool = new GpkBoolProperty(baseProp)
                    {
                        value = Convert.ToBoolean(Value)
                    };
                    iProp = tmpBool;
                    break;

                case "IntProperty":
                    var tmpInt = new GpkIntProperty(baseProp)
                    {
                        value = Convert.ToInt32(Value)
                    };
                    iProp = tmpInt;
                    break;

                case "FloatProperty":
                    var tmpFloat = new GpkFloatProperty(baseProp)
                    {
                        value = Convert.ToSingle(Value)
                    };
                    iProp = tmpFloat;
                    break;

                case "StrProperty":
                    var tmpStr = new GpkStringProperty(baseProp)
                    {
                        value = (Value.ToString())
                    };
                    iProp = tmpStr;
                    break;

                case "":
                    //new line, nothing selected
                    throw new Exception($"You need to select a Property Type for {baseProp.name}!");
                default:
                    throw new Exception($"Unknown Property Type {baseProp.type}, Prop_Name {baseProp.name}");

            }

            iProp.RecalculateSize();
            return iProp;
        }

    }
}