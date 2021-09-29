using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class gym : Page
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
			regCloseCheck();
			regPresentTBLoad();
			regHistoryTBLoad();
			gymInfoTBLoad();
		}		
	}

	private void adminCheck()
	{
		dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select EMPL_CODE
from [insa].[dbo].[dh_empl_m]
where (DEPT_CODE = '303500' or DEPT_CODE = '300600' or ANDX_DEPT = '300600') and (DEGR_CODE <= 'B0' and EXRE_GUBN = '1')")), "SELECT");

		foreach (DataRow dr in dt.Rows)
		{
			if (dr.ItemArray[0].ToString().Equals(empno))
			{
				movePage.Visible = true;
				break;
			}
		}
	}
    
	private void regCloseCheck()
	{
		dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select reg_close from [event].[dbo].[gym_info]")), "SELECT");

		if (dt.Rows[0].ItemArray[0].Equals(true))	// 마감되었을 경우
		{
			regBtn.Enabled = false;
			cancelBtn.Enabled = false;
			regBtn.ToolTip = "마감되었습니다.";
			cancelBtn.ToolTip = "마감되었습니다.";
		}
		else
		{
			regBtn.Enabled = true;
			cancelBtn.Enabled = true;
		}
	}
    
	////////// 나의 등록 현황 테이블 //////////
	private void regPresentTBLoad()
	{
		dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select reg_month, cont, opt1, opt2, opt3, opt4, opt5 
from [event].[dbo].[gym]
where empno = '{0}' and present = 1", empno)), "SELECT");

		if (dt.Rows.Count < 1)	// 등록 현황이 없을 경우
		{
			regPresentMonth.Text = DateTime.Now.AddMonths(1).ToString("yyyy") + "년 " + DateTime.Now.AddMonths(1).ToString("MM") + "월";
			regPresentCont.Text = "미등록";
			regPresentOpt1.Checked = false;
			regPresentOpt1.Enabled = true;
			regPresentOpt2.Checked = false;
			regPresentOpt2.Enabled = true;
            regPresentOpt3.Checked = false;
            regPresentOpt3.Enabled = true;
            regPresentOpt4.Checked = false;
            regPresentOpt4.Enabled = true;
            regPresentOpt5.Checked = false;
            regPresentOpt5.Enabled = true;
            regBtn.Visible = true;
			cancelBtn.Visible = false;
		}
		else					// 등록 현황이 있을 경우
		{
			regPresentMonth.Text = string.Format("{0:####년 ##월}", Convert.ToInt32(dt.Rows[0].ItemArray[0]));
			regPresentCont.Text = Convert.ToBoolean(dt.Rows[0].ItemArray[1]) ? "연장(자동 등록) 예정" : "신규 등록 예정";
			regPresentOpt1.Checked = Convert.ToBoolean(dt.Rows[0].ItemArray[2]);
			regPresentOpt1.Enabled = false;
			regPresentOpt2.Checked = Convert.ToBoolean(dt.Rows[0].ItemArray[3]);
			regPresentOpt2.Enabled = false;
            regPresentOpt3.Checked = Convert.ToBoolean(dt.Rows[0].ItemArray[4]);
            regPresentOpt3.Enabled = false;
            regPresentOpt4.Checked = Convert.ToBoolean(dt.Rows[0].ItemArray[5]);
            regPresentOpt4.Enabled = false;
            regPresentOpt5.Checked = Convert.ToBoolean(dt.Rows[0].ItemArray[6]);
            regPresentOpt5.Enabled = false;
            regBtn.Visible = false;
			cancelBtn.Visible = true;
		}

        dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select opt1, opt2, opt3, opt4, opt5 
from [event].[dbo].[gym_info]")), "SELECT");

        for (int i = 1; i < dt.Columns.Count + 1; i++)
        {
            if (dt.Rows[0].ItemArray[i - 1].ToString() == "")
                ((CheckBox)FindControl("regPresentOpt" + i)).Visible = false;
            else
                ((CheckBox)FindControl("regPresentOpt" + i)).Text = " " + dt.Rows[0].ItemArray[i - 1].ToString() + " ";
        }
    }

    protected void regBtn_Click(object sender, EventArgs e)
	{		
		dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select empl.KORE_NAME, dept.GUK_NAME, dept.TEAM_NAME
from [insa].[dbo].[dh_empl_m] empl, [insa].[dbo].[dh_dept_m] dept
where empl.EMPL_CODE = '{0}' and dept.DEPT_ID = empl.DEPT_CODE", empno)), "SELECT");

		string empname = dt.Rows[0].ItemArray[0].ToString();
		string empdept = dt.Rows[0].ItemArray[1].ToString().Trim() + " " + dt.Rows[0].ItemArray[2].ToString().Trim();

		string reg_month = regPresentMonth.Text.Replace("년", "").Replace(" ", "").Replace("월", "");
        bool opt1 = regPresentOpt1.Checked;
        bool opt2 = regPresentOpt2.Checked;
        bool opt3 = regPresentOpt3.Checked;
        bool opt4 = regPresentOpt4.Checked;
        bool opt5 = regPresentOpt5.Checked;

        dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select top(1) reg_month
from [event].[dbo].[gym]
where empno = '{0}' and present = 0 and cancel = 1
order by reg_month desc", empno)), "SELECT");

		if (dt.Rows.Count > 0 && dt.Rows[0].ItemArray[0].Equals(reg_month))     // 같은 달 취소 기록이 있을 경우
		{
			dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select cancel, opt1, opt2, opt3, opt4, opt5
from [event].[dbo].[gym]
where empno = '{0}' and reg_month = '{1}'", empno, DateTime.Now.ToString("yyyyMM"))), "SELECT");

			if (dt.Rows.Count > 0 && dt.Rows[0].ItemArray[0].Equals(false) && dt.Rows[0].ItemArray[1].Equals(opt1) && dt.Rows[0].ItemArray[2].Equals(opt2) && dt.Rows[0].ItemArray[3].Equals(opt3) && dt.Rows[0].ItemArray[4].Equals(opt4) && dt.Rows[0].ItemArray[5].Equals(opt5))  // 이전 달이 내역에 있고, 취소 내역이 아니고, 옵션이 같을 경우
			{
				dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"update [event].[dbo].[gym]
set empname = '{0}', empdept = '{1}', cont = 1, present = 1, cancel = 0, opt1 = '{2}', opt2 = '{3}', opt3 = '{4}', opt4 = '{5}', opt5 = '{6}', user_ip = '{7}', upddate = getdate() 
where empno = '{8}' and reg_month = '{9}'", empname, empdept, opt1, opt2, opt3, opt4, opt5, Request.UserHostAddress, empno, reg_month)), "UPDATE");
			}
			else                                                                                                    // 이전 달이 내역에 없거나, 있지만 취소했거나, 있지만 옵션이 다를 경우
			{
				dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"update [event].[dbo].[gym]
set cont = 0, present = 1, cancel = 0, opt1 = '{0}', opt2 = '{1}', opt3 = '{2}', opt4 = '{3}', opt5 = '{4}', user_ip = '{5}', upddate = getdate() 
where empno = '{6}' and reg_month = '{7}'", opt1, opt2, opt3, opt4, opt5, Request.UserHostAddress, empno, reg_month)), "UPDATE");
			}
		}
		else                                                                    // 같은 달 취소 기록이 없을 경우
		{
			dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"insert into event.dbo.gym (empno, empname, empdept, reg_month, cont, present, opt1, opt2, opt3, opt4, opt5, user_ip, upddate)
values ('{0}', '{1}', '{2}', '{3}', '{4}', {5}, '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', getdate())", empno, empname, empdept, reg_month, 0, 1, opt1, opt2, opt3, opt4, opt5, Request.UserHostAddress)), "INSERT");
		}

		regPresentTBLoad();		
	}

    protected void cancelBtn_Click(object sender, EventArgs e)
	{
		dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"update [event].[dbo].[gym]
set present = 0, cancel = 1, user_ip = '{0}', upddate = getdate()
where empno = '{1}' and present = 1", Request.UserHostAddress, empno)), "UPDATE");

		regPresentTBLoad();
	}
	
	////////// 나의 이용 내역 테이블 //////////
	private void regHistoryTBLoad()
	{
		dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select distinct left(reg_month, 4) as year
from [event].[dbo].[gym]
where empno = '{0}' and present = 0 and cancel = 0
order by year desc", empno)), "SELECT");

		if (dt.Rows.Count < 1)
		{
			regHistoryTableWrap.Visible = false;
			emptyTableWrap.Visible = true;
		}
		else
		{
			regHistoryTableWrap.Visible = true;
			emptyTableWrap.Visible = false;

			foreach (DataRow dr in dt.Rows)
				regHistoryTabYear.Items.Add(dr.ItemArray[0].ToString());			
		}

		regHistoryTrLoad();
	}

    private void regHistoryTrLoad()
    {
        dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select reg_month, cont, opt1, opt2, opt3, opt4, opt5,
(select opt1 from [event].[dbo].[gym_info]) as 'opt1_name',
(select opt2 from [event].[dbo].[gym_info]) as 'opt2_name',
(select opt3 from [event].[dbo].[gym_info]) as 'opt3_name',
(select opt4 from [event].[dbo].[gym_info]) as 'opt4_name',
(select opt5 from [event].[dbo].[gym_info]) as 'opt5_name'
from [event].[dbo].[gym]
where empno = '{0}' and left(reg_month, 4) = '{1}' and present = 0 and cancel = 0
order by reg_month desc", empno, regHistoryTabYear.Text)), "SELECT");

        regHistoryTr.DataSource = dt;
        regHistoryTr.DataBind();
    }

    protected void regHistoryTabYear_SelectedIndexChanged(object sender, EventArgs e)
	{
		regHistoryTrLoad();
	}
	
	////////// 헬스장 정보 테이블 //////////
	protected void gymInfoTBLoad()
	{
		dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select info from [event].[dbo].[gym_info]")), "SELECT");

		if (dt.Rows.Count > 0)
			gymInfo.Text = dt.Rows[0].ItemArray[0].ToString();
	}
}
