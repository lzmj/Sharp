using SharpDB.Entity;
using SharpTools.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpTools.DbDAL
{
    public static class DBScript
    {
        public static string GetTableScript(DataBaseType currDBType, TableInfo tableInfo, DataBaseType tgtDBType)
        {
            StringPlus sPlus = new StringPlus();
            var lstCols = tableInfo.LstColInfo;
            if (currDBType == DataBaseType.MsSql && tgtDBType == DataBaseType.MsSql)
            {
                #region CREATE TABLE By MsSql
                sPlus.AppendLine("CREATE TABLE [" + tableInfo.TableName + "]");
                sPlus.AppendLine("(");
                List<string> lstDefSpt = new List<string>();
                List<string> lstComments = new List<string>();
                for (int j = 0; j < lstCols.Count; j++)
                {
                    ColumnInfo col = lstCols[j];
                    sPlus.AppendSpace(2, "[" + col.ColumnName + "] " + col.NewTypeName(currDBType) + "");
                    if (col.IsIdentity)
                    {
                        sPlus.Append(" identity(1,1) ");
                    }
                    if (col.CanNull)
                    {
                        sPlus.Append(" null ");
                    }
                    else
                    {
                        sPlus.Append(" not null ");
                    }

                    if (col.IsPK)
                    {
                        sPlus.Append(" primary key ");
                    }

                    if (j != (lstCols.Count - 1))
                    {
                        sPlus.AppendLine(",");
                    }
                    else
                    {
                        sPlus.AppendLine();
                    }

                    if (!string.IsNullOrEmpty(col.DefaultVal))
                    {
                        string def_spt = "ALTER TABLE [" + tableInfo.TableName + "] ADD DEFAULT " + col.DefaultVal + " FOR [" + col.ColumnName + "]";
                        lstDefSpt.Add(def_spt);
                    }

                    if (!string.IsNullOrWhiteSpace(col.DeText))
                    {
                        string desc_spt = "EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'" + col.DeText + "' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'" + tableInfo.TableName + "', @level2type=N'COLUMN',@level2name=N'" + col.ColumnName + "'";
                        lstComments.Add(desc_spt);
                    }
                }
                sPlus.AppendLine(") ON [PRIMARY] ");
                sPlus.AppendLine("GO");
                if (lstDefSpt.Count > 0)
                {
                    string def_script = string.Join("\r\nGO\r\n", lstDefSpt.ToArray());
                    sPlus.AppendLine(def_script);
                    sPlus.AppendLine("GO");
                }

                if (!string.IsNullOrEmpty(tableInfo.TabComment))
                {
                    string tab_comment_spt = @"EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'" + tableInfo.TabComment + "' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'" + tableInfo.TableName + "'";
                    sPlus.AppendLine(tab_comment_spt);
                    sPlus.AppendLine("GO");
                }
                if (lstComments.Count > 0)
                {
                    string col_comment_spt = string.Join("\r\nGO\r\n", lstComments.ToArray());
                    sPlus.AppendLine(col_comment_spt);
                    sPlus.Append("GO");
                } 
                #endregion
            }
            else if (currDBType == DataBaseType.Oracle && tgtDBType == DataBaseType.Oracle)
            {
                #region CREATE TABLE By MsSql
                sPlus.AppendLine("CREATE TABLE " + tableInfo.TableName + "");
                sPlus.AppendLine("(");
                List<string> lstDefSpt = new List<string>();
                List<string> lstComments = new List<string>();
                for (int j = 0; j < lstCols.Count; j++)
                {
                    ColumnInfo col = lstCols[j];
                    sPlus.AppendSpace(2, col.ColumnName +" "+col.NewTypeName(currDBType) + "");                    
                    if (col.CanNull)
                    {
                        sPlus.Append(" null ");
                    }
                    else
                    {
                        sPlus.Append(" not null ");
                    }

                    if (col.IsPK)
                    {
                        sPlus.Append(" primary key ");
                    }

                    if (j != (lstCols.Count - 1))
                    {
                        sPlus.AppendLine(",");
                    }
                    else
                    {
                        sPlus.AppendLine();
                    }

                    //if (!string.IsNullOrEmpty(col.DefaultVal))
                    //{
                    //    string def_spt = "ALTER TABLE [" + tableInfo.TableName + "] ADD DEFAULT " + col.DefaultVal + " FOR [" + col.ColumnName + "]";
                    //    lstDefSpt.Add(def_spt);
                    //}

                    if (!string.IsNullOrWhiteSpace(col.DeText))
                    {
                        string desc_spt = "COMMENT ON TABLE " + tableInfo.TableName + "." + col.ColumnName + " IS '" + col.DeText + "'";
                        lstComments.Add(desc_spt);
                    }

                    //if (col.IsIdentity)
                    //{
                    //    sPlus.Append(" identity(1,1) ");
                    //}
                }
                sPlus.AppendLine(")");
                sPlus.AppendLine("LOGGING");
                sPlus.AppendLine("NOCOMPRESS");
                sPlus.AppendLine("NOCACHE");
                sPlus.AppendLine(";");

                //if (lstDefSpt.Count > 0)
                //{
                //    string def_script = string.Join("\r\nGO\r\n", lstDefSpt.ToArray());
                //    sPlus.AppendLine(def_script);
                //    sPlus.AppendLine("GO");
                //}

                if (!string.IsNullOrEmpty(tableInfo.TabComment))
                {
                    string tab_comment_spt = @"COMMENT ON TABLE " + tableInfo.TableName + " IS '" + tableInfo.TabComment + "';";
                    sPlus.AppendLine(tab_comment_spt);
                }
                if (lstComments.Count > 0)
                {
                    string col_comment_spt = string.Join("\r\n", lstComments.ToArray());
                    sPlus.AppendLine(col_comment_spt);
                }
                #endregion
            }
            else if (currDBType == DataBaseType.MySql && tgtDBType == DataBaseType.MySql)
            {

            }
            else if (currDBType == DataBaseType.Sqlite && tgtDBType == DataBaseType.Sqlite)
            {

            }

            return sPlus.Value;
        }


        private static string NewName(this ColumnInfo col, DataBaseType targetDBType)
        {
            List<string> lstScript = new List<string>();


            string columnName = col.ColumnName;
            if (targetDBType == DataBaseType.MsSql)
            {
                columnName = "[" + columnName + "]";
            }
            lstScript.Add(columnName);

            string typeName = col.TypeName;
            if (targetDBType != DataBaseType.MySql)
            {
                if (col.Length.HasValue && col.Scale.HasValue)
                {
                    typeName += "(" + col.Length.Value + "," + col.Scale.Value + ")";
                }
                else if (col.Length.HasValue && !col.Scale.HasValue)
                {
                    typeName += "(" + col.Length.Value + ")";
                }
            }
            lstScript.Add(typeName);

            if (col.IsIdentity)
            {

            }

            if (!string.IsNullOrEmpty(col.DefaultVal))
            {
                string defVal = col.DefaultVal.Replace("(", "").Replace(")", "");
                lstScript.Add(defVal);
            }


            return columnName;
        }

        private static string NewTypeName(this ColumnInfo col, DataBaseType currDBType)
        {
            string typeName = col.TypeName;
            if (currDBType != DataBaseType.MySql && !typeName.Equals("bit"))
            {
                if (col.Length.HasValue && !col.Scale.HasValue)
                {
                    typeName += "(" + col.Length.Value + ")";
                }
                else if (col.Length.HasValue && col.Scale.HasValue)
                {
                    typeName += "(" + col.Length.Value + "," + col.Scale.Value + ")";
                }
            }
            return typeName;
        }

    }
}
