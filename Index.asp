﻿<%
function DecodeBySType(strData)
	Dim strRet
	Dim objDec
	
	Set objDec		=	Server.CreateObject("EDCodeCom.EDCodeObj.1")	
	strRet			=	objDec.Decode(strData)
	
	Set objDec		=	Nothing
	DecodeBySType	=	strRet
end function
%>

<%
Dim id
Dim pwd

If Request.Cookies( "jis1" ) = "" Then
    Response.Redirect "http://sis.seoul.co.kr"
Else
    id = DecodeBySType(Request.Cookies( "jis1" ))    
End If
%>

<form name="form1" action="gym.aspx" method="post">
    <input type="hidden" name="id" value="<%=id%>" />
</form>

<script language="javascript">
	//alert('준비중입니다.');
	//history.back();
    form1.submit();
</script>