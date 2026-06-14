using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Threading;
using System.Xml;
using System.Data.SqlClient;
using System.Collections.Generic;


// benny 
    enum ImplementationType
    {
        NA = -1,
        Days = 0,
        Month,
        Year
    }
    
    class TypeImplementation
    {
        public ImplementationType implementationType { get; set; }
        public string TypeImplement  { get; set; }
        public string TypeCalc       { get; set; }
    }

    public class DateCalculation
    {
        private static Dictionary<int, TypeImplementation> typeImplementationList;
        private static DateTime lastUpdate = DateTime.Now;
        
        /// <summary>
        /// return type implementation dictionary 
        /// </summary>
        /// <returns></returns>
        private static Dictionary<int, TypeImplementation> GetTypeImplementationDateList()
        {
            if (typeImplementationList == null)
            {
                typeImplementationList = new Dictionary<int, TypeImplementation>();
                FillList();
            }
            else if (RecordExpired())
            {
                typeImplementationList.Clear();
                FillList();
            }
            return typeImplementationList;
        }

       

        private static bool RecordExpired()
        {
            if ((DateTime.Now - lastUpdate).Minutes > 30)
                return true;
            return false;
        }

        private static void FillList()
        {
            lastUpdate = DateTime.Now;
            string query = @"Select ID, NumToImplement, TypeImplement, TypeCalc From DTS_OnLine..TypeImplementationDate";
            DataTable typeImplementationDate = DataBase.FillDataTable(query, "typeImplementationDate");

            foreach (DataRow row in typeImplementationDate.Rows)
            {
                typeImplementationList.Add(Convert.ToInt32(row["ID"]), ParseRow(row));
            }
        }

        /// <summary>
        /// extract database data into TypeImplementation object 
        /// </summary>
        /// <param name="row">single row of TypeImplementationDate table</param>
        /// <returns>TypeImplementation class object </returns>
        private static TypeImplementation ParseRow(DataRow row)
        {
            TypeImplementation typeImp = new TypeImplementation();
            try
            {
                typeImp.implementationType = (ImplementationType)row["NumToImplement"];
                typeImp.TypeImplement      = row["TypeImplement"].ToString();
                typeImp.TypeCalc           = row["TypeCalc"].ToString();
            }
            catch (Exception)
            {
                typeImp.implementationType = ImplementationType.NA;
                typeImp.TypeImplement  = string.Empty;
                typeImp.TypeCalc       = string.Empty;
            }
            return typeImp;
        }

        private static TypeImplementation GetTypeImplementation(int parameter)
        {
            Dictionary<int, TypeImplementation> typeImplementationList = GetTypeImplementationDateList();
            if (typeImplementationList.ContainsKey(parameter))
                return typeImplementationList[parameter];
            else
                return null; // implementation type does not exists
            // Todo - Send notification mail
        }

        public static DateTime CalculateExpirationDate(int implementationType, DateTime lastImplementationDate)
        {
            DateTime result;
            if (implementationType == 1) // constant expiration date  (תאריך אחרון למימוש קבוע)
            {
                //change by eldad
                return new DateTime(lastImplementationDate.Year, lastImplementationDate.Month, lastImplementationDate.Day, 23, 59, 59);
                //if (DateTime.TryParse(lastImplementationDate, out result) == false)

                //    result = DateTime.MaxValue;
            }
            else
            {
                TypeImplementation type = GetTypeImplementation(implementationType);
                if (type == null)
                    return new DateTime(2030, 1, 1);

                switch (type.implementationType)
                {
                    case ImplementationType.Days:
                        result = DateTime.Now.AddDays(double.Parse(type.TypeCalc));
                        break;

                    case ImplementationType.Month:
                        result = GetDateByMonths(type.TypeImplement, type.TypeCalc);
                        break;
                    
                    case ImplementationType.Year:
                        result = GetDateByYears(type.TypeImplement, type.TypeCalc);
                        break;

                    
                    case ImplementationType.NA:
                    default:
                        result = DateTime.MaxValue;
                        break;
                }
            }
            if (result > lastImplementationDate)
                result = lastImplementationDate;
            return new DateTime(result.Year, result.Month, result.Day, 23, 59, 59);
        }


//        public static DateTime GetDateForPayment(string Parameter, DateTime DateToCalc)
//        {
//            string NumToImplement = string.Empty;
//            string TypeImplement = string.Empty;
//            string TypeCalc = string.Empty;

//            if (Parameter == "1")
//            {
//                return DateToCalc;
//            }

//            SqlDataReader reader = null;
//            SqlConnection con = new SqlConnection();
//            string query = string.Format(@"Select NumToImplement, TypeImplement, TypeCalc
//                                        From [DTS_OnLine].[dbo].[TypeImplementationDate]
//                                        Where ID = {0}", Parameter);
//            try
//            {
//                reader = DataBase.ExecuteReader(query, con);
//                if (reader.Read())
//                {
//                    NumToImplement = reader["NumToImplement"].ToString();
//                    TypeImplement = reader["TypeImplement"].ToString();
//                    TypeCalc = reader["TypeCalc"].ToString();

//                }
//            }
//            catch (Exception ex)
//            {
//                NumToImplement = "";
//                TypeImplement = "";
//                TypeCalc = "";
//            }

//            switch (NumToImplement)
//                {
//                    case "0": // days;
//                        DateToCalc = GetDateByWeeks(TypeCalc, DateToCalc);
//                        break;
//                    case "1"://month 
//                        DateToCalc = GetDateByMonths(TypeImplement, TypeCalc, DateToCalc);
//                        break;
//                    case "2": //year
//                        DateToCalc = GetDateByYears(TypeImplement, TypeCalc, DateToCalc);
//                        break;
//                }
//           return DateToCalc;
//        }
        
        private static DateTime GetDateByWeeks(string TypeCalc, DateTime DateToCalc)
        {
            DateToCalc = DateToCalc.AddDays(double.Parse(TypeCalc.ToString()));
            return DateToCalc;
        }
        private static DateTime GetDateByMonths(string TypeImplement, string TypeCalc)
        {
            DateTime result = DateTime.Now.AddMonths(int.Parse(TypeCalc.ToString()));
            if (TypeImplement.Trim() == "1")// שוטף
            {
                int days = DateTime.DaysInMonth(result.Year, result.Month);
                result = new DateTime(result.Year, result.Month, days, 23, 59, 59);
            }
            return result;
        }
        private static DateTime GetDateByYears(string TypeImplement, string TypeCalc)
        {
            DateTime result = DateTime.Now.AddYears(int.Parse(TypeCalc.ToString()));
            if (TypeImplement.Trim() == "1") 
            {
                result = new DateTime(result.Year, 12, 31, 23, 59, 59);
            }
            return result;
        }

    }

