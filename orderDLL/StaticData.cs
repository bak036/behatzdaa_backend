using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

public static class StaticData
{
    class IrgunPriceValues
    {
        string perentForNormal;
        string percentForTrustProgram;

        public IrgunPriceValues(string n, string t)
        {
            perentForNormal = n;
            percentForTrustProgram = t;
        }

        public string getDynamicIrgunPrice(int TrustProgram)
        {
            if (TrustProgram == 0)
                return perentForNormal;
            return percentForTrustProgram;
        }
    }

    static Dictionary<int, Dictionary<string, IrgunPriceValues>> subtyps;
    static Dictionary<string, double> BusinessFee;

    static StaticData()
    {
        subtyps = new Dictionary<int, Dictionary<string, IrgunPriceValues>>();
        updateSubtyps();
        BusinessFee = new Dictionary<string, double>();
    }
    private static void updateSubtyps()
    {
        string query = "select BusinessSubTypeID,ltrim(rtrim(IrgunPriceVariable))as IrgunPriceVariable,percentage,percentageTrust from IrgunPriceForSubType";
        using (IDataReader reader = DataBase.ExecuteReader_DefaultDataBase(query))
        {
            while (reader.Read())
            {
                int subtype = reader.GetInt32(reader.GetOrdinal("BusinessSubTypeID"));
                string IrgunPriceVariable = reader.GetString(reader.GetOrdinal("IrgunPriceVariable"));
                string percentage = reader.GetString(reader.GetOrdinal("percentage"));

                int trustIndex = reader.GetOrdinal("percentageTrust");
                string percentageTrust = reader[trustIndex] == DBNull.Value ? percentage : reader.GetString(trustIndex);

                //init the dictionery for each inner type
                if (!subtyps.ContainsKey(subtype))
                    subtyps.Add(subtype, new Dictionary<string, IrgunPriceValues>());
                //add the inner types for subtype
                if (!subtyps[subtype].ContainsKey(IrgunPriceVariable))
                {
                    IrgunPriceValues vals = new IrgunPriceValues(percentage, percentageTrust);
                    subtyps[subtype].Add(IrgunPriceVariable, vals);
                }
            }
        }
    }
    private static double getBusinessFee(string fullBarCode)
    {
        if (BusinessFee.ContainsKey(fullBarCode))
            return BusinessFee[fullBarCode];
        string query = string.Format(@"SELECT isnull(MarketingCommission,0) as MarketingCommission
                      FROM [DTS_Online].[dbo].[ProductsVarsGlobal] G  with (nolock)
                      inner join [DTS_Online].[dbo].Business B  with (nolock) on  G.[BusinessId] = B.BuisnessID
                      where FullBarCode = '{0}'", fullBarCode);

        using (IDataReader reader = DataBase.ExecuteReader_DefaultDataBase(query))
        {
            if (reader.Read())
            {
                int index = reader.GetOrdinal("MarketingCommission");
                BusinessFee.Add(fullBarCode,Double.Parse(reader.GetDecimal(index).ToString()));
            }
            else
                return 0;
        }
        return BusinessFee[fullBarCode];
    }

    private static bool isFormulaCalculated(DataRow row)
    {
        double checkFormula;
        return double.TryParse(row["IrgunPriceFormula"].ToString(), out checkFormula);
    }
    private static bool isFormulaCalculated(DynamicPriceMembers row)
    {
        double checkFormula;
        return double.TryParse(row.irgunPriceFormula, out checkFormula);
    }
    public static void CalculatIrgunPrice(DataTable variantTable, DataRow memberRow)
    {
        string variable = memberRow["DynamicIrgunPrice"].ToString();
        try
        {
            for (int i = 0; i < variantTable.Rows.Count; i++)
            {
                if (isFormulaCalculated(variantTable.Rows[i]))
                    continue;
                double catalogPrice = System.Convert.ToDouble(variantTable.Rows[i]["CatalogicPrice"]);
                int businessSubtype = System.Convert.ToInt32(variantTable.Rows[i]["BusinessSubTypeID"]);

                double percent = 5.66;//define default percentage
                double Fee = getBusinessFee(variantTable.Rows[i]["FullBarCode"].ToString());
                //update values from DB
                if (subtyps.ContainsKey(businessSubtype) && subtyps[businessSubtype].ContainsKey(variable))
                {
                    int trustProgram = 0;
                    if (variantTable.Rows[i]["TrustProgram"] != DBNull.Value)
                        trustProgram = System.Convert.ToInt32(variantTable.Rows[i]["TrustProgram"]);
                    percent = Convert.ToDouble(subtyps[businessSubtype][variable].getDynamicIrgunPrice(trustProgram));
                }

                //formula for calculating the percent to take
                double calculatedIrgunPrice = catalogPrice * (100 - Fee) / (100 - percent);

                variantTable.Rows[i]["IrgunPriceFormula"] = (
                    catalogPrice > calculatedIrgunPrice ?
                    catalogPrice : calculatedIrgunPrice
                    ).ToString("0.00");

            }
        }
        catch (Exception ex)
        {
            SqlMethods.SendMail(string.Format("Error while calculating irgun price for {0} {1}", memberRow["MemberId"], ex.Message + ex.StackTrace), "Error at CalculatIrgunPrice!!!");
            return;
        }
        variantTable.AcceptChanges();
    }
    public static void CalculatIrgunPrice(DynamicPriceMembers variant, string variable)
    {
        try
        {
            if (isFormulaCalculated(variant))
                return;
            double catalogPrice = System.Convert.ToDouble(variant.catalogicPrice);
            int businessSubtype = System.Convert.ToInt32(variant.BusinessSubTypeID);

            double percent = 5.66;//define default percentage
            double Fee = getBusinessFee(variant.FullBarCode);//business marketing commition

            //update values from DB
            if (subtyps.ContainsKey(businessSubtype) && subtyps[businessSubtype].ContainsKey(variable))
            {
                string dynamicPrice = subtyps[businessSubtype][variable].getDynamicIrgunPrice(variant.TrustProgram);
                percent = Convert.ToDouble(dynamicPrice);
            }
            //formula for calculating the percent to take
            double calculatedIrgunPrice = catalogPrice * (100 - Fee) / (100 - percent);

            variant.irgunPriceFormula = (
                catalogPrice > calculatedIrgunPrice ?
                catalogPrice : calculatedIrgunPrice
                ).ToString("0.00");

        }
        catch (Exception ex)
        {
            SqlMethods.SendMail(string.Format("Error while calculating irgun price for {0} {1}", "no member id for lcwallet", ex.Message + ex.StackTrace), "Error at CalculatIrgunPrice!!!");
            return;
        }
    }
    public static string CalculatIrgunPrice_Editor(EditorPriceMember variant)
    {
        try
        {
            double catalogPrice = System.Convert.ToDouble(variant.catalogicPrice);
            int businessSubtype = System.Convert.ToInt32(variant.BusinessSubTypeID);

            double percent = 5.66;//define default percentage
            double Fee = double.Parse(variant.marketingCommition);//getBusinessFee(variant.FullBarCode);//business marketing commition

            //update values from DB
            if (subtyps.ContainsKey(businessSubtype) && subtyps[businessSubtype].ContainsKey(variant.variableY))
            {
                string dynamicPrice = subtyps[businessSubtype][variant.variableY].getDynamicIrgunPrice(variant.TrustProgram);
                percent = Convert.ToDouble(dynamicPrice);
            }
            //formula for calculating the percent to take
            double calculatedIrgunPrice = catalogPrice * (100 - Fee) / (100 - percent);

            return(
                catalogPrice > calculatedIrgunPrice ?
                catalogPrice : calculatedIrgunPrice
                ).ToString("0.00");

        }
        catch (Exception ex)
        {
            SqlMethods.SendMail(string.Format("Error while calculating irgun price for {0} {1}", "no member id for lcwallet", ex.Message + ex.StackTrace), "Error at CalculatIrgunPrice!!!");
            return null;
        }
    }
}

