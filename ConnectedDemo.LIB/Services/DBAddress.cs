using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectedDemo.LIB.Entities;

namespace ConnectedDemo.LIB.Services
{
    public class DBAddress
    {
        public static List<Address> GetAdresses(string orderbyExpression = "order by naam", string whereExpression = "")
        {
            string sql;
            sql = "select * from Address ";
            sql += " " + whereExpression + " ";
            sql += " " + orderbyExpression + " ";
            DataTable dt = DBConnector.ExecuteSelect(sql);
            if (dt is null)
                return null;
            Address address;
            List<Address> addresses = new List<Address>();
            foreach(DataRow dr in dt.Rows)
            {
                address = new Address();
                address.ID = dr["id"].ToString();
                address.Naam = dr["naam"].ToString();
                address.Adres = dr["adres"].ToString();
                address.Post = dr["post"].ToString();
                address.Gemeente = dr["gemeente"].ToString();
                address.Land = dr["land"].ToString();
                address.Soort_ID = dr["soort_id"].ToString();
                addresses.Add(address);
            }
            return addresses;

        }
        public static Address FindAddressByID(string id)
        {
            string sql;
            sql = "select * from Address where id = '" + id + "' ";
            DataTable dt = DBConnector.ExecuteSelect(sql);
            if (dt is null)
                return null;
            if (dt.Rows.Count > 0)
            {
                Address address = new Address();
                address.ID = dt.Rows[0]["id"].ToString();
                address.Naam = dt.Rows[0]["naam"].ToString();
                address.Adres = dt.Rows[0]["adres"].ToString();
                address.Post = dt.Rows[0]["post"].ToString();
                address.Gemeente = dt.Rows[0]["gemeente"].ToString();
                address.Land = dt.Rows[0]["land"].ToString();
                address.Soort_ID = dt.Rows[0]["soort_id"].ToString();
                return address;
            }
            else
                return null;

        }
        public static bool SaveNewAddress(Address address)
        {
            string sql;
            sql = "insert into Address (id, naam, adres, post, gemeente, land, soort_id) values (";
            sql += "'" + address.ID + "' , ";
            sql += "'" + Helper.HandleQuotes(address.Naam) + "' , ";
            sql += "'" + Helper.HandleQuotes(address.Adres) + "' , ";
            sql += "'" + Helper.HandleQuotes(address.Post) + "' , ";
            sql += "'" + Helper.HandleQuotes(address.Gemeente) + "' , ";
            sql += "'" + Helper.HandleQuotes(address.Land) + "' , ";
            sql += "'" + Helper.HandleQuotes(address.Soort_ID) + "' ) ";

            return DBConnector.ExecuteCommand(sql);
        }
        public static bool UpdateAddress(Address address)
        {
            string sql;
            sql = "update Address set ";
            sql += " naam = '" + Helper.HandleQuotes(address.Naam) + "' , ";
            sql += " adres = '" + Helper.HandleQuotes(address.Adres) + "' , ";
            sql += " post = '" + Helper.HandleQuotes(address.Post) + "' , ";
            sql += " gemeente = '" + Helper.HandleQuotes(address.Gemeente) + "' , ";
            sql += " land = '" + Helper.HandleQuotes(address.Land) + "' , ";
            sql += " soort_id = '" + Helper.HandleQuotes(address.Soort_ID) + "'  ";
            sql += " where id = '" + address.ID + "' ";
            return DBConnector.ExecuteCommand(sql);
        }
        public static bool DeleteAddress(Address address)
        {
            string sql;
            sql = "delete from Address where id = '" + address.ID + "' ";
            return DBConnector.ExecuteCommand(sql);
        }
    }
}
