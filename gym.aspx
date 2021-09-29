<%@ Page Language="C#" AutoEventWireup="true" CodeFile="gym.aspx.cs" Inherits="gym" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="X-UA-Compatible" content="IE=edge" />
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title></title>
	<link href="css/style.css" rel="stylesheet" />
	<link href="css/reset.css" rel="stylesheet" />
</head>

<body>
	<form id="form1" runat="server">
	<div class="wrap">
		<asp:Panel runat="server" CssClass="table_wrap">
			<table class="reg_present">
				<colgroup>
					<col width="30%" />
					<col width="30%" />
					<col width="40%" />
				</colgroup>
				<caption>
					<img src="./image/rectangle1.gif" alt="" />
					나의 등록 현황
					<asp:Button runat="server" ID="regBtn" CssClass="caption_btn" Text="등록하기" OnClientClick="return confirm('등록하시겠습니까?')" OnClick="regBtn_Click" />
					<asp:Button runat="server" ID="cancelBtn" CssClass="caption_btn" Text="해지하기" OnClientClick="return confirm('해지하시겠습니까?')" OnClick="cancelBtn_Click" />
				</caption>
				<thead>
					<tr>
						<th>등록 월</th>
						<th>등록 구분</th>
						<th>부대시설</th>
					</tr>
				</thead>
				<tbody>
					<tr>
						<td>
							<asp:Label runat="server" ID="regPresentMonth" />
						</td>
						<td>
							<asp:Label runat="server" ID="regPresentCont" />
						</td>
						<td>
							<asp:CheckBox runat="server" ID="regPresentOpt1" />
							<asp:CheckBox runat="server" ID="regPresentOpt2" />
                            <asp:CheckBox runat="server" ID="regPresentOpt3" />
                            <asp:CheckBox runat="server" ID="regPresentOpt4" />
                            <asp:CheckBox runat="server" ID="regPresentOpt5" />
						</td>
					</tr>
				</tbody>
			</table>
            <p style="font-size: 11px; color: #999; margin-top: 5px; text-align: right;">* 부대시설 변경 : 해지 후 재등록</p>
		</asp:Panel>


		<asp:Panel runat="server" ID="regHistoryTableWrap" CssClass="table_wrap">
			<table class="reg_history">
				<colgroup>
					<col width="30%" />
					<col width="30%" />
					<col width="40%" />
				</colgroup>
				<caption>
					<img src="./image/rectangle1.gif" alt="" />
					나의 이용 내역
					<asp:DropDownList runat="server" ID="regHistoryTabYear" OnSelectedIndexChanged="regHistoryTabYear_SelectedIndexChanged" AutoPostBack="true" />
				</caption>
				<thead>
					<tr>
						<th>이용 월</th>
						<th>등록 구분</th>
						<th>부대시설</th>
					</tr>
				</thead>
				<tbody>
					<asp:Repeater runat="server" ID="regHistoryTr">
						<ItemTemplate>
							<tr>
								<td>
									<asp:Label runat="server" Text='<%# string.Format("{0:####년 ##월}", Convert.ToInt32(Eval("reg_month"))) %>' />
								</td>
								<td>
									<asp:Label runat="server" Text='<%# Eval("cont").Equals(true) ? "연장(자동 등록)" : "신규 등록" %>' />
								</td>
								<td>
									<asp:CheckBox runat="server" Enabled="false" Text='<%# " " + Eval("opt1_name") + " " %>' Checked='<%# Eval("opt1") %>' Visible='<%# Eval("opt1_name").ToString() == "" ? false : true %>' />
									<asp:CheckBox runat="server" Enabled="false" Text='<%# " " + Eval("opt2_name") + " " %>' Checked='<%# Eval("opt2") %>' Visible='<%# Eval("opt2_name").ToString() == "" ? false : true %>' />
                                    <asp:CheckBox runat="server" Enabled="false" Text='<%# " " + Eval("opt3_name") + " " %>' Checked='<%# Eval("opt3") %>' Visible='<%# Eval("opt3_name").ToString() == "" ? false : true %>' />
                                    <asp:CheckBox runat="server" Enabled="false" Text='<%# " " + Eval("opt4_name") + " " %>' Checked='<%# Eval("opt4") %>' Visible='<%# Eval("opt4_name").ToString() == "" ? false : true %>' />
                                    <asp:CheckBox runat="server" Enabled="false" Text='<%# " " + Eval("opt5_name") + " " %>' Checked='<%# Eval("opt5") %>' Visible='<%# Eval("opt5_name").ToString() == "" ? false : true %>' />
								</td>
							</tr>
						</ItemTemplate>
					</asp:Repeater>
				</tbody>
			</table>
		</asp:Panel>
		<asp:Panel runat="server" ID="emptyTableWrap" CssClass="table_wrap">
			<table>
				<colgroup>
					<col width="*" />
				</colgroup>
				<caption>
					<img src="./image/rectangle1.gif" alt="" />
					나의 이용 내역
				</caption>
				<tbody>
					<tr>
						<td style="color: #999; height: 80px;">이용 내역이 없습니다.</td>
					</tr>	
				</tbody>
			</table>
		</asp:Panel>


		<asp:Panel runat="server" CssClass="table_wrap">
			<table class="gym_info">
				<colgroup>
					<col width="*" />
				</colgroup>
				<caption>
					<img src="./image/rectangle1.gif" alt="" />
					헬스장 정보
				</caption>
				<tbody>
					<tr>
						<td style="text-align: left; height: 80px; vertical-align: top; line-height: 160%;">
							<asp:Label runat="server" ID="gymInfo" />
						</td>
					</tr>
				</tbody>
			</table>
		</asp:Panel>


		<asp:LinkButton runat="server" ID="movePage" PostBackUrl="~/gym_admin.aspx" Visible="false">관리자 페이지</asp:LinkButton>
	</div>
	</form>
</body>
</html>
