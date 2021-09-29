using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class gym_admin : Page
{
	DataTable dt;
	string empno = "";

	protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["id"] == null)
        {
            if (Session["empno"] == null)
                Response.Redirect("http://sis.seoul.co.kr");
            else
                empno = Session["empno"].ToString();
        }
        else
        {
            empno = Request["id"].ToString();
            Session["empno"] = empno;
        }

        adminCheck();        

        if (!IsPostBack)
		{			
			historyTabYearLoad();
			gymInfoTBLoad();
            optTBLoad();
		}
    }

	private void adminCheck()
	{
		bool admin_chk = false;

		dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select EMPL_CODE
from [insa].[dbo].[dh_empl_m]
where (DEPT_CODE = '303500' or DEPT_CODE = '300600' or ANDX_DEPT = '300600') and (DEGR_CODE <= 'B0' and EXRE_GUBN = '1')")), "SELECT");
		
		foreach (DataRow dr in dt.Rows)
		{
			if (dr.ItemArray[0].ToString().Equals(empno))
			{
				admin_chk = true;
				break;
			}
		}

		if (!admin_chk)
			ScriptManager.RegisterClientScriptBlock(this, GetType(), "alert", "alert('권한이 없습니다!'); history.back();", true);
	}

    protected void regClose_Click(object sender, EventArgs e)
    {
        dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select reg_close from [event].[dbo].[gym_info]")), "SELECT");

        // 마감 설정(1) ↔ 마감 취소(0)
        dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"update [event].[dbo].[gym_info] 
set reg_close = " + Convert.ToInt32(!Convert.ToBoolean(dt.Rows[0].ItemArray[0])) + ", user_ip = '{0}', upddate = getdate()", Request.UserHostAddress)), "UPDATE");
    }

    ////////// 년-월 DropDownList //////////
    private void historyTabYearLoad()
	{
		dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select distinct left(reg_month, 4) as year
from [event].[dbo].[gym]
order by year desc")), "SELECT");

		if (dt.Rows.Count < 1)
		{
			historyTabYear.Visible = false;
			historyTabMonth.Visible = false;
		}
		else
		{
			historyTabYear.Visible = true;
			historyTabMonth.Visible = true;

			foreach (DataRow dr in dt.Rows)
				historyTabYear.Items.Add(dr.ItemArray[0].ToString());
		}

		historyTabMonthLoad();
	}

	private void historyTabMonthLoad()
	{
		string month = "";

		dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select distinct right(reg_month, 2) as month
from [event].[dbo].[gym]
where left(reg_month, 4) = '{0}'
order by month desc", historyTabYear.Text)), "SELECT");

		if (dt.Rows.Count > 0)
		{
			historyTabMonth.Items.Clear();

			foreach (DataRow dr in dt.Rows)
				historyTabMonth.Items.Add(dr.ItemArray[0].ToString());

			month = historyTabMonth.Text;
		}			

		regTBLoad();
		cancelTBLoad();
	}

	protected void historyTabYear_SelectedIndexChanged(object sender, EventArgs e)
	{
		historyTabMonthLoad();
	}

	protected void historyTabMonth_SelectedIndexChanged(object sender, EventArgs e)
	{
		regTBLoad();
		cancelTBLoad();
	}
    
	////////// 신규 등록/연장자 테이블 //////////
	private void regTBLoad()
	{
        dt = Util.ExecuteQuery(new SqlCommand(getTableSql("REG")), "SELECT");

		if (dt.Rows.Count < 1)
		{
			regTableWrap.Visible = false;
			regEmptyTableWrap.Visible = true;
		}
		else
		{
			regTableWrap.Visible = true;
			regEmptyTableWrap.Visible = false;

			regTr.DataSource = dt;
			regTr.DataBind();
		}		
	}

    private string getTableSql(string code)
    {
        string sql = @"select empno, empname, empdept, cont, opt1, opt2, opt3, opt4, opt5, ";
        sql += "(select opt1 from [event].[dbo].[gym_info]) as 'opt1_name', ";
        sql += "(select opt2 from [event].[dbo].[gym_info]) as 'opt2_name', ";
        sql += "(select opt3 from [event].[dbo].[gym_info]) as 'opt3_name', ";
        sql += "(select opt4 from [event].[dbo].[gym_info]) as 'opt4_name', ";
        sql += "(select opt5 from [event].[dbo].[gym_info]) as 'opt5_name' ";
        sql += "from [event].[dbo].[gym] ";
        sql += "where reg_month = '" + historyTabYear.Text + historyTabMonth.Text + "' and ";
        sql += (code == "REG") ? "cancel = 0 " : "cont = 1 and cancel = 1 ";
        sql += (code == "REG") ? "order by cont asc, " : "order by ";

        switch ((code == "REG") ? historyTabOrder1.Text : historyTabOrder2.Text)
        {
            case "등록순":
            case "취소순":
                sql += "upddate asc";
                break;
            case "이름순":
                sql += "empname asc";
                break;
            case "사번순":
                sql += "empno asc";
                break;
            case "부서순":
                sql += "empdept asc";
                break;
        }

        return sql;
    }

	protected void historyTabOrder1_SelectedIndexChanged(object sender, EventArgs e)
	{
        regTBLoad();        
    }

    protected void Cancel_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;

        dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"update [event].[dbo].[gym] 
set present = 0, cancel = 1, user_ip = '{0}', upddate = getdate() 
where empno = '{1}' and reg_month = '{2}'", Request.UserHostAddress, btn.CommandArgument.ToString(), historyTabYear.Text + historyTabMonth.Text)), "UPDATE");

        regTBLoad();
        cancelTBLoad();
    }

    protected void regSMS_Click(object sender, EventArgs e)
	{
		dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select empno, empname, empdept 
from [event].[dbo].[gym]
where cancel = 0 and reg_month = '{0}'
order by cont asc, empno asc ", historyTabYear.Text + historyTabMonth.Text)), "SELECT");

		smsRecipientFormat(dt);
	}

	private void smsRecipientFormat(DataTable dt)
	{
		string txt = "";
		
		foreach (DataRow dr in dt.Rows)
			txt += dr.ItemArray[2].ToString().Split(' ')[0] + " " + dr.ItemArray[1].ToString() + " =" + dr.ItemArray[0].ToString() + Environment.NewLine;

		clipBoard.Text = txt;
		ScriptManager.RegisterStartupScript(this, GetType(), "copy", "copySmsRecipient();", true);
	}

    protected void regExcel_Click(object sender, EventArgs e)
    {
        SqlDataSource1.SelectCommand = getExcelSql("REG");
        SqlDataSource1.Select(DataSourceSelectArguments.Empty);
        SqlDataSource1.DataBind();
        GridView1.DataBind();
         
        exportExcel("REG");        
    }

    private string getExcelSql(string code)
    {
        string sql = "declare @opt1_name varchar(10), @opt2_name varchar(10), @opt3_name varchar(10), @opt4_name varchar(10), @opt5_name varchar(10) ";
        sql += "select @opt1_name = (select opt1 from [event].[dbo].[gym_info]), @opt2_name = (select opt2 from [event].[dbo].[gym_info]), @opt3_name = (select opt3 from [event].[dbo].[gym_info]), @opt4_name = (select opt4 from [event].[dbo].[gym_info]), @opt5_name = (select opt5 from [event].[dbo].[gym_info]) ";
        sql += "select row_number() over(order by (select null)) as 'NO', ";
        sql += (code == "REG") ? "iif(cont = 0, '신규', '연장') as '구분', " : "'취소' as '구분', ";
        sql += "empname as '이름', ";
        sql += "empno as '사번', ";
        sql += "empdept as '부서', ";
        sql += "iif(opt1 = 1, @opt1_name, '-') as '부대시설1', ";
        sql += "iif(opt2 = 1, @opt2_name, '-') as '부대시설2', ";
        sql += "iif(opt3 = 1, @opt3_name, '-') as '부대시설3', ";
        sql += "iif(opt4 = 1, @opt4_name, '-') as '부대시설4', ";
        sql += "iif(opt5 = 1, @opt5_name, '-') as '부대시설5' ";
        sql += "from [event].[dbo].[gym] ";
        sql += "where reg_month = " + historyTabYear.Text + historyTabMonth.Text + " and ";
        sql += (code == "REG") ? "cancel = 0 " : "cont = 1 and cancel = 1 ";
        sql += (code == "REG") ? "order by cont asc, " : "order by ";

        switch ((code == "REG") ? historyTabOrder1.Text: historyTabOrder2.Text)
        {
            case "등록순":
            case "취소순":
                sql += "upddate asc";
                break;
            case "이름순":
                sql += "empname asc";
                break;
            case "사번순":
                sql += "empno asc";
                break;
            case "부서순":
                sql += "empdept asc";
                break;
        }

        return sql;
    }

    private void exportExcel(string code)
    {
        StringWriter strwritter = new StringWriter();
        string filename = "gym" + historyTabYear.Text + historyTabMonth.Text + "_" + ((code == "REG") ? "신규연장" : "취소") + ".xls";

        Response.Clear();
        Response.Buffer = true;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Charset = "";        
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode(filename));

        GridView1.GridLines = GridLines.Both;
        GridView1.HeaderStyle.Font.Bold = true;
        GridView1.RenderControl(new HtmlTextWriter(strwritter));

        Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />" + strwritter.ToString());
        Response.End();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the run time error "  
        //Control 'GridView1' of type 'Grid View' must be placed inside a form tag with runat=server."  
    }
    
	////////// 연장 취소자 테이블 //////////
	private void cancelTBLoad()
	{
        dt = Util.ExecuteQuery(new SqlCommand(getTableSql("CANCEL")), "SELECT");

        if (dt.Rows.Count < 1)
		{
			cancelTableWrap.Visible = false;
			cancelEmptyTableWrap.Visible = true;
		}
		else
		{
			cancelTableWrap.Visible = true;
			cancelEmptyTableWrap.Visible = false;

			cancelTr.DataSource = dt;
			cancelTr.DataBind();
		}		
	}

	protected void historyTabOrder2_SelectedIndexChanged(object sender, EventArgs e)
	{
        cancelTBLoad();
    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;

        dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"delete from [event].[dbo].[gym] 
where empno = '{0}' and reg_month = '{1}'", btn.CommandArgument.ToString(), historyTabYear.Text + historyTabMonth.Text)), "DELETE");

        cancelTBLoad();
    }

    protected void cancelSMS_Click(object sender, EventArgs e)
	{
		dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select empno, empname, empdept 
from [event].[dbo].[gym]
where cont = 1 and cancel = 1 and reg_month = '{0}'
order by empno asc", historyTabYear.Text + historyTabMonth.Text)), "SELECT");

		smsRecipientFormat(dt);
	}

    protected void cancelExcel_Click(object sender, EventArgs e)
    {
        SqlDataSource1.SelectCommand = getExcelSql("CANCEL");
        SqlDataSource1.Select(DataSourceSelectArguments.Empty);
        SqlDataSource1.DataBind();
        GridView1.DataBind();

        exportExcel("CANCEL");
    }

    ////////// 헬스장 정보 테이블 //////////
    protected void gymInfoTBLoad()
	{
		dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select info from [event].[dbo].[gym_info]")), "SELECT");

		if (dt.Rows.Count > 0)
			gymInfo.Text = dt.Rows[0].ItemArray[0].ToString().Replace("<br>", "\r\n").Replace("&nbsp;", " ");
	}

	protected void gymInfoSave_Click(object sender, EventArgs e)
	{
		dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"update [event].[dbo].[gym_info] 
set info = '{0}', user_ip = '{1}', upddate = getdate()", gymInfo.Text.Replace("\r\n", "<br>").Replace(" ", "&nbsp;"), Request.UserHostAddress)), "UPDATE");
	}
    
    ////////// 부대시설 관리 테이블 //////////
    protected void optTBLoad()
    {
        dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select opt1, opt2, opt3, opt4, opt5 from [event].[dbo].[gym_info]")), "SELECT");

        opt1.Text = dt.Rows[0].ItemArray[0].ToString();
        opt2.Text = dt.Rows[0].ItemArray[1].ToString();
        opt3.Text = dt.Rows[0].ItemArray[2].ToString();
        opt4.Text = dt.Rows[0].ItemArray[3].ToString();
        opt5.Text = dt.Rows[0].ItemArray[4].ToString();
    }

    protected void optSave_Click(object sender, EventArgs e)
    {
        dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"update [event].[dbo].[gym_info] 
set opt1 = '{0}', opt2 = '{1}', opt3 = '{2}', opt4 = '{3}', opt5 = '{4}', user_ip = '{5}', upddate = getdate()", opt1.Text.Trim(), opt2.Text.Trim(), opt3.Text.Trim(), opt4.Text.Trim(), opt5.Text.Trim(), Request.UserHostAddress)), "UPDATE");
                
        Response.Redirect(Request.RawUrl);  // 페이지 새로고침
    }
}
