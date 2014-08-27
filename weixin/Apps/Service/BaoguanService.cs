using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using bgweb;
using Weixin.Models;
using System.Text;
using Weixin.Apps.Entity;

namespace Weixin.Apps.Service
{
    /// <summary>
    /// 报关业务数据查询
    /// </summary>
    public class BaoguanService
    {
        private Models.bgxtEntities mdb = new Models.bgxtEntities();

        /// <summary>
        /// 根据微信id取该用户一段时间的报关汇总数据
        /// </summary>
        /// <param name="yun_openid"></param>
        /// <returns></returns>
        public List<object[]> countCompByEmp(int conditionKind, string yun_openid, string apply_dt, string apply_dt2)
        {
            List<object[]> resultList = null;
            
            string querySql = "";

            if (conditionKind == WeixinConstant.CLAUSE_COUNTDAILY)//报关公司的客户日小结
            {
                querySql = "select Employees.yun_openid,org1.short as cust_name,org2.short as decl_name,"
                      + "(select count(1) from Bg_Mains where Bg_Mains.ef_flag='F' and Bg_Mains.organize_id=OrganizeCustoms.custom_id and Bg_Mains.customer_id=OrganizeCustoms.customer_id and Bg_Mains.apply_dt>=@apply_dt1 and Bg_Mains.apply_dt<DATEADD(day,1,@apply_dt2)) as count_total,"
                    + "(select COUNT(1) from Bg_Mains where Bg_Mains.ef_flag='F' and Bg_Mains.organize_id=OrganizeCustoms.custom_id and Bg_Mains.customer_id=OrganizeCustoms.customer_id and Bg_Mains.apply_dt>=@apply_dt1 and Bg_Mains.apply_dt<DATEADD(day,1,@apply_dt2) and declare_id=1) as count_exp,"
                    + "(select COUNT(1) from Bg_Mains where Bg_Mains.ef_flag='F' and Bg_Mains.organize_id=OrganizeCustoms.custom_id and Bg_Mains.customer_id=OrganizeCustoms.customer_id and Bg_Mains.apply_dt>=@apply_dt1 and Bg_Mains.apply_dt<DATEADD(day,1,@apply_dt2) and declare_id=2) as count_imp,"
                    + "(select COUNT(1) from Bg_Mains where Bg_Mains.ef_flag='F' and Bg_Mains.organize_id=OrganizeCustoms.custom_id and Bg_Mains.customer_id=OrganizeCustoms.customer_id and Bg_Mains.apply_dt>=@apply_dt1 and Bg_Mains.apply_dt<DATEADD(day,1,@apply_dt2) and declare_id=3) as count_trans_exp,"
                    + "(select COUNT(1) from Bg_Mains where Bg_Mains.ef_flag='F' and Bg_Mains.organize_id=OrganizeCustoms.custom_id and Bg_Mains.customer_id=OrganizeCustoms.customer_id and Bg_Mains.apply_dt>=@apply_dt1 and Bg_Mains.apply_dt<DATEADD(day,1,@apply_dt2) and declare_id=4) as count_trans_imp,"
                    + "(select COUNT(1) from Bg_Mains where Bg_Mains.ef_flag='F' and Bg_Mains.organize_id=OrganizeCustoms.custom_id and Bg_Mains.customer_id=OrganizeCustoms.customer_id and Bg_Mains.apply_dt>=@apply_dt1 and Bg_Mains.apply_dt<DATEADD(day,1,@apply_dt2) and declare_id=5) as count_locat,"
                    + "(select COUNT(1) from Bg_Mains where Bg_Mains.ef_flag='F' and Bg_Mains.organize_id=OrganizeCustoms.custom_id and Bg_Mains.customer_id=OrganizeCustoms.customer_id and Bg_Mains.pass_dt>=@apply_dt1 and Bg_Mains.pass_dt<DATEADD(day,1,@apply_dt2)) as count_pass,"
                    + "(select COUNT(1) from Bg_Mains where Bg_Mains.ef_flag='F' and Bg_Mains.organize_id=OrganizeCustoms.custom_id and Bg_Mains.customer_id=OrganizeCustoms.customer_id and Bg_Mains.apply_dt is not null and Bg_Mains.pass_dt is null) as count_unpass,"
                    + "(select COUNT(1) from Bg_Mains where Bg_Mains.ef_flag='F' and Bg_Mains.organize_id=OrganizeCustoms.custom_id and Bg_Mains.customer_id=OrganizeCustoms.customer_id and Bg_Mains.hgcg_dt>=@apply_dt1 and Bg_Mains.hgcg_dt<DATEADD(day,1,@apply_dt2)) as count_hgcheck,"
                    + "(select COUNT(1) from Bg_Mains where Bg_Mains.ef_flag='F' and Bg_Mains.organize_id=OrganizeCustoms.custom_id and Bg_Mains.customer_id=OrganizeCustoms.customer_id and Bg_Mains.sjcg_dt>=@apply_dt1 and Bg_Mains.sjcg_dt<DATEADD(day,1,@apply_dt2)) as count_sjcheck,"
                    + "STUFF((SELECT ','+Bg_Details.container_no FROM Bg_Mains,Bg_Details WHERE Bg_Mains.ef_flag='F' and Bg_Mains.organize_id=OrganizeCustoms.custom_id and Bg_Mains.customer_id=OrganizeCustoms.customer_id and Bg_mains.id=Bg_Details.bg_main_id and ((Bg_Details.check_dt>=@apply_dt1 and Bg_Details.check_dt<DATEADD(day,1,@apply_dt2)) or (Bg_Details.check01_dt>=@apply_dt1 and Bg_Details.check01_dt<DATEADD(day,1,@apply_dt2))) ORDER BY Bg_Details.num_no FOR XML PATH('')),1,1,'') as list_check,"
                    + "STUFF((SELECT ','+Bg_Mains.container_no1 FROM Bg_Mains WHERE Bg_Mains.ef_flag='F' and Bg_Mains.organize_id=OrganizeCustoms.custom_id and Bg_Mains.customer_id=OrganizeCustoms.customer_id and Bg_Mains.apply_dt is not null and Bg_Mains.pass_dt is null FOR XML PATH('')),1,1,'') as list_unpass"
                    + " from Employees,OrganizeCustoms,Organizes org1,Organizes org2"
                    + " where Employees.yun_openid is not null and Employees.Organize_id=OrganizeCustoms.organize_id and OrganizeCustoms.organize_id=org1.id and OrganizeCustoms.custom_id=org2.id";
            }else{//报关公司日报、周报、月报
                querySql = "select Employees.yun_openid,'' as cust_name,org2.short as decl_name,"
                  + "(select count(1) from Bg_Mains where Bg_Mains.ef_flag='F' and Bg_Mains.organize_id=org2.id and Bg_Mains.apply_dt>=@apply_dt1 and Bg_Mains.apply_dt<DATEADD(day,1,@apply_dt2)) as count_total,"
                + "(select COUNT(1) from Bg_Mains where Bg_Mains.ef_flag='F' and Bg_Mains.organize_id=org2.id and Bg_Mains.apply_dt>=@apply_dt1 and Bg_Mains.apply_dt<DATEADD(day,1,@apply_dt2) and declare_id=1) as count_exp,"
                + "(select COUNT(1) from Bg_Mains where Bg_Mains.ef_flag='F' and Bg_Mains.organize_id=org2.id and Bg_Mains.apply_dt>=@apply_dt1 and Bg_Mains.apply_dt<DATEADD(day,1,@apply_dt2) and declare_id=2) as count_imp,"
                + "(select COUNT(1) from Bg_Mains where Bg_Mains.ef_flag='F' and Bg_Mains.organize_id=org2.id and Bg_Mains.apply_dt>=@apply_dt1 and Bg_Mains.apply_dt<DATEADD(day,1,@apply_dt2) and declare_id=3) as count_trans_exp,"
                + "(select COUNT(1) from Bg_Mains where Bg_Mains.ef_flag='F' and Bg_Mains.organize_id=org2.id and Bg_Mains.apply_dt>=@apply_dt1 and Bg_Mains.apply_dt<DATEADD(day,1,@apply_dt2) and declare_id=4) as count_trans_imp,"
                + "(select COUNT(1) from Bg_Mains where Bg_Mains.ef_flag='F' and Bg_Mains.organize_id=org2.id and Bg_Mains.apply_dt>=@apply_dt1 and Bg_Mains.apply_dt<DATEADD(day,1,@apply_dt2) and declare_id=5) as count_locat,"
                + "(select COUNT(1) from Bg_Mains where Bg_Mains.ef_flag='F' and Bg_Mains.organize_id=org2.id and Bg_Mains.pass_dt>=@apply_dt1 and Bg_Mains.pass_dt<DATEADD(day,1,@apply_dt2)) as count_pass,"
                + "(select COUNT(1) from Bg_Mains where Bg_Mains.ef_flag='F' and Bg_Mains.organize_id=org2.id and Bg_Mains.apply_dt is not null and Bg_Mains.pass_dt is null) as count_unpass,"
                + "(select COUNT(1) from Bg_Mains where Bg_Mains.ef_flag='F' and Bg_Mains.organize_id=org2.id and Bg_Mains.hgcg_dt>=@apply_dt1 and Bg_Mains.hgcg_dt<DATEADD(day,1,@apply_dt2)) as count_hgcheck,"
                + "(select COUNT(1) from Bg_Mains where Bg_Mains.ef_flag='F' and Bg_Mains.organize_id=org2.id and Bg_Mains.sjcg_dt>=@apply_dt1 and Bg_Mains.sjcg_dt<DATEADD(day,1,@apply_dt2)) as count_sjcheck,"
                + "STUFF((SELECT ','+Bg_Details.container_no FROM Bg_Mains,Bg_Details WHERE Bg_Mains.ef_flag='F' and Bg_Mains.organize_id=org2.id and Bg_mains.id=Bg_Details.bg_main_id and ((Bg_Details.check_dt>=@apply_dt1 and Bg_Details.check_dt<DATEADD(day,1,@apply_dt2)) or (Bg_Details.check01_dt>=@apply_dt1 and Bg_Details.check01_dt<DATEADD(day,1,@apply_dt2))) ORDER BY Bg_Details.num_no FOR XML PATH('')),1,1,'') as list_check,"
                + "STUFF((SELECT ','+Bg_Mains.container_no1 FROM Bg_Mains WHERE Bg_Mains.ef_flag='F' and Bg_Mains.organize_id=org2.id and Bg_Mains.apply_dt is not null and Bg_Mains.pass_dt is null FOR XML PATH('')),1,1,'') as list_unpass"
                + " from Employees,Organizes org2"
                + " where Employees.yun_openid is not null and Employees.Organize_id=org2.id";
            }
            if (yun_openid != null && !"".Equals(yun_openid))
            {
                querySql = querySql + " and Employees.yun_openid='" + yun_openid+"'";
            }
            DBHelp dbHelp = new DBHelp();
            SqlDataReader reader = null;
            try
            {
                dbHelp.Open();
                SqlCommand cmd = dbHelp.GetCommand(querySql);
                cmd.Parameters.Add(new SqlParameter("@apply_dt1", apply_dt));
                cmd.Parameters.Add(new SqlParameter("@apply_dt2", apply_dt2));
                reader = cmd.ExecuteReader();
                resultList = new List<object[]>();
                while (reader.Read())
                {
                    //Console.WriteLine(String.Format("{0}, {1}", reader[0], reader[1]));
                    object[] result = { (string)reader[0], (string)reader[1], (string)reader[2], (int)reader[3], (int)reader[4], (int)reader[5], (int)reader[6], (int)reader[7], (int)reader[8], (int)reader[9], (int)reader[10], (int)reader[11], (int)reader[12], (string)(reader[13] + ""), (string)(reader[14] + "") };
                    resultList.Add(result);
                }
            }
            finally
            {
                // Always call Close when done reading.
                if (reader != null)
                {
                    reader.Close();
                }
                dbHelp.Close();
            }
            return resultList;
        }
        /// <summary>
        /// 获取HS编码详情
        /// </summary>
        /// <param name="conditionCode"></param>
        /// <returns></returns>
        public Stcodes searchStcodeby(string conditionCode)
        {
            Stcodes stcode  = null;
            var linq = from p in mdb.Stcodes
                       where p.code_ts == conditionCode
                       select new
                       {
                           p.ver_ts,
                           p.code_ts,
                           p.g_name,
                           p.control,
                           p.low_rate,
                           p.high_rate,
                           p.tax_type,
                           p.tax_rate,
                           p.note_s,
                           unit_no = (from un in mdb.Codes where un.zucode == p.unit_no && un.zclass_id == 1 select un.znamec).Distinct().FirstOrDefault(),
                           sunit_no = (from sun in mdb.Codes where sun.zucode == p.sunit_no && sun.zclass_id == 1 select sun.znamec).Distinct().FirstOrDefault()
                       };
            var stcodes = linq.ToList().Select(p => new Stcodes { ver_ts = p.ver_ts, code_ts = p.code_ts, g_name = p.g_name, control = p.control, low_rate = p.low_rate, high_rate = p.high_rate, tax_type = p.tax_type, tax_rate = p.tax_rate, note_s = p.note_s, unit_no = p.unit_no, sunit_no = p.sunit_no });
            List<Stitems> stitems = mdb.Stitems.Where(
                    p => p.code_ts == conditionCode).OrderBy(p => p.sort).ToList();
            StringBuilder declaredElements = new StringBuilder();
            if (stcodes != null && stcodes.Count() > 0)
            {
                stcode = stcodes.First();
                //申报要素
                if (stitems != null && stitems.Count() > 0)
                {
                    foreach (Stitems itm in stitems)
                    {
                        declaredElements.Append(itm.znamec + WeixinConstant.SEPT);
                    }
                }
                stcode.tax_type = cutOffTail(declaredElements, WeixinConstant.SEPT);
                string control = stcode.control;
                string wherein = str2Sqlin(control);
                if (wherein != null)
                {
                    //监管条件
                    List<Codes> codeList = mdb.Codes.Where(p => p.zclass_id == 11 && wherein.Contains(p.zucode)).ToList();
                    String controlStr = code2controls(control, codeList);
                    stcode.control = controlStr;
                }
            }
            return stcode;
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chars"></param>
        /// <returns></returns>
        private string str2Sqlin(string str)
        {
            if (str == null)
            {
                return null;
            }
            StringBuilder sb = null;
            str = str.Trim();
            if (str != "" && str.Length > 0)
            {
                sb = new StringBuilder();
                foreach (char s in str.ToCharArray())
                {
                    sb.Append("'" + s + "'" + WeixinConstant.SEPT);
                }
            }
            return cutOffTail(sb, WeixinConstant.SEPT);
        }
        /// <summary>
        /// 监管条件转换
        /// </summary>
        /// <param name="str"></param>
        /// <param name="codeList"></param>
        /// <returns></returns>
        private string code2controls(string str, List<Codes> codeList)
        {
            if (str == null || codeList == null)
            {
                return null;
            }
            StringBuilder sb = null;
            str = str.Trim();
            if (str != "" && str.Length > 0)
            {
                sb = new StringBuilder();
                foreach (char s in str.ToCharArray())
                {
                    foreach (Codes c in codeList)
                    {
                        if (c.zucode.Equals(s.ToString()))
                        {
                            sb.Append(s + "." + c.znamec + WeixinConstant.SEPT);
                            continue;
                        }
                    }
                }
            }
            return cutOffTail(sb, WeixinConstant.SEPT);
        }
        /// <summary>
        /// 去掉字符串尾巴
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="tail"></param>
        /// <returns></returns>
        private string cutOffTail(StringBuilder sb, string tail)
        {
            string sbstr = null;
            if (sb != null)
            {
                sbstr = sb.ToString();
                if (sbstr.LastIndexOf(tail) == sbstr.Length - 1)
                {
                    sbstr = sbstr.Substring(0, sbstr.Length - 1);
                }
            }
            return sbstr;
        }

        
    }
}