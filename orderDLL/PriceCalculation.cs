using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.JScript.Vsa;
using Microsoft.JScript;



public class PriceCalculation
{
    public static bool CheckFormola(string formola)
    {
        formola = checkFormulaForEncoding(formola);
        if (formola.Contains("@"))
            RebuildFormula(ref formola);
        if (formola.Contains("@") || formola.Contains("#"))
            return false;
        else
        {
            double ans = 0;
            if (double.TryParse(formola, out ans))
                return true;
            CalculatFormula(ref formola, ref ans);
            if (formola == null)
                return false;
            else
            {
                object obj = EvalExpression(formola);
                if (obj == null)
                    return false;
                else
                {
                    if (double.TryParse(obj.ToString(), out ans))
                        return true;
                    else
                        return false;
                }
            }
        }
    }

    public static void CalculatVariantTable_PriceCalculation(DataTable variantTable, DataRow memberRow)
    {
        if (memberRow.Table.Columns.Contains("DynamicIrgunPrice") && memberRow["DynamicIrgunPrice"] != DBNull.Value && variantTable.Columns.Contains("CatalogicPrice"))
        {
            StaticData.CalculatIrgunPrice(variantTable, memberRow);
        }
        CalculatTable_PriceCalculation(variantTable, memberRow);
    }

    private static void CalculatTable_PriceCalculation(DataTable variantTable, DataRow memberRow)
    {
        //                                      מחיר קטלוגי - להצגה בלבד    מחיר ללקוח קצה           סובסידיה              עמלת הפצה             מחיר לארגון
        string[] coloumnsName = new string[] { "IrgunPriceFormula", "VarComissionFormula", "VarDiscountFormula", "VarPriceFormula", 
                                                "MemberGeneralLimitFormula", "MemberDailyLimitFormula", "MemberWeeklyLimitFormula", "MemberMonthlyLimitFormula", "MemberQuarterLimitFormula", "MemberYearlyLimitFormula",
                                                "ProductGeneralLimitFormula", "ProductDailyLimitFormula", "ProductWeeklyLimitFormula", "ProductMonthlyLimitFormula", "ProductQuarterLimitFormula", "ProductYearlyLimitFormula"};//"CatalogicPrice"
        double var;
        List<string> lst = new List<string>();
        string formola = null;
        for (int i = 0; i < variantTable.Rows.Count; i++)
        {
            for (int z = 0; z < coloumnsName.Length; z++)
            {
                if (variantTable.Rows[i].Table.Columns.IndexOf(coloumnsName[z]) > -1)
                {
                    if (variantTable.Rows[i][coloumnsName[z]] != DBNull.Value 
                        && !double.TryParse(variantTable.Rows[i][coloumnsName[z]].ToString(), out var))
                    {
                        lst.Clear();
                        formola = variantTable.Rows[i][coloumnsName[z]].ToString().Trim();
                        formola = checkFormulaForEncoding(formola);
                        if (formola.Contains("@"))
                        {
                            RebuildFormula(ref formola, ref lst, memberRow, variantTable.Rows[i]);
                            if (lst.Count > 0)
                            
                            {
                                //formola = string.Format(formola, lst.ToArray());
                                for (int t = 0; t < lst.ToArray().Length; t++)
                                    formola = formola.Replace("{" + t + "}", lst[t]);
                            }
                            if (formola.Contains("@"))
                                continue;
                            CalculatFormula(ref formola, ref var);
                            variantTable.Rows[i][coloumnsName[z]] = var;
                        }
                    }
                }
            }
        }
        variantTable.AcceptChanges();
    }

    private static string checkFormulaForEncoding(string formula)
    {
        if (formula.Contains("&gt") || formula.Contains("&lt") || formula.Contains("&amp") || formula.Contains("&quot"))
            return System.Web.HttpUtility.HtmlDecode(formula);
        return formula;
    }

    private static void RebuildFormula(ref string formola, ref List<string> memberParams, DataRow memberRow, DataRow varRow)
    {
        int countParams = CountVar(formola);
        for (int i = 0; i < countParams; i++)
        {
            int startIndex = formola.IndexOf('@');
            if (startIndex < 0)
                break;
            int stopIndex = formola.IndexOf('#', startIndex);
            string par = formola.Substring(startIndex + 1, (stopIndex - 1) - startIndex);
            if (memberRow.Table.Columns.IndexOf(par) > -1)
            {
                double value;
                if (memberRow[par] != DBNull.Value && double.TryParse(memberRow[par].ToString(), out value))
                {
                    memberParams.Add(value.ToString());
                    formola = formola.Replace("@" + par + "#", "{" + i.ToString() + "}");
                }
                else
                    break;
            }
            else if (varRow.Table.Columns.IndexOf(par) > -1)
            {
                double value;
                if (varRow[par] != DBNull.Value && double.TryParse(varRow[par].ToString(), out value))
                {
                    memberParams.Add(value.ToString());
                    formola = formola.Replace("@" + par + "#", "{" + i.ToString() + "}");
                }
                else
                    break;
            }
            else
                break;
        }
    }

    private static void RebuildFormula(ref string formola)
    {
        int countParams = CountVar(formola);
        try
        {
            for (int i = 0; i < countParams; i++)
            {
                int startIndex = formola.IndexOf('@');
                if (startIndex != -1)
                {
                    int stopIndex = formola.IndexOf('#', startIndex);
                    string par = formola.Substring(startIndex + 1, (stopIndex - 1) - startIndex);
                    formola = formola.Replace("@" + par + "#", "1");
                }
            }
        }
        catch (Exception ex)
        {
          
        }
    }

    private static int CountVar(string formola)
    {
        int counter = 0;
        char[] formolaArray = formola.ToCharArray();
        for (int i = 0; i < formolaArray.Length; i++)
        {
            if (formolaArray[i] == '@')
                counter++;
        }
        return counter;
    }

    public static void CalculatFormula(ref string formola, ref double ans)
    {
        if (formola == null)
            return;

        object result = EvalExpression(formola);
        if (result != null)
        {
            if (double.TryParse(result.ToString(), out ans))
                return;
        }
        else
        {
            if (formola.Contains("IF"))
            {
                SearchAndExecuteCondition(ref formola);
                CalculatFormula(ref formola, ref ans);
            }
            else
                formola = null;
        }
    }

    private static void SearchAndExecuteCondition(ref string formola)
    {
        int startIndex = formola.LastIndexOf("[");
        int stopIndex = formola.IndexOf("]");
        if (stopIndex != -1 && stopIndex < startIndex)
            stopIndex = formola.IndexOf("]", stopIndex + 1);

        if (startIndex > -1 && stopIndex > -1)
        {
            string fiCondition = formola.Substring(startIndex + 1, stopIndex - (startIndex + 1));
            string[] split = fiCondition.Split(new string[] { "{", "}" }, StringSplitOptions.RemoveEmptyEntries);//(';');//(new string[] { "IF", "THEN", "ELSE" }, StringSplitOptions.RemoveEmptyEntries);//(';');
            if (split.Length > 3)
            {
                double endIF;
                object cond;
                cond = EvalIF(split);
                if (cond != null)
                {
                    cond = EvalExpression(cond.ToString());
                    if (double.TryParse(cond.ToString(), out endIF))
                    {
                        formola = formola.Remove(startIndex, (stopIndex + 1) - startIndex);
                        formola = formola.Insert(startIndex, endIF.ToString());
                    }
                }
            }
        }
    }

    private static object EvalIF(string[] lst)
    {
        if (lst[0].Contains("IF") && lst[0].Contains("THEN"))
            lst[0] = lst[0].Replace("IF", "").Replace("THEN", "");
        else
            return null;
        object cond;
        bool returnBoolean = false;
        cond = EvalExpression(lst[0]);
        if (cond != null && bool.TryParse(cond.ToString(), out returnBoolean))
            if (returnBoolean)
                return lst[1];
            else
                return lst[3];
        else
            return null;
    }

    private static VsaEngine Engine = null;
    private static object EvalExpression(string expression)
    {
        //object obj;
        //VsaEngine Engine = new VsaEngine();
        //try
        //{
        //    if (expression.Contains("IF"))
        //        return null;
        //    Engine = VsaEngine.CreateEngine();
        //    obj = Eval.JScriptEvaluate(expression, Engine);
        //}
        //catch (Exception ex)
        //{
        //    //SqlMethods.SendMail("Function -> EvalExpression"
        //    //                        + "\n expression -> " + expression
        //    //                        + "\n Exception TYPE -> " + ex.GetType()
        //    //                        + "\n Exception Message -> " + ex.Message
        //    //                        + "\n ex.Source -> " + ex.Source
        //    //                        + "\n ex.StackTrace -> " + ex.StackTrace, "PriceCalculation");
        //    obj = null;
        //}
        //return obj;

        //object obj;
        //float f;
        //if (Engine == null)// || !Engine.IsRunning)
        //{
        //    Engine = new VsaEngine();
        //    Engine = VsaEngine.CreateEngine();
        //}
        //try
        //{
        //    if (expression.Contains("IF"))
        //        return null;
        //    if (float.TryParse(expression, out f))
        //    {
        //        obj = f;
        //        return obj;
        //    }
        //    Engine = VsaEngine.CreateEngine();
        //    obj = Eval.JScriptEvaluate(expression, Engine);
        //}
        //catch (Exception ex)
        //{
        //    SqlMethods.SendMail("Function -> EvalExpression"
        //                            + "\n expression -> " + expression
        //                            + "\n Exception TYPE -> " + ex.GetType()
        //                            + "\n Exception Message -> " + ex.Message
        //                            + "\n ex.Source -> " + ex.Source
        //                            + "\n ex.StackTrace -> " + ex.StackTrace, "PriceCalculation");
        //    obj = null;
        //}
        //return obj;


        //2011-11-27
        //Replace the VsaEngine

        object ans = 0;
        Int16 round = 0;
        try
        {


            float f = 0;
            if (expression.Contains("IF"))
                return null;
            if (float.TryParse(expression, out f))
                return f;

            if (expression.Contains("Math"))
            {
                if (expression.Contains("Math.ceil"))
                {
                    round = 1;
                    expression = expression.Replace("Math.ceil", "");
                }
                else if (expression.Contains("Math.round"))
                {
                    round = 2;
                    expression = expression.Replace("Math.round", "");
                }
                else if (expression.Contains("Math.floor"))
                {
                    round = 3;
                    expression = expression.Replace("Math.floor", "");
                }
            }

            expression = expression.Replace("(", "").Replace(")", "");

            string[] calcParams = expression.Split(' ');

            List<string> parsList = new List<string>();
            for (int i = 0; i < calcParams.Length; i++)
            {
                if (calcParams[i] == "")
                    continue;
                parsList.Add(calcParams[i]);
            }

            if (parsList.Count == 3)
            {
                double[] dParsArr = new double[2];
                double.TryParse(parsList[0], out dParsArr[0]);
                double.TryParse(parsList[2], out dParsArr[1]);


                switch (parsList[1])
                {
                    case ">":
                        ans = dParsArr[0] > dParsArr[1];
                        break;
                    case "<":
                        ans = dParsArr[0] < dParsArr[1];
                        break;
                    case ">=":
                        ans = dParsArr[0] >= dParsArr[1];
                        break;
                    case "<=":
                        ans = dParsArr[0] <= dParsArr[1];
                        break;
                    case "==":
                        ans = dParsArr[0] == dParsArr[1];
                        break;
                    case "!=":
                        ans = dParsArr[0] != dParsArr[1];
                        break;
                    case "*":
                        ans = dParsArr[0] * dParsArr[1];
                        break;
                    case "/":
                        ans = dParsArr[0] / dParsArr[1];
                        break;
                    case "+":
                        ans = dParsArr[0] + dParsArr[1];
                        break;
                    case "-":
                        ans = dParsArr[0] - dParsArr[1];
                        break;
                    default:
                        ans = 0;
                        break;
                }
            }

            double final = 0;
            if (round != 0
                && double.TryParse(ans.ToString(), out final)
                && final != 0)
            {
                switch (round)
                {
                    case 1:
                        return Math.Ceiling(final);
                    case 2:
                        return Math.Round(final);
                    case 3:
                        return Math.Floor(final);
                    default:
                        return 0;
                }
            }
        }
        catch (Exception ex)
        {
            SqlMethods.SendMail("Function -> EvalExpression"
                                    + "\n expression -> " + expression
                                    + "\n Exception TYPE -> " + ex.GetType()
                                    + "\n Exception Message -> " + ex.Message
                                    + "\n ex.Source -> " + ex.Source
                                    + "\n ex.StackTrace -> " + ex.StackTrace, "PriceCalculation");
        }
        return ans;


    }
    //private static string CheckCondition(string condition)
    //{
    //    if (condition.Contains("=="))
    //        return "==";
    //    else if (condition.Contains("!="))
    //        return "!=";
    //    else if (condition.Contains(">="))
    //        return ">=";
    //    else if (condition.Contains("<="))
    //        return "<=";
    //    else if (condition.Contains(">"))
    //        return ">";
    //    else
    //        return "<";
    //}


    public static decimal CalculatFormula(string varPriceFormula, string irgunPriceFormula, int darga, int yamam)
    {
        decimal irgunPrice = 0;
        decimal.TryParse(irgunPriceFormula, out irgunPrice);
        if ((irgunPrice - darga) > (irgunPrice - yamam))
            irgunPrice = irgunPrice - yamam;
        else
            irgunPrice = irgunPrice - darga;
        //For Now 
        if (irgunPrice < 0)
            irgunPrice = 0;
        return irgunPrice;
    }

}



public class PriceMembers
{
    
    public string catalogicPrice { get; set; }
    public string irgunPriceFormula { get; set; }
    public string varDiscountFormula { get; set; }
    public string varPriceFormula { get; set; }
    public string varComissionFormula { get; set; }
    public string cancelComission { get; set; }

    public PriceMembers(string CatalogicPrice,
        string IrgunPriceFormula,
        string VarDiscountFormula,
        string VarPriceFormula,
        string VarComissionFormula)
    {
        this.catalogicPrice = CatalogicPrice;
        this.irgunPriceFormula = IrgunPriceFormula;
        this.varDiscountFormula = VarDiscountFormula;
        this.varPriceFormula = VarPriceFormula;
        this.varComissionFormula = VarComissionFormula;
    }

    public PriceMembers(string CatalogicPrice,
        string IrgunPriceFormula,
        string VarDiscountFormula,
        string VarPriceFormula,
        string VarComissionFormula,
        string CancelComission)
    {
        this.catalogicPrice = CatalogicPrice;
        this.irgunPriceFormula = IrgunPriceFormula;
        this.varDiscountFormula = VarDiscountFormula;
        this.varPriceFormula = VarPriceFormula;
        this.varComissionFormula = VarComissionFormula;
        this.cancelComission = CancelComission;
    }
}

public class DynamicPriceMembers : PriceMembers
{
    public string FullBarCode;
    public short BusinessSubTypeID;
    public int TrustProgram;

    public DynamicPriceMembers(PriceMembers p,short businessSubTypeId, int trustProgramType,string fullBarCode)
        : base(p.catalogicPrice, p.irgunPriceFormula, p.varDiscountFormula, p.varPriceFormula, p.varComissionFormula)
    {
        BusinessSubTypeID = businessSubTypeId;
        TrustProgram = trustProgramType;
        FullBarCode = fullBarCode;
    }
}
public class EditorPriceMember
{
    public string catalogicPrice;
    public string FullBarCode;
    public short BusinessSubTypeID;
    public int TrustProgram;
    public string variableY;
    public string marketingCommition;

    public EditorPriceMember(string CatalogicPrice,
                                string FullBarCode,
                                short BusinessSubTypeID,
                                int TrustProgram,
                                string variableY,
                                string marketingCommition)
    {
        catalogicPrice = CatalogicPrice;
        this.FullBarCode = FullBarCode;
        this.BusinessSubTypeID = BusinessSubTypeID;
        this.TrustProgram = TrustProgram;
        this.variableY = variableY;
        this.marketingCommition = marketingCommition;
    }
}