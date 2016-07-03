<%@ Import Namespace="www.BankBals.Service" %>
<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AggsList.aspx.cs" Inherits="www.BankBals.Service.Sources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class='div-bread'> 
        <a href="/BankBals">Главная</a> &#8594; 
        <a href="Default.aspx">Service</a> &#8594; 
        Sources Map (Form <b> <%=this.FormID%></b>)
    </div>

    <table class="table-datatable table-sources-legend">
    <tr> <td class='td-blue-darker'>CBR</td><td>Данные с <a href="http://www.cbr.ru/">cbr.ru</a> с оборотами </td> </tr>
    <tr> <td class='td-blue'>CBR</td><td>Данные с <a href="http://www.cbr.ru/">cbr.ru</a> без оборотов </td> </tr>
    <tr> <td class='td-yellow-darker'>MBK</td><td>Данные с <a href="http://mbkcentre.ru/">mbkcentre.ru</a> с оборотами </td> </tr>
    <tr> <td class='td-yellow'>MBK</td><td>Данные с <a href="http://mbkcentre.ru/">mbkcentre.ru</a> без оборотов </td> </tr>
    <tr> <td>---</td><td>Данных нет</td> </tr>
    </table>

    <table class="table-datatable table-sources">
    <thead>
        <tr>
        <th>Reg.Num.</th>
        <th>Name</th>
        <% foreach (A_DATE item in this.Dates) { %>
                <th><%= item.Date.ToString("yyyy-MM-dd")%></th>    
        <% } %>
        </tr></thead>
        <tbody>
        <%
            int BankID = 0;
            foreach (SourcesData.Data item in this.SourcesData) {
                if (BankID != item.BankID) {
                    if (BankID != 0) {
                        %> </tr> <%
                    }
                    %> 
                    <tr>
                    <td><a href='/BankBals/Bank/Data?BankID=<%=item.BankID%>&ViewID=1'><%= item.BankID %></a></td>
                    <td><%= item.NameRus %></td>
                    <%
                    BankID = item.BankID;
                }
                if (!item.Src.HasValue) {
                //If Not item.Src.HasValue Then
                    %> <td>-</td> <%
                } else if (item.Src.Value == 1) {
                    %> <td class='td-blue'>CBR</td> <%
                } else if (item.Src.Value == 11) {
                    %> <td class='td-blue-darker'>CBR</td> <%
                } else if (item.Src.Value == 0) {
                    %> <td class='td-yellow'>MBK</td> <%
                } else if (item.Src.Value == 10) {
                    %> <td class='td-yellow-darker'>MBK</td> <%
                }
            }
         %>
    </tr>
    </tbody>
</table>
</asp:Content>
