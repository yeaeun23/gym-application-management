<%@ Page Language="C#" AutoEventWireup="true" CodeFile="gym_admin.aspx.cs" Inherits="gym_admin" ValidateRequest="false" EnableEventValidation="false" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="X-UA-Compatible" content="IE=edge" />
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title></title>
	<link href="css/style.css" rel="stylesheet" />
	<link href="css/reset.css" rel="stylesheet" />
	<script type="text/javascript" src="js/jquery.min.js"></script>
    <script type="text/javascript" src="js/gym_admin.js"></script>
</head>

<body>
	<form id="form1" runat="server">
	<div class="wrap">
		<table>
			<caption>
				<img src="./image/rectangle1.gif" alt="" />
				등록 내역
				<asp:DropDownList runat="server" ID="historyTabYear" OnSelectedIndexChanged="historyTabYear_SelectedIndexChanged" AutoPostBack="true" /> -
				<asp:DropDownList runat="server" ID="historyTabMonth" OnSelectedIndexChanged="historyTabMonth_SelectedIndexChanged" AutoPostBack="true" />
			</caption>
		</table>


		<asp:Panel runat="server" ID="regTableWrap" CssClass="table_wrap">
			<table class="reg">
				<colgroup>
					<col width="5%" />
					<col width="8%" />
					<col width="12%" />
					<col width="12%" />
					<col width="30%" />
					<col width="25%" />
                    <col width="8%" />
				</colgroup>
				<caption>
					□ 신규 등록/연장자
					<asp:DropDownList runat="server" ID="historyTabOrder1" OnSelectedIndexChanged="historyTabOrder1_SelectedIndexChanged" AutoPostBack="true">
						<asp:ListItem Value="등록순" Selected="True"></asp:ListItem>
						<asp:ListItem Value="이름순"></asp:ListItem>
						<asp:ListItem Value="사번순"></asp:ListItem>
						<asp:ListItem Value="부서순"></asp:ListItem>
					</asp:DropDownList>					
					<asp:Button runat="server" ID="regSMS" CssClass="caption_btn" Text="문자" OnClientClick="return confirm('[문자-신규 등록/연장자]\n\n수신자 명단을 복사하시겠습니까?')" OnClick="regSMS_Click" />		                    			
					<asp:Button runat="server" ID="regExcel" CssClass="caption_btn" Text="엑셀" OnClientClick="return confirm('[엑셀-신규 등록/연장자]\n\n파일을 다운로드하시겠습니까?')" OnClick="regExcel_Click" ForeColor="DarkGreen" />
                    <asp:Button runat="server" ID="regClose" CssClass="caption_btn" Text="마감/취소" OnClientClick="return confirm('마감/취소하시겠습니까?')" OnClick="regClose_Click" ForeColor="Red" />
				</caption>
				<thead>
					<tr>
						<th>NO</th>
						<th>구분</th>
						<th>이름</th>
						<th>사번</th>
						<th>부서</th>
						<th>부대시설</th>
                        <th></th>
					</tr>
				</thead>
				<tbody>
					<asp:Repeater runat="server" ID="regTr">
						<ItemTemplate>
							<tr>
								<td class="table_no">
									<asp:Label runat="server" CssClass="reg_no" />
								</td>
								<td>
									<asp:Label runat="server" Text='<%# Eval("cont").Equals(true) ? "연장" : "신규" %>' />
								</td>
								<td>
									<asp:Label runat="server" Text='<%# Eval("empname") %>' />
								</td>
                                <td>
									<asp:Label runat="server" Text='<%# Eval("empno") %>' />
								</td>
								<td>
									<asp:Label runat="server" Text='<%# Eval("empdept") %>' />
								</td>
								<td>
									<asp:CheckBox runat="server" Enabled="false" Text='<%# " " + Eval("opt1_name") + " " %>' Checked='<%# Eval("opt1") %>' Visible='<%# Eval("opt1_name").ToString() == "" ? false : true %>' />
									<asp:CheckBox runat="server" Enabled="false" Text='<%# " " + Eval("opt2_name") + " " %>' Checked='<%# Eval("opt2") %>' Visible='<%# Eval("opt2_name").ToString() == "" ? false : true %>' />
                                    <asp:CheckBox runat="server" Enabled="false" Text='<%# " " + Eval("opt3_name") + " " %>' Checked='<%# Eval("opt3") %>' Visible='<%# Eval("opt3_name").ToString() == "" ? false : true %>' />
                                    <asp:CheckBox runat="server" Enabled="false" Text='<%# " " + Eval("opt4_name") + " " %>' Checked='<%# Eval("opt4") %>' Visible='<%# Eval("opt4_name").ToString() == "" ? false : true %>' />
                                    <asp:CheckBox runat="server" Enabled="false" Text='<%# " " + Eval("opt5_name") + " " %>' Checked='<%# Eval("opt5") %>' Visible='<%# Eval("opt5_name").ToString() == "" ? false : true %>' />
								</td>
                                <td>
                                    <asp:Button runat="server" CssClass="cancel_btn" Text="해지" OnClientClick="return confirm('해지하시겠습니까?')" OnClick="Cancel_Click" CommandArgument='<%# Eval("empno") %>' />
                                </td>
							</tr>
						</ItemTemplate>
					</asp:Repeater>
				</tbody>
			</table>
		</asp:Panel>
		<asp:Panel runat="server" ID="regEmptyTableWrap" CssClass="table_wrap">
			<table>
				<colgroup>
					<col width="*" />
				</colgroup>
				<caption>
					□ 신규 등록/연장자
				</caption>
				<tbody>
					<tr>
						<td style="color: #999; height: 80px;">신규 등록/연장자가 없습니다.</td>
					</tr>	
				</tbody>
			</table>
		</asp:Panel>


		<asp:Panel runat="server" ID="cancelTableWrap" CssClass="table_wrap">
			<table class="cancel">
				<colgroup>
					<col width="5%" />
					<col width="8%" />
					<col width="12%" />
					<col width="12%" />
					<col width="30%" />
					<col width="25%" />
                    <col width="8%" />
				</colgroup>
				<caption>
					□ 연장 취소자
					<asp:DropDownList runat="server" ID="historyTabOrder2" OnSelectedIndexChanged="historyTabOrder2_SelectedIndexChanged" AutoPostBack="true">
						<asp:ListItem Value="취소순" Selected="True"></asp:ListItem>
						<asp:ListItem Value="이름순"></asp:ListItem>
                        <asp:ListItem Value="사번순"></asp:ListItem>
						<asp:ListItem Value="부서순"></asp:ListItem>
					</asp:DropDownList>
					<asp:Button runat="server" ID="cancelSMS" CssClass="caption_btn" Text="문자" OnClientClick="return confirm('[문자-연장 취소자]\n\n수신자 명단을 복사하시겠습니까?')" OnClick="cancelSMS_Click" />
                    <asp:Button runat="server" ID="cancelExcel" CssClass="caption_btn" Text="엑셀" OnClientClick="return confirm('[엑셀-연장 취소자]\n\n파일을 다운로드하시겠습니까?')" OnClick="cancelExcel_Click" ForeColor="DarkGreen" />
				</caption>
				<thead>
					<tr>
						<th>NO</th>
						<th>구분</th>
						<th>이름</th>
                        <th>사번</th>
						<th>부서</th>
						<th>부대시설</th>
                        <th></th>
					</tr>
				</thead>
				<tbody>
					<asp:Repeater runat="server" ID="cancelTr">
						<ItemTemplate>
							<tr>
								<td class="table_no">
									<asp:Label runat="server" CssClass="cancel_no" />
								</td>
                                <td>
									취소
								</td>
								<td>
									<asp:Label runat="server" Text='<%# Eval("empname") %>' />
								</td>
                                <td>
									<asp:Label runat="server" Text='<%# Eval("empno") %>' />
								</td>
								<td>
									<asp:Label runat="server" Text='<%# Eval("empdept") %>' />
								</td>
								<td>
									<asp:CheckBox runat="server" Enabled="false" Text='<%# " " + Eval("opt1_name") + " " %>' Checked='<%# Eval("opt1") %>' Visible='<%# Eval("opt1_name").ToString() == "" ? false : true %>' />
									<asp:CheckBox runat="server" Enabled="false" Text='<%# " " + Eval("opt2_name") + " " %>' Checked='<%# Eval("opt2") %>' Visible='<%# Eval("opt2_name").ToString() == "" ? false : true %>' />
                                    <asp:CheckBox runat="server" Enabled="false" Text='<%# " " + Eval("opt3_name") + " " %>' Checked='<%# Eval("opt3") %>' Visible='<%# Eval("opt3_name").ToString() == "" ? false : true %>' />
                                    <asp:CheckBox runat="server" Enabled="false" Text='<%# " " + Eval("opt4_name") + " " %>' Checked='<%# Eval("opt4") %>' Visible='<%# Eval("opt4_name").ToString() == "" ? false : true %>' />
                                    <asp:CheckBox runat="server" Enabled="false" Text='<%# " " + Eval("opt5_name") + " " %>' Checked='<%# Eval("opt5") %>' Visible='<%# Eval("opt5_name").ToString() == "" ? false : true %>' />
								</td>
                                <td>
                                    <asp:Button runat="server" CssClass="cancel_btn" Text="삭제" OnClientClick="return confirm('삭제하시겠습니까?')" OnClick="Delete_Click" CommandArgument='<%# Eval("empno") %>' />
                                </td>
							</tr>
						</ItemTemplate>
					</asp:Repeater>
				</tbody>
			</table>
		</asp:Panel>
		<asp:Panel runat="server" ID="cancelEmptyTableWrap" CssClass="table_wrap">
			<table>
				<colgroup>
					<col width="*" />
				</colgroup>
				<caption>
					□ 연장 취소자
				</caption>
				<tbody>
					<tr>
						<td style="color: #999; height: 80px;">연장 취소자가 없습니다.</td>
					</tr>	
				</tbody>
			</table>
		</asp:Panel>


		<asp:Panel runat="server" CssClass="table_wrap">
			<table class="admin_gym_info">
				<colgroup>
					<col width="*" />
				</colgroup>
				<caption>
					<img src="./image/rectangle1.gif" alt="" />
					헬스장 정보
					<asp:Button runat="server" ID="gymInfoSave" CssClass="caption_btn" Text="저장" OnClick="gymInfoSave_Click" OnClientClick="return confirm('[헬스장 정보]\n\n저장하시겠습니까?');" />
				</caption>
				<tbody>
					<tr>
						<td>
							<asp:TextBox runat="server" ID="gymInfo" TextMode="MultiLine" Rows="8" />
						</td>
					</tr>
				</tbody>
			</table>
		</asp:Panel>


        <asp:Panel runat="server" CssClass="table_wrap">
			<table class="admin_opt">
				<colgroup>
					<col width="20%" />
					<col width="20%" />
					<col width="20%" />
                    <col width="20%" />
                    <col width="20%" />
				</colgroup>
				<caption>
					<img src="./image/rectangle1.gif" alt="" />
					부대시설 관리
					<asp:Button runat="server" ID="optSave" CssClass="caption_btn" Text="저장" OnClick="optSave_Click" OnClientClick="return confirm('[부대시설 관리]\n\n저장하시겠습니까?');" />
				</caption>
				<thead>
					<tr>
						<th>1</th>
						<th>2</th>
						<th>3</th>
						<th>4</th>
						<th>5</th>
					</tr>
				</thead>
				<tbody>
					<tr>
						<td>
							<asp:TextBox runat="server" ID="opt1" Width="95%" placeholder="ex) 로커" />
						</td>
                        <td>
							<asp:TextBox runat="server" ID="opt2" Width="95%" placeholder="ex) 밀론" />
						</td>
                        <td>
							<asp:TextBox runat="server" ID="opt3" Width="95%" placeholder="ex) 골프" />
						</td>
                        <td>
							<asp:TextBox runat="server" ID="opt4" Width="95%" />
						</td>
                        <td>
							<asp:TextBox runat="server" ID="opt5" Width="95%" />
						</td>
					</tr>
				</tbody>
			</table>
		</asp:Panel>


		<asp:LinkButton runat="server" ID="movePage" PostBackUrl="~/index.asp">등록자 페이지</asp:LinkButton>

        <asp:TextBox runat="server" ID="clipBoard" CssClass="blind" TextMode="MultiLine" Rows="8" />

        <asp:GridView runat="server" ID="GridView1" DataSourceID="SqlDataSource1" Visible="true" />
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ appSettings:system6strConn %>" SelectCommand=""></asp:SqlDataSource>
	</div>
	</form>
</body>
</html>
