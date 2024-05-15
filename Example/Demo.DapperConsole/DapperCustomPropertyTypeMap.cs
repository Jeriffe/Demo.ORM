using Dapper;
using Demo.DapperConsole.Models;
using System.Reflection;

namespace Demo.DapperConsole
{
    public class DapperCustomPropertyTypeMap
    {
        public static void Registers()
        {

            PropertyMap<Patient>.Register();
        }
    }

    public static class PropertyMap<T>
    {
        static Dictionary<Type, Dictionary<string, string>> PropertyMapDics = new Dictionary<Type, Dictionary<string, string>>();

        static PropertyMap()
        {
           // PropertyMapDics[typeof(Formulary)] = new Dictionary<string, string> { { "DEA_Class", "DEAClass" }, { "FormularyID", "ID" } }; ;

          //  PropertyMapDics[typeof(Barcode)] = new Dictionary<string, string> { { "Barcode", "BarcodeContent" }, { "BarcodeID", "ID" } }; ;


            PropertyMapDics[typeof(Patient)] = new Dictionary<string, string> { { "MedRecNumber", "MedRecordNumber" }, { "BarcodeID", "ID" } }; ;
        }

        public static void Register()
        {
            if (!PropertyMapDics.Keys.Contains(typeof(T)))
            {
                //logger
                return;
            }

            var mapper = new CustomPropertyTypeMap(typeof(T), FormualryMapperFunc);
            SqlMapper.SetTypeMap(typeof(T), mapper);
        }


        public static Func<Type, string, PropertyInfo> FormualryMapperFunc = new Func<Type, string,
            PropertyInfo>((type, columnName) =>
                {
                    var propertyColumnMap = PropertyMapDics[type];
                    if (propertyColumnMap.ContainsKey(columnName))
                    {
                        return type.GetProperty(propertyColumnMap[columnName]);
                    }

                    return type.GetProperty(columnName);
                });
    }
}
