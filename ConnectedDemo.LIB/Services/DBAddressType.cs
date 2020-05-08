using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectedDemo.LIB.Entities;
using System.Data;

namespace ConnectedDemo.LIB.Services
{
    public class DBAddressType
    {
        public static List<AddressType> GetAdressTypes()
        {
            string sql;
            sql = "select * from AddressType order by soort";
            DataTable dt = DBConnector.ExecuteSelect(sql);
            if (dt == null)
                return null;
            AddressType addressType;
            List<AddressType> addressTypes = new List<AddressType>();
            foreach (DataRow dr in dt.Rows)
            {
                addressType = new AddressType();
                addressType.ID = dr["id"].ToString();
                addressType.Soort = dr["soort"].ToString();
                addressTypes.Add(addressType);
            }
            return addressTypes;
        }
        public static AddressType FindAddressTypeByID(string id)
        {
            string sql;
            sql = "select * from AddressType where id = '" + id + "' ";
            DataTable dt = DBConnector.ExecuteSelect(sql);
            if (dt is null)
                return null;
            if (dt.Rows.Count > 0)
            {
                AddressType addressType = new AddressType();
                addressType.ID = dt.Rows[0]["id"].ToString();
                addressType.Soort = dt.Rows[0]["soort"].ToString();
                return addressType;
            }
            else
                return null;

        }
        public static bool SaveNewAddressType(AddressType addressType)
        {
            string sql;
            sql = "insert into AddressType (id, soort) values (";
            sql += "'" + addressType.ID + "' , ";
            sql += "'" + Helper.HandleQuotes(addressType.Soort) + "' ) ";
            return DBConnector.ExecuteCommand(sql);
        }
        public static bool UpdateAddressType(AddressType addressType)
        {
            string sql;
            sql = "update AddressType set ";
            sql += " soort = '" + Helper.HandleQuotes(addressType.Soort) + "' , ";
            sql += " where id = '" + addressType.ID + "' ";
            return DBConnector.ExecuteCommand(sql);
        }
        public static bool DeleteAddressType(AddressType addressType)
        {
            string sql;
            sql = "delete from AddressType where id = '" + addressType.ID + "' ";
            return DBConnector.ExecuteCommand(sql);
        }

        public static bool IsAddressTypeInUse(AddressType addressType)
        {
            string sql;
            sql = "select count(*) from address where soort_id = '" + addressType.ID + "' ";
            if (DBConnector.ExecuteScalaire(sql) == "0")
                return false;
            else
                return true;
        }

    }
}
