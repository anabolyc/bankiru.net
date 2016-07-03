<%@ Import Namespace="www.BankBals.Service" %>
<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AggsList.aspx.cs" Inherits="www.BankBals.Service.AggsList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class='div-bread'> 
        <a href="/BankBals">Главная</a> &#8594; 
        <a href="Default.aspx">Service</a> &#8594; 
        Aggregate Members List (<%= this.Aggregate.FullNameRUS %>)
    </div>

    <table class="table-datatable table-aggsmembers">
    <thead>
        <tr>
        <th>  Reg.Num.  </th>
        <th>  Name  </th>
        </tr>
    </thead>

    <tbody>
        <%  foreach (W_AGG_COMP item in this.Members) { %>
                <tr>
                <td><%= item.BankID %></td>
                <td><a href='/BankBals/Bank/Data?BankID=<%=item.BankID%>&ViewID=1'><%= item.A_BANK.FullNameRUS %></a></td>
                </tr>
        <%  } %>
    </tbody>
    </table>
</asp:Content>


